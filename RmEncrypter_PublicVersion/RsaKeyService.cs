// <copyright file="RsaKeyService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Represents service for interaction with Rsa-key of the account.
    /// </summary>
    public class RsaKeyService : IRsaKeyService
    {
        private readonly IRsaKeyGenerator keyGenerator;
        private readonly IModelValidator<RsaKey> modelValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaKeyService"/> class.
        /// </summary>
        /// <param name="keyGenerator">The generator which used for generation <see cref="IRsaKey"/> instance.</param>
        /// <param name="modelValidator">The validator for validating <see cref="RsaKey"/> model.</param>
        public RsaKeyService(IRsaKeyGenerator keyGenerator, IModelValidator<RsaKey> modelValidator)
        {
            this.keyGenerator = keyGenerator;
            this.modelValidator = modelValidator;
        }

        /// <inheritdoc/>
        public RsaKey CreateRsaKey(Bits keySize)
        {
            IRsaKey rsaKey = new RsaKey(this.modelValidator);
            this.keyGenerator.Generate(keySize, ref rsaKey);
            return (RsaKey)rsaKey;
        }
    }
}