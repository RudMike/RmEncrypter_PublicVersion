// <copyright file="DoNothingRsaKeyObfuscator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides the obfuscator for an <see cref="IRsaKey"/>.
    /// This obfuscator do nothing and created for the public version.
    /// </summary>
    public class DoNothingRsaKeyObfuscator : IRsaKeyObfuscator
    {
        /// <inheritdoc/>
        public void Obfuscate(ref IRsaKey key, IAuthData user)
        {
        }

        /// <inheritdoc/>
        public void Deobfuscate(ref IRsaKey key, IAuthData user)
        {
        }
    }
}
