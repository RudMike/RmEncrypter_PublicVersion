// <copyright file="SelectFilesViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using Prism.Commands;
    using Prism.Regions;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// ViewModel for interaction with a select files view.
    /// </summary>
    public class SelectFilesViewModel : ViewModelBase
    {
        private readonly IFileDialogService fileDialog;
        private readonly string fileFormats = "Encrypter files|*.enc";
        private DelegateCommand continueCommand;
        private DelegateCommand<IEnumerable<string>> addFilesCommand;
        private DelegateCommand openFileDialogCommand;
        private DelegateCommand<string> removeFromListCommand;
        private DelegateCommand cancelCommand;
        private RsaKey rsaKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectFilesViewModel"/> class.
        /// </summary>
        /// <param name="fileDialog">Service which provides functionality for work with file dialogs.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public SelectFilesViewModel(IFileDialogService fileDialog, INavigationRepository navigation, IMessageBoxService messageBox)
            : base(navigation, messageBox)
        {
            this.fileDialog = fileDialog;
        }

        /// <summary>
        /// Gets or sets the list with selected file paths.
        /// </summary>
        public ObservableCollection<string> SelectedFiles
        { get; set; } = new ();

        /// <summary>
        /// Gets the list with file extensions which can be added in <see cref="SelectedFiles"/> list.
        /// </summary>
        public ObservableCollection<string> SupportedExtensions
        { get; } = new () { ".enc", };

        /// <summary>
        /// Gets the command to go to the next step.
        /// </summary>
        public DelegateCommand ContinueCommand
        {
            get => this.continueCommand ??= new DelegateCommand(this.Continue);
        }

        /// <summary>
        /// Gets the command for removing element from the <see cref="SelectedFiles"/> list.
        /// </summary>
        public DelegateCommand<string> RemoveFromListCommand
        {
            get => this.removeFromListCommand ??= new DelegateCommand<string>(this.RemoveFromList);
        }

        /// <summary>
        /// Gets the command to cancel the files selection and navigate to the authorization view.
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get => this.cancelCommand ??= new DelegateCommand(this.Cancel);
        }

        /// <summary>
        /// Gets the command for opening file dialog to select files.
        /// </summary>
        public DelegateCommand OpenFileDialogCommand
        {
            get => this.openFileDialogCommand ??= new DelegateCommand(this.OpenFileDialog);
        }

        /// <summary>
        /// Gets the command for adding an enumeraton in the <see cref="SelectedFiles"/> list.
        /// </summary>
        public DelegateCommand<IEnumerable<string>> AddFilesCommand
        {
            get => this.addFilesCommand ??= new DelegateCommand<IEnumerable<string>>(this.AddFiles);
        }

        /// <inheritdoc/>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.rsaKey = (RsaKey)navigationContext.Parameters[NavigationParameterKeys.Key];
        }

        private void Continue()
        {
            this.Navigation.ToReEncryptFiles(this.SelectedFiles, this.rsaKey);
        }

        private void RemoveFromList(string filePath)
        {
            _ = this.SelectedFiles.Remove(filePath);
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

        private void OpenFileDialog()
        {
            var result = this.fileDialog.OpenFileDialog(this.fileFormats);
            if (result == true)
            {
                this.TryAddFile(this.fileDialog.Path);
            }
        }

        private void AddFiles(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                this.TryAddFile(file);
            }
        }

        private void TryAddFile(string file)
        {
            if (!this.SelectedFiles.Contains(file))
            {
                this.SelectedFiles.Add(file);
            }
        }
    }
}
