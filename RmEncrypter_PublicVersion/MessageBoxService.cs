// <copyright file="MessageBoxService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Windows;
    using Prism.Services.Dialogs;

    /// <summary>
    /// Provides  service for showing a message box.
    /// </summary>
    public class MessageBoxService : IMessageBoxService
    {
        private readonly IDialogService dialogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxService"/> class.
        /// </summary>
        /// <param name="dialogService">Service for showing a modal dialog.</param>
        public MessageBoxService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        /// <inheritdoc/>
        public MessageBoxResult Show(string message, string title, MessageBoxButton buttons = MessageBoxButton.OK)
        {
            MessageBoxResult result = MessageBoxResult.None;
            var parameters = new DialogParameters
            {
                { "title", title },
                { "message", message },
                { "buttons", buttons },
            };

            Application.Current.Dispatcher.Invoke(() =>
            {
                this.dialogService.ShowDialog(
                    name: "MessageBoxView",
                    parameters,
                    r =>
                    {
                        result = ConvertToMessageBoxResult(r.Result);
                    },
                    windowName: "MessageBox");
            });

            return result;
        }

        private static MessageBoxResult ConvertToMessageBoxResult(ButtonResult buttonResult)
        {
            var result = buttonResult switch
            {
                ButtonResult.Cancel => MessageBoxResult.Cancel,
                ButtonResult.No => MessageBoxResult.No,
                ButtonResult.None => MessageBoxResult.None,
                ButtonResult.OK => MessageBoxResult.OK,
                ButtonResult.Yes => MessageBoxResult.Yes,
                _ => MessageBoxResult.None,
            };

            return result;
        }
    }
}
