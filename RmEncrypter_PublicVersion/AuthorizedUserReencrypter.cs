// <copyright file="AuthorizedUserReencrypter.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RmEncrypter_PublicVersion.Exceptions;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Provides functionality for updating <see cref="RsaKey"/> of the <see cref="IAuthorizedUser"/> and reencrypting it files.
    /// </summary>
    public class AuthorizedUserReencrypter : IAccountReencrypter
    {
        private readonly IFileService<SiteAuthData> fileService;
        private readonly IEncryptionService<IRsaKey, SiteAuthData> encryptionService;
        private readonly IDatabaseWriter databaseWriter;
        private string stageDescription;
        private RsaKey newKey;
        private IEnumerable<string> filePaths;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedUserReencrypter"/> class.
        /// </summary>
        /// <param name="authorizedUser">The class which contains an authorized user data.</param>
        /// <param name="fileService">Service for writing/reading <see cref="SiteAuthData"/> entities from file.</param>
        /// <param name="encryptionService">A service for encrypting and decrypting entities.</param>
        /// <param name="databaseWriter">The class for writing in a database.</param>
        public AuthorizedUserReencrypter(IAuthorizedUser authorizedUser, IFileService<SiteAuthData> fileService, IEncryptionService<IRsaKey, SiteAuthData> encryptionService, IDatabaseWriter databaseWriter)
        {
            this.fileService = fileService;
            this.encryptionService = encryptionService;
            this.databaseWriter = databaseWriter;
            this.UserName = authorizedUser.UserName;
            this.OldKey = authorizedUser.RsaKey as RsaKey;
        }

        /// <inheritdoc/>
        public event EventHandler OnStageDescriptionChanged;

        /// <inheritdoc/>
        public string StageDescription
        {
            get => this.stageDescription;
            private set
            {
                this.stageDescription = value;
                this.OnStageDescriptionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public RsaKey OldKey
        { get; set; }

        /// <inheritdoc/>
        public string UserName
        { get; set; }

        /// <inheritdoc/>
        public async Task UpdateAsync(RsaKey newKey, IEnumerable<string> filePaths)
        {
            this.newKey = newKey;
            this.filePaths = filePaths;
            List<IEnumerable<SiteAuthData>> decryptedEntities = default;
            if (this.filePaths.Any())
            {
                this.StageDescription = LocalizationService.GetString("UpdateKeyStage1");
                decryptedEntities = await this.ReadAndDecrypt();
            }

            this.StageDescription = LocalizationService.GetString("UpdateKeyStage2");
            this.UpdateKeyInDb();

            if (decryptedEntities != null && decryptedEntities.Any())
            {
                this.StageDescription = LocalizationService.GetString("UpdateKeyStage3");
                await this.EncryptAndWrite(decryptedEntities);
            }
        }

        private async Task<List<IEnumerable<SiteAuthData>>> ReadAndDecrypt()
        {
            var decryptedEntities = new List<IEnumerable<SiteAuthData>>();
            foreach (var path in this.filePaths)
            {
                var encryptedEntities = this.fileService.Read(path);
                if (encryptedEntities == null)
                {
                    decryptedEntities.Add(null);
                    continue;
                }

                await this.TryDecryptAsync(encryptedEntities, path);
                decryptedEntities.Add(encryptedEntities);
            }

            return decryptedEntities;
        }

        private void UpdateKeyInDb()
        {
            _ = this.databaseWriter.UpdateRsaKey(this.UserName, this.newKey);
        }

        private async Task EncryptAndWrite(List<IEnumerable<SiteAuthData>> entities)
        {
            int index = 0;
            foreach (var entity in entities)
            {
                if (entity != null)
                {
                    await this.encryptionService.EncryptAsync(this.newKey, entity);
                    this.fileService.Write(entity, this.filePaths.ElementAt(index));
                }

                index++;
            }
        }

        private async Task TryDecryptAsync(IEnumerable<SiteAuthData> entities, string filePath)
        {
            try
            {
                await this.encryptionService.DecryptAsync(this.OldKey, entities);
            }
            catch (DecryptException ex)
            {
                throw new DecryptException(LocalizationService.GetString("ErrorKey"), filePath, ex);
            }
        }
    }
}
