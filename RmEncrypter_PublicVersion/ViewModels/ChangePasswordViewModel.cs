// <copyright file="ChangePasswordViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Threading.Tasks;
    using Prism.Commands;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// ViewModel for interaction with a changing password view.
    /// </summary>
    public class ChangePasswordViewModel : ViewModelBase
    {
        private readonly IAccountService accountService;
        private readonly IAuthorizedUser authorizedUser;
        private readonly IDatabaseReader databaseReader;
        private readonly IDatabaseWriter databaseWriter;
        private string oldPassword;
        private string newPassword;
        private string repeatedNewPassword;
        private bool requestClose = false;
        private DelegateCommand changePasswordCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordViewModel"/> class.
        /// </summary>
        /// <param name="accountService">The service for interaction with data of the account.</param>
        /// <param name="authorizedUser">The class which contains an authorized user data.</param>
        /// <param name="databaseReader">The class for reading from a database.</param>
        /// <param name="databaseWriter">The class for writing in a database.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public ChangePasswordViewModel(IAccountService accountService, IAuthorizedUser authorizedUser, IDatabaseReader databaseReader, IDatabaseWriter databaseWriter, IMessageBoxService messageBox)
            : base(messageBox)
        {
            this.accountService = accountService;
            this.authorizedUser = authorizedUser;
            this.databaseReader = databaseReader;
            this.databaseWriter = databaseWriter;
        }

        /// <summary>
        /// Sets the old password of the current account.
        /// </summary>
        public string OldPassword
        {
            private get => this.oldPassword;
            set => this.SetProperty(ref this.oldPassword, value);
        }

        /// <summary>
        /// Sets the new password of the current account.
        /// </summary>
        public string NewPassword
        {
            private get => this.newPassword;
            set => this.SetProperty(ref this.newPassword, value);
        }

        /// <summary>
        /// Sets the repeated password of the current account.
        /// </summary>
        public string RepeatedNewPassword
        {
            private get => this.repeatedNewPassword;
            set => this.SetProperty(ref this.repeatedNewPassword, value);
        }

        /// <summary>
        /// Gets a value indicating whether the shell has been requested to close.
        /// </summary>
        public bool RequestClose
        {
            get => this.requestClose;
            private set => this.SetProperty(ref this.requestClose, value);
        }

        /// <summary>
        /// Gets the command for changing password of the current account.
        /// </summary>
        public DelegateCommand ChangePasswordCommand
        {
            get => this.changePasswordCommand ??= new DelegateCommand(async () => await Task.Run(() => this.ChangePassword()));
        }

        private void ChangePassword()
        {
            this.IsBusy = true;
            if (!this.IsCorrectPasswords())
            {
                this.IsBusy = false;
                return;
            }

            var newAuthData = this.CreateAuthData();
            if (newAuthData != null)
            {
                var updateResult = this.databaseWriter.UpdateUser(this.authorizedUser.UserName, newAuthData);

                if (updateResult.IsSuccessful)
                {
                    _ = this.MessageBox.Show(
                        LocalizationService.GetString("PasswordWasChanged"),
                        LocalizationService.GetString("Success"));
                    this.RequestClose = true;
                }
                else
                {
                    _ = this.MessageBox.Show(updateResult.ExceptionMessage, LocalizationService.GetString("Error"));
                }
            }

            this.IsBusy = false;
        }

        private bool IsCorrectPasswords()
        {
            bool result = true;
            if (this.NewPassword != this.RepeatedNewPassword)
            {
                _ = this.MessageBox.Show(
                    LocalizationService.GetString("RepeatedPasswordDoesntMatch"),
                    LocalizationService.GetString("Error"));
                result = false;
            }
            else if (this.OldPassword == this.NewPassword)
            {
                _ = this.MessageBox.Show(
                    LocalizationService.GetString("PasswordsOldNewEquals"),
                    LocalizationService.GetString("Error"));
                result = false;
            }
            else if (!this.IsPasswordHashEquals())
            {
                _ = this.MessageBox.Show(
                    LocalizationService.GetString("PasswordNotMatchCurrent"),
                    LocalizationService.GetString("Error"));
                result = false;
            }

            return result;
        }

        private bool IsPasswordHashEquals()
        {
            bool isEquals = true;
            _ = this.accountService.CreateAuthData(
                        this.authorizedUser.UserName,
                        this.OldPassword,
                        out AuthenticationData oldAuthData);

            var dbAuthData = this.databaseReader.ReadAuthData(this.authorizedUser.UserName);

            if (oldAuthData == null || dbAuthData.PasswordHash != oldAuthData.PasswordHash)
            {
                isEquals = false;
            }

            return isEquals;
        }

        private AuthenticationData CreateAuthData()
        {
            var createResult = this.accountService.CreateAuthData(
                    this.authorizedUser.UserName,
                    this.NewPassword,
                    out AuthenticationData authData);

            if (!createResult.IsSuccessful)
            {
                _ = this.MessageBox.Show(createResult.ExceptionMessage, LocalizationService.GetString("Error"));
            }

            return authData;
        }
    }
}