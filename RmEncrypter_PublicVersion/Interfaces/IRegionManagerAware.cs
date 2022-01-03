// <copyright file="IRegionManagerAware.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using Prism.Regions;

    /// <summary>
    /// Represents a region manager.
    /// </summary>
    public interface IRegionManagerAware
    {
        /// <summary>
        /// Gets or sets the region manager.
        /// </summary>
        IRegionManager RegionManager { get; set; }
    }
}
