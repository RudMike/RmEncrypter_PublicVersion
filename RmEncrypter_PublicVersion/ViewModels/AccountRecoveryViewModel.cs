// <copyright file="AccountRecoveryViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.IO;
    using System.Threading.Tasks;
    using Prism.Commands;
    using Prism.Regions;
    using RmEncrypter_PublicVersion.Exceptions;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// ViewModel for interaction with a recovering account view.
    /// </summary>
    public class AccountRecoveryViewModel : ViewModelBase
    {
        private readonly IAccountService accountService;
        private readonly IAccountTransfer transfer;
        private string selectedFile;
        private string userName = string.Empty;
        private string userPassword = string.Empty;
        private string userRepeatedPassword = string.Empty;
        private bool isUseFileAuthData = true;
        private DelegateCommand recoveryCommand;
        private DelegateCommand cancelCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRecoveryViewModel"/> class.
        /// </summary>
        /// <param name="accountService">The service for interaction with data of the account.</param>
        /// <param name="transfer">Service which provides functionality for work with account transfer.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public AccountRecoveryViewModel(IAccountService accountService, IAccountTransfer transfer, INavigationRepository navigation, IMessageBoxService messageBox)
            : base(navigation, messageBox)
        {
            this.accountService = accountService;
            this.transfer = transfer;
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName
        {
            get => this.userName;
            set => this.SetProperty(ref this.userName, value);
        }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string UserPassword
        {
            get => this.userPassword;
            set => this.SetProperty(ref this.userPassword, value);
        }

        /// <summary>
        /// Sets the user's repeated password.
        /// </summary>
        public string UserRepeatedPassword
        {
            private get => this.userRepeatedPassword;
            set => this.SetProperty(ref this.userRepeatedPassword, value);
        }

        /// <summary>
        /// Gets a value indicating whether the authentication data from the file should be used for recovery.
        /// </summary>
        public bool IsUseFileAuthData
        {
            get => this.isUseFileAuthData;
            private set => this.SetProperty(ref this.isUseFileAuthData, value);
        }

        /// <summary>
        /// Gets the command to recovery the account.
        /// </summary>
        public DelegateCommand RecoveryCommand
        {
            get => this.recoveryCommand ??= new DelegateCommand(async () => await Task.Run(() => this.Recovery()));
        }

        /// <summary>
        /// Gets the command to cancel the account recovering.
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get => this.cancelCommand ??= new DelegateCommand(this.Navigation.ToAuthorization);
        }

        /// <inheritdoc/>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.selectedFile = navigationContext.Parameters.GetValue<string>(NavigationParameterKeys.FilePath);
        }

        private void Recovery()
        {
            if (this.IsUseFileAuthData)
            {
                this.FullRecoveryFromFile();
            }
            else
            {
                this.KeyRecoveryFromFile();
            }
        }

        private void FullRecoveryFromFile()
        {
            var fileAuthData = this.TryGetAuthData();
            if (fileAuthData == null)
            {
                this.Navigation.ToSelectRecoveryFile();
                return;
            }

            this.IsBusy = true;
            var createResult = this.accountService.CreateAuthData(this.UserName, this.UserPassword, out AuthenticationData checkingData);

            if (createResult.IsSuccessful && fileAuthData.Equals(checkingData))
            {
                bool isRecoverySuccessful = this.TryRecovery(null);
                this.IsBusy = false;

                if (isRecoverySuccessful)
                {
                    _ = this.MessageBox.Show(
                        LocalizationService.GetString("RecoverySuccess") + LocalizationService.GetString("RedirectToAuthorization"),
                        LocalizationService.GetString("Success"));
                    this.Navigation.ToAuthorization();
                }
                else
                {
                    this.IsUseFileAuthData = false;
                    this.UserPassword = string.Empty;
                }
            }
            else
            {
                this.IsBusy = false;
                _ = this.MessageBox.Show(LocalizationService.GetString("DataDoesntMatch"), LocalizationService.GetString("Error"));
            }
        }

        private void KeyRecoveryFromFile()
        {
            if (this.UserPassword != this.UserRepeatedPassword)
            {
                _ = this.MessageBox.Show(LocalizationService.GetString("PasswordsDontMatch"), LocalizationService.GetString("Error"));
                return;
            }

            var createResult = this.accountService.CreateAuthData(this.userName, this.userPassword, out AuthenticationData authData);

            if (!createResult.IsSuccessful)
            {
                _ = this.MessageBox.Show(createResult.ExceptionMessage, LocalizationService.GetString("Error"));
                return;
            }

            this.IsBusy = true;
            bool isRecoverySuccessful = this.TryRecovery(authData);

            if (isRecoverySuccessful)
            {
                _ = this.MessageBox.Show(
                    LocalizationService.GetString("RecoverySuccess") + LocalizationService.GetString("RedirectToAuthorization"),
                    LocalizationService.GetString("Success"));
                this.Navigation.ToAuthorization();
            }

            this.IsBusy = false;
        }

        private AuthenticationData TryGetAuthData()
        {
            AuthenticationData authData = null;
            try
            {
                authData = this.transfer.GetAuthData(this.selectedFile);
            }
            catch (FileLoadException ex)
            {
                _ = this.MessageBox.Show(
                    ex.Message,
                    LocalizationService.GetString("Error"));
            }

            return authData;
        }

        private bool TryRecovery(AuthenticationData authData)
        {
            bool result = true;
            try
            {
                this.transfer.Recovery(this.selectedFile, authData);
            }
            catch (AccountTransferException ex)
            {
                result = false;
                _ = this.MessageBox.Show(
                    ex.Message + LocalizationService.GetString("EnterNewAuthData"),
                    LocalizationService.GetString("Error"));
            }
            catch (FileLoadException ex)
            {
                result = false;
                _ = this.MessageBox.Show(
                    ex.Message,
                    LocalizationService.GetString("Error"));
            }

            return result;
        }
    }
}
