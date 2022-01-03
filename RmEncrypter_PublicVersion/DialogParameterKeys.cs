// <copyright file="DialogParameterKeys.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides the access to the all keys of the dialog parameters.
    /// </summary>
    public static class DialogParameterKeys
    {
        /// <summary>
        /// Gets the navigation parameter's key of an entity.
        /// </summary>
        public const string Entity = "entity";

        /// <summary>
        /// Gets the navigation parameter's key with a state of an entity.
        /// </summary>
        public const string State = "state";

        /// <summary>
        /// Gets the navigation parameter's key which indicates whether an entity was changed.
        /// </summary>
        public const string IsChanged = "isChanged";
    }
}
