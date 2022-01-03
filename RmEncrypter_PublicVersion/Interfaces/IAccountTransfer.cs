// <copyright file="IAccountTransfer.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.IO;
    using RmEncrypter_PublicVersion.Exceptions;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides functionality for account's transferring.
    /// </summary>
    public interface IAccountTransfer
    {
        /// <summary>
        /// Gets authentication data of the account which is stored in the file.
        /// </summary>
        /// <param name="path">Path to the file where the transfer data is stored.</param>
        /// <returns>Returns the authentication data stored in the transfer file.</returns>
        /// <exception cref="FileLoadException">Throws if the authentication data can't be read from file because the file is corrupted.</exception>
        AuthenticationData GetAuthData(string path);

        /// <summary>
        /// Transfers the account data to a file.
        /// </summary>
        /// <param name="userName">The username of the account for transferring.</param>
        /// <param name="path">Path to the file where the data will be written to.</param>
        void Transfer(string userName, string path);

        /// <summary>
        /// Recoveries account on the device.
        /// </summary>
        /// <param name="path">Path to the file with transferring data.</param>
        /// <param name="authData">Data about user that will be written on the new device.
        /// If it is <see langword="null"/> recovered all data from the file.
        /// Otherwise passed parameter writes in the database and the key from the file joins him.</param>
        /// It can be useful when the database on the new device already has account with same username.
        /// <exception cref="AccountTransferException">Throws if there are problems with account transfer.</exception>
        /// <exception cref="FileLoadException"> Throws if the authentication data can't be read from file because the file is corrupted.</exception>
        void Recovery(string path, AuthenticationData authData = null);
    }
}
