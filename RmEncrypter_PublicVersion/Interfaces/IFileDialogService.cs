// <copyright file="IFileDialogService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides the functionality for work with file dialogs.
    /// </summary>
    public interface IFileDialogService
    {
        /// <summary>
        /// Gets or sets path to a file.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Show dialog for opening a file.
        /// </summary>
        /// <param name="filter">The file name filter string, which determines the file extension and/or extension description.</param>
        /// <returns>If the user clicks the OK button of the dialog that is displayed, <see langword="true"/> is returned; otherwise, <see langword="false"/>.</returns>
        bool? OpenFileDialog(string filter);

        /// <summary>
        /// Show dialog for saving a file.
        /// </summary>
        /// <param name="filter">The file name filter string, which determines the file extension and/or extension description.</param>
        /// <returns>If the user clicks the OK button of the dialog that is displayed, <see langword="true"/> is returned; otherwise, <see langword="false"/>.</returns>
        bool? SaveFileDialog(string filter);

        /// <summary>
        /// Show dialog for creating empty file.
        /// </summary>
        /// <param name="filter">The file name filter string, which determines the file extension and/or extension description.</param>
        /// <returns>If the user clicks the OK button of the dialog that is displayed, <see langword="true"/> is returned; otherwise, <see langword="false"/>.</returns>
        bool? CreateFileDialog(string filter);
    }
}
