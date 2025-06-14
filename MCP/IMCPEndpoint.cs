/*
 * Copyright (c) 2010-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of Vanaheimr MCP <https://www.github.com/Vanaheimr/MCP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    public delegate ValueTask  RegisterNotificationDelegate (JSONRPCNotification? JSONRPCNotification,
                                                             CancellationToken    CancellationToken);


    /// <summary>
    /// Represents a client or server Model Context Protocol (MCP) endpoint.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The MCP endpoint provides the core communication functionality used by both clients and servers:
    /// <list type="bullet">
    ///   <item>Sending JSON-RPC requests and receiving responses.</item>
    ///   <item>Sending notifications to the connected endpoint.</item>
    ///   <item>Registering handlers for receiving notifications.</item>
    /// </list>
    /// </para>
    /// <para>
    /// <see cref="IMCPEndpoint"/> serves as the base interface for both <see cref="IMCPClient"/> and 
    /// <see cref="IMCPServer"/> interfaces, providing the common functionality needed for MCP protocol 
    /// communication. Most applications will use these more specific interfaces rather than working with 
    /// <see cref="IMCPEndpoint"/> directly.
    /// </para>
    /// <para>
    /// All MCP endpoints should be properly disposed after use as they implement <see cref="IAsyncDisposable"/>.
    /// </para>
    /// </remarks>
    public interface IMCPEndpoint : IAsyncDisposable
    {

        /// <summary>
        /// Sends a JSON-RPC request to the connected endpoint and waits for a response.
        /// </summary>
        /// <param name="request">The JSON-RPC request to send.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task containing the endpoint's response.</returns>
        /// <exception cref="InvalidOperationException">The transport is not connected, or another error occurs during request processing.</exception>
        /// <exception cref="MCPException">An error occured during request processing.</exception>
        /// <remarks>
        /// This method provides low-level access to send raw JSON-RPC requests. For most use cases,
        /// consider using the strongly-typed extension methods that provide a more convenient API.
        /// </remarks>
        Task<JSONRPCResponse> SendRequestAsync(JSONRPCRequest     request,
                                               CancellationToken  cancellationToken   = default);


        /// <summary>
        /// Sends a JSON-RPC message to the connected endpoint.
        /// </summary>
        /// <param name="message">
        /// The JSON-RPC message to send. This can be any type that implements JsonRpcMessage, such as
        /// JsonRpcRequest, JsonRpcResponse, JsonRpcNotification, or JsonRpcError.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous send operation.</returns>
        /// <exception cref="InvalidOperationException">The transport is not connected.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>
        /// This method provides low-level access to send any JSON-RPC message. For specific message types,
        /// consider using the higher-level methods such as <see cref="SendRequestAsync"/> or extension methods
        /// like <see cref="McpEndpointExtensions.SendNotificationAsync(IMCPEndpoint, String, CancellationToken)"/>,
        /// which provide a simpler API.
        /// </para>
        /// <para>
        /// The method will serialize the message and transmit it using the underlying transport mechanism.
        /// </para>
        /// </remarks>
        Task SendMessageAsync(AJSONRPCMessage    message,
                              CancellationToken  cancellationToken   = default);


        /// <summary>Registers a handler to be invoked when a notification for the specified method is received.</summary>
        /// <param name="method">The notification method.</param>
        /// <param name="handler">The handler to be invoked.</param>
        /// <returns>An <see cref="IDisposable"/> that will remove the registered handler when disposed.</returns>
        IAsyncDisposable RegisterNotificationHandler(String                        method,
                                                     RegisterNotificationDelegate  handler);

    }

}
