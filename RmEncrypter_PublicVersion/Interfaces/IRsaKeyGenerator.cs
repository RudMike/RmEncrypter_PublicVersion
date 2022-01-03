// <copyright file="IRsaKeyGenerator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides functionality for generating RSA-keys.
    /// </summary>
    public interface IRsaKeyGenerator
    {
        /// <summary>
        /// Gets or sets a value indicating whether is use parallel mode for generating the RSA-key or not.
        /// </summary>
        bool IsParallelMode { get; set; }

        /// <summary>
        /// Generate the RSA-key with given size.
        /// </summary>
        /// <param name="keySize">The size of the RSA-key.</param>
        /// <param name="rsaKey"><see cref="IRsaKey"/> for filing it fields.</param>
        void Generate(Bits keySize, ref IRsaKey rsaKey);
    }
}
