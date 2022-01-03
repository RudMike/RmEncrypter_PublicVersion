// <copyright file="WidthToColumnsConverter.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Provides the converter for converting grid width to grid columns count.
    /// </summary>
    public class WidthToColumnsConverter : IValueConverter
    {
        /// <summary>
        /// Converts a grid width and minimal width of inner element to the grid columns count.
        /// </summary>
        /// <param name="value">A grid width.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">A minimal width of the grid inner element. Must include also horizontal margins if it has.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Returns columns count of the grid.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "<Reviewed>")]

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var gridWidth = (double)value;
            var elementWidth = System.Convert.ToDouble(parameter);
            int columnsCount;
            if (gridWidth != 0 && elementWidth != 0)
            {
                columnsCount = (int)(gridWidth / elementWidth);
            }
            else
            {
                columnsCount = 0;
            }

            return columnsCount;
        }

        /// <summary>
        /// Throws <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="value">This parameter is not used.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>This parameter is not used.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "<Reviewed>")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
