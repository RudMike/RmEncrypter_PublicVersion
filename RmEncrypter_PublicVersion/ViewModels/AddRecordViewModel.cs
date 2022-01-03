// <copyright file="AddRecordViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System;
    using System.Linq;
    using Prism.Commands;
    using Prism.Services.Dialogs;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// ViewModel for interaction with an adding record view.
    /// </summary>
    public class AddRecordViewModel : ViewModelBase, IDialogAware
    {
        private readonly IModelValidator<SiteAuthData> validator;
        private SiteAuthData siteAuthData = new ();
        private DelegateCommand addCommand;
        private DelegateCommand cancelCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddRecordViewModel"/> class.
        /// </summary>
        /// <param name="validator">A validator for validating <see cref="Models.SiteAuthData"/> model.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public AddRecordViewModel(IModelValidator<SiteAuthData> validator, IMessageBoxService messageBox)
            : base(messageBox)
        {
            this.validator = validator;
        }

        /// <inheritdoc/>
        public event Action<IDialogResult> RequestClose;

        /// <inheritdoc/>
        public string Title => LocalizationService.GetString("NewRecord");

        /// <summary>
        /// Gets or sets the data for authorization on a site.
        /// </summary>
        public SiteAuthData SiteAuthData
        {
            get => this.siteAuthData;
            set => this.SetProperty(ref this.siteAuthData, value);
        }

        /// <summary>
        /// Gets the command for adding authorization data and closing current dialog.
        /// </summary>
        public DelegateCommand AddCommand
        {
            get => this.addCommand ??= new DelegateCommand(this.Add);
        }

        /// <summary>
        /// Gets the command which cancel all changes and closing current dialog.
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get => this.cancelCommand ??= new DelegateCommand(this.Cancel);
        }

        /// <inheritdoc/>
        public bool CanCloseDialog() => true;

        /// <inheritdoc/>
        public void OnDialogClosed()
        {
        }

        /// <inheritdoc/>
        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        private void Cancel()
        {
            this.RequestClose(null);
        }

        private void Add()
        {
            var validateResult = this.validator.Validate(this.SiteAuthData);
            if (validateResult.Count == 0)
            {
                var parameter = new DialogParameters
                {
                    { DialogParameterKeys.Entity, this.SiteAuthData },
                };
                var result = new DialogResult(ButtonResult.OK, parameter);
                this.RequestClose(result);
            }
            else
            {
                _ = this.MessageBox.Show(validateResult.Values.First(), LocalizationService.GetString("Error"));
            }
        }
    }
}
