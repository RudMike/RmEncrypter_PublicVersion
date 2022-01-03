// <copyright file="AuthDataValidator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Propvides the validator for the <see cref="AuthenticationData"/> model.
    /// </summary>
    public class AuthDataValidator : ValidateErrorManager, IModelValidator<AuthenticationData>
    {
        /// <inheritdoc/>
        public IDictionary<string, string> Validate(AuthenticationData model)
        {
            this.ValidateUserName(model.UserName);
            this.ValidatePasswordHash(model.PasswordHash);
            return this.Errors;
        }

        private void ValidateUserName(string userName)
        {
            var propName = nameof(AuthenticationData.UserName);
            if (string.IsNullOrEmpty(userName))
            {
                _ = this.TryAddError(propName, LocalizationService.GetString("ErrorEmptyUsername"));
            }
            else if (userName.Length > 20 || userName.Length < 5)
            {
                _ = this.TryAddError(propName, LocalizationService.GetString("ErrorLengthUsername"));
            }
            else
            {
                _ = this.TryRemoveError(propName);
            }
        }

        private void ValidatePasswordHash(string passwordHash)
        {
            var propName = nameof(AuthenticationData.PasswordHash);
            if (string.IsNullOrEmpty(passwordHash))
            {
                _ = this.TryAddError(propName, LocalizationService.GetString("ErrorEmptyPassword"));
            }
            else
            {
                _ = this.TryRemoveError(propName);
            }
        }
    }
}
