﻿using org.GraphDefined.Vanaheimr.Hermod.DNS;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents a transport mechanism for MCP (Model Context Protocol) communication between clients and servers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="ITransport"/> interface is the core abstraction for bidirectional communication.
    /// It provides methods for sending and receiving messages, abstracting away the underlying transport mechanism
    /// and allowing protocol implementations to be decoupled from communication details.
    /// </para>
    /// <para>
    /// Implementations of <see cref="ITransport"/> handle the serialization, transmission, and reception of
    /// messages over various channels like standard input/output streams and HTTP (Server-Sent Events).
    /// </para>
    /// <para>
    /// While <see cref="IClientTransport"/> is responsible for establishing a client's connection,
    /// <see cref="ITransport"/> represents an established session. Client implementations typically obtain an
    /// <see cref="ITransport"/> instance by calling <see cref="IClientTransport.ConnectAsync"/>.
    /// </para>
    /// </remarks>
    public interface ITransport : IAsyncDisposable
    {

        /// <summary>
        /// Gets a channel reader for receiving messages from the transport.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="MessageReader"/> provides access to incoming JSON-RPC messages received by the transport.
        /// It returns a <see cref="ChannelReader{T}"/> which allows consuming messages in a thread-safe manner.
        /// </para>
        /// <para>
        /// The reader will continue to provide messages as long as the transport is connected. When the transport
        /// is disconnected or disposed, the channel will be completed and no more messages will be available after
        /// any already transmitted messages are consumed.
        /// </para>
        /// </remarks>
        ChannelReader<AJSONRPCMessage> MessageReader { get; }

        /// <summary>
        /// Sends a JSON-RPC message through the transport.
        /// </summary>
        /// <param name="message">The JSON-RPC message to send.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous send operation.</returns>
        /// <exception cref="InvalidOperationException">The transport is not connected.</exception>
        /// <remarks>
        /// <para>
        /// This method serializes and sends the provided JSON-RPC message through the transport connection.
        /// </para>
        /// <para>
        /// This is a core method used by higher-level abstractions in the MCP protocol implementation.
        /// Most client code should use the higher-level methods provided by <see cref="IMCPEndpoint"/>,
        /// <see cref="McpEndpointExtensions"/>, <see cref="McpClientExtensions"/>, or <see cref="McpServerExtensions"/>,
        /// rather than accessing this method directly.
        /// </para>
        /// </remarks>
        Task SendMessageAsync(AJSONRPCMessage message, CancellationToken cancellationToken = default);

    }

}
