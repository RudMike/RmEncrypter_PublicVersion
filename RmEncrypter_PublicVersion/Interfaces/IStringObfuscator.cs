// <copyright file="IStringObfuscator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;

    /// <summary>
    /// Provides methods for obfuscating of a <see cref="string"/>.
    /// </summary>
    public interface IStringObfuscator
    {
        /// <summary>
        /// Obfuscate a <see cref="string"/>.
        /// </summary>
        /// <param name="input">A string for obfuscating.</param>
        void Obfuscate(ref string input);

        /// <summary>
        /// Deobfuscate a <see cref="string"/>.
        /// </summary>
        /// <param name="input">A string for deobfuscating.</param>
        /// <exception cref="FormatException"> Throws if the input can't be deobfuscated by this obfuscator.</exception>
        void Deobfuscate(ref string input);
    }
}
