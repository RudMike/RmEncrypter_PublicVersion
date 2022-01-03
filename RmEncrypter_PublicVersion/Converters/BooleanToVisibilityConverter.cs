// <copyright file="BooleanToVisibilityConverter.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Represents the converter that converts <see cref="bool"/> values to and from <see cref="Visibility"/> enum values.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="bool"/> value to a <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The current <see cref="bool"/> value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">Indicates whether to use the converter in direct mode.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Returns <see cref="Visibility.Visible"/> if the <see langword="value"/> is <see langword="true"/>
        /// and <see langword="parameter"/> is <see langword="true"/> or <see langword="null"/>. Otherwise returns <see cref="Visibility.Collapsed"/>.
        /// If <see langword="parameter"/> is <see langword="false"/> it returns <see cref="Visibility.Collapsed"/> if the <see langword="value"/> is <see langword="true"/>.
        /// Otherwise returns <see cref="Visibility.Visible"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "Reviewed")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? isDirect = parameter as bool?;
            if (isDirect == null || isDirect == true)
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return !(bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Converts a <see cref="Visibility"/> value to a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The current <see cref="Visibility"/> value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">Indicates whether to use the converter in direct mode.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Returns <see langword="true"/> if the <see langword="value"/> is <see cref="Visibility.Visible"/> and
        /// <see langword="parameter"/> is <see langword="true"/> or <see langword="null"/>. Otherwise returns <see langword="false"/>.
        /// If <see langword="parameter"/> is <see langword="false"/> it returns <see langword="false"/> if the <see langword="value"/> is <see cref="Visibility.Visible"/>.
        /// Otherwise returns <see langword="true"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "Reviewed")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? isDirect = parameter as bool?;
            if (isDirect == null || isDirect == true)
            {
                return (Visibility)value == Visibility.Visible;
            }
            else
            {
                return (Visibility)value != Visibility.Visible;
            }
        }

        /// <summary>
        /// Returns an object that should be set on the property where this extension is applied.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the markup extension provided value is evaluated.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
