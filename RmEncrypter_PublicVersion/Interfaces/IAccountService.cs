// <copyright file="IAccountService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Represents service for interaction with data of the account.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Creates the <see cref="AuthenticationData"/> instance with given parameters.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <param name="userPassword">The password of the user.</param>
        /// <param name="authData"><see cref="AuthenticationData"/> instance for getting variable with given parameters.
        /// if creating was not successful it will be <see langword="null"/>.</param>
        /// <returns>Returns <see cref="MethodResult"/> instance with description of the authentication data creating result.</returns>
        MethodResult CreateAuthData(string userName, string userPassword, out AuthenticationData authData);
    }
}
