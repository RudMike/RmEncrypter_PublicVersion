// <copyright file="IDbAuthData.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Represents the data about an user for authentication, which stored in a database.
    /// Inherits the <see cref="IAuthData"/> interface.
    /// </summary>
    public interface IDbAuthData : IAuthData, IValidatable
    {
        /// <summary>
        /// Gets or sets the unique key of the record about an user.
        /// </summary>
        int Id { get; set; }
    }
}
