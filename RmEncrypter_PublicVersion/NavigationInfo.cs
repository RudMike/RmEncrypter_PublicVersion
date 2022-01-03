// <copyright file="NavigationInfo.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using Prism.Regions;

    /// <summary>
    /// Provides information for the views navigation.
    /// </summary>
    public class NavigationInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationInfo"/> class.
        /// </summary>
        public NavigationInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationInfo"/> class.
        /// </summary>
        /// <param name="viewName">The view name.</param>
        /// <param name="parameters">The navigation parameter. Can be null if passing nothing.</param>
        public NavigationInfo(string viewName, NavigationParameters parameters = null)
        {
            this.ViewName = viewName;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the navigation parameters.
        /// </summary>
        public NavigationParameters Parameters { get; set; }
    }
}
