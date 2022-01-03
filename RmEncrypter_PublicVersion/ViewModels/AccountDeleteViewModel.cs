// <copyright file="AccountDeleteViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;
    using Prism.Commands;

    /// <summary>
    /// ViewModel for interaction with an account deleting view.
    /// </summary>
    public class AccountDeleteViewModel : ViewModelBase
    {
        private readonly IDatabaseWriter databaseWriter;
        private readonly IAuthorizedUser authorizedUser;
        private readonly string correctConfirmWord;
        private string confirmWord = string.Empty;
        private DelegateCommand continueCommand;
        private DelegateCommand cancelCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountDeleteViewModel"/> class.
        /// </summary>
        /// <param name="databaseWriter">A class for writing in a database.</param>
        /// <param name="authorizedUser">A class which contains the current authorized user data.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public AccountDeleteViewModel(IDatabaseWriter databaseWriter, IAuthorizedUser authorizedUser, INavigationRepository navigation, IMessageBoxService messageBox)
            : base(navigation, messageBox)
        {
            this.databaseWriter = databaseWriter;
            this.authorizedUser = authorizedUser;
            this.correctConfirmWord = LocalizationService.GetString("Confirm").ToLower();
        }

        /// <summary>
        /// Sets the "Confirm" word on the application language.
        /// </summary>
        public string ConfirmWord
        {
            private get => this.confirmWord;
            set => this.SetProperty(ref this.confirmWord, value);
        }

        /// <summary>
        /// Gets the command to delete the account and navigates to the authorization view.
        /// </summary>
        public DelegateCommand ContinueCommand
        {
            get => this.continueCommand ??= new DelegateCommand(async () => await Task.Run(this.Continue), this.CanContinue)
                .ObservesProperty(() => this.ConfirmWord);
        }

        /// <summary>
        /// Gets the command to cancel the account deleting.
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get => this.cancelCommand ??= new DelegateCommand(this.Navigation.ToAuthorization);
        }

        private void Continue()
        {
            var dialogResult = this.MessageBox.Show(
                LocalizationService.GetString("AccountDeleteConfirm"),
                LocalizationService.GetString("Warning"),
                MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                this.IsBusy = true;
                _ = this.databaseWriter.DeleteAccount(this.authorizedUser.UserName);
                this.IsBusy = false;
                _ = this.MessageBox.Show(
                    LocalizationService.GetString("AccountDeleted"),
                    LocalizationService.GetString("Success"));
                this.Navigation.ToAuthorization();
            }
        }

        private bool CanContinue()
        {
            return this.ConfirmWord.ToLower() == this.correctConfirmWord;
        }
    }
}
