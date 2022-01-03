// <copyright file="EntityStates.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Specifies the encryption state of an entity.
    /// </summary>
    public enum EntityStates
    {
        /// <summary>
        /// Indicates entity with all encrypted fields.
        /// </summary>
        FullyEncrypted,

        /// <summary>
        /// Indicates entity with decrypted header and encrypted other fields.
        /// </summary>
        HeaderDecrypted,

        /// <summary>
        /// Indicates entity with all decrypted fields.
        /// </summary>
        FullyDecrypted,
    }
}
