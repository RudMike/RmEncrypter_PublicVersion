// <copyright file="RegionManagerAwareBehavior.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;
    using Prism.Regions;

    /// <summary>
    /// Provides custom region behavior for setting region manager in views.
    /// </summary>
    public class RegionManagerAwareBehavior : RegionBehavior
    {
        /// <summary>
        /// Represents the key of this behavior.
        /// </summary>
        public const string BehaviorKey = "RegionManagerAwareBehavior";

        /// <inheritdoc/>
        protected override void OnAttach()
        {
            this.Region.ActiveViews.CollectionChanged += this.ActiveViews_CollectionChanged;
        }

        private static void InvokeOnRegionManagerAwareElement(object item, Action<IRegionManagerAware> invocation)
        {
            if (item is IRegionManagerAware rmAwareItem)
            {
                invocation(rmAwareItem);
            }

            if (item is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is IRegionManagerAware rmAwareDataContext)
                {
                    if (frameworkElement.Parent is FrameworkElement frameworkElementParent)
                    {
                        if (frameworkElementParent.DataContext is IRegionManagerAware rmAwareDataContextParent)
                        {
                            if (rmAwareDataContext == rmAwareDataContextParent)
                            {
                                return;
                            }
                        }
                    }

                    invocation(rmAwareDataContext);
                }
            }
        }

        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var regionManager = this.Region.RegionManager;

                    if (item is FrameworkElement element)
                    {
                        if (element.GetValue(RegionManager.RegionManagerProperty) is IRegionManager scopedRegionManager)
                        {
                            regionManager = scopedRegionManager;
                        }
                    }

                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = regionManager);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = null);
                }
            }
        }
    }
}
