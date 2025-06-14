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

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// A request message in the JSON-RPC protocol.
    /// </summary>
    /// <remarks>
    /// Requests are messages that require a response from the receiver. Each request includes a unique ID
    /// that will be included in the corresponding response message (either a success response or an error).
    /// 
    /// The receiver of a request message is expected to execute the specified method with the provided parameters
    /// and return either a <see cref="JsonRpcResponse"/> with the result, or a <see cref="JSONRPCError"/>
    /// if the method execution fails.
    /// </remarks>
    public class JSONRPCRequest : AJSONRPCMessageWithId
    {

        #region Properties

        /// <summary>
        /// Name of the method to invoke.
        /// </summary>
        [JsonPropertyName("method")]
        public String     Method    { get; init; }

        /// <summary>
        /// Optional parameters for the method.
        /// </summary>
        [JsonPropertyName("params")]
        public JsonNode?  Params    { get; init; }

        #endregion


        public JSONRPCRequest(Request_Id  Id,
                              String      method,
                              JsonNode?   parameters       = null,
                              String?     JSONRPCVersion   = null,
                              JObject?    CustomData       = null)

            : base(Id,
                   JSONRPCVersion,
                   CustomData)

        {

            this.Method  = method;
        //    this.Params  = parameters;

        }

        public JSONRPCRequest(Request_Id  Id,
                              String      method,
                              JObject?    parameters       = null,
                              String?     JSONRPCVersion   = null,
                              JObject?    CustomData       = null)

            : base(Id,
                   JSONRPCVersion,
                   CustomData)

        {

            this.Method  = method;
            this.Params  = parameters is not null
                               ? JsonNode.Parse(parameters.ToString())
                               : null;

        }





        internal JSONRPCRequest WithId(Request_Id id)

            => new (id,
                    Method,
                  //  Params,
                    JSONRPCVersion) {

                    RelatedTransport = RelatedTransport

               };

    }

}
