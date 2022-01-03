// <copyright file="RegionNavigationInfo.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides information for region navigation.
    /// </summary>
    public class RegionNavigationInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNavigationInfo"/> class.
        /// </summary>
        public RegionNavigationInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNavigationInfo"/> class.
        /// </summary>
        /// <param name="regionName">A region name in which the navigationInfo will be opened.</param>
        /// <param name="viewName">A view name which will be opened in the region.
        /// This value will be sets as <see cref="NavigationInfo"/> property.</param>
        public RegionNavigationInfo(string regionName, string viewName)
        {
            this.RegionName = regionName;
            this.NavigationInfo = new (viewName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionNavigationInfo"/> class.
        /// </summary>
        /// <param name="regionName">A region name in which the navigationInfo will be opened.</param>
        /// <param name="navigationInfo">A navigation info which will be opened in the region.</param>
        public RegionNavigationInfo(string regionName, NavigationInfo navigationInfo)
        {
            this.RegionName = regionName;
            this.NavigationInfo = navigationInfo;
        }

        /// <summary>
        /// Gets or sets a region name in which the <see cref="NavigationInfo"/> will be opened.
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets a navigation info which will be opened in the region.
        /// </summary>
        public NavigationInfo NavigationInfo { get; set; }
    }
}