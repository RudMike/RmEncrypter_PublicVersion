// <copyright file="MessageBoxWindow.xaml.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.MessageBox
{
    using System.Windows;
    using Prism.Services.Dialogs;

    /// <summary>
    /// Interaction logic for DialogWindow.xaml.
    /// </summary>
    public partial class MessageBoxWindow : Window, IDialogWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBoxWindow"/> class.
        /// </summary>
        public MessageBoxWindow()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public IDialogResult Result { get; set; }
    }
}
