// <copyright file="BindablePassword.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Provide service for <see cref="PasswordBox"/> which allow to use it <see cref="PasswordBox.Password"/> property bindable as well.
    /// </summary>
    public static class BindablePassword
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for using bindable password of the <see cref="PasswordBox"/>.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
                "Password",
                typeof(string),
                typeof(BindablePassword),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordPropertyChanged,
                    OnPasswordPropertyChanging));

        /// <summary>
        /// Gets the current value of the <see cref="PasswordProperty"/>.
        /// </summary>
        /// <param name="dependencyObject">The identifier of the dependency property to get.</param>
        /// <returns>A string value of the <see cref="PasswordProperty"/>.</returns>
        public static string GetPassword(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(PasswordProperty);
        }

        /// <summary>
        /// Sets the local value of the <see cref="PasswordProperty"/>.
        /// </summary>
        /// <param name="dependencyObject">The identifier of the dependency property to set.</param>
        /// <param name="value">A string value for setting the <see cref="PasswordProperty"/>.</param>
        public static void SetPassword(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(PasswordProperty, value);
        }

        private static object OnPasswordPropertyChanging(DependencyObject sender, object baseValue)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged += OnPasswordChanged;
            }

            return baseValue;
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.Password = (string)e.NewValue;
                SetCaretToEnd(passwordBox);
            }
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= OnPasswordChanged;
                SetPassword(passwordBox, passwordBox.Password);
            }
        }

        private static void SetCaretToEnd(PasswordBox passwordBox)
        {
            passwordBox.GetType()
                       .GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic)
                       .Invoke(passwordBox, new object[] { passwordBox.Password.Length, 0 });
        }
    }
}
