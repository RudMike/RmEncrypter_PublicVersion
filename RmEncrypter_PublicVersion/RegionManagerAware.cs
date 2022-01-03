// <copyright file="RegionManagerAware.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Windows;
    using Prism.Regions;

    /// <summary>
    /// Provides methods for <see cref="RegionManager"/> management.
    /// </summary>
    public static class RegionManagerAware
    {
        /// <summary>
        /// Sets a region manager for a shell or view.
        /// </summary>
        /// <param name="item">A shell or view for region manager setting.</param>
        /// <param name="regionManager">A region manager which binds to.</param>
        public static void SetRegionManagerAware(object item, IRegionManager regionManager)
        {
            if (item is IRegionManagerAware rmAware)
            {
                rmAware.RegionManager = regionManager;
            }

            if (item is FrameworkElement rmAwareFrameworkElement)
            {
                if (rmAwareFrameworkElement.DataContext is IRegionManagerAware rmAwareDataContext)
                {
                    rmAwareDataContext.RegionManager = regionManager;
                }
            }
        }
    }
}
