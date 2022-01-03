// <copyright file="MainShellViewModel.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.ViewModels
{
    using System;
    using System.Windows;

    /// <summary>
    /// ViewModel for interaction with a main shell.
    /// </summary>
    public class MainShellViewModel : ViewModelBase
    {
        private readonly IMessageBoxService messageBox;
        private Func<bool> canClose;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainShellViewModel"/> class.
        /// </summary>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        public MainShellViewModel(IMessageBoxService messageBox)
            : base(messageBox)
        {
            this.messageBox = messageBox;
        }

        /// <summary>
        /// Gets the delegate which uses to confirm window's closing.
        /// </summary>
        public Func<bool> CanClose
        {
            get => this.canClose ??= this.ConfirmClosing;
            private set => this.SetProperty(ref this.canClose, value);
        }

        private bool ConfirmClosing()
        {
            var dialogResult = this.messageBox.Show(
                LocalizationService.GetString("ProgramCloseMessage"),
                LocalizationService.GetString("Warning"),
                MessageBoxButton.YesNo);

            return dialogResult == MessageBoxResult.Yes;
        }
    }
}
