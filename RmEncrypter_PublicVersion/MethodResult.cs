// <copyright file="MethodResult.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion
{
    /// <summary>
    /// Provides information to describe the results of a method.
    /// </summary>
    public class MethodResult
    {
        private string exceptionMessage;
        private bool isSuccessful;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodResult"/> class.
        /// </summary>
        public MethodResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodResult"/> class.
        /// </summary>
        /// <param name="isSuccessful">The value indicating whether the method completed successfully.</param>
        /// <param name="exceptionMessage">The value containing the exception message.
        /// The value will be ignored if <see cref="IsSuccessful"/> is <see langword="true"/>.</param>
        public MethodResult(bool isSuccessful, string exceptionMessage = null)
        {
            this.IsSuccessful = isSuccessful;
            this.ExceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the method completed successfully.
        /// If sets to <see langword="true"/>, the <see cref="ExceptionMessage"/> will be sets to <see langword="null"/>.
        /// </summary>
        public bool IsSuccessful
        {
            get => this.isSuccessful;
            set
            {
                this.isSuccessful = value;
                this.TryClearExceptionMessage();
            }
        }

        /// <summary>
        /// Gets or sets a value containing the exception message if <see cref="IsSuccessful"/> is <see langword="false"/>.
        /// If <see cref="IsSuccessful"/> is <see langword="true"/> it will be <see langword="null"/>.
        /// </summary>
        public string ExceptionMessage
        {
            get => this.exceptionMessage;
            set => this.exceptionMessage = this.IsSuccessful ? null : value;
        }

        private void TryClearExceptionMessage()
        {
            if (this.isSuccessful)
            {
                this.exceptionMessage = null;
            }
        }
    }
}
