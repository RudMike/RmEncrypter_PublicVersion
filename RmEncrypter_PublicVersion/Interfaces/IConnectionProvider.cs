// <copyright file="IConnectionProvider.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides options for managing the connection to a database.
    /// </summary>
    public interface IConnectionProvider
    {
        /// <summary>
        /// Gets the string for connection to the database.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the path to the database file. Can be null if the database has no local file.
        /// </summary>
        string FilePath { get; }
    }
}
