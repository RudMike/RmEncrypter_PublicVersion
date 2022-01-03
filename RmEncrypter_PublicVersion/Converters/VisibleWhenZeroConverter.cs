// <copyright file="VisibleWhenZeroConverter.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Represents the converter that converts a number value to and from <see cref="Visibility"/> enum value.
    /// </summary>
    public class VisibleWhenZeroConverter : IValueConverter
    {
        /// <summary>
        /// Converts a number value to a <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The current number value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">Indicates whether to use the converter in invert mode.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Returns <see cref="Visibility.Visible"/> if the <see langword="value"/> is equals zero. Otherwise returns <see cref="Visibility.Collapsed"/>.
        /// If <see langword="parameter"/> is <see langword="true"/> the returns is inverts.</returns>
        /// <exception cref="ArgumentException">Throws if the parameter is not <see cref="bool"/> type.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "Reviewed")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result;
            if (parameter == null || (bool)parameter == false)
            {
                result = (int)value == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if ((bool)parameter == true)
            {
                result = (int)value != 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                throw new ArgumentException("The parameter must be a bool value.");
            }

            return result;
        }

        /// <summary>
        /// Throws <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="value">This parameter is not used.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>This parameter is not used.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "Reviewed")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
