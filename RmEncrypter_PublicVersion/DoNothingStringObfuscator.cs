// <copyright file="DoNothingStringObfuscator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides the obfuscator for a <see cref="string"/>.
    /// This obfuscator do nothing and created for the public version.
    /// </summary>
    public class DoNothingStringObfuscator : IStringObfuscator
    {
        /// <inheritdoc/>
        public void Deobfuscate(ref string input)
        {
        }

        /// <inheritdoc/>
        public void Obfuscate(ref string input)
        {
        }
    }
}
