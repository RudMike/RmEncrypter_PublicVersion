// <copyright file="KeyRegistrationViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using Prism.Commands;
    using Prism.Regions;

    /// <summary>
    /// ViewModel for interaction with a Rsa-key registration view.
    /// </summary>
    public class KeyRegistrationViewModel : ViewModelBase
    {
        private readonly Dictionary<Bits, string> keySizes = new ()
        {
            { Bits._128, "128 bit" },
            { Bits._256, "256 bit" },
            { Bits._512, "512 bit" },
            { Bits._1024, "1024 bit" },
            { Bits._2048, "2048 bit" },
        };

        private readonly IRsaKeyService rsaKeyService;
        private readonly IDatabaseWriter databaseWriter;
        private Bits chosenKeySize;
        private string userName;
        private bool isSaveKey;
        private DelegateCommand cancelCommand;
        private DelegateCommand registerKeyCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyRegistrationViewModel"/> class.
        /// </summary>
        /// <param name="rsaKeyService">The service for interaction with the Rsa-key of the account.</param>
        /// <param name="databaseWriter">The class for writing in a database.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public KeyRegistrationViewModel(IRsaKeyService rsaKeyService, IDatabaseWriter databaseWriter, INavigationRepository navigation, IMessageBoxService messageBox)
            : base(navigation, messageBox)
        {
            this.rsaKeyService = rsaKeyService;
            this.databaseWriter = databaseWriter;
        }

        /// <summary>
        /// Gets the dictionary of all supported sizes of keys for generation RSA key, where
        /// <see langword="Key"/> is value of the <see cref="Bits"/> enum,
        /// <see langword="Value"/> is string representation of the enums value.
        /// </summary>
        public Dictionary<Bits, string> KeySizes
        {
            get => this.keySizes;
        }

        /// <summary>
        /// Gets or sets currently chosen size of a RSA key.
        /// </summary>
        public Bits ChosenKeySize
        {
            get => this.chosenKeySize;
            set => this.SetProperty(ref this.chosenKeySize, value);
        }

        /// <summary>
        /// Gets the command to cancel the key registration and navigate to the authorization view.
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get => this.cancelCommand ??= new DelegateCommand(this.Cancel);
        }

        /// <summary>
        /// Gets the command which register RSA-key with size indicated with <see cref="ChosenKeySize"/> property in the database.
        /// </summary>
        public DelegateCommand RegisterKeyCommand
        {
            get => this.registerKeyCommand ??= new DelegateCommand(async () => await Task.Run(() => this.GenerateKey()), this.IsCanGenerateKey)
                .ObservesProperty(() => this.ChosenKeySize);
        }

        /// <inheritdoc/>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.userName = (string)navigationContext.Parameters[NavigationParameterKeys.UserName];
            this.isSaveKey = (bool)navigationContext.Parameters[NavigationParameterKeys.IsSaveKey];
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

        private void GenerateKey()
        {
            this.IsBusy = true;
            var rsaKey = this.rsaKeyService.CreateRsaKey(this.ChosenKeySize);

            if (this.isSaveKey)
            {
                var writeResult = this.databaseWriter.WriteRsaKey(rsaKey, this.userName);
                if (writeResult.IsSuccessful)
                {
                    _ = this.MessageBox.Show(LocalizationService.GetString("KeyReady"), LocalizationService.GetString("Success"));
                    this.Navigation.ToAuthorization();
                }
                else
                {
                    _ = this.MessageBox.Show(writeResult.ExceptionMessage, LocalizationService.GetString("Error"));
                }
            }
            else
            {
                this.Navigation.ToSelectFiles(rsaKey);
            }

            this.IsBusy = false;
        }

        private bool IsCanGenerateKey()
        {
            return this.ChosenKeySize != 0;
        }
    }
}
