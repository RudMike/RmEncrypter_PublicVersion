// <copyright file="OffsetValueConverter.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Provides the converter which change any number value by some number.
    /// </summary>
    [ValueConversion(typeof(double[]), typeof(double))]
    public class OffsetValueConverter : IValueConverter
    {
        /// <summary>
        /// Changes a number value by some offset.
        /// </summary>
        /// <param name="value">The number value which must be changed.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">Contains string where the first element is operation symbol,
        /// the second element is divider |,
        /// the third element is a offset (number by which the value should be changed).
        /// E.g. "-|10" means "value-10".</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>Returns the value changed by some offset.</returns>
        /// <exception cref="FormatException">Throws if the parameter has invalid format.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "<Reviewed>")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((double)value == 0)
            {
                return 0;
            }

            var parameters = TrySplitParameter(parameter);
            double offset = TryGetOffset(parameters);
            var operation = TryGetOperation(parameters[0]);
            return operation((double)value, offset);
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
            throw new NotSupportedException();
        }

        private static string[] TrySplitParameter(object parameter)
        {
            string parameterString = parameter as string;
            string[] parameters = null;

            if (!string.IsNullOrEmpty(parameter as string))
            {
                parameters = parameterString.Split(new char[] { '|' });
            }

            if (parameters.Length != 2)
            {
                throw new FormatException($"The parameter has wrong format. It must be e.g. {"\"-|50\""}.");
            }

            return parameters;
        }

        private static double TryGetOffset(string[] parameters)
        {
            try
            {
                return double.Parse(parameters[1].Replace('.', ','));
            }
            catch (FormatException)
            {
                throw new FormatException($"Unknown symbol {parameters[1]}. It must be a double number (e.g. {"\"10.5 or 10\""}).");
            }
        }

        private static Func<double, double, double> TryGetOperation(string mathOperator)
        {
            Func<double, double, double> operation;

            switch (mathOperator)
            {
                case "+":
                    {
                        operation = (arg0, arg1) => arg0 + arg1;
                        break;
                    }

                case "-":
                    {
                        operation = (arg0, arg1) => arg0 - arg1;
                        break;
                    }

                case "*":
                    {
                        operation = (arg0, arg1) => arg0 * arg1;
                        break;
                    }

                case "/":
                    {
                        operation = (arg0, arg1) => arg0 / arg1;
                        break;
                    }

                default:
                    {
                        throw new FormatException($"Unknown symbol {mathOperator}. It must be operation symbol (e.g. +-*/).");
                    }
            }

            return operation;
        }
    }
}
