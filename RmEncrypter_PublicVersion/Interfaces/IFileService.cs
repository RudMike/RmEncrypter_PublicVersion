// <copyright file="IFileService.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides methods for reading and writing in files.
    /// </summary>
    /// <typeparam name="T">The data type for reading/writing.</typeparam>
    public interface IFileService<T>
        where T : class
    {
        /// <summary>
        /// Write entities in the file.
        /// </summary>
        /// <param name="entities">List of the entities for writing.</param>
        /// <param name="path">File path for writing.</param>
        /// <exception cref="System.IO.IOException">Throws if an I/O error occurs.</exception>
        void Write(IEnumerable<T> entities, string path);

        /// <summary>
        /// Read entities from the file.
        /// </summary>
        /// <param name="path">File path for reading.</param>
        /// <returns>Returns list of the readed entities.</returns>
        /// <exception cref="System.IO.IOException">Throws if an I/O error occurs.</exception>
        /// <exception cref="System.IO.FileLoadException">Throws if the file content can't be read.</exception>
        IEnumerable<T> Read(string path);
    }
}
