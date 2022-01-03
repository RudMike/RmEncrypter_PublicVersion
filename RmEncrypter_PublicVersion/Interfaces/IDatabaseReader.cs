// <copyright file="IDatabaseReader.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Represents methods for reading accounts informaton in the database.
    /// </summary>
    public interface IDatabaseReader
    {
        /// <summary>
        /// Reads the data about an user for authentication and his Rsa-key from the database.
        /// </summary>
        /// <param name="userName">The username for which data will be reading.</param>
        /// <returns>Returns the tuple where first element is <see cref="IDbAuthData"/>,
        /// second element is <see cref="IDbRsaKey"/> by the username.
        /// Any element can be <see langword="null"/> if it is not exist.</returns>
        /// <exception cref="System.InvalidOperationException">Throws if the database has more than one user with same user name.</exception>
        (IDbAuthData, IDbRsaKey) ReadAll(string userName);

        /// <summary>
        /// Reads the data about an user for authentication from the database.
        /// </summary>
        /// <param name="userName">The username for which data will be reading.</param>
        /// <returns>Returns <see cref="IDbAuthData"/> by the username. Returns <see langword="null"/> if user is not exist.</returns>
        /// <exception cref="System.InvalidOperationException">Throws if the database has more than one user with same user name.</exception>
        IDbAuthData ReadAuthData(string userName);

        /// <summary>
        /// Reads the Rsa-key of the user from the database.
        /// </summary>
        /// <param name="userName">The username for which data will be reading.</param>
        /// <returns>Returns <see cref="IDbRsaKey"/> by the username. Returns <see langword="null"/> if key is not exist.</returns>
        /// <exception cref="System.InvalidOperationException">Throws if the database has more than one user with same user name.</exception>
        IDbRsaKey ReadRsaKey(string userName);
    }
}
