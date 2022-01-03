// <copyright file="RsaKeyGenerator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Numerics;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides the RSA-key generation.
    /// </summary>
    public class RsaKeyGenerator : IRsaKeyGenerator
    {
        private readonly IPrimeGenerator primeGenerator;
        private Bits keySize;
        private BigInteger multiplyResult;
        private BigInteger eulerFunction;
        private BigInteger publicExponent;
        private BigInteger privateExponent;

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaKeyGenerator"/> class.
        /// </summary>
        /// <param name="primeGenerator">A Generator for generating prime numbers.</param>
        public RsaKeyGenerator(IPrimeGenerator primeGenerator)
        {
            this.primeGenerator = primeGenerator;
        }

        /// <inheritdoc/>
        public bool IsParallelMode { get; set; } = true;

        /// <inheritdoc/>
        public void Generate(Bits keySize, ref IRsaKey rsaKey)
        {
            this.keySize = keySize;
            do
            {
                this.CreateKey();
            }
            while (!this.IsValidKey());

            rsaKey.PrivateExponent = this.privateExponent;
            rsaKey.PublicExponent = this.publicExponent;
            rsaKey.MultiplyResult = this.multiplyResult;
        }

        /// <summary>
        /// Finds the greatest common divisor and Bezout coefficients for two numbers using extended Euclidean algorithm.
        /// </summary>
        /// <param name="firstNumber">First number.</param>
        /// <param name="secondNumber">Second number.</param>
        /// <returns>Returns tuple where first item is greatest common divisor, second and third items are Bezout coefficients.
        /// For finding private exponent of RSA need to use <see langword="bezoutCoeff1"/>.</returns>
        private static (BigInteger gcd, BigInteger bezoutCoeff1, BigInteger bezoutCoeff2) ExtendedEuclideanAlgorithm(BigInteger firstNumber, BigInteger secondNumber)
        {
            BigInteger quotient, remainder, s0, s1, s2, t0, t1, t2, gcd;
            s0 = 1;
            s1 = 0;
            t0 = 0;
            t1 = 1;
            gcd = 0;

            while (firstNumber != 0)
            {
                quotient = secondNumber / firstNumber;
                remainder = secondNumber % firstNumber;

                s2 = s0 - (quotient * s1);
                t2 = t0 - (quotient * t1);

                secondNumber = firstNumber;
                firstNumber = remainder;

                s0 = s1;
                t0 = t1;

                s1 = s2;
                t1 = t2;

                gcd = secondNumber;
            }

            return (gcd, t0, s0);
        }

        private bool IsValidKey()
        {
            BigInteger input = BigInteger.Divide(this.multiplyResult, 3);
            BigInteger encrypted = BigInteger.ModPow(input, this.publicExponent, this.multiplyResult);
            BigInteger decrypted = BigInteger.ModPow(encrypted, this.privateExponent, this.multiplyResult);
            return decrypted == input;
        }

        private void CreateKey()
        {
            var coprimes = this.GetCoprimes();
            var firstPrime = coprimes.firstPrime;
            var secondPrime = coprimes.secondPrime;
            this.multiplyResult = firstPrime * secondPrime;
            this.eulerFunction = (firstPrime - 1) * (secondPrime - 1);
            this.publicExponent = this.FindPublicExponent();
            this.privateExponent = this.FindPrivateExponent();
        }

        private (BigInteger firstPrime, BigInteger secondPrime) GetCoprimes()
        {
            return this.IsParallelMode ? this.GetCoprimesParralel() : this.GetCoprimesSingle();
        }

        private (BigInteger, BigInteger) GetCoprimesSingle()
        {
            BigInteger firstprime = this.primeGenerator.GenerateProbablyPrime(this.keySize);
            BigInteger secondPrime;
            while (true)
            {
                secondPrime = this.primeGenerator.GenerateProbablyPrime(this.keySize);
                if (this.IsCoprimes(firstprime, secondPrime))
                {
                    break;
                }
            }

            return (firstprime, secondPrime);
        }

        private (BigInteger, BigInteger) GetCoprimesParralel()
        {
            Task<BigInteger> generateFirstPrime, generateSecondPrime;

            generateFirstPrime = Task.Run(() => this.primeGenerator.GenerateProbablyPrime(this.keySize));
            generateSecondPrime = Task.Run(() => this.primeGenerator.GenerateProbablyPrime(this.keySize));

            var result = Task.WhenAll(generateFirstPrime, generateSecondPrime).Result;

            BigInteger firstPrime = result[0];
            BigInteger secondPrime = result[1];

            while (true)
            {
                if (!this.IsCoprimes(firstPrime, secondPrime))
                {
                    generateSecondPrime = Task.Run(() => this.primeGenerator.GenerateProbablyPrime(this.keySize));
                    secondPrime = generateSecondPrime.Result;
                }
                else
                {
                    break;
                }
            }

            return (firstPrime, secondPrime);
        }

        private BigInteger FindPublicExponent()
        {
            while (true)
            {
                this.publicExponent = this.primeGenerator.GenerateProbablyPrime(this.keySize);
                if (this.publicExponent < this.eulerFunction &&
                    this.IsCoprimes(this.publicExponent, this.eulerFunction))
                {
                    return this.publicExponent;
                }
            }
        }

        private BigInteger FindPrivateExponent()
        {
            var (_, bezoutCoeff1, _) = ExtendedEuclideanAlgorithm(this.publicExponent, this.eulerFunction);
            return bezoutCoeff1 < 0 ? (this.eulerFunction + bezoutCoeff1) : bezoutCoeff1;
        }

        private bool IsCoprimes(BigInteger firstNumber, BigInteger secondNumber)
        {
            bool result;
            if (firstNumber == 0 || secondNumber == 0)
            {
                result = firstNumber + secondNumber == 1;
            }
            else
            {
                if (firstNumber >= secondNumber)
                {
                    firstNumber %= secondNumber;
                }
                else if (firstNumber < secondNumber)
                {
                    secondNumber %= firstNumber;
                }

                result = this.IsCoprimes(firstNumber, secondNumber);
            }

            return result;
        }
    }
}
