// <copyright file="WindowClosingBehavior.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using Microsoft.Xaml.Behaviors;

    /// <summary>
    /// Allows the behavior for a <see cref="Window"/> which managing it closing.
    /// </summary>
    public class WindowClosingBehavior : Behavior<Window>
    {
        /// <summary>
        /// Identifies the <see cref="CanClose"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CanCloseProperty = DependencyProperty.Register(
            "CanClose",
            typeof(Func<bool>),
            typeof(WindowClosingBehavior),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the delegate which cancel window's closing if returns <see langword="false"/>.
        /// </summary>
        public Func<bool> CanClose
        {
            get { return (Func<bool>)this.GetValue(CanCloseProperty); }
            set { this.SetValue(CanCloseProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Closing += this.AssociatedObject_Closing;
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            this.AssociatedObject.Closing -= this.AssociatedObject_Closing;
            base.OnDetaching();
        }

        private void AssociatedObject_Closing(object sender, CancelEventArgs e)
        {
            if (this.CanClose() == false)
            {
                e.Cancel = true;
            }
        }
    }
}
