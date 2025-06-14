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

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents an error response message in the JSON-RPC protocol.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Error responses are sent when a request cannot be fulfilled or encounters an error during processing.
    /// Like successful responses, error messages include the same ID as the original request, allowing the
    /// sender to match errors with their corresponding requests.
    /// </para>
    /// <para>
    /// Each error response contains a structured error detail object with a numeric code, descriptive message,
    /// and optional additional data to provide more context about the error.
    /// </para>
    /// </remarks>
    public class JSONRPCError : AJSONRPCMessageWithId
    {

        #region Properties

        /// <summary>
        /// Gets detailed error information for the failed request, containing an error code, 
        /// message, and optional additional data
        /// </summary>
        [JsonPropertyName("error")]
        public JSONRPCErrorDetail  Error    { get; }

        #endregion

        #region Constructor(s)

        public JSONRPCError(Request_Id           Id,
                            JSONRPCErrorDetail  Error)

            : base(Id)

        {

            this.Error = Error;

        }

        #endregion


    }

}
