// <copyright file="AccountService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Linq;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Represents service for interaction with data of the account.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IStringHasher stringHasher;
        private readonly IModelValidator<AuthenticationData> modelValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="stringHasher">The hasher which used for password hashing.</param>
        /// <param name="modelValidator">The validator for validating <see cref="AuthenticationData"/> model.</param>
        public AccountService(IStringHasher stringHasher, IModelValidator<AuthenticationData> modelValidator)
        {
            this.stringHasher = stringHasher;
            this.modelValidator = modelValidator;
        }

        /// <inheritdoc/>
        public MethodResult CreateAuthData(string userName, string userPassword, out AuthenticationData authData)
        {
            if (!IsAllowedPasswordLenght(userPassword))
            {
                authData = null;
                return new MethodResult(false, LocalizationService.GetString("ExceptionLengthPassword"));
            }

            authData = this.CreateAuthData(userName, userPassword);
            MethodResult result;

            if (!authData.Errors.Any())
            {
                result = new (true);
            }
            else
            {
                result = new (false, authData.Errors.First().Value);
                authData = null;
            }

            return result;
        }

        private static bool IsAllowedPasswordLenght(string password)
        {
            return password.Length >= 6;
        }

        private AuthenticationData CreateAuthData(string userName, string password)
        {
            var passwordHash = this.stringHasher.GetHash(password, userName);
            return new AuthenticationData(userName, passwordHash, this.modelValidator);
        }
    }
}
