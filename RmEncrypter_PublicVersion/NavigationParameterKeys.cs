// <copyright file="NavigationParameterKeys.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides the access to the all keys of the navigation parameters.
    /// </summary>
    public static class NavigationParameterKeys
    {
        /// <summary>
        /// Gets the navigation parameter's key of an username.
        /// </summary>
        public const string UserName = "username";

        /// <summary>
        /// Gets the navigation parameter's key with path to the selected file.
        /// </summary>
        public const string FilePath = "path";

        /// <summary>
        /// Gets the navigation parameter's key with paths to the selected files.
        /// </summary>
        public const string FilePaths = "paths";

        /// <summary>
        /// Gets the navigation parameter's key which indicates whether to save a key for an account.
        /// </summary>
        public const string IsSaveKey = "isSaveKey";

        /// <summary>
        /// Gets the navigation parameter's key which contain the key of an account.
        /// </summary>
        public const string Key = "key";

        /// <summary>
        /// Gets the navigation parameter's key which contain the navigation parameter for a next view.
        /// </summary>
        public const string NextViewInfo = "nextViewInfo";
    }
}
