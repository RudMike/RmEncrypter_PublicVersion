// <copyright file="IAuthData.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Represents a data about an user for authentication.
    /// </summary>
    public interface IAuthData
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password hash of the user.
        /// </summary>
        string PasswordHash { get; set; }
    }
}
