// <copyright file="ShellService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;
    using System.Windows;
    using Prism.Ioc;
    using Prism.Regions;

    /// <summary>
    /// Provides service for showing new shell in dialog mode and not.
    /// </summary>
    public class ShellService : IShellService
    {
        private readonly IContainerProvider container;
        private readonly IRegionManager regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellService"/> class.
        /// </summary>
        /// <param name="container">The dependency injection container.</param>
        /// <param name="regionManager">Current region manager.</param>
        public ShellService(IContainerProvider container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        /// <inheritdoc/>
        public void ShowShell<TShell>(RegionNavigationInfo regionInfo, bool isDialogMode)
            where TShell : Window
        {
            this.ShowShell<TShell>(new[] { regionInfo }, isDialogMode);
        }

        /// <inheritdoc/>
        public void ShowShell<TShell>(IEnumerable<RegionNavigationInfo> regionsInfo, bool isDialogMode)
            where TShell : Window
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var shell = this.container.Resolve<TShell>();
                var scopedRegion = this.regionManager.CreateRegionManager();
                RegionManager.SetRegionManager(shell, scopedRegion);
                RegionManagerAware.SetRegionManagerAware(shell, scopedRegion);
                RegionNavigate(scopedRegion, regionsInfo);
                ShowShell(shell, isDialogMode);
            });
        }

        private static void RegionNavigate(IRegionManager regionManager, IEnumerable<RegionNavigationInfo> regionsInfo)
        {
            if (regionsInfo == null || regionManager == null)
            {
                return;
            }

            foreach (var regionInfo in regionsInfo)
            {
                regionManager.RequestNavigate(
                    regionInfo.RegionName,
                    regionInfo.NavigationInfo.ViewName,
                    regionInfo.NavigationInfo.Parameters);
            }
        }

        private static void ShowShell<TShell>(TShell shell, bool isDialogMode)
            where TShell : Window
        {
            if (isDialogMode)
            {
                _ = shell.ShowDialog();
            }
            else
            {
                shell.Show();
            }
        }
    }
}
