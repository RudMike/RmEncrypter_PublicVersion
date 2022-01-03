// <copyright file="EnumToBooleanConverter.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Represents the converter that converts enumeration values to and from <see cref="bool"/> values.
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBooleanConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Converts an enumeration value to a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The current <see cref="Enum"/> value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">The enumeration value which compare with <see langword="value"/> parameter of the method.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Returns <see langword="true"/> if the value is equals to the parameter. Otherwise <see langword="false"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "Reviewed")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(parameter);
        }

        /// <summary>
        /// Converts a <see cref="bool"/> value to an enumeration value.
        /// </summary>
        /// <param name="value">The current <see cref="bool"/> value.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">The value of the <see cref="Enum"/>.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Returns <see langword="parameter"/> if the <see langword="value"/> is <see langword="true"/>. Otherwise <see cref="Binding.DoNothing"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "Reviewed")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
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
