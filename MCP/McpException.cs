﻿
namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents an exception that is thrown when an Model Context Protocol (MCP) error occurs.
    /// </summary>
    /// <remarks>
    /// This exception is used to represent failures to do with protocol-level concerns, such as invalid JSON-RPC requests,
    /// invalid parameters, or internal errors. It is not intended to be used for application-level errors.
    /// <see cref="Exception.Message"/> or <see cref="ErrorCode"/> from a <see cref="MCPException"/> may be 
    /// propagated to the remote endpoint; sensitive information should not be included. If sensitive details need
    /// to be included, a different exception type should be used.
    /// </remarks>
    public class MCPException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MCPException"/> class.
        /// </summary>
        public MCPException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCPException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MCPException(String  message)

            : base(message)

        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCPException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public MCPException(String      message,
                            Exception?  innerException)

            : base(message, innerException)

        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCPException"/> class with a specified error message and JSON-RPC error code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="errorCode">A <see cref="MCPErrorCodes"/>.</param>
        public MCPException(String         message,
                            MCPErrorCodes  errorCode)

            : this (message,
                    null,
                    errorCode)

        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCPException"/> class with a specified error message, inner exception, and JSON-RPC error code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        /// <param name="errorCode">A <see cref="MCPErrorCodes"/>.</param>
        public MCPException(String         message,
                            Exception?     innerException,
                            MCPErrorCodes  errorCode)

            : base (message,
                    innerException)

        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the error code associated with this exception.
        /// </summary>
        /// <remarks>
        /// This property contains a standard JSON-RPC error code as defined in the MCP specification. Common error codes include:
        /// <list type="bullet">
        /// <item><description>-32700: Parse error - Invalid JSON received</description></item>
        /// <item><description>-32600: Invalid request - The JSON is not a valid Request object</description></item>
        /// <item><description>-32601: Method not found - The method does not exist or is not available</description></item>
        /// <item><description>-32602: Invalid params - Invalid method parameters</description></item>
        /// <item><description>-32603: Internal error - Internal JSON-RPC error</description></item>
        /// </list>
        /// </remarks>
        public MCPErrorCodes ErrorCode { get; }
            = MCPErrorCodes.InternalError;

    }

}
