// <copyright file="PrimeGenerator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides functionality for generating prime numbers with error probability is 1E-40.
    /// </summary>
    public class PrimeGenerator : IPrimeGenerator
    {
        private readonly int maxDegreeOfParallelism = Environment.ProcessorCount - 1;
        private int bitsCount;

        /// <summary>
        /// Gets or sets a value indicating whether is use parallel mode for generating prime or not.
        /// Default value is <see langword="true"/>.
        /// </summary>
        public bool IsParallelMode
        { get; set; } = true;

        /// <inheritdoc/>
        public BigInteger GenerateProbablyPrime(Bits bitsCount)
        {
            this.bitsCount = (int)bitsCount;
            return (this.IsParallelMode == true) ? this.GeneratePrimeParallel() : this.GeneratePrimeSingle();
        }

        /// <summary>
        /// Decomposites the (number-1) as (2^degree)*factor.
        /// </summary>
        /// <param name="number">Odd number.</param>
        /// <returns>Returns tuple where first item is degree, second is factor.</returns>
        private static Tuple<int, BigInteger> DecompositeNumber(BigInteger number)
        {
            int degree = 0;
            BigInteger factor = number - 1;
            while (factor % 2 == 0)
            {
                factor >>= 1;
                ++degree;
            }

            return new Tuple<int, BigInteger>(degree, factor);
        }

        /// <summary>
        /// Infinity enumeration.
        /// </summary>
        /// <returns>Always true.</returns>
        private static IEnumerable<bool> Infinite()
        {
            while (true)
            {
                yield return true;
            }
        }

        private BigInteger GeneratePrimeSingle()
        {
            BigInteger prime;
            while (true)
            {
                prime = this.GetPositiveRandomNumber();

                if (prime % 2 == 0)
                {
                    prime += 1;
                }

                if (this.IsProbablyPrime(prime))
                {
                    return prime;
                }
            }
        }

        private BigInteger GeneratePrimeParallel()
        {
            BigInteger prime = BigInteger.Zero;
            CancellationTokenSource cts = new ();
            ParallelOptions parallelOptions = new ()
            {
                MaxDegreeOfParallelism = this.maxDegreeOfParallelism,
                CancellationToken = cts.Token,
            };
            var locker = new object();
            try
            {
                Parallel.ForEach(Infinite(), parallelOptions, (i, breakLoopState) =>
                {
                    BigInteger randomNumber = this.GetPositiveRandomNumber();
                    if (randomNumber % 2 == 0)
                    {
                        randomNumber += 1;
                    }

                    if (this.IsProbablyPrime(randomNumber))
                    {
                        lock (locker)
                        {
                            prime = randomNumber;
                            cts.Cancel();
                        }
                    }
                });
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                cts.Dispose();
            }

            return prime;
        }

        private BigInteger GetPositiveRandomNumber()
        {
            BigInteger randomNumber;
            do
            {
                randomNumber = this.GetRandomNumber();
            }
            while (randomNumber == 0);

            return randomNumber < 0 ? -randomNumber : randomNumber;
        }

        private BigInteger GetRandomNumber()
        {
            BigInteger randomNumber;
            while (true)
            {
                RNGCryptoServiceProvider rngCsp = new ();
                byte[] bytes = new byte[this.bitsCount / 8];
                rngCsp.GetBytes(bytes);
                randomNumber = new BigInteger(bytes);
                return randomNumber;
            }
        }

        private bool IsProbablyPrime(BigInteger number)
        {
            bool result;
            if (this.bitsCount > 512)
            {
                result = this.LargeNumberPrimeCheck(number);
            }
            else
            {
                result = this.SmallNumberPrimeCheck(number);
            }

            return result;
        }

        private bool SmallNumberPrimeCheck(BigInteger number)
        {
            bool isPrime;
            if (number > 3)
            {
                isPrime = this.MillerRabinPrimalityTest(number);
            }
            else if (number < 3)
            {
                isPrime = false;
            }
            else
            {
                isPrime = true;
            }

            return isPrime;
        }

        private bool LargeNumberPrimeCheck(BigInteger number)
        {
            bool isPrime;
            if (!IsDividableUpTo11())
            {
                if (!IsDividableUpTo2411())
                {
                    isPrime = this.MillerRabinPrimalityTest(number);
                }
                else
                {
                    isPrime = false;
                }
            }
            else
            {
                isPrime = false;
            }

            return isPrime;

            bool IsDividableUpTo11()
            {
                if ((number % 2) == BigInteger.Zero ||
                    (number % 3) == BigInteger.Zero ||
                    (number % 5) == BigInteger.Zero ||
                    (number % 7) == BigInteger.Zero ||
                    (number % 11) == BigInteger.Zero)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            bool IsDividableUpTo2411()
            {
                int iterationCount = 200;
                for (int i = 1; i <= iterationCount; i++)
                {
                    if (number % ((12 * i) + 1) == 0 ||
                        number % ((12 * i) + 5) == 0 ||
                        number % ((12 * i) + 7) == 0 ||
                        number % ((12 * i) + 11) == 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private bool MillerRabinPrimalityTest(BigInteger number)
        {
            int millerRabinTestRoundsCount = this.CalculateMillerRabinRoundsCount();
            return this.IsParallelMode ?
                this.MillerRabinPrimalityTestParallel(number, millerRabinTestRoundsCount) :
                this.MillerRabinPrimalityTestSingle(number, millerRabinTestRoundsCount);
        }

        private int CalculateMillerRabinRoundsCount()
        {
            const double acceptableErrorProbability = 1E-40;
            int roundsCount;
            for (roundsCount = 3; roundsCount <= this.bitsCount; roundsCount++)
            {
                var currentErrorProbability = Math.Pow(this.bitsCount, 3 / 2)
                                              * Math.Pow(2, roundsCount)
                                              * Math.Pow(roundsCount, -1 / 2)
                                              * Math.Pow(4, 2 - Math.Sqrt(roundsCount * this.bitsCount));
                if (currentErrorProbability <= acceptableErrorProbability)
                {
                    break;
                }
            }

            return roundsCount;
        }

        private bool MillerRabinPrimalityTestSingle(BigInteger number, int roundsCount)
        {
            var decompositionResult = DecompositeNumber(number);
            int degree = decompositionResult.Item1;
            BigInteger factor = decompositionResult.Item2;

            for (int round = 0; round < roundsCount; round++)
            {
                BigInteger randomNumber;
                do
                {
                    randomNumber = this.GetPositiveRandomNumber();
                }
                while (randomNumber < 2 || randomNumber > number - 2);

                BigInteger x = BigInteger.ModPow(randomNumber, factor, number);
                if (x == BigInteger.One || x == number - 1)
                {
                    continue;
                }

                for (int j = 0; j < degree - 1; j++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if (x == BigInteger.One)
                    {
                        return false;
                    }
                    else if (x == number - 1)
                    {
                        break;
                    }
                }

                if (x != number - 1)
                {
                    return false;
                }
            }

            return true;
        }

        private bool MillerRabinPrimalityTestParallel(BigInteger number, int roundsCount)
        {
            var decompositionResult = DecompositeNumber(number);
            int degree = decompositionResult.Item1;
            BigInteger factor = decompositionResult.Item2;
            bool result = true;

            Parallel.For(0, roundsCount, (i, breakLoopState) =>
            {
                BigInteger randomNumber;
                do
                {
                    randomNumber = this.GetPositiveRandomNumber();
                }
                while (randomNumber < 2 || randomNumber > number - 2);

                BigInteger x;
                x = BigInteger.ModPow(randomNumber, factor, number);
                if (x == BigInteger.One || x == number - 1)
                {
                    return;
                }

                for (int j = 0; j < degree - 1; j++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if (x == BigInteger.One)
                    {
                        result = false;
                        breakLoopState.Stop();
                    }
                    else if (x == number - 1)
                    {
                        break;
                    }
                }

                if (x != number - 1)
                {
                    result = false;
                    breakLoopState.Stop();
                }
            });

            return result;
        }
    }
}
