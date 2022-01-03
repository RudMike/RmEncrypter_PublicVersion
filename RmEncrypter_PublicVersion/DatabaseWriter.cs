// <copyright file="DatabaseWriter.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides methods for writing <see cref="AccountContext"/> entities.
    /// </summary>
    public class DatabaseWriter : IDatabaseWriter
    {
        private readonly IConnectionProvider connection;
        private readonly IRsaKeyObfuscator obfuscator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseWriter"/> class.
        /// </summary>
        /// <param name="connection">Provider for managing the connection to a database.</param>
        /// <param name="obfuscator">The obfuscator for a conversion the Rsa-key.</param>
        public DatabaseWriter(IConnectionProvider connection, IRsaKeyObfuscator obfuscator)
        {
            this.connection = connection;
            this.obfuscator = obfuscator;
        }

        /// <inheritdoc/>
        public MethodResult WriteRsaKey(RsaKey key, string userName)
        {
            using AccountContext accountContext = new (this.connection);
            LoadLocalContext(accountContext, userName, true);
            if (IsKeyExistLocal(accountContext))
            {
                return new MethodResult(false, LocalizationService.GetString("ExceptionKeyExist"));
            }

            var authData = GetAuthDataLocal(accountContext);
            MethodResult result;

            if (authData != null)
            {
                this.AddKeyToAuthData(accountContext, key, authData);
                result = TrySaveChanges(accountContext);
            }
            else
            {
                result = new (false, LocalizationService.GetString("SameOrNoneUsernames"));
            }

            return result;
        }

        /// <inheritdoc/>
        public MethodResult WriteUser(AuthenticationData authData)
        {
            using AccountContext accountContext = new (this.connection);
            LoadLocalContext(accountContext, authData.UserName, false);
            MethodResult result;

            if (!IsUserExistLocal(accountContext))
            {
                accountContext.AuthenticationData.Add(authData);
                result = TrySaveChanges(accountContext);
            }
            else
            {
                result = new (false, LocalizationService.GetString("ExceptionUserExist"));
            }

            return result;
        }

        /// <inheritdoc/>
        public MethodResult UpdateUser(string userName, AuthenticationData authData)
        {
            using AccountContext accountContext = new (this.connection);
            var dbAuthData = GetAuthData(accountContext, authData.UserName, true);
            MethodResult result;

            if (dbAuthData != null)
            {
                this.ReobfuscateKey(dbAuthData, authData);
                UpdateAuthData(dbAuthData, authData);
                result = TrySaveChanges(accountContext);
            }
            else
            {
                result = new (false, LocalizationService.GetString("ExceptionUserNotExist"));
            }

            return result;
        }

        /// <inheritdoc/>
        public MethodResult UpdateRsaKey(string userName, RsaKey key)
        {
            using AccountContext accountContext = new (this.connection);
            var dbKey = GetKey(accountContext, userName);
            MethodResult result;

            if (dbKey != null)
            {
                this.UpdateRsaKey(dbKey, key);
                result = TrySaveChanges(accountContext);
            }
            else
            {
                result = new (false, LocalizationService.GetString("ExceptionKeyNotFound"));
            }

            return result;
        }

        /// <inheritdoc/>
        public MethodResult DeleteAccount(string userName)
        {
            using AccountContext accountContext = new (this.connection);
            var account = GetAuthData(accountContext, userName, false);
            MethodResult result;

            if (account != null)
            {
                accountContext.AuthenticationData.Remove(account);
                result = TrySaveChanges(accountContext);
            }
            else
            {
                result = new (false, LocalizationService.GetString("ExceptionUserNotExist"));
            }

            return result;
        }

        private static MethodResult TrySaveChanges(AccountContext context)
        {
            MethodResult result = new (true);
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                result = new MethodResult(false, ex.InnerException.Message);
            }

            return result;
        }

        private static void LoadLocalContext(AccountContext context, string userName, bool isAttachRsaKey)
        {
            if (isAttachRsaKey)
            {
                context.AuthenticationData.Include(entity => entity.RsaKey)
                                          .Where(entity => entity.UserName == userName)
                                          .Load();
            }
            else
            {
                context.AuthenticationData.Where(entity => entity.UserName == userName)
                                          .Load();
            }
        }

        private static AuthenticationData GetAuthData(AccountContext context, string userName, bool isAttachRsaKey)
        {
            AuthenticationData result;
            if (isAttachRsaKey)
            {
                result = context.AuthenticationData
                                .Include(entity => entity.RsaKey)
                                .SingleOrDefault(entity => entity.UserName == userName);
            }
            else
            {
                result = context.AuthenticationData
                                .SingleOrDefault(authData => authData.UserName == userName);
            }

            return result;
        }

        private static AuthenticationData GetAuthDataLocal(AccountContext localContext)
        {
            return localContext.AuthenticationData.Local.SingleOrDefault();
        }

        private static RsaKey GetKey(AccountContext context, string userName)
        {
            return context.RsaKey.Include(entity => entity.AuthData)
                                 .SingleOrDefault(entity => entity.AuthData.UserName == userName);
        }

        private static bool IsUserExistLocal(AccountContext localContext)
        {
            return localContext.AuthenticationData.Local.Any();
        }

        private static bool IsKeyExistLocal(AccountContext localContext)
        {
            return localContext.AuthenticationData
                               .Local
                               .Select(entity => entity.RsaKey)
                               .Any(rsaKey => rsaKey != null);
        }

        private static void UpdateAuthData(AuthenticationData oldValue, AuthenticationData newValue)
        {
            if (oldValue.UserName != newValue.UserName)
            {
                oldValue.UserName = newValue.UserName;
            }

            if (oldValue.PasswordHash != newValue.PasswordHash)
            {
                oldValue.PasswordHash = newValue.PasswordHash;
            }
        }

        private void AddKeyToAuthData(AccountContext context, RsaKey key, AuthenticationData authData)
        {
            key.AuthData = authData;
            IRsaKey obfuscatedKey = key;
            this.obfuscator.Obfuscate(ref obfuscatedKey, authData);
            _ = context.RsaKey.Add(key);
        }

        private void ReobfuscateKey(AuthenticationData oldValue, AuthenticationData newValue)
        {
            if (oldValue.RsaKey == null)
            {
                throw new NullReferenceException("The old auth data must has not null RsaKey property.");
            }

            var authDataKey = (IRsaKey)oldValue.RsaKey;
            this.obfuscator.Deobfuscate(ref authDataKey, oldValue);
            this.obfuscator.Obfuscate(ref authDataKey, newValue);
        }

        private void UpdateRsaKey(RsaKey oldValue, RsaKey newValue)
        {
            if (oldValue.AuthData == null)
            {
                throw new NullReferenceException("The old Rsa-key must has not null AuthData property.");
            }

            var obfuscatedKey = (IRsaKey)newValue.Clone();
            this.obfuscator.Obfuscate(ref obfuscatedKey, oldValue.AuthData);
            oldValue.MultiplyResult = obfuscatedKey.MultiplyResult.ToString();
            oldValue.PrivateExponent = obfuscatedKey.PrivateExponent.ToString();
            oldValue.PublicExponent = obfuscatedKey.PublicExponent.ToString();
        }
    }
}
