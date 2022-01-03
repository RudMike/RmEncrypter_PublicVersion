// <copyright file="IModelValidator.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents logic for validation a model.
    /// </summary>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    public interface IModelValidator<TModel>
    {
        /// <summary>
        /// Validate the model.
        /// </summary>
        /// <param name="model">Model for validation.</param>
        /// <returns>Returns collection, where <see langword="TKey"/> is a property name, <see langword="TValue"/> is the error description.</returns>
        IDictionary<string, string> Validate(TModel model);
    }
}
