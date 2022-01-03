// <copyright file="AccountTransferViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System.Threading.Tasks;
    using Prism.Commands;

    /// <summary>
    /// ViewModel for interaction with an account transfer view.
    /// </summary>
    public class AccountTransferViewModel : ViewModelBase
    {
        private const string FileFilter = "User data file|*.encu";
        private readonly IAccountTransfer transfer;
        private readonly IAuthorizedUser authorizedUser;
        private readonly IFileDialogService fileDialog;
        private bool requestClose = false;
        private DelegateCommand cancelCommand;
        private DelegateCommand createFileCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountTransferViewModel"/> class.
        /// </summary>
        /// <param name="transfer">Service which provides functionality for work with account transfer.</param>
        /// <param name="authorizedUser">The class which contains the current authorized user data.</param>
        /// <param name="fileDialog">Service which provides functionality for work with file dialogs.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public AccountTransferViewModel(IAccountTransfer transfer, IAuthorizedUser authorizedUser, IFileDialogService fileDialog, IMessageBoxService messageBox)
            : base(messageBox)
        {
            this.transfer = transfer;
            this.authorizedUser = authorizedUser;
            this.fileDialog = fileDialog;
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
        /// Gets the command that create transfer file with data of the current account.
        /// </summary>
        public DelegateCommand CreateFileCommand
        {
            get => this.createFileCommand ??= new DelegateCommand(async () => await Task.Run(() => this.CreateFile()));
        }

        /// <summary>
        /// Gets the command to cancel account transfer.
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get => this.cancelCommand ??= new DelegateCommand(this.Cancel);
        }

        private void CreateFile()
        {
            var dialogResult = this.fileDialog.CreateFileDialog(FileFilter);
            if (dialogResult == true)
            {
                this.IsBusy = true;
                this.transfer.Transfer(this.authorizedUser.UserName, this.fileDialog.Path);
                this.IsBusy = false;
                _ = this.MessageBox.Show(LocalizationService.GetString("FileCreated"), LocalizationService.GetString("Success"));
                this.RequestClose = true;
            }
        }

        private void Cancel()
        {
            this.RequestClose = true;
        }
    }
}
