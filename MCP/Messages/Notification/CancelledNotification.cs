﻿
using System.Text.Json.Serialization;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents a notification indicating that a request has been cancelled by the client,
    /// and that any associated processing should cease immediately.
    /// </summary>
    /// <remarks>
    /// This class is typically used in conjunction with the <see cref="NotificationMethods.CancelledNotification"/>
    /// method identifier. When a client sends this notification, the server should attempt to
    /// cancel any ongoing operations associated with the specified request ID.
    /// </remarks>
    public sealed class CancelledNotification
    {

        /// <summary>
        /// Gets or sets the ID of the request to cancel.
        /// </summary>
        /// <remarks>
        /// This must match the ID of an in-flight request that the sender wishes to cancel.
        /// </remarks>
        [JsonPropertyName("requestId")]
        public Request_Id RequestId { get; set; }

        /// <summary>
        /// Gets or sets an optional string describing the reason for the cancellation request.
        /// </summary>
        [JsonPropertyName("reason")]
        public String? Reason { get; set; }

    }

}
