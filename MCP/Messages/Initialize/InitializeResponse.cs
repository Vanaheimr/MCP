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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents the result of a <see cref="RequestMethods.Initialize"/> request sent to the server during connection establishment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="InitializeResult"/> is sent by the server in response to an <see cref="InitializeRequestParams"/> 
    /// message from the client. It contains information about the server, its capabilities, and the protocol version
    /// that will be used for the session.
    /// </para>
    /// <para>
    /// After receiving this response, the client should send an <see cref="NotificationMethods.InitializedNotification"/>
    /// notification to complete the handshake.
    /// </para>
    /// <para>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </para>
    /// </remarks>
    public class InitializeResponse : AResponse<InitializeRequest,
                                                InitializeResponse>,
                                      IResponse//<Result>
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/InitializeResponse");

        public const String DefaultProtocolVersion = "2025-03-26";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the version of the Model Context Protocol that the server will use for this session.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the protocol version the server has agreed to use, which should match the client's 
        /// requested version. If there's a mismatch, the client should throw an exception to prevent 
        /// communication issues due to incompatible protocol versions.
        /// </para>
        /// <para>
        /// The protocol uses a date-based versioning scheme in the format "YYYY-MM-DD".
        /// </para>
        /// <para>
        /// See the <see href="https://spec.modelcontextprotocol.io/specification/">protocol specification</see> for version details.
        /// </para>
        /// </remarks>
        [JsonPropertyName("protocolVersion")]
        public String              ProtocolVersion    { get; }

        /// <summary>
        /// Gets or sets the server's capabilities.
        /// </summary>
        /// <remarks>
        /// This defines the features the server supports, such as "tools", "prompts", "resources", or "logging", 
        /// and other protocol-specific functionality.
        /// </remarks>
        [JsonPropertyName("capabilities")]
        public ServerCapabilities  Capabilities       { get; }

        /// <summary>
        /// Gets or sets information about the server implementation, including its name and version.
        /// </summary>
        /// <remarks>
        /// This information identifies the server during the initialization handshake.
        /// Clients may use this information for logging, debugging, or compatibility checks.
        /// </remarks>
        [JsonPropertyName("serverInfo")]
        public ServerInfo          ServerInfo         { get; }

        /// <summary>
        /// Gets or sets optional instructions for using the server and its features.
        /// </summary>
        /// <remarks>
        /// <para>
        /// These instructions provide guidance to clients on how to effectively use the server's capabilities.
        /// They can include details about available tools, expected input formats, limitations,
        /// or any other information that helps clients interact with the server properly.
        /// </para>
        /// <para>
        /// Client applications often use these instructions as system messages for LLM interactions
        /// to provide context about available functionality.
        /// </para>
        /// </remarks>
        [JsonPropertyName("instructions")]
        public String?             Instructions       { get; }

        #endregion

        #region Constructor(s)

        public InitializeResponse(InitializeRequest   Request,
                                  Request_Id          Id,
                                  ServerInfo          ServerInfo,
                                  ServerCapabilities  Capabilities,
                                  String?             Instructions        = null,
                                  String?             ProtocolVersion     = DefaultProtocolVersion,
                                  //JObject?            Parameters          = null,
                                  String?             JSONRPCVersion      = null,
                                  JObject?            CustomData          = null,

                                  DateTime?           ResponseTimestamp   = null,

                                  CancellationToken   CancellationToken   = default)

            : base(Request,
                   ResponseTimestamp,
                   //JObject.Parse("{}"),//Parameters,
                   JSONRPCVersion,
                   CustomData)

        {

            this.ServerInfo       = ServerInfo;
            this.Capabilities     = Capabilities;
            this.Instructions     = Instructions;
            this.ProtocolVersion  = ProtocolVersion ?? DefaultProtocolVersion;
            //this.Parameters       = Parameters;

        }

        #endregion


        #region Documentation

        // {
        //     "result": {
        //         "protocolVersion": "2025-03-26",
        //         "capabilities": {
        //             "logging": {},
        //             "tools": {
        //                 "listChanged": true
        //             }
        //         },
        //         "serverInfo": {
        //             "name":    "MCP Web Server Test",
        //             "version": "1.0.0"
        //         }
        //     },
        //     "id":       0,
        //     "jsonrpc": "2.0"
        // }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of an InitializeResponse.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ResponseTimestamp">An optional request timestamp.</param>
        /// <param name="ResponseTimeout">An optional request timeout.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomInitializeResponseParser">A delegate to parse custom InitializeResponses.</param>
        public static InitializeResponse Parse(InitializeRequest                                 Request,
                                               JObject                                           JSON,
                                               DateTime?                                         ResponseTimestamp                = null,
                                               TimeSpan?                                         ResponseTimeout                  = null,
                                               EventTracking_Id?                                 EventTrackingId                  = null,
                                               CustomJObjectParserDelegate<InitializeResponse>?  CustomInitializeResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         out var initializeResponse,
                         out var errorResponse,
                         ResponseTimestamp,
                         ResponseTimeout,
                         EventTrackingId,
                         CustomInitializeResponseParser))
            {
                return initializeResponse;
            }

            throw new ArgumentException("The given JSON representation of an InitializeResponse is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out InitializeResponse, out ErrorResponse, ...)

        /// <summary>
        /// Try to parse the given JSON representation of an InitializeResponse.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="InitializeResponse">The parsed InitializeResponse.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="ResponseTimestamp">An optional request timestamp.</param>
        /// <param name="ResponseTimeout">An optional request timeout.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomInitializeResponseParser">A delegate to parse custom InitializeResponses.</param>
        public static Boolean TryParse(InitializeRequest                                 Request,
                                       JObject                                           JSON,
                                       [NotNullWhen(true)]  out InitializeResponse?      InitializeResponse,
                                       [NotNullWhen(false)] out String?                  ErrorResponse,
                                       DateTime?                                         ResponseTimestamp                = null,
                                       TimeSpan?                                         ResponseTimeout                  = null,
                                       EventTracking_Id?                                 EventTrackingId                  = null,
                                       CustomJObjectParserDelegate<InitializeResponse>?  CustomInitializeResponseParser   = null)
        {

            ErrorResponse = null;

            try
            {

                InitializeResponse = null;

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

                var id                = JSON["id"];
                Request_Id? requestId  = null;

                if (id is null)
                {
                    ErrorResponse = "The request identification must not null null!";
                    return false;
                }

                switch (id.Type)
                {

                    case JTokenType.String:
                        if (!Request_Id.TryParse(id.Value<String>() ?? "", out var parsedRequestId))
                        {
                            ErrorResponse = "The given request identification must be a valid string!";
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


                if (JSON["result"] is not JObject parameters)
                {
                    ErrorResponse = "The 'params' object must not be null!";
                    return false;
                }

                #region ProtocolVersion     [mandatory]

                if (!parameters.ParseMandatoryText("protocolVersion",
                                                   "protocol version",
                                                   out String? protocolVersion,
                                                   out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region ServerInfo          [mandatory]

                if (!parameters.ParseMandatoryJSON("serverInfo",
                                                   "server information",
                                                   ServerInfo.TryParse,
                                                   out ServerInfo? clientInfo,
                                                   out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Capabilities        [optional]

                if (!parameters.ParseOptionalJSON("capabilities",
                                                  "server capabilities",
                                                  ServerCapabilities.TryParse,
                                                  out ServerCapabilities? capabilities,
                                                  out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Instructions        [optional]

                if (parameters.ParseOptional("instructions",
                                             "server instructions",
                                             out String? instructions,
                                             out ErrorResponse))
                {
                    if (ErrorResponse is not null)
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

                var customData       = new JObject();
                var knownProperties  = new[] { "serverInfo", "capabilities", "protocolVersion" };

                foreach (var property in parameters.Properties())
                {
                    if (!knownProperties.Contains(property.Name))
                        customData.Add(property.Name, property.Value);
                }

                #endregion


                InitializeResponse = new InitializeResponse(

                                         Request,
                                         requestId.Value,
                                         clientInfo,
                                         capabilities,
                                         instructions,
                                         protocolVersion,
                                         jsonRPCVersion,
                                         customData

                                         //Signatures,

                                         //ResponseTimestamp,
                                         //ResponseTimeout,
                                         //EventTrackingId

                                     );

                if (CustomInitializeResponseParser is not null)
                    InitializeResponse = CustomInitializeResponseParser(JSON,
                                                                        InitializeResponse);

                return true;

            }
            catch (Exception e)
            {
                InitializeResponse  = null;
                ErrorResponse       = "The given JSON representation of an InitializeResponse is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomInitializeResponseSerializer = null, CustomSignatureSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomInitializeResponseSerializer">A delegate to serialize custom InitializeResponses.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        public JObject ToJSON(Boolean                                               IncludeJSONLDContext                 = false,
                              CustomJObjectSerializerDelegate<InitializeResponse>?  CustomInitializeResponseSerializer   = null)
                              //CustomJObjectSerializerDelegate<Signature>?          CustomSignatureSerializer           = null)
        {

            var json = JSONObject.Create(

                           IncludeJSONLDContext
                               ? new JProperty("@context",   DefaultJSONLDContext.ToString())
                               : null,

                                 new JProperty("jsonrpc",    JSONRPCVersion),
                                 new JProperty("id",         Id.AsJSONToken()),

                                 new JProperty("params",     JSONObject.Create(

                                           new JProperty("protocolVersion",   ProtocolVersion),
                                           new JProperty("serverInfo",        ServerInfo.   ToJSON()),

                                     Capabilities is not null
                                         ? new JProperty("capabilities",      Capabilities?.ToJSON())
                                         : null,

                                     Instructions.IsNotNullOrEmpty()
                                         ? new JProperty("instructions",      Instructions)
                                         : null

                                 ))

                       //Signatures.Any()
                       //    ? new JProperty("signatures",         new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                       //                                                                                                     CustomCustomDataSerializer))))
                       //    : null

                       );

            return CustomInitializeResponseSerializer is not null
                       ? CustomInitializeResponseSerializer(this, json)
                       : json;

        }

        #endregion


        public override bool Equals(InitializeResponse? AResponse)
        {
            throw new NotImplementedException();
        }


    }


}
