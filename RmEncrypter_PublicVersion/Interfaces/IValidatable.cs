// <copyright file="IValidatable.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides the functionality to offer validation errors.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Gets all model errors, where <see langword="TKey"/> is a property name, <see langword="TValue"/> is an error description.
        /// The dictionary is <see langword="Empty"/> if the model is valid.
        /// </summary>
        IDictionary<string, string> Errors { get; }
    }
}