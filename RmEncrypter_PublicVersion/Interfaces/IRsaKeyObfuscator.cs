// <copyright file="IRsaKeyObfuscator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides methods for obfuscating of an <see cref="IRsaKey"/> associated with an user.
    /// </summary>
    public interface IRsaKeyObfuscator
    {
        /// <summary>
        /// Obfuscate the <see cref="IRsaKey"/> of the user.
        /// </summary>
        /// <param name="key">Rsa-key for obfuscating.</param>
        /// <param name="user">Information about user-owner the key.</param>
        void Obfuscate(ref IRsaKey key, IAuthData user);

        /// <summary>
        /// Deobfuscate the <see cref="IRsaKey"/> of the user.
        /// </summary>
        /// <param name="key">Rsa-key for deobfuscating.</param>
        /// <param name="user">Information about user-owner the key.</param>
        void Deobfuscate(ref IRsaKey key, IAuthData user);
    }
}