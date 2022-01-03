// <copyright file="MessageBoxViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.MessageBox
{
    using System;
    using System.Windows;
    using Prism.Commands;
    using Prism.Mvvm;
    using Prism.Services.Dialogs;

    /// <summary>
    /// ViewModel for interaction with an message box view.
    /// </summary>
    public class MessageBoxViewModel : BindableBase, IDialogAware
    {
        private string title;
        private string message;
        private MessageBoxButton buttons;
        private DelegateCommand<string> closeDialogCommand;

        /// <inheritdoc/>
        public event Action<IDialogResult> RequestClose;

        /// <inheritdoc/>
        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        /// <summary>
        /// Gets or sets the text to display.
        /// </summary>
        public string Message
        {
            get { return this.message; }
            set { this.SetProperty(ref this.message, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies which button or buttons to display.
        /// </summary>
        public MessageBoxButton Buttons
        {
            get { return this.buttons; }
            set { this.SetProperty(ref this.buttons, value); }
        }

        /// <summary>
        /// Gets the command which close the current dialog.
        /// </summary>
        public DelegateCommand<string> CloseDialogCommand =>
            this.closeDialogCommand ??= new DelegateCommand<string>(this.CloseDialog);

        /// <inheritdoc/>
        public virtual bool CanCloseDialog() => true;

        /// <inheritdoc/>
        public virtual void OnDialogClosed()
        {
        }

        /// <inheritdoc/>
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            this.Message = parameters.GetValue<string>("message");
            this.Title = parameters.GetValue<string>("title");
            this.Buttons = parameters.GetValue<MessageBoxButton>("buttons");
        }

        private void CloseDialog(string parameter)
        {
            var result = parameter?.ToLower() switch
            {
                "yes" => ButtonResult.Yes,
                "ok" => ButtonResult.OK,
                "no" => ButtonResult.No,
                "cancel" => ButtonResult.Cancel,
                _ => ButtonResult.None,
            };
            this.RaiseRequestClose(new DialogResult(result));
        }

        private void RaiseRequestClose(IDialogResult dialogResult)
        {
            this.RequestClose?.Invoke(dialogResult);
        }
    }
}
