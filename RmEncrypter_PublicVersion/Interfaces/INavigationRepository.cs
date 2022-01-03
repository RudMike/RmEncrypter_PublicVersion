// <copyright file="INavigationRepository.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides functionality for navigation between views. The view can opens in a dialog window or a new shell.
    /// Some from the methods can request password confirming before open the requested view.
    /// </summary>
    /// <exception cref="InvalidOperationException">Throws if navigation was required in a main shell region, but main shell was not opened.</exception>
    public interface INavigationRepository : IRegionManagerAware
    {
        /// <summary>
        /// Show main shell with the neccesary views.
        /// </summary>
        void ShowMainShell();

        /// <summary>
        /// Navigate to a view specified in navigation info and pass the parameters in.
        /// </summary>
        /// <param name="navigationInfo">Information about the opening view.</param>
        void ToView(NavigationInfo navigationInfo);

        /// <summary>
        /// Navigate to a view to delete the account of a current user.
        /// </summary>
        void ToAccountDelete();

        /// <summary>
        /// Navigate to a view to recovery an account.
        /// </summary>
        /// <param name="recoveryFile">The path to a recovery file.</param>
        void ToAccountRecovery(string recoveryFile);

        /// <summary>
        /// Navigate to a view to transfer the account of a current user.
        /// </summary>
        void ToAccountTransfer();

        /// <summary>
        /// Navigate to a view to add a new record in an user's file.
        /// </summary>
        /// <returns>Returns the added value with <see cref="EntityStates.FullyDecrypted"/> state.
        /// Can return <see langword="null"/> if added nothing.</returns>
        SiteAuthData ToAddRecord();

        /// <summary>
        /// Navigate to an authorization view.
        /// </summary>
        void ToAuthorization();

        /// <summary>
        /// Navigate to a bottom view.
        /// </summary>
        void ToBottom();

        /// <summary>
        /// Navigate to a view to change the current program language.
        /// </summary>
        void ToChangeLanguage();

        /// <summary>
        /// Navigate to a view to change the account password for a current user.
        /// </summary>
        void ToChangePassword();

        /// <summary>
        /// Navigate to a view to confirm the password of a current user.
        /// </summary>
        /// <param name="nextView">The name of a view which must be opened if the password will be confirmed.</param>
        /// <param name="isDialog">Indicates whether to open the new dialog shell if necessary in dialog mode or not.</param>
        void ToConfirmAccountPassword(NavigationInfo nextView, bool isDialog = true);

        /// <summary>
        /// Navigate to a view with main content.
        /// </summary>
        /// <param name="filePath">The path to a file with saved user's information about the sites.</param>
        void ToContentPanel(string filePath);

        /// <summary>
        /// Navigate to a view to register a new key.
        /// </summary>
        /// <param name="userName">An username for which the key should be saved.</param>
        /// <param name="isSaveKey">Indicates whether the view needs to save the key or pass it to another view.</param>
        void ToKeyRegistration(string userName, bool isSaveKey);

        /// <summary>
        /// Navigate to a view to open an user's file.
        /// </summary>
        void ToOpenFile();

        /// <summary>
        /// Navigate to a view to select files for reencrypting files of a current user.
        /// </summary>
        /// <param name="filePaths">An enumerable with file paths, which will be reencrypted and resaved.</param>
        /// <param name="key">A new Rsa-key to be used for encryption, which will be updated in the database instead of the old one.</param>
        void ToReEncryptFiles(IEnumerable<string> filePaths, RsaKey key);

        /// <summary>
        /// Navigate to a view to select files for reencrypting.
        /// </summary>
        /// <param name="key">A new Rsa-key to be used for encryption, which will be updated in the database instead of the old one.</param>
        void ToSelectFiles(RsaKey key);

        /// <summary>
        /// Navigate to a view to select a recovery file.
        /// </summary>
        void ToSelectRecoveryFile();

        /// <summary>
        /// Navigate to a view to show and edit an user's file.
        /// </summary>
        /// <param name="entity">An entity to show or edit.</param>
        /// <param name="state">A state of the passed entity.</param>
        /// <returns>Returns the tuple where the first element is entity with <see cref="EntityStates.FullyDecrypted"/> state, which was passed in.
        /// The second element indicates whether the entity was changed. If the entity was only decrypted the value will be <see langword="false"/>.
        /// Otherwise <see langword="true"/>. Returns <see langword="null"/> if entity was removed.</returns>
        (SiteAuthData resultEntity, bool isChanged) ToShowEditRecord(SiteAuthData entity, EntityStates state);

        /// <summary>
        /// Navigate to a view which shows informaton about the user and program settings.
        /// </summary>
        void ToTopPanel();

        /// <summary>
        /// Navigate to a view to register a new user.
        /// </summary>
        void ToUserRegistration();
    }
}