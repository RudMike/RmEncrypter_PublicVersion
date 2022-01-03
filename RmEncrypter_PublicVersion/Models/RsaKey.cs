// <copyright file="RsaKey.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.Models
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    /// <summary>
    /// Provides the user's Rsa-key, stored in a database.
    /// </summary>
    public class RsaKey : IDbRsaKey, ICloneable
    {
        private readonly IModelValidator<RsaKey> validator;
        private BigInteger multiplyResultB;
        private BigInteger privateExponentB;
        private BigInteger publicExponentB;
        private string multiplyResultS;
        private string privateExponentS;
        private string publicExponentS;

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaKey"/> class.
        /// </summary>
        /// <param name="validator">The validator for validating this model.</param>
        public RsaKey(IModelValidator<RsaKey> validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaKey"/> class.
        /// </summary>
        /// <param name="multiplyResult">Multiply result of the key.</param>
        /// <param name="privateExponent">Private exponent of the key.</param>
        /// <param name="publicExponent">Public exponent of the key.</param>
        public RsaKey(BigInteger multiplyResult, BigInteger privateExponent, BigInteger publicExponent)
        {
            ((IRsaKey)this).MultiplyResult = multiplyResult;
            ((IRsaKey)this).PrivateExponent = privateExponent;
            ((IRsaKey)this).PublicExponent = publicExponent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaKey"/> class.
        /// </summary>
        /// <param name="multiplyResult">Multiply result of the key.</param>
        /// <param name="privateExponent">Private exponent of the key.</param>
        /// <param name="publicExponent">Public exponent of the key.</param>
        public RsaKey(string multiplyResult, string privateExponent, string publicExponent)
        {
            this.MultiplyResult = multiplyResult;
            this.PrivateExponent = privateExponent;
            this.PublicExponent = publicExponent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaKey"/> class. Don't use this constructor manually.
        /// </summary>
        public RsaKey()
        {
        }

        /// <summary>
        /// Gets or sets the unique key of the record.
        /// </summary>
        public int Id
        { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for setting the relationship with the <see cref="AuthenticationData"/> entity.
        /// </summary>
        public AuthenticationData AuthData
        { get; set; }

        /// <summary>
        /// Gets or sets the public exponent of the rsa key for storing in a database.
        /// </summary>
        public string PublicExponent
        {
            get => this.publicExponentS;
            set
            {
                SetLinkedProperties(value, ref this.publicExponentS, ref this.publicExponentB);
                this.ValidateModel();
            }
        }

        /// <summary>
        /// Gets or sets the private exponent of the rsa key for storing in a database.
        /// </summary>
        public string PrivateExponent
        {
            get => this.privateExponentS;
            set
            {
                SetLinkedProperties(value, ref this.privateExponentS, ref this.privateExponentB);
                this.ValidateModel();
            }
        }

        /// <summary>
        /// Gets or sets the multiply result of the rsa key for storing in a database.
        /// </summary>
        public string MultiplyResult
        {
            get => this.multiplyResultS;
            set
            {
                SetLinkedProperties(value, ref this.multiplyResultS, ref this.multiplyResultB);
                this.ValidateModel();
            }
        }

        /// <summary>
        /// Gets all <see cref="RsaKey"/> errors, which can't allowed to save key in the database.
        /// </summary>
        public IDictionary<string, string> Errors
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the private exponent of the rsa key.
        /// </summary>
        BigInteger IRsaKey.PrivateExponent
        {
            get => this.privateExponentB;
            set
            {
                SetLinkedProperties(value, ref this.privateExponentS, ref this.privateExponentB);
                this.ValidateModel();
            }
        }

        /// <summary>
        /// Gets or sets the public exponent of the rsa key.
        /// </summary>
        BigInteger IRsaKey.PublicExponent
        {
            get => this.publicExponentB;
            set
            {
                SetLinkedProperties(value, ref this.publicExponentS, ref this.publicExponentB);
                this.ValidateModel();
            }
        }

        /// <summary>
        /// Gets or sets the multiply result of the rsa key.
        /// </summary>
        BigInteger IRsaKey.MultiplyResult
        {
            get => this.multiplyResultB;
            set
            {
                SetLinkedProperties(value, ref this.multiplyResultS, ref this.multiplyResultB);
                this.ValidateModel();
            }
        }

        /// <inheritdoc/>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        private static void SetLinkedProperties(string value, ref string stringProperty, ref BigInteger bigIntProperty)
        {
            stringProperty = value;
            bigIntProperty = value.ToBigInteger();
        }

        private static void SetLinkedProperties(BigInteger value, ref string stringProperty, ref BigInteger bigIntProperty)
        {
            stringProperty = value.ToString();
            bigIntProperty = value;
        }

        private void ValidateModel()
        {
            if (this.validator != null)
            {
                this.Errors = this.validator.Validate(this);
            }
        }
    }
}
