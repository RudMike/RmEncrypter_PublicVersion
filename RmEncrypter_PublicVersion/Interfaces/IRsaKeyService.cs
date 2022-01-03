// <copyright file="IRsaKeyService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Represents service for interaction with Rsa-key of the account.
    /// </summary>
    public interface IRsaKeyService
    {
        /// <summary>
        /// Create the <see cref="RsaKey"/> instance with given size.
        /// </summary>
        /// <param name="keySize">The size of the Rsa-key.</param>
        /// <returns>Returns the <see cref="RsaKey"/> instance.</returns>
        RsaKey CreateRsaKey(Bits keySize);
    }
}
