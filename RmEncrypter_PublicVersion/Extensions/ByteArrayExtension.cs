// <copyright file="ByteArrayExtension.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Numerics;

    /// <summary>
    /// Provides extension methods for <see cref="byte"/> array type.
    /// </summary>
    public static class ByteArrayExtension
    {
        /// <summary>
        /// Converts the <see cref="byte"/> array of this instance with one or zero to it's equivalent <see cref="BigInteger"/> number.
        /// </summary>
        /// <param name="array">A <see cref="byte"/> array for the converting.</param>
        /// <returns>The number representation of the <see cref="byte"/> array instance.</returns>
        /// <exception cref="ArgumentException">Inputted byte array have numbers except one or zero. </exception>
        public static BigInteger ToBigInteger(this byte[] array)
        {
            BigInteger result = BigInteger.Zero;

            for (int bitNumber = 0; bitNumber <= array.Length - 1; bitNumber++)
            {
                if (IsOneOrZero(array[bitNumber]))
                {
                    if (array[bitNumber] == 1)
                    {
                        result = BigInteger.Add(result, BigInteger.One << bitNumber);
                    }
                }
                else
                {
                    throw new ArgumentException("Inputted array must have only one or zero.");
                }
            }

            return result;
        }

        private static bool IsOneOrZero(byte value)
        {
            return value == 0 || value == 1;
        }
    }
}
