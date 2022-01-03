// <copyright file="AuthenticationDataConfiguration.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides configuration for the <see cref="AuthenticationData"/> model.
    /// </summary>
    public class AuthenticationDataConfiguration : IEntityTypeConfiguration<AuthenticationData>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<AuthenticationData> builder)
        {
            SetPrimaryKey(builder);
            SetIgnorableProperties(builder);
            SetRelationshipWithRsaKey(builder);
            SetIndexes(builder);
            ConfigureProperties(builder);
        }

        private static void SetPrimaryKey(EntityTypeBuilder<AuthenticationData> builder)
        {
            _ = builder.HasKey(prop => prop.Id);
        }

        private static void SetIgnorableProperties(EntityTypeBuilder<AuthenticationData> builder)
        {
            _ = builder.Ignore(prop => prop.Errors);
        }

        private static void SetRelationshipWithRsaKey(EntityTypeBuilder<AuthenticationData> builder)
        {
            _ = builder.HasOne(authData => authData.RsaKey)
                .WithOne(rsaKey => rsaKey.AuthData)
                .HasForeignKey<RsaKey>(rsaKey => rsaKey.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void SetIndexes(EntityTypeBuilder<AuthenticationData> builder)
        {
            _ = builder.HasIndex(prop => prop.UserName);
        }

        private static void ConfigureProperties(EntityTypeBuilder<AuthenticationData> builder)
        {
            _ = builder.Property(prop => prop.UserName)
                .HasColumnType("nvarchar")
                .HasMaxLength(20)
                .IsRequired(true);

            _ = builder.Property(prop => prop.PasswordHash)
                .HasColumnType("nvarchar")
                .IsRequired(true);
        }
    }
}
