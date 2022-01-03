// <copyright file="SiteAuthDataListManager.cs" company="Mike Rudnikov">
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
    /// Represents functionality for managing the list with the <see cref="SiteAuthData"/> encrypted entities.
    /// </summary>
    public class SiteAuthDataListManager : IEncryptedListManager<SiteAuthData>
    {
        private readonly IEncryptionService<IRsaKey, SiteAuthData> encryptionService;
        private readonly RsaKey rsaKey;
        private readonly List<string> headerProperty = new () { nameof(SiteAuthData.Site) };

        /// <summary>
        /// The list which contains records as decrypted as possible.
        /// </summary>
        private readonly List<(SiteAuthData entity, EntityStates state)> currentList = new ();

        /// <summary>
        /// The list which contains records as encrypted as possible.
        /// </summary>
        private readonly List<(SiteAuthData entity, EntityStates state)> maxEncryptedList = new ();
        private string headerFilter = string.Empty;
        private List<Tuple<int, string>> headers;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteAuthDataListManager"/> class.
        /// </summary>
        /// <param name="encryptionService">A service for encrypting and decrypting entities.</param>
        /// <param name="authorizedUser">The class which contains an authorized user data.</param>
        public SiteAuthDataListManager(IEncryptionService<IRsaKey, SiteAuthData> encryptionService, IAuthorizedUser authorizedUser)
        {
            this.encryptionService = encryptionService;
            this.rsaKey = (RsaKey)authorizedUser.RsaKey;
        }

        /// <inheritdoc/>
        public event EventHandler OnHeadersChanged;

        /// <inheritdoc/>
        public List<Tuple<int, string>> Headers
        {
            get => this.headers;
            set
            {
                this.headers = value;
                this.OnHeadersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public async Task LoadAsync(IEnumerable<SiteAuthData> encryptedList)
        {
            this.Clear();
            CloneToMaxEncryptedList();
            await this.encryptionService.DecryptAsync(this.rsaKey, encryptedList, this.headerProperty);
            FillCurrentList();
            this.UpdateHeaders();

            void CloneToMaxEncryptedList()
            {
                this.maxEncryptedList.AddRange(encryptedList.Select(entity => ((SiteAuthData)entity.Clone(), EntityStates.FullyEncrypted)));
            }

            void FillCurrentList()
            {
                foreach (var item in encryptedList)
                {
                    this.currentList.Add((item, EntityStates.HeaderDecrypted));
                }
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SiteAuthData>> UnloadAsync()
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < this.maxEncryptedList.Count; i++)
                {
                    switch (this.maxEncryptedList[i].state)
                    {
                        case EntityStates.HeaderDecrypted:
                            this.EncryptHeader(this.maxEncryptedList[i].entity);
                            this.maxEncryptedList[i] = (this.maxEncryptedList[i].entity, EntityStates.FullyEncrypted);
                            break;

                        case EntityStates.FullyDecrypted:
                            this.EncryptAll(this.maxEncryptedList[i].entity);
                            this.maxEncryptedList[i] = (this.maxEncryptedList[i].entity, EntityStates.FullyEncrypted);
                            break;
                    }
                }
            });

            return this.maxEncryptedList.Select(tuple => tuple.entity);
        }

        /// <inheritdoc/>
        public void Filter(string filter)
        {
            this.headerFilter = filter;
            this.UpdateHeaders();
        }

        /// <inheritdoc/>
        public void Add(SiteAuthData entity, EntityStates state)
        {
            this.maxEncryptedList.Add(((SiteAuthData)entity.Clone(), state));

            if (state == EntityStates.FullyEncrypted)
            {
                this.DecryptHeader(entity);
                state = EntityStates.HeaderDecrypted;
            }

            this.currentList.Add((entity, state));
            this.UpdateHeaders();
        }

        /// <inheritdoc/>
        public void Remove(int headerId)
        {
            this.maxEncryptedList.RemoveAt(headerId);
            this.currentList.RemoveAt(headerId);
            this.UpdateHeaders();
        }

        /// <inheritdoc/>
        public void Update(SiteAuthData entity, int headerId, EntityStates state, bool isChanged)
        {
            if (isChanged)
            {
                this.maxEncryptedList[headerId] = ((SiteAuthData)entity.Clone(), state);
            }

            if (state == EntityStates.FullyEncrypted)
            {
                this.DecryptHeader(entity);
                state = EntityStates.HeaderDecrypted;
            }

            this.currentList[headerId] = (entity, state);
            this.UpdateHeaders();
        }

        /// <inheritdoc/>
        public (SiteAuthData entity, EntityStates state) Copy(int headerId)
        {
            return ((SiteAuthData)this.currentList[headerId].entity.Clone(), this.currentList[headerId].state);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.currentList.Clear();
            this.maxEncryptedList.Clear();
            this.UpdateHeaders();
        }

        private void EncryptHeader(SiteAuthData entity)
        {
            this.encryptionService.Encrypt(this.rsaKey, entity, this.headerProperty);
        }

        private void EncryptAll(SiteAuthData entity)
        {
            var allProperties = new List<string>()
            {
                nameof(SiteAuthData.Site),
                nameof(SiteAuthData.UserName),
                nameof(SiteAuthData.Password),
                nameof(SiteAuthData.Note),
            };
            this.encryptionService.Encrypt(this.rsaKey, entity, allProperties);
        }

        private void DecryptHeader(SiteAuthData entity)
        {
            this.encryptionService.Decrypt(this.rsaKey, entity, this.headerProperty);
        }

        private void UpdateHeaders()
        {
            if (this.headerFilter == string.Empty)
            {
                this.Headers = this.currentList
                    .Select((obj, index) => new Tuple<int, string>(index, obj.entity.Site))
                    .ToList();
            }
            else
            {
                this.Headers = this.currentList
                    .Select((obj, index) => new Tuple<int, string>(index, obj.entity.Site))
                    .Where((obj) => obj.Item2.Contains(this.headerFilter))
                    .ToList();
            }
        }
    }
}
