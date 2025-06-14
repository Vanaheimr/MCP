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
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents a notification message in the JSON-RPC protocol.
    /// </summary>
    /// <remarks>
    /// Notifications are messages that do not require a response and are not matched with a response message.
    /// They are useful for one-way communication, such as log notifications and progress updates.
    /// Unlike requests, notifications do not include an ID field, since there will be no response to match with it.
    /// </remarks>
    public class JSONRPCNotification : AJSONRPCMessage
    {

        #region Properties

        /// <summary>
        /// Gets or sets the name of the notification method.
        /// </summary>
        [JsonPropertyName("method")]
        public String     Method    { get; }

        /// <summary>
        /// Gets or sets optional parameters for the notification.
        /// </summary>
        [JsonPropertyName("params")]
        public JsonNode?  Params    { get; }

        #endregion

        #region Constructor(s)

        public JSONRPCNotification(String     Method,
                                   JsonNode?  Params           = null,
                                   String?    JSONRPCVersion   = null,
                                   JObject?   CustomData       = null)

            : base(JSONRPCVersion,
                   CustomData)

        {

            this.Method  = Method;
            this.Params  = Params;

        }

        #endregion



        public virtual JObject ToJSON()
        {

            // // {"jsonrpc":"2.0","method":"notifications/initialized"}

            var json = JSONObject.Create(

                                 new JProperty("jsonrpc",   JSONRPCVersion),
                                 new JProperty("method",    Method),

                           Params is not null
                               ? new JProperty("params",    JObject.Parse(Params.ToString()))
                               : null

                       );

            return json;

        }


    }

}
