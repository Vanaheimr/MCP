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

namespace org.GraphDefined.Vanaheimr.Hermod.MCP.Client
{

    /// <summary>
    /// Provides configuration options for creating <see cref="IMcpClient"/> instances.
    /// </summary>
    /// <remarks>
    /// These options are typically passed to <see cref="McpClientFactory.CreateAsync"/> when creating a client.
    /// They define client capabilities, protocol version, and other client-specific settings.
    /// </remarks>
    public class MCPClientOptions
    {

        /// <summary>
        /// Gets or sets information about this client implementation, including its name and version.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This information is sent to the server during initialization to identify the client.
        /// It's often displayed in server logs and can be used for debugging and compatibility checks.
        /// </para>
        /// <para>
        /// When not specified, information sourced from the current process will be used.
        /// </para>
        /// </remarks>
        public ClientInfo? ClientInfo { get; set; }

        /// <summary>
        /// Gets or sets the client capabilities to advertise to the server.
        /// </summary>
        public ClientCapabilities? Capabilities { get; set; }

        /// <summary>
        /// Gets or sets the protocol version to request from the server, using a date-based versioning scheme.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The protocol version is a key part of the initialization handshake. The client and server must 
        /// agree on a compatible protocol version to communicate successfully.
        /// </para>
        /// <para>
        /// If non-<see langword="null"/>, this version will be sent to the server, and the handshake
        /// will fail if the version in the server's response does not match this version.
        /// If <see langword="null"/>, the client will request the latest version supported by the server
        /// but will allow any supported version that the server advertizes in its response.
        /// </para>
        /// </remarks>
        public string? ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets a timeout for the client-server initialization handshake sequence.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This timeout determines how long the client will wait for the server to respond during
        /// the initialization protocol handshake. If the server doesn't respond within this timeframe,
        /// an exception will be thrown.
        /// </para>
        /// <para>
        /// Setting an appropriate timeout prevents the client from hanging indefinitely when
        /// connecting to unresponsive servers.
        /// </para>
        /// <para>The default value is 60 seconds.</para>
        /// </remarks>
        public TimeSpan InitializationTimeout { get; set; } = TimeSpan.FromSeconds(60);

    }

}
