// <copyright file="DoNothingPasswordHasher.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;

    /// <summary>
    /// Provides the method for getting hash of a password.
    /// This hasher do nothing and created for the public version.
    /// </summary>
    public class DoNothingPasswordHasher : IStringHasher
    {
        /// <summary>
        /// Get hash of a string.
        /// </summary>
        /// <param name="str">A string for getting hash.</param>
        /// <param name="param">A salt which used for salting hash.</param>
        /// <returns>Returns the inputted string.</returns>
        /// <exception cref="ArgumentNullException">Str parameter is null or empty.</exception>
        public string GetHash(string str, object param = null)
        {
            return str;
        }
    }
}
