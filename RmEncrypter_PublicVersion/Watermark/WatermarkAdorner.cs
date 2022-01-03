// <copyright file="WatermarkAdorner.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Adorner for the watermark.
    /// </summary>
    public class WatermarkAdorner : Adorner
    {
        private readonly ContentPresenter contentPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkAdorner"/> class.
        /// </summary>
        /// <param name="containerControl"><see cref="UIElement"/> which used as container for watermark.</param>
        /// <param name="watermark"><see cref="TextBlock"/> which used as a watermark.</param>
        public WatermarkAdorner(UIElement containerControl, TextBlock watermark)
            : base(containerControl)
        {
            this.IsHitTestVisible = false;
            this.contentPresenter = this.CreateContentPresenter(watermark);
            this.SetVisibilityBinding(containerControl);
            LocalizationService.LanguageChanged += AppLanguage_LanguageChanged;

            void AppLanguage_LanguageChanged(object sender, EventArgs e)
            {
                containerControl.Focus();
                Keyboard.ClearFocus();
                LocalizationService.LanguageChanged -= AppLanguage_LanguageChanged;
            }
        }

        /// <inheritdoc/>
        protected override int VisualChildrenCount => 1;

        /// <summary>
        /// Gets the control element that this adorner is bound to.
        /// </summary>
        private Control ContainerControl => (Control)this.AdornedElement;

        /// <inheritdoc/>
        protected override Visual GetVisualChild(int index) => this.contentPresenter;

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size constraint)
        {
            this.contentPresenter.Measure(this.ContainerControl.RenderSize);
            return this.ContainerControl.RenderSize;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }

        private ContentPresenter CreateContentPresenter(TextBlock watermark)
        {
            return new ContentPresenter
            {
                Content = watermark,
                Margin = new Thickness(
                    this.ContainerControl.Padding.Left + 2,
                    this.ContainerControl.Padding.Top,
                    this.ContainerControl.Padding.Right + 2,
                    this.ContainerControl.Padding.Bottom),
            };
        }

        private void SetVisibilityBinding(UIElement container)
        {
            var binding = new Binding("IsVisible")
            {
                Source = container,
                Converter = new BooleanToVisibilityConverter(),
            };
            this.SetBinding(VisibilityProperty, binding);
        }
    }
}
