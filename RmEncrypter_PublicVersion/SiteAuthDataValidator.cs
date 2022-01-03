// <copyright file="SiteAuthDataValidator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides the validator for the <see cref="SiteAuthData"/> model.
    /// </summary>
    public class SiteAuthDataValidator : ValidateErrorManager, IModelValidator<SiteAuthData>
    {
        /// <inheritdoc/>
        public IDictionary<string, string> Validate(SiteAuthData model)
        {
            this.ValidateSite(model.Site);
            this.ValidateUserName(model.UserName);
            this.ValidatePassword(model.Password);
            return this.Errors;
        }

        private void ValidateSite(string site)
        {
            var propName = nameof(SiteAuthData.Site);
            if (string.IsNullOrEmpty(site))
            {
                _ = this.TryAddError(propName, LocalizationService.GetString("ErrorEmptySite"));
            }
            else
            {
                _ = this.TryRemoveError(propName);
            }
        }

        private void ValidateUserName(string userName)
        {
            var propName = nameof(SiteAuthData.UserName);
            if (string.IsNullOrEmpty(userName))
            {
                _ = this.TryAddError(propName, LocalizationService.GetString("ErrorEmptyUsername"));
            }
            else
            {
                _ = this.TryRemoveError(propName);
            }
        }

        private void ValidatePassword(string password)
        {
            var propName = nameof(SiteAuthData.Password);
            if (string.IsNullOrEmpty(password))
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
