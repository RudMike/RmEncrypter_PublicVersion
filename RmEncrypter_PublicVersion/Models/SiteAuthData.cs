// <copyright file="SiteAuthData.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Contains record about the authorization data of the user on a site.
    /// </summary>
    public class SiteAuthData : ICloneable, IEquatable<SiteAuthData>
    {
        /// <summary>
        /// Gets or sets the site name.
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Gets or sets the username for the site.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for the site.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the note for the site.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets max length of the note. Default value is <see langword="100"/>.
        /// </summary>
        [JsonIgnore]
        public int NoteMaxLength { get; set; } = 100;

        /// <inheritdoc/>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return this.Equals((SiteAuthData)obj);
        }

        /// <inheritdoc/>
        public bool Equals(SiteAuthData other)
        {
            if ((other == null) || !this.GetType().Equals(other.GetType()))
            {
                return false;
            }
            else if (ReferenceEquals(this, other))
            {
                return true;
            }
            else
            {
                return this.IsPropertiesEquals(other);
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (this.Site + this.UserName + this.Password + this.Note).GetHashCode();
        }

        private bool IsPropertiesEquals(SiteAuthData other)
        {
            return this.Site == other.Site
                   && this.UserName == other.UserName
                   && this.Password == other.Password
                   && this.Note == other.Note;
        }
    }
}
