// <copyright file="UserRecordsShell.xaml.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.Views
{
    using System.Windows;
    using Prism.Services.Dialogs;

    /// <summary>
    /// Interaction logic for UserRecordsWindow.xaml.
    /// </summary>
    public partial class UserRecordsShell : Window, IDialogWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRecordsShell"/> class.
        /// </summary>
        public UserRecordsShell()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        public IDialogResult Result
        { get; set; }
    }
}
