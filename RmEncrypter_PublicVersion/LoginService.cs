// <copyright file="LoginService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using RmEncrypter_PublicVersion.Exceptions;

    /// <summary>
    /// Represents service for logging and logg off of the account.
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly IDatabaseReader databaseReader;
        private readonly IStringHasher stringHasher;
        private readonly IAuthorizedUser authorizedUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginService"/> class.
        /// </summary>
        /// <param name="databaseReader">The class for reading from a database.</param>
        /// <param name="stringHasher">The hasher which used for password hashing.</param>
        /// <param name="authorizedUser">The class which contains an authorized user data.</param>
        public LoginService(IDatabaseReader databaseReader, IStringHasher stringHasher, IAuthorizedUser authorizedUser)
        {
            this.databaseReader = databaseReader;
            this.stringHasher = stringHasher;
            this.authorizedUser = authorizedUser;
        }

        /// <inheritdoc/>
        public MethodResult Login(string userName, string password)
        {
            if (this.IsLogged())
            {
                return CreateFailedResult(LocalizationService.GetString("LoggedAlready"));
            }

            if (string.IsNullOrEmpty(userName))
            {
                return CreateFailedResult(LocalizationService.GetString("ErrorEmptyUsername"));
            }

            if (string.IsNullOrEmpty(password))
            {
                return CreateFailedResult(LocalizationService.GetString("ErrorEmptyPassword"));
            }

            string passwordHash = this.stringHasher.GetHash(password, userName);
            (IDbAuthData dbAuthData, IDbRsaKey rsaKey) = this.databaseReader.ReadAll(userName);

            if (dbAuthData == null)
            {
                return CreateFailedResult(LocalizationService.GetString("UserNotExist"));
            }

            if (passwordHash != dbAuthData.PasswordHash)
            {
                return CreateFailedResult(LocalizationService.GetString("IncorrectPassword"));
            }

            if (rsaKey != null)
            {
                this.SetAuthorizedUser(userName, rsaKey);
            }
            else
            {
                throw new EntityNotExistException(LocalizationService.GetString("KeyNotExist"));
            }

            return new MethodResult(true);
        }

        /// <inheritdoc/>
        public void Logout()
        {
            this.SetAuthorizedUser(null, null);
        }

        private static MethodResult CreateFailedResult(string message)
        {
            return new MethodResult(false, message);
        }

        private void SetAuthorizedUser(string userName, IRsaKey rsaKey)
        {
            this.authorizedUser.GetType()
                               .GetProperty(nameof(this.authorizedUser.UserName))
                               .SetValue(this.authorizedUser, userName);
            this.authorizedUser.GetType()
                               .GetProperty(nameof(this.authorizedUser.RsaKey))
                               .SetValue(this.authorizedUser, rsaKey);
        }

        private bool IsLogged()
        {
            return this.authorizedUser.RsaKey != null || this.authorizedUser.UserName != null;
        }
    }
}
