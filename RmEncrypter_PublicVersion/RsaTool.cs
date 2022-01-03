// <copyright file="RsaTool.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using RmEncrypter_PublicVersion.Exceptions;

    /// <summary>
    /// Provides functionality for encryption/decryption any strings with Rsa algorithm.
    /// </summary>
    public abstract class RsaTool
    {
        private const int ConvertBase = 8;

        /// <summary>
        /// Gets the value that specifies how many characters will be decomposed each character of the original string.
        /// </summary>
        private const int CharLength = 4;
        private int blockLenght = 2;

        /// <summary>
        /// Gets or sets the value that specifies the blocks length into which the original string will be split. Each block is encrypted independently of the other.
        /// </summary>
        protected int BlockLenght
        {
            get => this.blockLenght;
            set
            {
                if (value >= 1)
                {
                    this.blockLenght = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(this.BlockLenght), "The BlockLength must be more or equal that one.");
                }
            }
        }

        /// <summary>
        /// Decrypts the string.
        /// </summary>
        /// <param name="input">A string for decryption.</param>
        /// <param name="key">Rsa-key which uses for decryption.</param>
        /// <returns>Returns the decrypted string.</returns>
        protected virtual string Decrypt(string input, IRsaKey key)
        {
            var splittedInput = SplitInput(input);
            StringBuilder result = new ();

            foreach (string current in splittedInput)
            {
                string decryptedBlock = string.Empty;
                try
                {
                    decryptedBlock = DecryptBlock(BigInteger.Parse(current), key);
                }
                catch (FormatException)
                {
                    throw new FormatException("Inputed string has wrong format. It must be a number.");
                }

                AlignString(ref decryptedBlock);
                result.Append(decryptedBlock);
            }

            try
            {
                return RepresentNumberAsString(result.ToString());
            }
            catch (FormatException)
            {
                throw new DecryptException("The string can't be decrypted with this key.");
            }
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="input">A string for encryption.</param>
        /// <param name="key">Rsa-key which uses for encryption.</param>
        /// <returns>Returns the encrypted string.</returns>
        protected virtual string Encrypt(string input, IRsaKey key)
        {
            input = RepresentStringAsNumber(input);
            StringBuilder result = new ();
            var inputBlocks = this.DivideOnBlocks(input);

            foreach (string current in inputBlocks)
            {
                BigInteger currentNumber = BigInteger.Parse(current);
                if (currentNumber < key.MultiplyResult)
                {
                    result.AppendLine(EncryptBlock(currentNumber, key));
                }
                else
                {
                    throw new ArgumentException("The key's multiply result is too small. Try to use shorter block size or bigger key.");
                }
            }

            return result.ToString();
        }

        private static IEnumerable<string> SplitInput(string input)
        {
            string[] splitSymbol = { Environment.NewLine };
            return input.Split(splitSymbol, StringSplitOptions.RemoveEmptyEntries).AsEnumerable();
        }

        private static string RepresentStringAsNumber(string input)
        {
            byte[] bytedInput = Encoding.UTF8.GetBytes(input);
            StringBuilder strBuild = new (bytedInput.Length * CharLength);

            foreach (byte b in bytedInput)
            {
                strBuild.Append(Convert.ToString(b, ConvertBase).PadLeft(CharLength, '0'));
            }

            return strBuild.ToString();
        }

        private static string RepresentNumberAsString(string number)
        {
            AlignString(ref number);
            int inputLenght = number.Length;
            int byteArrayLenght = inputLenght / CharLength;
            byte[] bytedNumber = new byte[byteArrayLenght];

            for (int index = 0; index < inputLenght; index += CharLength)
            {
                int arrayIndex = index / CharLength;
                try
                {
                    bytedNumber[arrayIndex] = Convert.ToByte(number.Substring(index, CharLength), ConvertBase);
                }
                catch (OverflowException)
                {
                    throw new DecryptException("The string can't be decrypted with this key.");
                }
            }

            return Encoding.UTF8.GetString(bytedNumber.ToArray());
        }

        private static void AlignString(ref string input)
        {
            if (input.Length % CharLength != 0)
            {
                int totalWidth = input.Length;

                while (totalWidth % CharLength != 0)
                {
                    totalWidth += 1;
                }

                input = input.PadLeft(totalWidth, '0');
            }
        }

        private static string EncryptBlock(BigInteger inputBlock, IRsaKey key)
        {
            var result = BigInteger.ModPow(inputBlock, key.PublicExponent, key.MultiplyResult);
            return result.ToString();
        }

        private static string DecryptBlock(BigInteger inputBlock, IRsaKey key)
        {
            var result = BigInteger.ModPow(inputBlock, key.PrivateExponent, key.MultiplyResult);
            return result.ToString();
        }

        private IEnumerable<string> DivideOnBlocks(string input)
        {
            int inputLenght = input.Length;
            int blocksCount = CharLength * this.BlockLenght;
            var result = new List<string>();

            for (int index = 0; index < inputLenght; index += blocksCount)
            {
                if (inputLenght - index >= blocksCount)
                {
                    result.Add(input.Substring(index, blocksCount));
                }
                else
                {
                    result.Add(input[index..]);
                }
            }

            return result.AsEnumerable();
        }
    }
}
