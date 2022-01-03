// <copyright file="AccountTransferService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using RmEncrypter_PublicVersion.Exceptions;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Represents service for account's transferring from one device to another.
    /// </summary>
    public class AccountTransferService : IAccountTransfer
    {
        private const string Separator = @"/*\";
        private readonly IDatabaseReader databaseReader;
        private readonly IDatabaseWriter databaseWriter;
        private readonly IStringObfuscator obfuscator;
        private readonly IFileService<string> fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountTransferService"/> class.
        /// </summary>
        /// <param name="databaseReader">The class for reading from a database.</param>
        /// <param name="databaseWriter">The class for writing in a database.</param>
        /// <param name="obfuscator">The obfuscator for a string conversion.</param>
        /// <param name="fileService">Service for writing/reading <see cref="string"/> entities from file.</param>
        public AccountTransferService(IDatabaseReader databaseReader, IDatabaseWriter databaseWriter, IStringObfuscator obfuscator, IFileService<string> fileService)
        {
            this.databaseReader = databaseReader;
            this.databaseWriter = databaseWriter;
            this.obfuscator = obfuscator;
            this.fileService = fileService;
        }

        /// <inheritdoc/>
        public AuthenticationData GetAuthData(string path)
        {
            var readedString = this.fileService.Read(path).First();
            this.Split(readedString, out AuthenticationData fileAuthData, out _);
            return fileAuthData;
        }

        /// <inheritdoc/>
        public void Recovery(string path, AuthenticationData authData = null)
        {
            var readedString = this.fileService.Read(path).First();
            this.Split(readedString, out AuthenticationData fileAuthData, out RsaKey key);
            authData ??= fileAuthData;
            this.TryRecoveryAuthData(authData);
            this.TryRecoveryKey(key, authData.UserName);
        }

        /// <inheritdoc/>
        public void Transfer(string userName, string path)
        {
            (IDbAuthData authData, IDbRsaKey rsaKey) = this.databaseReader.ReadAll(userName);
            var formedString = this.Form(authData, rsaKey);
            this.fileService.Write(new List<string>(1) { formedString }, path);
        }

        private string Form(IAuthData authData, IRsaKey key)
        {
            string result = string.Join(
                Separator,
                authData.UserName,
                authData.PasswordHash,
                key.MultiplyResult.ToString(),
                key.PrivateExponent.ToString(),
                key.PublicExponent.ToString());

            this.obfuscator.Obfuscate(ref result);
            return result;
        }

        private void Split(string input, out AuthenticationData authData, out RsaKey key)
        {
            int fileEntitiesCount = 5;
            try
            {
                this.obfuscator.Deobfuscate(ref input);
            }
            catch (FormatException)
            {
                throw new FileLoadException(LocalizationService.GetString("ErrorAuthDataObfuscation"));
            }

            var result = input.Split(
                new string[] { Separator, },
                fileEntitiesCount,
                StringSplitOptions.None);

            authData = new AuthenticationData(result[0], result[1]);
            key = new RsaKey(result[2], result[3], result[4]);
        }

        private void TryRecoveryAuthData(AuthenticationData authData)
        {
            var writeResult = this.databaseWriter.WriteUser(authData);
            if (!writeResult.IsSuccessful)
            {
                throw new AccountTransferException(writeResult.ExceptionMessage);
            }
        }

        private void TryRecoveryKey(RsaKey key, string userName)
        {
            var writeResult = this.databaseWriter.WriteRsaKey(key, userName);
            if (!writeResult.IsSuccessful)
            {
                throw new AccountTransferException(writeResult.ExceptionMessage);
            }
        }
    }
}
