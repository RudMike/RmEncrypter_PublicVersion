// <copyright file="AuthenticationData.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides the data for users authentication, stored in a database.
    /// </summary>
    public class AuthenticationData : IDbAuthData, IEquatable<AuthenticationData>
    {
        private readonly IModelValidator<AuthenticationData> validator;
        private string userName;
        private string passwordHash;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationData"/> class.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <param name="passwordHash">The hash of the user's password.</param>
        /// <param name="validator">The validator for validating this model.</param>
        public AuthenticationData(string userName, string passwordHash, IModelValidator<AuthenticationData> validator)
        {
            this.userName = userName;
            this.passwordHash = passwordHash;
            this.validator = validator;
            this.ValidateModel();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationData"/> class.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <param name="passwordHash">The hash of the user's password.</param>
        public AuthenticationData(string userName, string passwordHash)
        {
            this.userName = userName;
            this.passwordHash = passwordHash;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationData"/> class. Don't use this constructor manually.
        /// </summary>
        public AuthenticationData()
        {
        }

        /// <inheritdoc/>
        public int Id
        {
            get; set;
        }

        /// <inheritdoc/>
        public string UserName
        {
            get => this.userName;
            set
            {
                this.userName = value;
                this.ValidateModel();
            }
        }

        /// <inheritdoc/>
        public string PasswordHash
        {
            get => this.passwordHash;
            set
            {
                this.passwordHash = value;
                this.ValidateModel();
            }
        }

        /// <summary>
        /// Gets or sets the navigation property for setting the relationship with the <see cref="Models.RsaKey"/> entity.
        /// </summary>
        public RsaKey RsaKey
        { get; set; }

        /// <summary>
        /// Gets all <see cref="AuthenticationData"/> errors, which can't allowed to save data in the database.
        /// </summary>
        public IDictionary<string, string> Errors
        {
            get; private set;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return this.Equals((AuthenticationData)obj);
        }

        /// <inheritdoc/>
        public bool Equals(AuthenticationData other)
        {
            if ((other == null) || !this.GetType().Equals(other.GetType()))
            {
                return false;
            }
            else if (ReferenceEquals(this, other))
            {
                return true;
            }
            else
            {
                return this.UserName == other.UserName && this.PasswordHash == other.PasswordHash;
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (this.UserName + this.PasswordHash).GetHashCode();
        }

        private void ValidateModel()
        {
            if (this.validator != null)
            {
                this.Errors = this.validator.Validate(this);
            }
        }
    }
}