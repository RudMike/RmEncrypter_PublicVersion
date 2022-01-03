// <copyright file="IPrimeGenerator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Numerics;

    /// <summary>
    /// Provides functionality for generating probably prime numbers.
    /// </summary>
    public interface IPrimeGenerator
    {
        /// <summary>
        /// Gets or sets a value indicating whether is use parallel mode for generating prime or not.
        /// </summary>
        bool IsParallelMode { get; set; }

        /// <summary>
        /// Generate probably prime number.
        /// </summary>
        /// <param name="bitsCount">The count of bytes in the number.</param>
        /// <returns>Returns probably prime number.</returns>
        BigInteger GenerateProbablyPrime(Bits bitsCount);
    }
}
