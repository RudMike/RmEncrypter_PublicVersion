// <copyright file="IAuthorizedUser.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides information about current authorized user.
    /// </summary>
    public interface IAuthorizedUser
    {
        /// <summary>
        /// Gets the username of the authorized user.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Gets the <see cref="IRsaKey"/> of the authorized user.
        /// </summary>
        IRsaKey RsaKey { get; }
    }
}
