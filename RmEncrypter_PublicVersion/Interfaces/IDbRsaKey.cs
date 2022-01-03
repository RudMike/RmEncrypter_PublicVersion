// <copyright file="IDbRsaKey.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Represents a Rsa-key, which can storing in a database.
    /// Inherits the <see cref="IRsaKey"/> interface.
    /// </summary>
    public interface IDbRsaKey : IRsaKey, IValidatable
    {
        /// <summary>
        /// Gets or sets the unique key of the record.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the private exponent of a rsa key.
        /// </summary>
        new string PublicExponent { get; set; }

        /// <summary>
        /// Gets or sets the public exponent of a rsa key.
        /// </summary>
        new string PrivateExponent { get; set; }

        /// <summary>
        /// Gets or sets the multiply result of a rsa key.
        /// </summary>
        new string MultiplyResult { get; set; }
    }
}