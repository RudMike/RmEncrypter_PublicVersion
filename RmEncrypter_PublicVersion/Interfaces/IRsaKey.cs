// <copyright file="IRsaKey.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Numerics;

    /// <summary>
    /// Represents a Rsa-key.
    /// </summary>
    public interface IRsaKey
    {
        /// <summary>
        /// Gets or sets the private exponent of the Rsa-key.
        /// </summary>
        BigInteger PrivateExponent { get; set; }

        /// <summary>
        /// Gets or sets the public exponent of the Rsa-key.
        /// </summary>
        BigInteger PublicExponent { get; set; }

        /// <summary>
        /// Gets or sets the multiply result of the Rsa-key.
        /// </summary>
        BigInteger MultiplyResult { get; set; }
    }
}
