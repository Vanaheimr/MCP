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
    /// The common interface of all MCP clients.
    /// </summary>
    public interface IMCPClient : IMCPEndpoint
    {

        /// <summary>
        /// Gets the capabilities supported by the connected server.
        /// </summary>
        /// <exception cref="InvalidOperationException">The client is not connected.</exception>
        ServerCapabilities  ServerCapabilities    { get; }

        /// <summary>
        /// Gets the implementation information of the connected server.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property provides identification details about the connected server, including its name and version.
        /// It is populated during the initialization handshake and is available after a successful connection.
        /// </para>
        /// <para>
        /// This information can be useful for logging, debugging, compatibility checks, and displaying server
        /// information to users.
        /// </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">The client is not connected.</exception>
        ServerInfo          ServerInfo            { get; }

        /// <summary>
        /// Gets any instructions describing how to use the connected server and its features.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property contains instructions provided by the server during initialization that explain
        /// how to effectively use its capabilities. These instructions can include details about available
        /// tools, expected input formats, limitations, or any other helpful information.
        /// </para>
        /// <para>
        /// This can be used by clients to improve an LLM's understanding of available tools, prompts, and resources. 
        /// It can be thought of like a "hint" to the model and may be added to a system prompt.
        /// </para>
        /// </remarks>
        String?             ServerInstructions    { get; }

    }

}
