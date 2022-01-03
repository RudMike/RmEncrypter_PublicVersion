// <copyright file="AuthorizationViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Threading.Tasks;
    using Prism.Commands;
    using Prism.Regions;
    using RmEncrypter_PublicVersion.Exceptions;

    /// <summary>
    /// ViewModel for interaction with an authorization view.
    /// </summary>
    public class AuthorizationViewModel : ViewModelBase
    {
        private readonly ILoginService loginService;
        private string userName;
        private string userPassword;
        private bool requestClose = false;
        private DelegateCommand loginCommand;
        private DelegateCommand navigateToChangeLanguageCommand;
        private DelegateCommand navigateToUserRegistrationCommand;
        private DelegateCommand navigateToAccountRecoveryCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationViewModel"/> class.
        /// </summary>
        /// <param name="loginService">Current provider of a service for log in to the program.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public AuthorizationViewModel(ILoginService loginService, INavigationRepository navigation, IMessageBoxService messageBox)
            : base(navigation, messageBox)
        {
            this.loginService = loginService;
        }

        /// <summary>
        /// Sets an username.
        /// </summary>
        public string UserName
        {
            private get => this.userName;
            set => this.SetProperty(ref this.userName, value);
        }

        /// <summary>
        /// Sets an user's password.
        /// </summary>
        public string UserPassword
        {
            private get => this.userPassword;
            set => this.SetProperty(ref this.userPassword, value);
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
        /// Gets the command which do login in the program.
        /// </summary>
        public DelegateCommand LoginCommand
        {
            get => this.loginCommand ??= new DelegateCommand(async () => await Task.Run(() => this.Login()));
        }

        /// <summary>
        /// Gets the command for navigation to the change language view.
        /// </summary>
        public DelegateCommand NavigateToChangeLanguageCommand
        {
            get => this.navigateToChangeLanguageCommand ??= new DelegateCommand(this.Navigation.ToChangeLanguage);
        }

        /// <summary>
        /// Gets the command for navigation to the user registration view.
        /// </summary>
        public DelegateCommand NavigateToUserRegistrationCommand
        {
            get => this.navigateToUserRegistrationCommand ??= new DelegateCommand(this.Navigation.ToUserRegistration);
        }

        /// <summary>
        /// Gets the command for navigation to the account recovery view.
        /// </summary>
        public DelegateCommand NavigateToAccountRecoveryCommand
        {
            get => this.navigateToAccountRecoveryCommand ??= new DelegateCommand(this.Navigation.ToSelectRecoveryFile);
        }

        /// <inheritdoc/>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.loginService.Logout();
        }

        private void Login()
        {
            this.IsBusy = true;
            MethodResult loginResult;
            try
            {
                loginResult = this.loginService.Login(this.UserName, this.UserPassword);
            }
            catch (EntityNotExistException ex)
            {
                this.IsBusy = false;
                string dialogMessage = string.Concat(
                    ex.Message,
                    "\r\n\r",
                    LocalizationService.GetString("RedirectToKeyRegistration"));

                _ = this.MessageBox.Show(dialogMessage, LocalizationService.GetString("Error"));
                this.Navigation.ToKeyRegistration(this.UserName, true);
                return;
            }

            if (loginResult.IsSuccessful)
            {
                this.Navigation.ShowMainShell();
                this.RequestClose = true;
            }
            else
            {
                this.IsBusy = false;
                _ = this.MessageBox.Show(loginResult.ExceptionMessage, LocalizationService.GetString("Error"));
            }
        }
    }
}
