// <copyright file="ContentPanelViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Prism.Commands;
    using Prism.Regions;
    using RmEncrypter_PublicVersion.Exceptions;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// ViewModel for interaction with a content panel view.
    /// </summary>
    public class ContentPanelViewModel : ViewModelBase
    {
        private readonly IFileService<SiteAuthData> fileService;
        private readonly IEncryptedListManager<SiteAuthData> listManager;
        private string filePath;
        private DelegateCommand addRecordCommand;
        private DelegateCommand<string> filterCommand;
        private DelegateCommand loadRecordsCommand;
        private DelegateCommand<int?> openRecordCommand;
        private DelegateCommand<int?> removeCommand;
        private DelegateCommand saveCommand;
        private DelegateCommand openAnotherFileCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentPanelViewModel"/> class.
        /// </summary>
        /// <param name="listManager">Manager of the encrypted <see cref="SiteAuthData"/> entities.</param>
        /// <param name="fileService">Service for writing/reading <see cref="SiteAuthData"/> entities from file.</param>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public ContentPanelViewModel(IEncryptedListManager<SiteAuthData> listManager, IFileService<SiteAuthData> fileService, INavigationRepository navigation, IMessageBoxService messageBox)
            : base(navigation, messageBox)
        {
            this.fileService = fileService;
            this.listManager = listManager;
            listManager.OnHeadersChanged += this.ListManager_OnHeadersChanged;
        }

        /// <summary>
        /// Gets the tuple where the first element is record's Id, the second element is record's header.
        /// </summary>
        public List<Tuple<int, string>> SiteHeaders
        {
            get => this.listManager.Headers;
        }

        /// <summary>
        /// Gets the command for adding a new record in new dialog window.
        /// </summary>
        public DelegateCommand AddRecordCommand
        {
            get => this.addRecordCommand ??= new DelegateCommand(this.AddRecord, this.CanExecute)
                .ObservesProperty(() => this.IsBusy);
        }

        /// <summary>
        /// Gets the command for filtering records header by a string.
        /// </summary>
        public DelegateCommand<string> FilterCommand
        {
            get => this.filterCommand ??= new DelegateCommand<string>((input) => this.listManager.Filter(input));
        }

        /// <summary>
        /// Gets the command which load all records from a resource.
        /// </summary>
        public DelegateCommand LoadRecordsCommand
        {
            get => this.loadRecordsCommand ??= new DelegateCommand(async () => await this.LoadRecords());
        }

        /// <summary>
        /// Gets the command for opening current record in new dialog window.
        /// </summary>
        public DelegateCommand<int?> OpenRecordCommand
        {
            get => this.openRecordCommand ??= new DelegateCommand<int?>((id) => this.OpenRecord(id));
        }

        /// <summary>
        /// Gets the command for deleting a record by id.
        /// </summary>
        public DelegateCommand<int?> RemoveCommand
        {
            get => this.removeCommand ??= new DelegateCommand<int?>((id) => this.Remove(id));
        }

        /// <summary>
        /// Gets the command for saving changes in the file.
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get => this.saveCommand ??= new DelegateCommand(async () => await this.Save(), this.CanExecute)
                .ObservesProperty(() => this.IsBusy);
        }

        /// <summary>
        /// Gets the command which open view with file's choose.
        /// </summary>
        public DelegateCommand OpenAnotherFileCommand
        {
            get => this.openAnotherFileCommand ??= new DelegateCommand(this.OpenAnotherFile, this.CanExecute)
                .ObservesProperty(() => this.IsBusy);
        }

        /// <inheritdoc/>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            this.filePath = (string)navigationContext.Parameters[NavigationParameterKeys.FilePath];
        }

        private void AddRecord()
        {
            SiteAuthData addedRecord = this.Navigation.ToAddRecord();
            if (addedRecord != null)
            {
                this.listManager.Add(addedRecord, EntityStates.FullyDecrypted);
            }
        }

        private async Task LoadRecords()
        {
            this.IsBusy = true;
            var encryptedEntities = this.TryReadFile();

            if (encryptedEntities != null)
            {
                await this.TryLoadRecords(encryptedEntities);
            }

            this.IsBusy = false;
        }

        private void OpenRecord(int? id)
        {
            var (entity, state) = this.listManager.Copy((int)id);
            var (resultEntity, isChanged) = this.Navigation.ToShowEditRecord(entity, state);

            if (resultEntity != null)
            {
                this.listManager.Update(resultEntity, (int)id, EntityStates.FullyDecrypted, isChanged);
            }
            else
            {
                this.listManager.Remove((int)id);
            }
        }

        private async Task Save()
        {
            var dialogResult = this.MessageBox.Show(
                LocalizationService.GetString("SaveFileMessage"),
                LocalizationService.GetString("Warning"),
                MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                this.IsBusy = true;
                var entities = await this.listManager.UnloadAsync();
                this.fileService.Write(entities, this.filePath);
                this.IsBusy = false;

                _ = this.MessageBox.Show(
                    LocalizationService.GetString("SaveSuccess"),
                    LocalizationService.GetString("Success"),
                    MessageBoxButton.OK);
            }
        }

        private void OpenAnotherFile()
        {
            var dialogResult = this.MessageBox.Show(
                LocalizationService.GetString("OpenNewFileMessage"),
                LocalizationService.GetString("Warning"),
                MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                this.listManager.Clear();
                this.Navigation.ToOpenFile();
            }
        }

        private void Remove(int? id)
        {
            var dialogResult = this.MessageBox.Show(
                LocalizationService.GetString("ConfirmDelete"),
                LocalizationService.GetString("Delete"),
                MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                this.listManager.Remove((int)id);
            }
        }

        private bool CanExecute() => !this.IsBusy;

        private IEnumerable<SiteAuthData> TryReadFile()
        {
            IEnumerable<SiteAuthData> result = null;
            try
            {
                result = this.fileService.Read(this.filePath);
            }
            catch (FileLoadException ex)
            {
                this.MessageBox.Show(ex.Message, LocalizationService.GetString("Error"));
                this.Navigation.ToOpenFile();
            }

            return result;
        }

        private async Task TryLoadRecords(IEnumerable<SiteAuthData> encryptedEntities)
        {
            try
            {
                await this.listManager.LoadAsync(encryptedEntities.ToList());
            }
            catch (DecryptException)
            {
                _ = this.MessageBox.Show(
                    $"{LocalizationService.GetString("ErrorKey")} {LocalizationService.GetString("WrongKey")}",
                    LocalizationService.GetString("Error"),
                    MessageBoxButton.OK);

                this.Navigation.ToOpenFile();
            }
        }

        private void ListManager_OnHeadersChanged(object sender, EventArgs e)
        {
            this.RaisePropertyChanged(nameof(this.SiteHeaders));
        }
    }
}
