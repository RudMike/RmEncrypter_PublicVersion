// <copyright file="IShellService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Represents service for showing new shell.
    /// </summary>
    public interface IShellService
    {
        /// <summary>
        /// Show a new shell with a single region and pass parameters in.
        /// </summary>
        /// <typeparam name="TShell">Type of the showing shell.</typeparam>
        /// <param name="regionInfo">Information for region navigating.</param>
        /// <param name="isDialogMode">Indicates is show new shell in dialog mode or not.</param>
        void ShowShell<TShell>(RegionNavigationInfo regionInfo, bool isDialogMode)
            where TShell : Window;

        /// <summary>
        /// Show a new shell with a some regions and pass parameters in.
        /// </summary>
        /// <typeparam name="TShell">Type of the showing shell.</typeparam>
        /// <param name="regionsInfo">Enumerable with information for regions navigating.</param>
        /// <param name="isDialogMode">Indicates is show new shell in dialog mode or not.</param>
        void ShowShell<TShell>(IEnumerable<RegionNavigationInfo> regionsInfo, bool isDialogMode)
            where TShell : Window;
    }
}
