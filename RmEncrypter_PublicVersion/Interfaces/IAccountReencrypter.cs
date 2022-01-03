// <copyright file="IAccountReencrypter.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using RmEncrypter_PublicVersion.Exceptions;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides functionality for updating <see cref="RsaKey"/> of an account and reencrypting it files.
    /// </summary>
    public interface IAccountReencrypter
    {
        /// <summary>
        /// Occurs when <see cref="StageDescription"/> was changed.
        /// </summary>
        event EventHandler OnStageDescriptionChanged;

        /// <summary>
        /// Gets the description of a current updating stage.
        /// </summary>
        string StageDescription { get; }

        /// <summary>
        /// Gets or sets the key that will be used for decrypting files.
        /// </summary>
        RsaKey OldKey { get; set; }

        /// <summary>
        /// Gets or sets the user name of the account for which will be updated key.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Updates Rsa-key of the account in the database and reencrypts account's files with the new Rsa-key.
        /// </summary>
        /// <param name="newKey">A new Rsa-key which will be written in the database.</param>
        /// <param name="filePaths">The file paths for reencrypting with the new Rsa-key.</param>
        /// <returns>A task that represents the asynchronous updates operation.</returns>
        /// <exception cref="DecryptException">Throws if it can't decrypts files with the <see cref="OldKey"/>.</exception>
        /// <exception cref="FileLoadException">Throws if any file from the list is corrupted and can't be read.</exception>
        Task UpdateAsync(RsaKey newKey, IEnumerable<string> filePaths);
    }
}
