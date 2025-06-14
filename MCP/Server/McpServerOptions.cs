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

namespace org.GraphDefined.Vanaheimr.Hermod.MCP.Server
{

    /// <summary>
    /// Provides configuration options for the MCP server.
    /// </summary>
    public class MCPServerOptions
    {

        #region Properties

        /// <summary>
        /// Gets or sets information about this server implementation, including its name and version.
        /// </summary>
        /// <remarks>
        /// This information is sent to the client during initialization to identify the server.
        /// It's displayed in client logs and can be used for debugging and compatibility checks.
        /// </remarks>
        public ClientInfo?          ServerInfo               { get; }

        /// <summary>
        /// Gets or sets server capabilities to advertise to the client.
        /// </summary>
        /// <remarks>
        /// These determine which features will be available when a client connects.
        /// Capabilities can include "tools", "prompts", "resources", "logging", and other 
        /// protocol-specific functionality.
        /// </remarks>
        public ServerCapabilities?  Capabilities             { get; }

        /// <summary>
        /// Gets or sets the protocol version supported by this server, using a date-based versioning scheme.
        /// </summary>
        /// <remarks>
        /// The protocol version defines which features and message formats this server supports.
        /// This uses a date-based versioning scheme in the format "YYYY-MM-DD".
        /// </remarks>
        public String               ProtocolVersion          { get; }

        /// <summary>
        /// Gets or sets a timeout used for the client-server initialization handshake sequence.
        /// </summary>
        /// <remarks>
        /// This timeout determines how long the server will wait for client responses during
        /// the initialization protocol handshake. If the client doesn't respond within this timeframe,
        /// the initialization process will be aborted.
        /// </remarks>
        public TimeSpan             InitializationTimeout    { get; }

        /// <summary>
        /// Gets or sets optional server instructions to send to clients.
        /// </summary>
        /// <remarks>
        /// These instructions are sent to clients during the initialization handshake and provide
        /// guidance on how to effectively use the server's capabilities. They can include details
        /// about available tools, expected input formats, limitations, or other helpful information.
        /// Client applications typically use these instructions as system messages for LLM interactions
        /// to provide context about available functionality.
        /// </remarks>
        public String?              ServerInstructions       { get; }

        /// <summary>
        /// Gets or sets whether to create a new service provider scope for each handled request.
        /// </summary>
        /// <remarks>
        /// The default is <see langword="true"/>. When <see langword="true"/>, each invocation of a request
        /// handler will be invoked within a new service scope.
        /// </remarks>
        public Boolean              ScopeRequests            { get; }

        /// <summary>
        /// Gets or sets preexisting knowledge about the client including its name and version to help support
        /// stateless Streamable HTTP servers that encode this knowledge in the mcp-session-id header.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When not specified, this information is sourced from the client's initialize request.
        /// </para>
        /// </remarks>
        public ClientInfo?          KnownClientInfo          { get; }

        #endregion

        #region Constructor(s)

        public MCPServerOptions(ClientInfo?          ServerInfo              = null,
                                ServerCapabilities?  Capabilities            = null,
                                String?              ProtocolVersion         = null,
                                TimeSpan?            InitializationTimeout   = null,
                                String?              ServerInstructions      = null,
                                Boolean?             ScopeRequests           = null,
                                ClientInfo?          KnownClientInfo         = null)
        {

            this.ServerInfo             = ServerInfo;
            this.Capabilities           = Capabilities;
            this.ProtocolVersion        = ProtocolVersion       ?? "2024-11-05";
            this.InitializationTimeout  = InitializationTimeout ?? TimeSpan.FromSeconds(60);
            this.ServerInstructions     = ServerInstructions;
            this.ScopeRequests          = ScopeRequests         ?? true;
            this.KnownClientInfo        = KnownClientInfo;

        }

        #endregion


    }

}
