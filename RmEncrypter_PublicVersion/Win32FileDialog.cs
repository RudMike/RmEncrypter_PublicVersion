// <copyright file="Win32FileDialog.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.IO;
    using Microsoft.Win32;

    /// <summary>
    /// Represents a standart Win32 dialog box that allows a user to specify a filename for opening or saving it.
    /// </summary>
    public class Win32FileDialog : IFileDialogService
    {
        /// <inheritdoc/>
        public string Path
        { get; set; }

        /// <inheritdoc/>
        public bool? CreateFileDialog(string filter)
        {
            var fileDialog = new SaveFileDialog
            {
                Filter = filter,
            };

            var isPressedOk = fileDialog.ShowDialog();

            if (isPressedOk == true)
            {
                this.Path = fileDialog.FileName;
                using (File.Create(this.Path))
                {
                }
            }

            return isPressedOk;
        }

        /// <inheritdoc/>
        public bool? OpenFileDialog(string filter)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = filter,
            };

            var isPressedOk = fileDialog.ShowDialog();

            if (isPressedOk == true)
            {
                this.Path = fileDialog.FileName;
            }

            return isPressedOk;
        }

        /// <inheritdoc/>
        public bool? SaveFileDialog(string filter)
        {
            var fileDialog = new SaveFileDialog
            {
                Filter = filter,
            };

            var isPressedOk = fileDialog.ShowDialog();

            if (isPressedOk == true)
            {
                this.Path = fileDialog.FileName;
            }

            return isPressedOk;
        }
    }
}
