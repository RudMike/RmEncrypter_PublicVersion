// <copyright file="ToCenterConverter.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Represents the converter for multibinding that converts width of a placement target and control to horisontal offset value
    /// so that control will be at width center of the placement target.
    /// </summary>
    [ValueConversion(typeof(double[]), typeof(double))]
    public class ToCenterConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts width of a placement target and control to horisontal offset value.
        /// </summary>
        /// <param name="values">Array where the first element is placement target width,
        /// second is the control width for centering in.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Returns horizontal offset for the control.</returns>
        /// <exception cref="NotSupportedException">Throws if the values array contains not two elements.</exception>
        /// <exception cref="InvalidCastException">Throws if an element from the values array is not <see cref="double"/> type.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "<Reviewed>")]
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                throw new NotSupportedException("The method supports only two input values.");
            }

            double placementTargetWidth = (double)values[0];
            double toolTipWidth = (double)values[1];
            return (placementTargetWidth / 2.0) - (toolTipWidth / 2.0);
        }

        /// <summary>
        /// Throws <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="value">This parameter is not used.</param>
        /// <param name="targetTypes">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>This parameter is not used.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "<Reviewed>")]
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
