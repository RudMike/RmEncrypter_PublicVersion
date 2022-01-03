// <copyright file="AccountContext.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Database context for work with <see cref="Models.AuthenticationData"/> and <see cref="Models.RsaKey"/> models.
    /// </summary>
    public class AccountContext : DbContext
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountContext"/> class.
        /// </summary>
        /// <param name="connection">Provider for managing the connection to a database.</param>
        public AccountContext(IConnectionProvider connection)
        {
            this.connectionString = connection.ConnectionString;
        }

        /// <summary>
        /// Gets or sets the database set of the <see cref="Models.AuthenticationData"/> entity.
        /// </summary>
        public DbSet<Models.AuthenticationData> AuthenticationData
        { get; set; }

        /// <summary>
        /// Gets or sets the database set of the <see cref="Models.RsaKey"/> entity.
        /// </summary>
        public DbSet<Models.RsaKey> RsaKey
        { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.ApplyConfiguration(new AuthenticationDataConfiguration());
            _ = modelBuilder.ApplyConfiguration(new RsaKeyConfiguration());
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                _ = optionsBuilder.UseSqlite(this.connectionString);
            }
        }
    }
}
