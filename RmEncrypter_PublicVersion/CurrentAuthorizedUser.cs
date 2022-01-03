// <copyright file="CurrentAuthorizedUser.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// The class which contains information about authorized user.
    /// </summary>
    public class CurrentAuthorizedUser : IAuthorizedUser
    {
        /// <inheritdoc/>
        public string UserName
        { get; set; }

        /// <inheritdoc/>
        public IRsaKey RsaKey
        { get; set; }
    }
}
