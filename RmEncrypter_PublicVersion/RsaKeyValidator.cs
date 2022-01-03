// <copyright file="RsaKeyValidator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides the validator for the <see cref="RsaKey"/> model.
    /// </summary>
    public class RsaKeyValidator : ValidateErrorManager, IModelValidator<RsaKey>
    {
        /// <inheritdoc/>
        public IDictionary<string, string> Validate(RsaKey model)
        {
            this.ValidatePrivateExponent(model.PrivateExponent);
            this.ValidatePublicExponent(model.PublicExponent);
            this.ValidateMultiplyResult(model.MultiplyResult);
            return this.Errors;
        }

        private void ValidatePrivateExponent(string privateExponent)
        {
            var propName = nameof(RsaKey.PrivateExponent);
            if (string.IsNullOrEmpty(privateExponent))
            {
                this.TryAddError(propName, LocalizationService.GetString("ErrorEmptyPassword"));
            }
            else
            {
                this.TryRemoveError(propName);
            }
        }

        private void ValidatePublicExponent(string publicExponent)
        {
            var propName = nameof(RsaKey.PublicExponent);
            if (string.IsNullOrEmpty(publicExponent))
            {
                this.TryAddError(propName, LocalizationService.GetString("ErrorEmptyPassword"));
            }
            else
            {
                this.TryRemoveError(propName);
            }
        }

        private void ValidateMultiplyResult(string multiplyResult)
        {
            var propName = nameof(RsaKey.MultiplyResult);
            if (string.IsNullOrEmpty(multiplyResult))
            {
                this.TryAddError(propName, LocalizationService.GetString("ErrorEmptyPassword"));
            }
            else
            {
                this.TryRemoveError(propName);
            }
        }
    }
}
