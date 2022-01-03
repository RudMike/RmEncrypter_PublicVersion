// <copyright file="OpenFileViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Collections.ObjectModel;
    using Prism.Commands;

    /// <summary>
    /// ViewModel for interaction with a open file view.
    /// </summary>
    public class OpenFileViewModel : ViewModelBase
    {
        private const string FileFilter = "Encrypter files|*.enc";
        private readonly IFileDialogService fileDialog;
        private DelegateCommand openFileDialogCommand;
        private DelegateCommand<string> openFileCommand;
        private DelegateCommand createFileCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileViewModel"/> class.
        /// </summary>
        /// <param name="fileDialog">Service which provides functionality for work with file dialogs.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        public OpenFileViewModel(IFileDialogService fileDialog, INavigationRepository navigation)
            : base(navigation)
        {
            this.fileDialog = fileDialog;
        }

        /// <summary>
        /// Gets the command for opening file dialog box and navigation to a content view.
        /// </summary>
        public DelegateCommand OpenFileDialogCommand
        {
            get => this.openFileDialogCommand ??= new DelegateCommand(() => this.OpenFileDialog());
        }

        /// <summary>
        /// Gets the command for opening existing file and navigation to a content view.
        /// </summary>
        public DelegateCommand<string> OpenFileCommand
        {
            get => this.openFileCommand ??= new DelegateCommand<string>((file) => this.OpenFile(file));
        }

        /// <summary>
        /// Gets the command for creating new empty file and navigation to a content view.
        /// </summary>
        public DelegateCommand CreateFileCommand
        {
            get => this.createFileCommand ??= new DelegateCommand(() => this.CreateFile());
        }

        /// <summary>
        /// Gets the list with file extensions which can be opened.
        /// </summary>
        public ObservableCollection<string> SupportedExtensions
        { get; } = new () { ".enc", };

        private void OpenFileDialog()
        {
            if (this.fileDialog.OpenFileDialog(FileFilter) == true)
            {
                this.OpenFile(this.fileDialog.Path);
            }
        }

        private void OpenFile(string filePath)
        {
            this.Navigation.ToContentPanel(filePath);
        }

        private void CreateFile()
        {
            if (this.fileDialog.CreateFileDialog(FileFilter) == true)
            {
                this.OpenFile(this.fileDialog.Path);
            }
        }
    }
}
