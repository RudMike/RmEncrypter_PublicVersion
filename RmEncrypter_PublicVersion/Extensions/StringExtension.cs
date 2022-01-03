// <copyright file="StringExtension.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Numerics;

    /// <summary>
    /// Provides extension methods for <see cref="string"/> type.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Converts the <see cref="string"/> value of this instance to it's equivalent <see cref="BigInteger"/> representation.
        /// </summary>
        /// <param name="value">A string value for the converting.</param>
        /// <returns>The number representation of the value of this instance, consisting of a negative
        /// sign if the value is negative, and a sequence of digits ranging from 0 to 9 with
        /// no leading zeroes.</returns>
        /// <exception cref="InvalidCastException">The value can't be converted to <see cref="BigInteger"/> type.</exception>
        public static BigInteger ToBigInteger(this string value)
        {
            if (!BigInteger.TryParse(value, out BigInteger result))
            {
                throw new InvalidCastException($"Value {value} can't be converted to BigInteger type.");
            }

            return result;
        }
    }
}
