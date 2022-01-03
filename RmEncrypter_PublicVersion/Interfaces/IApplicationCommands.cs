// <copyright file="IApplicationCommands.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using Prism.Commands;

    /// <summary>
    /// Represents application commands.
    /// </summary>
    public interface IApplicationCommands
    {
        /// <summary>
        /// Gets the command for navigation to any view.
        /// </summary>
        CompositeCommand NavigateCommand { get; }
    }
}
