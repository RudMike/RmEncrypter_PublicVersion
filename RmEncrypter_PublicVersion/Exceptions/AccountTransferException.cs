// <copyright file="AccountTransferException.cs" company="Mike Rudnikov">
// Copyright (c) Mike Rudnikov. All rights reserved.
// </copyright>

namespace RmEncrypter_PublicVersion.Exceptions
{
    using System;

    /// <summary>
    /// The exception that is thrown if there are problems with account transfer.
    /// </summary>
    [Serializable]
    public class AccountTransferException : ArgumentException
    {
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountTransferException"/> class.
        /// </summary>
        public AccountTransferException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountTransferException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public AccountTransferException(string message)
            : base(message)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountTransferException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException
        /// parameter is not a null reference, the current exception is raised in a catch
        /// block that handles the inner exception.</param>
        public AccountTransferException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountTransferException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        public AccountTransferException(string message, string paramName)
            : base(message, paramName)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountTransferException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException
        /// parameter is not a null reference, the current exception is raised in a catch
        /// block that handles the inner exception.</param>
        public AccountTransferException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException)
        {
            this.message = message;
        }

        /// <inheritdoc/>
        public override string Message => this.message;
    }
}
