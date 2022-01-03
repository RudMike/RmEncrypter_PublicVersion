// <copyright file="SqlLiteDatabaseConnection.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using Microsoft.Data.Sqlite;

    /// <summary>
    /// Provides the string to connection to the SQLite database.
    /// This class was modified for the public version.
    /// </summary>
    public class SqlLiteDatabaseConnection : IConnectionProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLiteDatabaseConnection"/> class.
        /// </summary>
        public SqlLiteDatabaseConnection()
        {
            this.BuildConnectionString();
        }

        /// <inheritdoc/>
        public string ConnectionString
        { get; private set; }

        /// <inheritdoc/>
        public string FilePath
        { get; private set; }

        private void BuildConnectionString()
        {
            string localAppDataPath = Environment.GetEnvironmentVariable("TEMP");
            this.FilePath = localAppDataPath + "\\RmPublic.db";
            var sb = new SqliteConnectionStringBuilder()
            {
                DataSource = this.FilePath,
                ForeignKeys = true,
            };

            this.ConnectionString = sb.ConnectionString;
        }
    }
}
