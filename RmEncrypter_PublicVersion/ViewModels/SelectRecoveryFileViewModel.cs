// <copyright file="SelectRecoveryFileViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Collections.ObjectModel;
    using Prism.Commands;

    /// <summary>
    /// ViewModel for interaction with a select recovery file view.
    /// </summary>
    public class SelectRecoveryFileViewModel : ViewModelBase
    {
        private const string FileFilter = "Account recovery file|*.encu";
        private readonly IFileDialogService dialogService;
        private string selectedFile = string.Empty;
        private DelegateCommand<string> selectFileCommand;
        private DelegateCommand openFileDialogCommand;
        private DelegateCommand cancelCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectRecoveryFileViewModel"/> class.
        /// </summary>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="dialogService">Service which provides functionality for work with file dialogs.</param>
        public SelectRecoveryFileViewModel(INavigationRepository navigation, IFileDialogService dialogService)
            : base(navigation)
        {
            this.dialogService = dialogService;
        }

        /// <summary>
        /// Gets the list with file extensions which can be uses for the recovery.
        /// </summary>
        public ObservableCollection<string> SupportedExtensions
        { get; } = new () { ".encu", };

        /// <summary>
        /// Gets or sets the selected file path.
        /// </summary>
        public string SelectedFile
        {
            get => this.selectedFile;
            set => this.SetProperty(ref this.selectedFile, value);
        }

        /// <summary>
        /// Gets the command for select the passed file path and navigating to the next view.
        /// </summary>
        public DelegateCommand<string> SelectFileCommand
        {
            get => this.selectFileCommand ??= new DelegateCommand<string>(this.SelectFile);
        }

        /// <summary>
        /// Gets the command for opening a file dialog to select file.
        /// </summary>
        public DelegateCommand OpenFileDialogCommand
        {
            get => this.openFileDialogCommand ??= new DelegateCommand(this.OpenFileDialog);
        }

        /// <summary>
        /// Gets the command to cancel account recovery.
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get => this.cancelCommand ??= new DelegateCommand(this.Navigation.ToAuthorization);
        }

        private void SelectFile(string path)
        {
            this.SelectedFile = path;
            this.Navigation.ToAccountRecovery(this.SelectedFile);
        }

        private void OpenFileDialog()
        {
            if (this.dialogService.OpenFileDialog(FileFilter) == true)
            {
                this.SelectedFile = this.dialogService.Path;
                this.Navigation.ToAccountRecovery(this.SelectedFile);
            }
        }
    }
}
