// <copyright file="ReEncryptFilesViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using Prism.Commands;
    using Prism.Regions;
    using RmEncrypter_PublicVersion.Exceptions;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// ViewModel for interaction with a reencrypt files view.
    /// </summary>
    public class ReEncryptFilesViewModel : ViewModelBase
    {
        private readonly IAccountReencrypter reencrypter;
        private RsaKey newRsaKey;
        private IEnumerable<string> filePaths;
        private DelegateCommand updateKeyCommand;
        private DelegateCommand cancelCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReEncryptFilesViewModel"/> class.
        /// </summary>
        /// <param name="reencrypter">A reencrypter for work with user files and updating Rsa-key.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public ReEncryptFilesViewModel(IAccountReencrypter reencrypter, INavigationRepository navigation, IMessageBoxService messageBox)
            : base(navigation, messageBox)
        {
            this.reencrypter = reencrypter;
        }

        /// <summary>
        /// Gets the command which updates Rsa-key in the database and reencrypts selected files with a new key.
        /// </summary>
        public DelegateCommand UpdateKeyCommand
        {
            get => this.updateKeyCommand ??= new DelegateCommand(async () => await Task.Run(this.UpdateKey));
        }

        /// <summary>
        /// Gets the command to cancel the files reencryption and navigate to the authorization view.
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get => this.cancelCommand ??= new DelegateCommand(this.Cancel);
        }

        /// <summary>
        /// Gets a text description of a current stage of the key updating.
        /// </summary>
        public string StageDescription
        {
            get => this.reencrypter.StageDescription;
        }

        /// <inheritdoc/>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.newRsaKey = (RsaKey)navigationContext.Parameters[NavigationParameterKeys.Key];
            this.filePaths = (IEnumerable<string>)navigationContext.Parameters[NavigationParameterKeys.FilePaths];
        }

        private async void UpdateKey()
        {
            this.IsBusy = true;
            this.reencrypter.OnStageDescriptionChanged += this.Reencrypter_OnStageDescriptionChanged;
            bool isSuccesfull = await this.TryUpdateKey();
            this.reencrypter.OnStageDescriptionChanged -= this.Reencrypter_OnStageDescriptionChanged;
            this.IsBusy = false;

            if (isSuccesfull)
            {
                _ = this.MessageBox.Show(
                    LocalizationService.GetString("UpdateKeyCompleted") + LocalizationService.GetString("RedirectToAuthorization"),
                    LocalizationService.GetString("Success"));
                this.Navigation.ToAuthorization();
            }
            else
            {
                this.Navigation.ToSelectFiles(this.newRsaKey);
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

        private async Task<bool> TryUpdateKey()
        {
            bool isSuccesfull = true;

            try
            {
                await this.reencrypter.UpdateAsync(this.newRsaKey, this.filePaths);
            }
            catch (DecryptException ex)
            {
                isSuccesfull = false;
                _ = this.MessageBox.Show(
                    $"{ex.Message} " +
                    $"{LocalizationService.GetString("RedirectToSelectFiles")}\r\r" +
                    $"{LocalizationService.GetString("FileName")} {ex.ParamName}",
                    LocalizationService.GetString("Error"));
            }
            catch (FileLoadException ex)
            {
                isSuccesfull = false;
                _ = this.MessageBox.Show(
                    $"{ex.Message} " +
                    $"{LocalizationService.GetString("RedirectToSelectFiles")}\r\r" +
                    $"{LocalizationService.GetString("FileName")} {ex.FileName}",
                    LocalizationService.GetString("Error"));
            }

            return isSuccesfull;
        }

        private void Reencrypter_OnStageDescriptionChanged(object sender, System.EventArgs e)
        {
            this.RaisePropertyChanged(nameof(this.StageDescription));
        }
    }
}
