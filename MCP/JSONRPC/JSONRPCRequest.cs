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
using org.GraphDefined.Vanaheimr.Illias;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

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
                              String      Method,
                              JsonNode?   Parameters       = null,
                              String?     JSONRPCVersion   = null,
                              JObject?    CustomData       = null)

            : base(Id,
                   JSONRPCVersion,
                   CustomData)

        {

            this.Method  = Method;
            this.Params  = Parameters;

        }

        public JSONRPCRequest(Request_Id         Id,
                              String             Method,
                              JObject?           Parameters          = null,
                              String?            JSONRPCVersion      = null,
                              JObject?           CustomData          = null,

                              DateTimeOffset?    Timestamp           = null,
                              EventTracking_Id?  EventTrackingId     = null,
                              TimeSpan?          RequestTimeout      = null,
                              CancellationToken  CancellationToken   = default)

            : base(Id,
                   JSONRPCVersion,
                   CustomData)

        {

            this.Method  = Method;
            this.Params  = Parameters is not null
                               ? JsonNode.Parse(Parameters.ToString())
                               : null;

        }


        #region (static) Parse   (JSON, OperatorIdURL, ..., CustomJSONRPCRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a JSON RPC request request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="OperatorIdURL">The EVSE operator identification given in the URL of the HTTP request.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomJSONRPCRequestParser">A delegate to parse custom JSON RPC request JSON objects.</param>
        public static JSONRPCRequest Parse(JObject                                       JSON,
                                           DateTime?                                     Timestamp                    = null,
                                           EventTracking_Id?                             EventTrackingId              = null,
                                           TimeSpan?                                     RequestTimeout               = null,
                                           CustomJObjectParserDelegate<JSONRPCRequest>?  CustomJSONRPCRequestParser   = null,
                                           CancellationToken                             CancellationToken            = default)
        {

            if (TryParse(JSON,
                         out var jsonRPCRequest,
                         out var errorResponse,
                         Timestamp,
                         EventTrackingId,
                         RequestTimeout,
                         CustomJSONRPCRequestParser,
                         CancellationToken))
            {
                return jsonRPCRequest;
            }

            throw new ArgumentException("The given JSON representation of a JSON RPC request request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out JSONRPCRequest, out ErrorResponse, ..., CustomJSONRPCRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a JSON RPC request request.
        /// </summary>
        /// <param name="JSON">The JSON to parse.</param>
        /// <param name="RequestTimeout">The timeout for this request.</param>
        /// <param name="JSONRPCRequest">The parsed JSON RPC request request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomJSONRPCRequestParser">A delegate to parse custom JSON RPC request request JSON objects.</param>
        public static Boolean TryParse(JObject                                       JSON,
                                       [NotNullWhen(true)]  out JSONRPCRequest?      JSONRPCRequest,
                                       [NotNullWhen(false)] out String?              ErrorResponse,

                                       DateTimeOffset?                               Timestamp                    = null,
                                       EventTracking_Id?                             EventTrackingId              = null,
                                       TimeSpan?                                     RequestTimeout               = null,
                                       CustomJObjectParserDelegate<JSONRPCRequest>?  CustomJSONRPCRequestParser   = null,
                                       CancellationToken                             CancellationToken            = default)
        {

            try
            {

                JSONRPCRequest = default;

                if (JSON?.HasValues != true)
                {
                    ErrorResponse = "The given JSON object must not be null or empty!";
                    return false;
                }

                #region Parse JSON-RPC Version    [mandatory]

                if (!JSON.ParseMandatoryText("jsonrpc",
                                             "JSON RPC version",
                                             out String? jsonRPCVersion,
                                             out ErrorResponse))
                {
                    return false;
                }

                if (jsonRPCVersion != "2.0")
                {
                    ErrorResponse = "The JSON-RPC version must be '2.0'!";
                    return false;
                }

                #endregion

                #region Parse Id                  [mandatory]

                var id = JSON["id"];
                if (id is null)
                {
                    ErrorResponse = "The request identification must not null null!";
                    return false;
                }

                Request_Id requestId;
                switch (id.Type)
                {

                    case JTokenType.String:
                        if (!Request_Id.TryParse(id.Value<String>() ?? "", out var parsedRequestId, out var errorResponse))
                        {
                            ErrorResponse = $"The given request identification must be a valid string: {errorResponse}";
                            return false;
                        }
                        requestId = parsedRequestId;
                        break;

                    case JTokenType.Integer:
                        if (!Request_Id.TryParse(id.Value<Int64>(), out parsedRequestId))
                        {
                            ErrorResponse = "The given request identification must be a valid string!";
                            return false;
                        }
                        requestId = parsedRequestId;
                        break;

                    default:
                        ErrorResponse = "The given request identification must be either a string or an integer!";
                        return false;

                }

                #endregion

                #region Parse Method              [mandatory]

                if (!JSON.ParseMandatoryText("method",
                                             "JSON RPC method",
                                             out String? method,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Parse Params              [mandatory]

                if (JSON["params"] is not JObject parameters)
                {
                    ErrorResponse = "The given JSON object must contain a 'params' property of type JObject!";
                    return false;
                }

                #endregion


                JSONRPCRequest = new JSONRPCRequest(

                                     requestId,
                                     method,
                                     parameters,
                                     jsonRPCVersion,
                                     null,

                                     Timestamp,
                                     EventTrackingId,
                                     RequestTimeout,
                                     CancellationToken

                                 );

                if (CustomJSONRPCRequestParser is not null)
                    JSONRPCRequest = CustomJSONRPCRequestParser(JSON,
                                                                JSONRPCRequest);

                return true;

            }
            catch (Exception e)
            {
                JSONRPCRequest  = default;
                ErrorResponse   = "The given JSON representation of a JSON RPC request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomJSONRPCRequestSerializer = null,...)

        /// <summary>
        /// Return a JSON-representation of this object.
        /// </summary>
        /// <param name="CustomJSONRPCRequestSerializer">A delegate to serialize custom JSON RPC objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<JSONRPCRequest>?  CustomJSONRPCRequestSerializer   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("method",   Method),

                           Params is not null
                               ? new JProperty("params",   JObject.Parse(Params.ToString()))
                               : null

                       );

            return CustomJSONRPCRequestSerializer is not null
                       ? CustomJSONRPCRequestSerializer(this, json)
                       : json;

        }

        #endregion


        internal JSONRPCRequest WithId(Request_Id id)

            => new (id,
                    Method,
                  //  Params,
                    JSONRPCVersion) {

                    RelatedTransport = RelatedTransport

               };

    }

}
