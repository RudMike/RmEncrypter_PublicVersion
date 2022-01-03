// <copyright file="UserRegistrationViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Threading.Tasks;
    using Prism.Commands;
    using Prism.Regions;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// ViewModel for interaction with a user registration view.
    /// </summary>
    public class UserRegistrationViewModel : ViewModelBase
    {
        private readonly IAccountService accountService;
        private readonly IDatabaseWriter databaseWriter;
        private string userName;
        private string userPassword;
        private string userRepeatedPassword;
        private IRegionNavigationJournal journal;
        private DelegateCommand backCommand;
        private DelegateCommand registerCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRegistrationViewModel"/> class.
        /// </summary>
        /// <param name="accountService">The service for interaction with data of the account.</param>
        /// <param name="databaseWriter">The class for writing in a database.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public UserRegistrationViewModel(IAccountService accountService, IDatabaseWriter databaseWriter, INavigationRepository navigation, IMessageBoxService messageBox)
            : base(navigation, messageBox)
        {
            this.accountService = accountService;
            this.databaseWriter = databaseWriter;
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
        /// Sets the user's password.
        /// </summary>
        public string UserPassword
        {
            set => this.SetProperty(ref this.userPassword, value);
        }

        /// <summary>
        /// Sets the user's repeated password.
        /// </summary>
        public string UserRepeatedPassword
        {
            set => this.SetProperty(ref this.userRepeatedPassword, value);
        }

        /// <summary>
        /// Gets the command which go to previous view in the navigation journal.
        /// </summary>
        public DelegateCommand BackCommand
        {
            get => this.backCommand ??= new DelegateCommand(this.Back);
        }

        /// <summary>
        /// Gets the command which register an user in the database.
        /// </summary>
        public DelegateCommand RegisterCommand
        {
            get => this.registerCommand ??= new DelegateCommand(async () => await Task.Run(() => this.Register()));
        }

        /// <inheritdoc/>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.journal = navigationContext.NavigationService.Journal;
        }

        private void Back()
        {
            this.journal.GoBack();
        }

        private void Register()
        {
            if (!this.IsPasswordsEqual())
            {
                return;
            }

            var account = this.CreateAuthData();
            if (account != null)
            {
                this.IsBusy = true;
                var writeResult = this.databaseWriter.WriteUser(account);
                if (writeResult.IsSuccessful)
                {
                    this.Navigation.ToKeyRegistration(account.UserName, true);
                }
                else
                {
                    _ = this.MessageBox.Show(writeResult.ExceptionMessage, LocalizationService.GetString("Error"));
                }

                this.IsBusy = false;
            }
        }

        private bool IsPasswordsEqual()
        {
            bool isEquals;
            if (this.userPassword == this.userRepeatedPassword)
            {
                isEquals = true;
            }
            else
            {
                _ = this.MessageBox.Show(LocalizationService.GetString("PasswordsDontMatch"), LocalizationService.GetString("Error"));
                isEquals = false;
            }

            return isEquals;
        }

        private AuthenticationData CreateAuthData()
        {
            var createResult = this.accountService.CreateAuthData(this.userName, this.userPassword, out AuthenticationData authData);

            if (createResult.IsSuccessful)
            {
                return authData;
            }
            else
            {
                _ = this.MessageBox.Show(createResult.ExceptionMessage, LocalizationService.GetString("Error"));
                return null;
            }
        }
    }
}