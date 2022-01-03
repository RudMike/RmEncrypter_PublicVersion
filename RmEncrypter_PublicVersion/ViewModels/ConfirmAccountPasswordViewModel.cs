// <copyright file="ConfirmAccountPasswordViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows;
    using Prism.Commands;
    using Prism.Regions;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// ViewModel for interaction with a confirming account password view.
    /// </summary>
    public class ConfirmAccountPasswordViewModel : ViewModelBase
    {
        private readonly IAccountService accountService;
        private readonly IDatabaseReader databaseReader;
        private readonly IAuthorizedUser authorizedUser;
        private string password;
        private NavigationInfo nextViewInfo;
        private DelegateCommand confirmPasswordCommand;
        private DelegateCommand cancelCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmAccountPasswordViewModel"/> class.
        /// </summary>
        /// <param name="accountService">The service for interaction with data of the account.</param>
        /// <param name="databaseReader">The class for reading from a database.</param>
        /// <param name="authorizedUser">The class which contains an authorized user data.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public ConfirmAccountPasswordViewModel(IAccountService accountService, IDatabaseReader databaseReader, IAuthorizedUser authorizedUser, INavigationRepository navigation, IMessageBoxService messageBox)
            : base(navigation, messageBox)
        {
            this.accountService = accountService;
            this.databaseReader = databaseReader;
            this.authorizedUser = authorizedUser;
        }

        /// <summary>
        /// Sets the password of the current account.
        /// </summary>
        public string Password
        {
            private get => this.password;
            set => this.SetProperty(ref this.password, value);
        }

        /// <summary>
        /// Gets the command for confirming account password.
        /// </summary>
        public DelegateCommand ConfirmPasswordCommand
        {
            get => this.confirmPasswordCommand ??= new DelegateCommand(async () => await Task.Run(() => this.ConfirmPassword()));
        }

        /// <summary>
        /// Gets the command to cancel the account confirmation and navigate to the authorization view .
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get => this.cancelCommand ??= new DelegateCommand(this.Cancel);
        }

        /// <inheritdoc/>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.nextViewInfo = (NavigationInfo)navigationContext.Parameters[NavigationParameterKeys.NextViewInfo];
        }

        private void ConfirmPassword()
        {
            this.IsBusy = true;
            var dbAuthData = this.databaseReader.ReadAuthData(this.authorizedUser.UserName);
            var creatingResult = this.accountService.CreateAuthData(this.authorizedUser.UserName, this.Password, out AuthenticationData checkingData);
            this.IsBusy = false;

            if (creatingResult.IsSuccessful && dbAuthData.PasswordHash == checkingData.PasswordHash)
            {
                this.Navigation.ToView(this.nextViewInfo);
            }
            else
            {
                _ = this.MessageBox.Show(LocalizationService.GetString("PasswordsDontMatch"), LocalizationService.GetString("Error"));
            }
        }

        private void Cancel()
        {
            var dialogResult = this.MessageBox.Show(
                LocalizationService.GetString("AreYouSure") + " " + LocalizationService.GetString("RedirectToAuthorization"),
                LocalizationService.GetString("Warning"),
                MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                this.Navigation.ToAuthorization();
            }
        }
    }
}