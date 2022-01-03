// <copyright file="IEncryptionService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods for encrypting and decrypting an entity with a key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IEncryptionService<TKey, TEntity>
        where TKey : class
        where TEntity : class
    {
        /// <summary>
        /// Encrypts the entity.
        /// </summary>
        /// <param name="key">A key which uses for encrypting.</param>
        /// <param name="entity">An entity which must be encrypted.</param>
        /// <param name="properties">The properties name, which must be encrypted.
        /// If it is <see langword="null"/> or empty all properties of the entity will be encrypted.</param>
        void Encrypt(TKey key, TEntity entity, IEnumerable<string> properties = null);

        /// <summary>
        /// Decrypts the entity.
        /// </summary>
        /// <param name="key">A key which uses for decrypting.</param>
        /// <param name="entity">An entity which must be decrypted.</param>
        /// <param name="properties">The properties name, which must be encrypted.
        /// If it is <see langword="null"/> or empty all properties of the entity will be decrypted.</param>
        void Decrypt(TKey key, TEntity entity, IEnumerable<string> properties = null);

        /// <summary>
        /// Encrypts the enumeration in async mode.
        /// </summary>
        /// <param name="key">A key which uses for encrypting.</param>
        /// <param name="entities">A list of entities which must be encrypted.</param>
        /// <param name="properties">The properties name, which must be encrypted.
        /// If it is <see langword="null"/> or empty all properties of the entity will be encrypted.</param>
        /// <returns>A task that represents the asynchronous encrypt operation.</returns>
        Task EncryptAsync(TKey key, IEnumerable<TEntity> entities, IEnumerable<string> properties = null);

        /// <summary>
        /// Decrypts the enumeration in async mode.
        /// </summary>
        /// <param name="key">A key which uses for decrypting.</param>
        /// <param name="entities">A list of entities which must be decrypted.</param>
        /// <param name="properties">The properties name, which must be decrypted.
        /// If it is <see langword="null"/> or empty all properties of the entity will be decrypted.</param>
        /// <returns>A task that represents the asynchronous decrypt operation.</returns>
        Task DecryptAsync(TKey key, IEnumerable<TEntity> entities, IEnumerable<string> properties = null);
    }
}
