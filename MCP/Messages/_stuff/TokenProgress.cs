﻿

using System.Net;
using System.Text.Json.Serialization;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Provides an <see cref="IProgress{ProgressNotificationValue}"/> tied to a specific progress token and that will issue
    /// progress notifications on the supplied endpoint.
    /// </summary>
    internal sealed class TokenProgress(IMCPEndpoint endpoint, ProgressToken progressToken) : IProgress<ProgressNotificationValue>
    {
        /// <inheritdoc />
        public void Report(ProgressNotificationValue value)
        {
            _ = endpoint.NotifyProgressAsync(progressToken, value, CancellationToken.None);
        }
    }

}
