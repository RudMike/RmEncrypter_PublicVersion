// <copyright file="JsonFileService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using RmEncrypter_PublicVersion.Models;

    /// <summary>
    /// Represents service for reading and writing from files with using json format.
    /// </summary>
    public class JsonFileService : IFileService<SiteAuthData>
    {
        /// <inheritdoc/>
        public IEnumerable<SiteAuthData> Read(string path)
        {
            FileExistCheck(path);
            string jsonEntities = ReadFile(path);
            IEnumerable<SiteAuthData> result = null;

            try
            {
                result = Deserialize(jsonEntities);
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is JsonReaderException)
                {
                    throw new FileLoadException(LocalizationService.GetString("ErrorFileContent"), path);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public void Write(IEnumerable<SiteAuthData> entities, string path)
        {
            FileExistCheck(path);
            var result = Serialize(entities);
            using StreamWriter streamWriter = new (path, false);
            streamWriter.Write(result);
        }

        private static void FileExistCheck(string path)
        {
            if (!File.Exists(path))
            {
                throw new IOException(LocalizationService.GetString("ErrorFileNotExist"));
            }
        }

        private static string ReadFile(string path)
        {
            using StreamReader streamReader = new (path);
            return streamReader.ReadToEnd();
        }

        private static string Serialize(IEnumerable<SiteAuthData> entities)
        {
            string jsonEntities = JsonConvert.SerializeObject(entities);
            byte[] bytes = Encoding.UTF8.GetBytes(jsonEntities);
            return Convert.ToBase64String(bytes);
        }

        private static IEnumerable<SiteAuthData> Deserialize(string jsonEntities)
        {
            byte[] bytes = Convert.FromBase64String(jsonEntities);
            return JsonConvert.DeserializeObject<IEnumerable<SiteAuthData>>(Encoding.UTF8.GetString(bytes));
        }
    }
}