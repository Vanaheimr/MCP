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

#region Usings

using System.Text.Json.Serialization;

using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents a JSON-RPC message used in the Model Context Protocol (MCP) and that includes an ID.
    /// </summary>
    /// <remarks>
    /// In the JSON-RPC protocol, messages with an ID require a response from the receiver.
    /// This includes request messages (which expect a matching response) and response messages
    /// (which include the ID of the original request they're responding to).
    /// The ID is used to correlate requests with their responses, allowing asynchronous
    /// communication where multiple requests can be sent without waiting for responses.
    /// </remarks>
    public abstract class AJSONRPCMessageWithId(Request_Id  Id,
                                                String?    JSONRPCVersion   = null,
                                                JObject?   CustomData       = null)

        : AJSONRPCMessage(JSONRPCVersion,
                          CustomData)

    {

        #region Properties

        /// <summary>
        /// Gets the message identifier
        /// (unique within the context of a given session).
        /// </remarks>
        [JsonPropertyName("id")]
        public Request_Id  Id   { get; } = Id;

        #endregion

    }

}
