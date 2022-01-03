// <copyright file="IMessageBoxService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Windows;

    /// <summary>
    /// Represents a service for displaying of a message box.
    /// </summary>
    public interface IMessageBoxService
    {
        /// <summary>
        /// Displays a message box that has a message, title and buttons.
        /// </summary>
        /// <param name="message">A <see cref="string"/> that specifies the text to display.</param>
        /// <param name="title">A <see cref="string"/> that specifies the title of the message box.</param>
        /// <param name="buttons">A <see cref="MessageBoxButton"/> value that specifies which button or buttons to display.</param>
        /// <returns>A <see cref="MessageBoxResult"/> value that specifies which message box button is clicked by the user.</returns>
        MessageBoxResult Show(string message, string title, MessageBoxButton buttons = MessageBoxButton.OK);
    }
}
