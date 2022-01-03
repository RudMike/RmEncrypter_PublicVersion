// <copyright file="ChangeVisibilityBehavior.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.ComponentModel;
    using System.Windows;
    using Microsoft.Xaml.Behaviors;

    /// <summary>
    /// Allows the behavior for an <see cref="UIElement"/> which changes it visibility depending on a current busy view-model status.
    /// It show attached element when <see cref="IsBusy"/> is <see langword="true"/> and collapse it if not.
    /// </summary>
    public class ChangeVisibilityBehavior : Behavior<UIElement>
    {
        /// <summary>
        /// The attached property which indicates whether view-model is currently busy.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.RegisterAttached(
            "IsBusy",
            typeof(bool),
            typeof(ChangeVisibilityBehavior));

        /// <summary>
        /// The attached property which indicates on the <see cref="UIElement"/> which must be collapse when attached object is visible.
        /// </summary>
        public static readonly DependencyProperty CollapsedElementProperty = DependencyProperty.RegisterAttached(
            "CollapsedElement",
            typeof(UIElement),
            typeof(ChangeVisibilityBehavior));

        /// <summary>
        /// Gets or sets a value indicating whether view-model is currently busy.
        /// </summary>
        public bool IsBusy
        {
            get { return (bool)this.GetValue(IsBusyProperty); }
            set { this.SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="UIElement"/> which must be collapse when attached object is visible.
        /// </summary>
        public UIElement CollapsedElement
        {
            get { return (UIElement)this.GetValue(CollapsedElementProperty); }
            set { this.SetValue(CollapsedElementProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.TryUpdateVisible();
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            this.TryUpdateVisible();
        }

        private void TryUpdateVisible()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            if (this.IsBusy)
            {
                this.AssociatedObject.Visibility = Visibility.Visible;
                this.HideCollapsedElement();
            }
            else
            {
                this.AssociatedObject.Visibility = Visibility.Collapsed;
                this.ShowCollapsedElement();
            }
        }

        private void HideCollapsedElement()
        {
            if (this.CollapsedElement != null)
            {
                this.CollapsedElement.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowCollapsedElement()
        {
            if (this.CollapsedElement != null)
            {
                this.CollapsedElement.Visibility = Visibility.Visible;
            }
        }
    }
}