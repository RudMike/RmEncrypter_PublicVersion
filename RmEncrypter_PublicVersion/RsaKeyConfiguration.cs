// <copyright file="RsaKeyConfiguration.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides configuration for the <see cref="RsaKey"/> model.
    /// </summary>
    public class RsaKeyConfiguration : IEntityTypeConfiguration<RsaKey>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<RsaKey> builder)
        {
            SetPrimaryKey(builder);
            SetIgnorableProperties(builder);
            SetIndexes(builder);
            ConfigureProperties(builder);
        }

        private static void SetPrimaryKey(EntityTypeBuilder<RsaKey> builder)
        {
            _ = builder.HasKey(prop => prop.Id);
        }

        private static void SetIgnorableProperties(EntityTypeBuilder<RsaKey> builder)
        {
            _ = builder.Ignore(prop => prop.Errors);
        }

        private static void SetIndexes(EntityTypeBuilder<RsaKey> builder)
        {
            _ = builder.HasIndex(prop => prop.Id);
        }

        private static void ConfigureProperties(EntityTypeBuilder<RsaKey> builder)
        {
            _ = builder.Property(rsaKey => rsaKey.PrivateExponent)
                .IsRequired(true)
                .HasColumnType("nvarchar");

            _ = builder.Property(rsaKey => rsaKey.PublicExponent)
                       .IsRequired(true)
                       .HasColumnType("nvarchar");

            _ = builder.Property(rsaKey => rsaKey.MultiplyResult)
                       .IsRequired(true)
                       .HasColumnType("nvarchar");
        }
    }
}
