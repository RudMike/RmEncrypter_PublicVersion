// <copyright file="WatermarkService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>
    /// Provide service for attaching <see cref="WatermarkProperty"/>.
    /// </summary>
    public static class WatermarkService
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for using watermark.
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty;

        static WatermarkService()
        {
            WatermarkProperty = DependencyProperty.RegisterAttached(
            "Watermark",
            typeof(TextBlock),
            typeof(WatermarkService),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnWatermarkChanged)));
        }

        /// <summary>
        /// Sets the local value of <see cref="WatermarkProperty"/> and binding it with a <see cref="TextBlock"/>.
        /// </summary>
        /// <param name="dependencyObject">The identifier of the dependency property to set.</param>
        /// <param name="value">A <see cref="TextBlock"/> for using as watermark.</param>
        public static void SetWatermark(DependencyObject dependencyObject, TextBlock value)
        {
            dependencyObject.SetValue(WatermarkProperty, value);
        }

        /// <summary>
        /// Gets the current <see cref="TextBlock"/> of the dependency property which used as watermark.
        /// </summary>
        /// <param name="dependencyObject">The identifier of the dependency property to get.</param>
        /// <returns><see cref="TextBlock"/> which used as watermark for <see cref="WatermarkProperty"/>.</returns>
        public static TextBlock GetWatermark(DependencyObject dependencyObject)
        {
            return (TextBlock)dependencyObject.GetValue(WatermarkProperty);
        }

        private static void OnWatermarkChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var containerControl = (Control)dependencyObject;
            var watermark = (TextBlock)e.NewValue;

            containerControl.Loaded += ManageVisibility;
            containerControl.GotKeyboardFocus += ManageVisibility;
            containerControl.LostKeyboardFocus += ManageVisibility;

            if (containerControl is TextBox textBox)
            {
                textBox.TextChanged += ManageVisibility;
            }
            else if (containerControl is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged += ManageVisibility;
            }
            else
            {
                throw new NotSupportedException("The service supports only TextBox and PasswordBox control types.");
            }
        }

        private static void ManageVisibility(object sender, RoutedEventArgs e)
        {
            var containerControl = (Control)sender;

            if (GetTextLenght(containerControl) == 0)
            {
                ShowWatermark(containerControl);
            }
            else
            {
                HideWatermark(containerControl);
            }
        }

        private static int GetTextLenght(Control container)
        {
            int result = -1;
            if (container is TextBox textBox)
            {
                result = textBox.Text.Length;
            }
            else if (container is PasswordBox passwordBox)
            {
                result = passwordBox.Password.Length;
            }

            return result;
        }

        private static void ShowWatermark(Control container)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(container);

            if (layer != null)
            {
                layer.Add(new WatermarkAdorner(container, GetWatermark(container)));
            }
        }

        private static void HideWatermark(Control container)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(container);

            if (layer != null)
            {
                Adorner[] adorners = layer.GetAdorners(container);
                if (adorners != null)
                {
                    foreach (Adorner adorner in adorners)
                    {
                        if (adorner is WatermarkAdorner)
                        {
                            adorner.Visibility = Visibility.Hidden;
                            layer.Remove(adorner);
                        }
                    }
                }
            }
        }
    }
}
