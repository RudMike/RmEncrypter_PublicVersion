// <copyright file="ShowEditRecordViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Prism.Commands;
    using Prism.Services.Dialogs;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// ViewModel for interaction with an editing or showing record view.
    /// </summary>
    public class ShowEditRecordViewModel : ViewModelBase, IDialogAware
    {
        private readonly IEncryptionService<IRsaKey, SiteAuthData> encryptionService;
        private readonly IModelValidator<SiteAuthData> validator;
        private readonly IRsaKey rsaKey;
        private SiteAuthData originalSiteAuthData;
        private SiteAuthData siteAuthData;
        private EntityStates state;
        private DelegateCommand saveAndCloseCommand;
        private DelegateCommand decryptCommand;
        private DelegateCommand deleteCommand;
        private DelegateCommand<string> openInBrowserCommand;
        private DelegateCommand<string> copyCommand;

        /// <summary>
        /// Indicates whether the closing event had raised by <see cref="RequestClose"/> or not.
        /// </summary>
        private bool isMethodRequestClosing = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowEditRecordViewModel"/> class.
        /// </summary>
        /// <param name="encryptionService">A service for encrypting and decrypting entities.</param>
        /// <param name="authorizedUser">A class which contains an authorized user data.</param>
        /// <param name="validator">A validator for validating <see cref="Models.SiteAuthData"/> model.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public ShowEditRecordViewModel(IEncryptionService<IRsaKey, SiteAuthData> encryptionService, IAuthorizedUser authorizedUser, IModelValidator<SiteAuthData> validator, IMessageBoxService messageBox)
            : base(messageBox)
        {
            this.encryptionService = encryptionService;
            this.rsaKey = authorizedUser.RsaKey;
            this.validator = validator;
            this.IsBusy = true;
        }

        /// <inheritdoc/>
        public event Action<IDialogResult> RequestClose;

        /// <summary>
        /// Gets or sets current record with data for authentification on the site.
        /// </summary>
        public SiteAuthData SiteAuthData
        {
            get => this.siteAuthData;
            set => this.SetProperty(ref this.siteAuthData, value);
        }

        /// <inheritdoc/>
        public string Title => LocalizationService.GetString("ShowEditRecord");

        /// <summary>
        /// Gets the command which try to save changes in the <see cref="SiteAuthData"/> model and close current dialog.
        /// </summary>
        public DelegateCommand SaveAndCloseCommand
        {
            get => this.saveAndCloseCommand ??= new DelegateCommand(this.Close);
        }

        /// <summary>
        /// Gets the command for decrypting the received entity.
        /// </summary>
        public DelegateCommand DecryptCommand
        {
            get => this.decryptCommand ??= new DelegateCommand(async () => await Task.Run(this.Decrypt));
        }

        /// <summary>
        /// Gets the command for deleting an entity.
        /// </summary>
        public DelegateCommand DeleteCommand
        {
            get => this.deleteCommand ??= new DelegateCommand(this.Delete);
        }

        /// <summary>
        /// Gets the command to open current site in the default browser.
        /// </summary>
        public DelegateCommand<string> OpenInBrowserCommand
        {
            get => this.openInBrowserCommand ??= new DelegateCommand<string>(this.OpenInBrowser);
        }

        /// <summary>
        /// Gets the command which copy text value to the clipboard.
        /// </summary>
        public DelegateCommand<string> CopyCommand
        {
            get => this.copyCommand ??= new DelegateCommand<string>(this.Copy);
        }

        /// <inheritdoc/>
        public void OnDialogClosed()
        {
        }

        /// <inheritdoc/>
        public bool CanCloseDialog() => this.isMethodRequestClosing;

        /// <inheritdoc/>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            this.state = parameters.GetValue<EntityStates>(DialogParameterKeys.State);
            this.SiteAuthData = parameters.GetValue<SiteAuthData>(DialogParameterKeys.Entity);
        }

        private static DialogResult CreateResult(SiteAuthData entity, ButtonResult button, bool isChanged)
        {
            var parameter = new DialogParameters()
            {
                { DialogParameterKeys.Entity, entity },
                { DialogParameterKeys.IsChanged, isChanged },
            };

            return new DialogResult(button, parameter);
        }

        private void Decrypt()
        {
            this.DecryptEntity();
            this.RaisePropertyChanged(nameof(this.SiteAuthData));
            this.originalSiteAuthData = (SiteAuthData)this.siteAuthData.Clone();
            this.IsBusy = false;
        }

        private void Delete()
        {
            var dialogResult = this.MessageBox.Show(
                LocalizationService.GetString("ConfirmDelete"),
                LocalizationService.GetString("Delete"),
                MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                this.isMethodRequestClosing = true;
                var result = CreateResult(null, ButtonResult.None, true);
                this.RequestClose(result);
            }
        }

        private void Close()
        {
            DialogResult result = null;
            bool requestClose = true;

            if (!this.IsEntityChanged())
            {
                result = CreateResult(this.SiteAuthData, ButtonResult.No, false);
            }
            else
            {
                var dialogResult = this.MessageBox.Show(
                    LocalizationService.GetString("SaveChanges"),
                    LocalizationService.GetString("Warning"),
                    MessageBoxButton.YesNoCancel);

                switch (dialogResult)
                {
                    case MessageBoxResult.Yes:
                        if (this.IsEntityValid())
                        {
                            result = CreateResult(this.SiteAuthData, ButtonResult.Yes, true);
                        }
                        else
                        {
                            requestClose = false;
                        }

                        break;

                    case MessageBoxResult.No:
                        result = CreateResult(this.originalSiteAuthData, ButtonResult.No, false);
                        break;

                    case MessageBoxResult.Cancel:
                        requestClose = false;
                        break;
                }
            }

            if (requestClose)
            {
                this.isMethodRequestClosing = true;
                this.RequestClose(result);
            }
        }

        private void OpenInBrowser(string url)
        {
            DoUrlValid();
            try
            {
                _ = Process.Start(url);
            }
            catch (Win32Exception)
            {
                _ = this.MessageBox.Show(LocalizationService.GetString("ErrorOpenInBrowser"), LocalizationService.GetString("Error"));
            }

            void DoUrlValid()
            {
                if (!(url.StartsWith("www.") || url.StartsWith("http://") || url.StartsWith("https://")))
                {
                    url = "www." + url;
                }
            }
        }

        private void Copy(string value)
        {
            Clipboard.SetText(value);
        }

        private bool IsEntityChanged()
        {
            bool result;
            if (this.originalSiteAuthData != null)
            {
                result = !this.SiteAuthData.Equals(this.originalSiteAuthData);
            }
            else
            {
                result = false;
            }

            return result;
        }

        private bool IsEntityValid()
        {
            bool result;
            var validateResult = this.validator.Validate(this.SiteAuthData);

            if (validateResult.Count != 0)
            {
                _ = this.MessageBox.Show(validateResult.Values.First(), LocalizationService.GetString("Error"));
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }

        private void DecryptEntity()
        {
            switch (this.state)
            {
                case EntityStates.FullyEncrypted:
                    this.encryptionService.Decrypt(this.rsaKey, this.SiteAuthData);
                    break;

                case EntityStates.HeaderDecrypted:
                    var decryptingFields = new List<string>()
                    {
                        nameof(this.SiteAuthData.UserName),
                        nameof(this.SiteAuthData.Password),
                        nameof(this.SiteAuthData.Note),
                    };
                    this.encryptionService.Decrypt(this.rsaKey, this.SiteAuthData, decryptingFields);
                    break;

                case EntityStates.FullyDecrypted:
                    break;
            }
        }
    }
}
