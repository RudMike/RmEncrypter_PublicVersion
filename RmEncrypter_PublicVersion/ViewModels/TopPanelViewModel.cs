// <copyright file="TopPanelViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System;
    using System.Windows;
    using Prism.Commands;

    /// <summary>
    /// ViewModel for interaction with a top panel view.
    /// </summary>
    public class TopPanelViewModel : ViewModelBase
    {
        private string userName;
        private bool requestClose = false;
        private DelegateCommand logoutCommand;
        private DelegateCommand changeLanguageCommand;
        private DelegateCommand changePasswordCommand;
        private DelegateCommand changeKeyCommand;
        private DelegateCommand accountTransferCommand;
        private DelegateCommand accountDeleteCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopPanelViewModel"/> class.
        /// </summary>
        /// <param name="authorizedUser">The class which contains the current authorized user data.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public TopPanelViewModel(IAuthorizedUser authorizedUser, INavigationRepository navigation, IMessageBoxService messageBox)
            : base(navigation, messageBox)
        {
            this.UserName = authorizedUser.UserName;
        }

        /// <summary>
        /// Gets the delegate which uses to close current shell without confirming.
        /// </summary>
        public static Func<bool> CanClose => () => true;

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string UserName
        {
            get => this.userName;
            private set => this.SetProperty(ref this.userName, value);
        }

        /// <summary>
        /// Gets a value indicating whether the current window should be closed.
        /// </summary>
        public bool RequestClose
        {
            get => this.requestClose;
            private set => this.SetProperty(ref this.requestClose, value);
        }

        /// <summary>
        /// Gets the command for log out from the account.
        /// </summary>
        public DelegateCommand LogoutCommand
        {
            get => this.logoutCommand ??= new DelegateCommand(this.Logout);
        }

        /// <summary>
        /// Gets the command for showing the view with changing language functionality.
        /// </summary>
        public DelegateCommand ChangeLanguageCommand
        {
            get => this.changeLanguageCommand ??= new DelegateCommand(this.Navigation.ToChangeLanguage);
         }

        /// <summary>
        /// Gets the command for showing the view with changing password functionality.
        /// </summary>
        public DelegateCommand ChangePasswordCommand
        {
            get => this.changePasswordCommand ??= new DelegateCommand(this.Navigation.ToChangePassword);
        }

        /// <summary>
        /// Gets the command for showing the view that changes the Rsa-key.
        /// </summary>
        public DelegateCommand ChangeKeyCommand
        {
            get => this.changeKeyCommand ??= new DelegateCommand(this.ChangeKey);
        }

        /// <summary>
        /// Gets the command for showing the view, which serves to transfer the account.
        /// </summary>
        public DelegateCommand AccountTransferCommand
        {
            get => this.accountTransferCommand ??= new DelegateCommand(this.AccountTransfer);
        }

        /// <summary>
        /// Gets the command for showing the view that delete the account.
        /// </summary>
        public DelegateCommand AccountDeleteCommand
        {
            get => this.accountDeleteCommand ??= new DelegateCommand(this.AccountDelete);
        }

        private void Logout()
        {
            var result = this.MessageBox.Show(
                LocalizationService.GetString("LogoutMessage"),
                LocalizationService.GetString("Logout"),
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                this.Navigation.ToAuthorization();
                this.RequestClose = true;
            }
        }

        private void ChangeKey()
        {
            var dialogResult = this.MessageBox.Show(
                LocalizationService.GetString("NewKeyGenerationMessage"),
                LocalizationService.GetString("Warning"),
                MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                this.Navigation.ToKeyRegistration(string.Empty, false);
                this.RequestClose = true;
            }
        }

        private void AccountTransfer()
        {
            var result = this.MessageBox.Show(
                LocalizationService.GetString("AccountTransferMessage"),
                LocalizationService.GetString("Information"),
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                this.Navigation.ToAccountTransfer();
            }
        }

        private void AccountDelete()
        {
            var result = this.MessageBox.Show(
                LocalizationService.GetString("AccountDeleteMessage"),
                LocalizationService.GetString("Warning"),
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                this.Navigation.ToAccountDelete();
                this.RequestClose = true;
            }
        }
    }
}
