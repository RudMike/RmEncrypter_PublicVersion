// <copyright file="LocalizationService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Windows;

    /// <summary>
    /// Provides service for localization the application.
    /// </summary>
    internal static class LocalizationService
    {
        private static readonly ResourceManager ResourceManager;
        private static Languages language;
        private static CultureInfo cultureInfo;

        static LocalizationService()
        {
            cultureInfo = new CultureInfo(Language.ToString());
            ResourceManager = new ResourceManager("RmEncrypter_PublicVersion.Resources.Localize.MessagesLocalize", Assembly.GetExecutingAssembly());
            LanguageChanged += AppLanguage_LanguageChanged;
        }

        /// <summary>
        /// Occurs when the language of the program was changed.
        /// </summary>
        public static event EventHandler LanguageChanged;

        /// <summary>
        /// Gets all supported languages for the program.
        /// </summary>
        public static IEnumerable<Languages> Languages
        {
            get => Enum.GetValues(typeof(Languages)).Cast<Languages>();
        }

        /// <summary>
        /// Gets or sets the current language of the program.
        /// </summary>
        public static Languages Language
        {
            get => language;

            set
            {
                if (value != language)
                {
                    language = value;
                    LanguageChanged(Application.Current, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Returns the value of the string resource localized for the specified culture.
        /// </summary>
        /// <param name="name">The name of the resource to retrieve.</param>
        /// <returns>The value of the resource localized for the specified culture, or null if name cannot be found in a resource set.</returns>
        /// <exception cref="ArgumentNullException">The name parameter is null.</exception>
        public static string GetString(string name)
        {
            return ResourceManager.GetString(name, cultureInfo);
        }

        private static void AppLanguage_LanguageChanged(object sender, EventArgs e)
        {
            var languageCode = Language.ToString();
            cultureInfo = new CultureInfo(languageCode);
            ChangeUILanguage(languageCode);
            SaveLanguage();
        }

        private static void ChangeUILanguage(string languageCode)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
            ResourceDictionary dict = CreateResourceDictionary(languageCode);
            ResourceDictionary oldDict = FindOldDictionary();

            if (oldDict != null)
            {
                ReplaceOldDictionary(oldDict, dict);
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }

        private static ResourceDictionary CreateResourceDictionary(string languageCode)
        {
            ResourceDictionary dictionary = new ();
            dictionary.Source = Language switch
            {
                RmEncrypter_PublicVersion.Languages.Ru => new Uri(
                    string.Format("Resources/Localize/UILocalize.{0}.xaml", languageCode),
                    UriKind.Relative),

                _ => new Uri("Resources/Localize/UILocalize.xaml", UriKind.Relative),
            };

            return dictionary;
        }

        private static ResourceDictionary FindOldDictionary()
        {
            var mergedDictionares = Application.Current.Resources.MergedDictionaries;
            return mergedDictionares.First(
                dict => dict.Source != null && dict.Source.OriginalString.StartsWith("Resources/Localize/UILocalize."));
        }

        private static void ReplaceOldDictionary(ResourceDictionary oldDict, ResourceDictionary newDict)
        {
            var mergedDictionares = Application.Current.Resources.MergedDictionaries;
            int index = mergedDictionares.IndexOf(oldDict);
            _ = mergedDictionares.Remove(oldDict);
            mergedDictionares.Insert(index, newDict);
        }

        private static void SaveLanguage()
        {
            Properties.Settings.Default.CurrentLanguage = Language;
            Properties.Settings.Default.Save();
        }
    }
}
