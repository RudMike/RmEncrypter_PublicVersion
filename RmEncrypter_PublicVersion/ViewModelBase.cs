// <copyright file="ViewModelBase.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.ComponentModel;
    using Prism.Mvvm;
    using Prism.Regions;

    /// <summary>
    /// Provides base implementation for the view-models. Inherited from <see cref="BindableBase"/>.
    /// In base implemented <see cref="IRegionMemberLifetime"/>,
    /// <see cref="INavigationAware"/>, <see cref="IRegionManagerAware"/> interfaces.
    /// </summary>
    public abstract class ViewModelBase : BindableBase, IRegionMemberLifetime, INavigationAware, IRegionManagerAware
    {
        private bool isBusy = false;
        private IRegionManager regionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class without navigation and message box supports.
        /// </summary>
        protected ViewModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class with message box support.
        /// </summary>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        protected ViewModelBase(IMessageBoxService messageBox)
        {
            this.MessageBox = messageBox;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class with navigation support.
        /// </summary>
        /// <param name="navigation">A repository which provides views navigation.</param>
        protected ViewModelBase(INavigationRepository navigation)
        {
            this.Navigation = navigation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class with navigation and message box support.
        /// </summary>
        /// <param name="navigation">A repository which provides views navigation.</param>
        /// <param name="messageBox">Current provider for showing message boxes.</param>
        protected ViewModelBase(INavigationRepository navigation, IMessageBoxService messageBox)
        {
            this.Navigation = navigation;
            this.MessageBox = messageBox;
        }

        /// <summary>
        /// Gets a value indicating whether this instance should be kept-alive upon deactivation.
        /// Default value is <see langword="false"/>.
        /// </summary>
        public virtual bool KeepAlive => false;

        /// <summary>
        /// Gets or sets a value indicating whether view-model is currently busy with long operation.
        /// Default value is <see langword="false"/>.
        /// The property implements <see cref="INotifyPropertyChanged"/>.
        /// </summary>
        public bool IsBusy
        {
            get => this.isBusy;
            set => this.SetProperty(ref this.isBusy, value);
        }

        /// <inheritdoc/>
        public IRegionManager RegionManager
        {
            get => this.regionManager;
            set
            {
                this.regionManager = value;
                if (this.Navigation != null)
                {
                    this.Navigation.RegionManager = value;
                }
            }
        }

        /// <summary>
        /// Gets a repository for navigation between views.
        /// </summary>
        protected INavigationRepository Navigation
        { get; private set; }

        /// <summary>
        /// Gets a message box for displaying a text.
        /// </summary>
        protected IMessageBoxService MessageBox
        { get; private set; }

        /// <inheritdoc/>
        /// <remarks> Default value is <see langword="true"/>.</remarks>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <inheritdoc/>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        /// <inheritdoc/>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}
