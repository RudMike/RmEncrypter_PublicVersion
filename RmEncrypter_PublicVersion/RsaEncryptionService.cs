// <copyright file="RsaEncryptionService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides the service for encryption/description <see cref="SiteAuthData"/> entities with using an <see cref="IRsaKey"/> key.
    /// </summary>
    public class RsaEncryptionService : RsaTool, IEncryptionService<IRsaKey, SiteAuthData>
    {
        /// <inheritdoc/>
        public void Decrypt(IRsaKey key, SiteAuthData entity, IEnumerable<string> properties = null)
        {
            if (IsNullOrEmpty(properties))
            {
                properties = CreatePropertiesEnumeration();
            }

            var maxDegreeOfParallelism = properties.Count();
            var decrypting = CreateOperations(key, entity, this.Decrypt, properties);
            try
            {
                Task.Run(() => decrypting.AsParallel()
                         .WithDegreeOfParallelism(maxDegreeOfParallelism)
                         .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                         .ForAll((action) => action()))
                    .Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.Flatten().InnerException;
            }
        }

        /// <inheritdoc/>
        public Task DecryptAsync(IRsaKey key, IEnumerable<SiteAuthData> entities, IEnumerable<string> properties = null)
        {
            return Task.Run(() =>
            {
                entities.AsParallel()
                        .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                        .ForAll((entity) => this.Decrypt(key, entity, properties));
            }).ContinueWith(
                task =>
                {
                    if (task.Exception is AggregateException aggregateException)
                    {
                        throw aggregateException.Flatten().InnerException;
                    }
                });
        }

        /// <inheritdoc/>
        public void Encrypt(IRsaKey key, SiteAuthData entity, IEnumerable<string> properties = null)
        {
            if (IsNullOrEmpty(properties))
            {
                properties = CreatePropertiesEnumeration();
            }

            var maxDegreeOfParallelism = properties.Count();
            var encrypting = CreateOperations(key, entity, this.Encrypt, properties);
            try
            {
                Task.Run(() => encrypting.AsParallel()
                         .WithDegreeOfParallelism(maxDegreeOfParallelism)
                         .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                         .ForAll((action) => action()))
                    .Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.Flatten().InnerException;
            }
        }

        /// <inheritdoc/>
        public Task EncryptAsync(IRsaKey key, IEnumerable<SiteAuthData> entities, IEnumerable<string> properties = null)
        {
            return Task.Run(() =>
            {
                entities.AsParallel()
                        .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                        .ForAll((entity) => this.Encrypt(key, entity, properties));
            });
        }

        private static List<Action> CreateOperations(IRsaKey key, SiteAuthData entity, Func<string, IRsaKey, string> action, IEnumerable<string> properties)
        {
            var resultOperations = new List<Action>();

            foreach (var property in properties)
            {
                if (IsSite(property))
                {
                    resultOperations.Add(CreateSiteAction(key, entity, action));
                }
                else if (IsUserName(property))
                {
                    resultOperations.Add(CreateUserNameAction(key, entity, action));
                }
                else if (IsPassword(property))
                {
                    resultOperations.Add(CreatePasswordAction(key, entity, action));
                }
                else if (IsNote(property))
                {
                    resultOperations.Add(CreateNoteAction(key, entity, action));
                }
            }

            return resultOperations;
        }

        private static bool IsSite(string property)
        {
            return property.ToLower() == nameof(SiteAuthData.Site).ToLower();
        }

        private static bool IsUserName(string property)
        {
            return property.ToLower() == nameof(SiteAuthData.UserName).ToLower();
        }

        private static bool IsPassword(string property)
        {
            return property.ToLower() == nameof(SiteAuthData.Password).ToLower();
        }

        private static bool IsNote(string property)
        {
            return property.ToLower() == nameof(SiteAuthData.Note).ToLower();
        }

        private static Action CreateSiteAction(IRsaKey key, SiteAuthData entity, Func<string, IRsaKey, string> action)
        {
            return () => entity.Site = action(entity.Site, key);
        }

        private static Action CreateUserNameAction(IRsaKey key, SiteAuthData entity, Func<string, IRsaKey, string> action)
        {
            return () => entity.UserName = action(entity.UserName, key);
        }

        private static Action CreatePasswordAction(IRsaKey key, SiteAuthData entity, Func<string, IRsaKey, string> action)
        {
            return () => entity.Password = action(entity.Password, key);
        }

        private static Action CreateNoteAction(IRsaKey key, SiteAuthData entity, Func<string, IRsaKey, string> action)
        {
            return () => entity.Note = action(entity.Note, key);
        }

        private static IEnumerable<string> CreatePropertiesEnumeration()
        {
            return new List<string>()
            {
                nameof(SiteAuthData.Site),
                nameof(SiteAuthData.UserName),
                nameof(SiteAuthData.Password),
                nameof(SiteAuthData.Note),
            }.AsEnumerable();
        }

        private static bool IsNullOrEmpty(IEnumerable<string> enumeration)
        {
            return enumeration == null || !enumeration.Any();
        }
    }
}
