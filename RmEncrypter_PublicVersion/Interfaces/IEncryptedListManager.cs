// <copyright file="IEncryptedListManager.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents functionality for managing the list with an encrypted entities.
    /// </summary>
    /// <typeparam name="T">The type of the entities.</typeparam>
    public interface IEncryptedListManager<T>
    {
        /// <summary>
        /// Occurs when <see cref="Headers"/> was changed.
        /// </summary>
        event EventHandler OnHeadersChanged;

        /// <summary>
        /// Gets the tuple where first element is ID of the entity, second element is decrypted header of the entity.
        /// </summary>
        List<Tuple<int, string>> Headers { get; }

        /// <summary>
        /// Loads encrypted enumeration in the manager.
        /// </summary>
        /// <param name="encryptedList">The enumeration with encrypted entities for managing.</param>
        /// <returns>A task that represents the asynchronous loads operation.</returns>
        Task LoadAsync(IEnumerable<T> encryptedList);

        /// <summary>
        /// Unloads all encrypted entities from the list of the manager.
        /// </summary>
        /// <returns>Returns the enumerable with encrypted entities.</returns>
        Task<IEnumerable<T>> UnloadAsync();

        /// <summary>
        /// Filter records in the Headers list.
        /// </summary>
        /// <param name="filter">A filter string.</param>
        void Filter(string filter);

        /// <summary>
        /// Adds new entity in the manager.
        /// </summary>
        /// <param name="entity">An entity for adding.</param>
        /// <param name="state">The encryption state of the being added entity.</param>
        void Add(T entity, EntityStates state);

        /// <summary>
        /// Removes entity from the manager.
        /// </summary>
        /// <param name="headerId">Id of the being removed entity.</param>
        void Remove(int headerId);

        /// <summary>
        /// Updates an entity for the new.
        /// It allows get fully decryped copy of the entity with the <see cref="Copy(int)"/> method
        /// and don't encrypt the entity again with the <see cref="UnloadAsync"/> method.
        /// </summary>
        /// <param name="entity">A new entity.</param>
        /// <param name="headerId">Id of the updates entity.</param>
        /// <param name="state">A new state of the updates entity.</param>
        /// <param name="isChanged"><see langword="true"/> if the entity was changed.
        /// <see langword="false"/> if the entity was decrypted and changed only your own state.</param>
        void Update(T entity, int headerId, EntityStates state, bool isChanged);

        /// <summary>
        /// Copies an entity from the manager.
        /// </summary>
        /// <param name="headerId">Id of the being copied entity.</param>
        /// <returns>Returns tuple where first element is copied entity,
        /// second element is the encryption state of the entity.</returns>
        (T entity, EntityStates state) Copy(int headerId);

        /// <summary>
        /// Removes all entities from the manager.
        /// </summary>
        void Clear();
    }
}
