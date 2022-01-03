// <copyright file="ILoginService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Represents a service for logging and log out from the account.
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Log in to the account.
        /// </summary>
        /// <param name="userName">Username of the current user.</param>
        /// <param name="password">Password of the current user.</param>
        /// <returns>Returns <see cref="MethodResult"/> instance with description of the logging result.</returns>
        /// <exception cref="Exceptions.EntityNotExistException">Throws if the user doesn't have Rsa-key.</exception>
        MethodResult Login(string userName, string password);

        /// <summary>
        /// Log out from the account.
        /// </summary>
        void Logout();
    }
}
