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
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.MCP.Client;
using System.Text.Json.Nodes;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents the parameters used with a <see cref="RequestMethods.Initialize"/> request sent by a client to a server during the protocol handshake.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="InitializeRequestParams"/> is the first message sent in the Model Context Protocol
    /// communication flow. It establishes the connection between client and server, negotiates the protocol
    /// version, and declares the client's capabilities.
    /// </para>
    /// <para>
    /// After sending this request, the client should wait for an <see cref="InitializeResult"/> response
    /// before sending an <see cref="NotificationMethods.InitializedNotification"/> notification to complete the handshake.
    /// </para>
    /// <para>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </para>
    /// </remarks>
    public class InitializeRequest : ARequest<InitializeRequest>,
                                     IRequest
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/InitializeRequest");


        public const String Method_Initialize = "initialize";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets information about the client implementation, including its name and version.
        /// </summary>
        /// <remarks>
        /// This information is required during the initialization handshake to identify the client.
        /// Servers may use this information for logging, debugging, or compatibility checks.
        /// </remarks>
        [JsonPropertyName("clientInfo")]
        public ClientInfo       ClientInfo        { get; }

        /// <summary>
        /// Gets or sets the client's capabilities.
        /// </summary>
        /// <remarks>
        /// Capabilities define the features the client supports, such as "sampling" or "roots".
        /// </remarks>
        [JsonPropertyName("capabilities")]
        public ClientCapabilities?  Capabilities      { get; }

        /// <summary>
        /// Gets or sets the version of the Model Context Protocol that the client wants to use.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Protocol version is specified using a date-based versioning scheme in the format "YYYY-MM-DD".
        /// The client and server must agree on a protocol version to communicate successfully.
        /// </para>
        /// <para>
        /// During initialization, the server will check if it supports this requested version. If there's a 
        /// mismatch, the server will reject the connection with a version mismatch error.
        /// </para>
        /// <para>
        /// See the <see href="https://spec.modelcontextprotocol.io/specification/">protocol specification</see> for version details.
        /// </para>
        /// </remarks>
        [JsonPropertyName("protocolVersion")]
        public String               ProtocolVersion   { get; }

        //public JObject?             Parameters        { get; } = Parameters;

        #endregion

        #region Constructor(s)

        public InitializeRequest(Request_Id            Id,
                                 ClientInfo           ClientInfo,
                                 ClientCapabilities?  Capabilities      = null,
                                 String               ProtocolVersion   = "2025-03-26",
                                 //JObject?             Parameters        = null,
                                 String?              JSONRPCVersion    = null,
                                 JObject?             CustomData        = null)

            : base(Id,
                   Method_Initialize,
                   JObject.Parse("{}"),//Parameters,
                   JSONRPCVersion,
                   CustomData)

        {

            this.ClientInfo       = ClientInfo;
            this.Capabilities     = Capabilities;
            this.ProtocolVersion  = ProtocolVersion;
            //this.Parameters       = Parameters;

        }

        #endregion


        #region Documentation

        // {
        //     "jsonrpc":   "2.0",
        //     "id":         0,
        //     "method":    "initialize",
        //     "params": {
        //         "protocolVersion": "2025-03-26",
        //         "capabilities": {
        //             "sampling": {},
        //             "roots": {
        //                 "listChanged": true
        //             }
        //         },
        //         "clientInfo": {
        //             "name":     "mcp-inspector",
        //             "version":  "0.14.0"
        //         }
        //     }
        // }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of an InitializeRequest.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomInitializeRequestParser">A delegate to parse custom InitializeRequests.</param>
        public static InitializeRequest Parse(JObject                                          JSON,
                                              DateTime?                                        RequestTimestamp                = null,
                                              TimeSpan?                                        RequestTimeout                  = null,
                                              EventTracking_Id?                                EventTrackingId                 = null,
                                              CustomJObjectParserDelegate<InitializeRequest>?  CustomInitializeRequestParser   = null)
        {

            if (TryParse(JSON,
                         out var initializeRequest,
                         out var errorResponse,
                         RequestTimestamp,
                         RequestTimeout,
                         EventTrackingId,
                         CustomInitializeRequestParser))
            {
                return initializeRequest;
            }

            throw new ArgumentException("The given JSON representation of an InitializeRequest is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out InitializeRequest, out ErrorResponse, ...)

        /// <summary>
        /// Try to parse the given JSON representation of an InitializeRequest.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="InitializeRequest">The parsed InitializeRequest.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomInitializeRequestParser">A delegate to parse custom InitializeRequests.</param>
        public static Boolean TryParse(JObject                                          JSON,
                                       [NotNullWhen(true)]  out InitializeRequest?      InitializeRequest,
                                       [NotNullWhen(false)] out String?                 ErrorResponse,
                                       DateTime?                                        RequestTimestamp                = null,
                                       TimeSpan?                                        RequestTimeout                  = null,
                                       EventTracking_Id?                                EventTrackingId                 = null,
                                       CustomJObjectParserDelegate<InitializeRequest>?  CustomInitializeRequestParser   = null)
        {

            ErrorResponse = null;

            try
            {

                InitializeRequest = null;

                #region JSON-RPC Version    [mandatory]

                if (!JSON.ParseMandatoryText("jsonrpc",
                                             "JSON-RPC version",
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

                #region Id                  [mandatory]

                var id         = JSON["id"];
                Request_Id? requestId  = null;

                if (id is null)
                {
                    ErrorResponse = "The request identification must not null null!";
                    return false;
                }

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

                if (!requestId.HasValue)
                {
                    ErrorResponse = "The given request identification could not be parsed!";
                    return false;
                }

                #endregion

                #region Method              [mandatory]

                if (!JSON.ParseMandatoryText("method",
                                             "request method",
                                             out String? method,
                                             out ErrorResponse))
                {
                    return false;
                }

                if (jsonRPCVersion != Method_Initialize)
                {
                    ErrorResponse = $"The method must be '{Method_Initialize}'!";
                    return false;
                }

                #endregion


                if (JSON["params"] is not JObject parameters)
                {
                    ErrorResponse = "The 'params' object must not be null!";
                    return false;
                }

                #region ClientInfo          [mandatory]

                if (!parameters.ParseMandatoryJSON("clientInfo",
                                                   "client information",
                                                   ClientInfo.TryParse,
                                                   out ClientInfo? clientInfo,
                                                   out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Capabilities        [optional]

                if (!parameters.ParseOptionalJSON("capabilities",
                                                  "client capabilities",
                                                  ClientCapabilities.TryParse,
                                                  out ClientCapabilities? capabilities,
                                                  out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region ProtocolVersion     [mandatory]

                if (!JSON.ParseMandatoryText("protocolVersion",
                                             "protocol version",
                                             out String? protocolVersion,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion



                #region Signatures          [optional]

                //if (JSON.ParseOptionalHashSet("signatures",
                //                              "cryptographic signatures",
                //                              Signature.TryParse,
                //                              out HashSet<Signature> Signatures,
                //                              out ErrorResponse))
                //{
                //    if (ErrorResponse is not null)
                //        return false;
                //}

                #endregion

                #region CustomData          [optional]

                //if (JSON.ParseOptionalJSON("customData",
                //                           "custom data",
                //                           WWCP.CustomData.TryParse,
                //                           out CustomData? CustomData,
                //                           out ErrorResponse))
                //{
                //    if (ErrorResponse is not null)
                //        return false;
                //}

                #endregion


                InitializeRequest = new InitializeRequest(

                                        requestId.Value,
                                        clientInfo,
                                        capabilities,
                                        protocolVersion

                                        //Destination,
                                        //CertificateChain,
                                        //CertificateType,

                                        //null,
                                        //null,
                                        //Signatures,

                                        //CustomData,

                                        //RequestId,
                                        //RequestTimestamp,
                                        //RequestTimeout,
                                        //EventTrackingId,
                                        //NetworkPath

                                    );

                if (CustomInitializeRequestParser is not null)
                    InitializeRequest = CustomInitializeRequestParser(JSON,
                                                                      InitializeRequest);

                return true;

            }
            catch (Exception e)
            {
                InitializeRequest  = null;
                ErrorResponse      = "The given JSON representation of an InitializeRequest is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomInitializeRequestSerializer = null, CustomSignatureSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomInitializeRequestSerializer">A delegate to serialize custom InitializeRequests.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(Boolean                                              IncludeJSONLDContext                = false,
                              CustomJObjectSerializerDelegate<InitializeRequest>?  CustomInitializeRequestSerializer   = null)
                              //CustomJObjectSerializerDelegate<Signature>?          CustomSignatureSerializer           = null)
        {

            var json = JSONObject.Create(

                           IncludeJSONLDContext
                               ? new JProperty("@context",   DefaultJSONLDContext.ToString())
                               : null,

                                 new JProperty("jsonrpc",    JSONRPCVersion),
                                 new JProperty("id",         Id.AsJSONToken()),
                                 new JProperty("method",     Method),

                                 new JProperty("params",     JSONObject.Create(

                                     new JProperty("protocolVersion",   ProtocolVersion),
                                     new JProperty("clientInfo",        ClientInfo.   ToJSON()),
                                     new JProperty("capabilities",      Capabilities?.ToJSON())

                                 ))

                       //Signatures.Any()
                       //    ? new JProperty("signatures",         new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                       //                                                                                                     CustomCustomDataSerializer))))
                       //    : null

                       );

            return CustomInitializeRequestSerializer is not null
                       ? CustomInitializeRequestSerializer(this, json)
                       : json;

        }

        #endregion


        public override bool Equals(InitializeRequest? TRequest)
        {
            throw new NotImplementedException();
        }


    }


}
