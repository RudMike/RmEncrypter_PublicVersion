// <copyright file="DatabaseReader.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides methods for reading <see cref="AccountContext"/> entities.
    /// </summary>
    public class DatabaseReader : IDatabaseReader
    {
        private readonly IConnectionProvider connection;
        private readonly IRsaKeyObfuscator obfuscator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseReader"/> class.
        /// </summary>
        /// <param name="connection">Provider for managing the connection to a database.</param>
        /// <param name="obfuscator">The obfuscator for a conversion the Rsa-key.</param>
        public DatabaseReader(IConnectionProvider connection, IRsaKeyObfuscator obfuscator)
        {
            this.connection = connection;
            this.obfuscator = obfuscator;
        }

        /// <inheritdoc/>
        public (IDbAuthData, IDbRsaKey) ReadAll(string userName)
        {
            using AccountContext accountContext = new (this.connection);
            var authData = ReadAuthData(accountContext, userName, true);

            if (authData == null)
            {
                return (null, null);
            }

            if (authData.RsaKey != null)
            {
                this.DeobfuscateKey(authData.RsaKey);
            }

            return (authData, authData.RsaKey);
        }

        /// <inheritdoc/>
        public IDbAuthData ReadAuthData(string userName)
        {
            using AccountContext accountContext = new (this.connection);
            var authData = ReadAuthData(accountContext, userName, false);
            return authData;
        }

        /// <inheritdoc/>
        public IDbRsaKey ReadRsaKey(string userName)
        {
            using AccountContext accountContext = new (this.connection);
            var key = ReadRsaKey(accountContext, userName);
            if (key != null)
            {
                this.DeobfuscateKey(key);
            }

            return key;
        }

        private static AuthenticationData ReadAuthData(AccountContext context, string userName, bool isAttachKey)
        {
            AuthenticationData authData;

            if (isAttachKey)
            {
                authData = context.AuthenticationData.Include(entity => entity.RsaKey)
                                                     .AsNoTracking()
                                                     .Where(entity => entity.UserName == userName)
                                                     .SingleOrDefault();
            }
            else
            {
                authData = context.AuthenticationData.AsNoTracking()
                                                     .Where(entity => entity.UserName == userName)
                                                     .SingleOrDefault();
            }

            return authData;
        }

        private static RsaKey ReadRsaKey(AccountContext context, string userName)
        {
             return context.RsaKey.Include(entity => entity.AuthData)
                                  .AsNoTracking()
                                  .Where(entity => entity.AuthData.UserName == userName)
                                  .SingleOrDefault();
        }

        private void DeobfuscateKey(RsaKey key)
        {
            if (key.AuthData == null)
            {
                throw new NullReferenceException("The key must has not null AuthData property.");
            }

            IRsaKey deobfuscatedKey = key;
            this.obfuscator.Deobfuscate(ref deobfuscatedKey, key.AuthData);
        }
    }
}
