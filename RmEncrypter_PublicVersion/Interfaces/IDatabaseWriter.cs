// <copyright file="IDatabaseWriter.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Represents methods for writing accounts informaton in the database.
    /// </summary>
    public interface IDatabaseWriter
    {
        /// <summary>
        /// Writes data about an user for authentication in the database.
        /// </summary>
        /// <param name="authData">Data about the user for authentication.</param>
        /// <returns>Returns <see cref="MethodResult"/> instance with description of the result of writing user in the database.</returns>
        MethodResult WriteUser(AuthenticationData authData);

        /// <summary>
        /// Writes a Rsa-key of the user in the database.
        /// </summary>
        /// <param name="key">The Rsa-key for writing.</param>
        /// <param name="userName">Username who must be owns this key.</param>
        /// <returns>Returns <see cref="MethodResult"/> instance with description of the result of writing Rsa-key in the database.</returns>
        MethodResult WriteRsaKey(RsaKey key, string userName);

        /// <summary>
        /// Updates the data about an user for authentication in the database.
        /// </summary>
        /// <param name="userName">The username for which the data must be updated.</param>
        /// <param name="authData">Updated data about an user for authentication.</param>
        /// <returns>Returns <see cref="MethodResult"/> instance with description of the result of updating user in the database.</returns>
        MethodResult UpdateUser(string userName, AuthenticationData authData);

        /// <summary>
        /// Updates the Rsa-key of the user in the database.
        /// </summary>
        /// <param name="userName">The username for which the key must be updated.</param>
        /// <param name="key">Updated rsa-key.</param>
        /// <returns>Returns <see cref="MethodResult"/> instance with description of the result of updating Rsa-key in the database.</returns>
        MethodResult UpdateRsaKey(string userName, RsaKey key);

        /// <summary>
        /// Deletes all account data from the database.
        /// </summary>
        /// <param name="userName">The username which account will be deleted.</param>
        /// <returns>Returns <see cref="MethodResult"/> instance with description of the result of deleting user from the database.</returns>
        MethodResult DeleteAccount(string userName);
    }
}
