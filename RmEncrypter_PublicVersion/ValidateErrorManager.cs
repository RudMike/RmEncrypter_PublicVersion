// <copyright file="ValidateErrorManager.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides functionality for adding and removing validation errors.
    /// </summary>
    public abstract class ValidateErrorManager
    {
        /// <summary>
        /// Gets or sets all model errors, where <see langword="TKey"/> is a property name, <see langword="TValue"/> is the error description.
        /// Dictionary is <see langword="Empty"/> if the model is valid.
        /// </summary>
        protected virtual IDictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Try to add error to the <see cref="Errors"/> if current property doesn't have an error.
        /// Do nothing if the property has any error.
        /// </summary>
        /// <param name="propName">The name of the property for which trying to add an error.</param>
        /// <param name="errorMessage">Description for the current error.</param>
        /// <returns>Returns <see langword="true"/> if any error was added. Otherwise return <see langword="false"/>.</returns>
        protected virtual bool TryAddError(string propName, string errorMessage)
        {
            bool isAdded = false;
            if (!this.Errors.ContainsKey(propName))
            {
                this.Errors.Add(propName, errorMessage);
                isAdded = true;
            }

            return isAdded;
        }

        /// <summary>
        /// Try to remove error from the <see cref="Errors"/> for the passed property.
        /// Do nothing if the property doesn't contains an error.
        /// </summary>
        /// <param name="propName">The name of the property for which trying to remove an error.</param>
        /// <returns>Returns <see langword="true"/> if any error was deleted. Otherwise return <see langword="false"/>.</returns>
        protected virtual bool TryRemoveError(string propName)
        {
            bool isRemoved = false;
            if (this.Errors.ContainsKey(propName))
            {
                this.Errors.Remove(propName);
                isRemoved = true;
            }

            return isRemoved;
        }
    }
}
