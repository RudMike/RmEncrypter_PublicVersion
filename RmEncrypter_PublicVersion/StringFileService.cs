// <copyright file="StringFileService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Represents service for reading and writing strings from files.
    /// </summary>
    public class StringFileService : IFileService<string>
    {
        /// <inheritdoc/>
        public IEnumerable<string> Read(string path)
        {
            var result = new List<string>();
            using (StreamReader streamReader = new (path))
            {
                while (!streamReader.EndOfStream)
                {
                    result.Add(streamReader.ReadLine());
                }
            }

            return result.AsEnumerable();
        }

        /// <inheritdoc/>
        public void Write(IEnumerable<string> entities, string path)
        {
            using StreamWriter streamWriter = new (path, false);
            foreach (var entity in entities)
            {
                streamWriter.WriteLine(entity);
            }
        }
    }
}
