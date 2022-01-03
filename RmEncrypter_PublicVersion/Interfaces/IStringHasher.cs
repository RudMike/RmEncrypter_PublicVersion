// <copyright file="IStringHasher.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Represents method for getting hash of a string.
    /// </summary>
    public interface IStringHasher
    {
        /// <summary>
        /// Get hash of a string.
        /// </summary>
        /// <param name="str">A string for getting hash.</param>
        /// <param name="param">A parameter which used for getting hash from the string.</param>
        /// <returns>Returns the hash of the inputted string.</returns>
        string GetHash(string str, object param = null);
    }
}
