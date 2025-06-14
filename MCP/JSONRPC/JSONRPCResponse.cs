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

using Newtonsoft.Json.Linq;

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// A successful response message in the JSON-RPC protocol.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Response messages are sent in reply to a request message and contain the result of the method execution.
    /// Each response includes the same ID as the original request, allowing the sender to match responses
    /// with their corresponding requests.
    /// </para>
    /// <para>
    /// This class represents a successful response with a result. For error responses, see <see cref="JSONRPCError"/>.
    /// </para>
    /// </remarks>
    public class JSONRPCResponse : AJSONRPCMessageWithId
    {

        /// <summary>
        /// Gets the result of the method invocation.
        /// </summary>
        /// <remarks>
        /// This property contains the result data returned by the server in response to the JSON-RPC method request.
        /// </remarks>
        [JsonPropertyName("result")]
        public JsonNode? Result { get; }



        public JSONRPCResponse(Request_Id  Id,
                               JsonNode?  Result           = null,
                               String?    JSONRPCVersion   = null,
                               JObject?   CustomData       = null)

            : base(Id,
                   JSONRPCVersion,
                   CustomData)

        {

            this.Result  = Result;

        }

        public JSONRPCResponse(Request_Id  Id,
                               JObject?   Result           = null,
                               String?    JSONRPCVersion   = null,
                               JObject?   CustomData       = null)

            : base(Id,
                   JSONRPCVersion,
                   CustomData)

        {

            this.Result  = Result is not null
                               ? JsonNode.Parse(Result.ToString())
                               : null;

        }

    }

}
