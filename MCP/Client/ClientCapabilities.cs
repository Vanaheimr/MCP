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

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP.Client
{

    /// <summary>
    /// Represents the capabilities that a client may support.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Capabilities define the features and functionality that a client can handle when communicating with an MCP server.
    /// These are advertised to the server during the initialize handshake.
    /// </para>
    /// <para>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </para>
    /// </remarks>
    public class ClientCapabilities : ACustomData

    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/clientCapabilities");


        /// <summary>Gets or sets notification handlers to register with the client.</summary>
        /// <remarks>
        /// <para>
        /// When constructed, the client will enumerate these handlers once, which may contain multiple handlers per notification method key.
        /// The client will not re-enumerate the sequence after initialization.
        /// </para>
        /// <para>
        /// Notification handlers allow the client to respond to server-sent notifications for specific methods.
        /// Each key in the collection is a notification method name, and each value is a callback that will be invoked
        /// when a notification with that method is received.
        /// </para>
        /// <para>
        /// Handlers provided via <see cref="NotificationHandlers"/> will be registered with the client for the lifetime of the client.
        /// For transient handlers, <see cref="IMCPEndpoint.RegisterNotificationHandler"/> may be used to register a handler that can
        /// then be unregistered by disposing of the <see cref="IAsyncDisposable"/> returned from the method.
        /// </para>
        /// </remarks>
        [JsonIgnore]
        public IEnumerable<KeyValuePair<String, Func<JSONRPCNotification, CancellationToken, ValueTask>>>? NotificationHandlers { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the client's roots capability, which are entry points for resource navigation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When <see cref="Roots"/> is non-<see langword="null"/>, the client indicates that it can respond to 
        /// server requests for listing root URIs. Root URIs serve as entry points for resource navigation in the protocol.
        /// </para>
        /// <para>
        /// The server can use <see cref="McpServerExtensions.RequestRootsAsync"/> to request the list of
        /// available roots from the client, which will trigger the client's <see cref="RootsCapability.RootsHandler"/>.
        /// </para>
        /// </remarks>
        [JsonPropertyName("roots")]
        public RootsCapability?             Roots           { get; }

        /// <summary>
        /// Gets or sets the client's sampling capability, which indicates whether the client 
        /// supports issuing requests to an LLM on behalf of the server.
        /// </summary>
        [JsonPropertyName("sampling")]
        public SamplingCapability?          Sampling        { get; }

        /// <summary>
        /// Gets or sets the client's elicitation capability, which indicates whether the client 
        /// supports elicitation of additional information from the user on behalf of the server.
        /// </summary>
        [JsonPropertyName("elicitation")]
        public ElicitationCapability?       Elicitation     { get; }

        /// <summary>
        /// Gets or sets experimental, non-standard capabilities that the client supports.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="Experimental"/> dictionary allows clients to advertise support for features that are not yet 
        /// standardized in the Model Context Protocol specification. This extension mechanism enables 
        /// future protocol enhancements while maintaining backward compatibility.
        /// </para>
        /// <para>
        /// Values in this dictionary are implementation-specific and should be coordinated between client 
        /// and server implementations. Servers should not assume the presence of any experimental capability 
        /// without checking for it first.
        /// </para>
        /// </remarks>
        [JsonPropertyName("experimental")]
        public Dictionary<String, Object>?  Experimental    { get; }

        #endregion

        #region Constructor(s)

        public ClientCapabilities(ElicitationCapability?                                                                       Elicitation            = null,
                                  RootsCapability?                                                                             Roots                  = null,
                                  SamplingCapability?                                                                          Sampling               = null,
                                  Dictionary<String, Object>?                                                                  Experimental           = null,
                                  JObject?                                                                                     CustomData             = null,

                                  IEnumerable<KeyValuePair<String, Func<JSONRPCNotification, CancellationToken, ValueTask>>>?  NotificationHandlers   = null)

            : base(CustomData)

        {

            this.Elicitation           = Elicitation;
            this.Roots                 = Roots;
            this.Sampling              = Sampling;
            this.Experimental          = Experimental;

            this.NotificationHandlers  = NotificationHandlers;

        }

        #endregion


        #region Documentation

        // {
        //     "sampling": {},
        //     "roots": {
        //         "listChanged": true
        //     }
        // }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of ClientCapabilities.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomClientCapabilitiesParser">A delegate to parse custom ClientCapabilities.</param>
        public static ClientCapabilities Parse(JObject                                           JSON,
                                               CustomJObjectParserDelegate<ClientCapabilities>?  CustomClientCapabilitiesParser   = null)
        {

            if (TryParse(JSON,
                         out var clientCapabilities,
                         out var errorResponse,
                         CustomClientCapabilitiesParser))
            {
                return clientCapabilities;
            }

            throw new ArgumentException("The given JSON representation of ClientCapabilities is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out ClientCapabilities, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of ClientCapabilities.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ClientCapabilities">The parsed ClientCapabilities.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomClientCapabilitiesParser">A delegate to parse custom ClientCapabilities.</param>
        public static Boolean TryParse(JObject                                       JSON,
                                       [NotNullWhen(true)]  out ClientCapabilities?  ClientCapabilities,
                                       [NotNullWhen(false)] out String?              ErrorResponse)

            => TryParse(JSON,
                        out ClientCapabilities,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of ClientCapabilities.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ClientCapabilities">The parsed ClientCapabilities.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomClientCapabilitiesParser">A delegate to parse custom ClientCapabilities.</param>
        public static Boolean TryParse(JObject                                           JSON,
                                       [NotNullWhen(true)]  out ClientCapabilities?      ClientCapabilities,
                                       [NotNullWhen(false)] out String?                  ErrorResponse,
                                       CustomJObjectParserDelegate<ClientCapabilities>?  CustomClientCapabilitiesParser   = null)
        {

            ErrorResponse = null;

            try
            {

                ClientCapabilities = null;

                #region Elicitation    [mandatory]

                if (JSON.ParseOptionalJSON("elicitation",
                                           "elicitation capability",
                                           ElicitationCapability.TryParse,
                                           out ElicitationCapability? elicitation,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Roots          [mandatory]

                if (JSON.ParseOptionalJSON("roots",
                                           "roots capability",
                                           RootsCapability.TryParse,
                                           out RootsCapability? roots,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Sampling       [mandatory]

                if (JSON.ParseOptionalJSON("sampling",
                                           "sampling capability",
                                           SamplingCapability.TryParse,
                                           out SamplingCapability? sampling,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region CustomData     [optional]

                var customData = ParseCustomData(
                                     JSON,
                                     [ "sampling", "roots", "elicitation" ]
                                 );

                #endregion


                ClientCapabilities = new ClientCapabilities(
                                         elicitation,
                                         roots,
                                         sampling,
                                         null,
                                         customData
                                     );

                if (CustomClientCapabilitiesParser is not null)
                    ClientCapabilities = CustomClientCapabilitiesParser(JSON,
                                                                        ClientCapabilities);

                return true;

            }
            catch (Exception e)
            {
                ClientCapabilities  = null;
                ErrorResponse       = "The given JSON representation of ClientCapabilities is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomClientCapabilitiesSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomClientCapabilitiesSerializer">A delegate to serialize custom ClientCapabilities.</param>
        public JObject ToJSON(Boolean                                                  IncludeJSONLDContext                    = false,
                              CustomJObjectSerializerDelegate<ClientCapabilities>?     CustomClientCapabilitiesSerializer      = null,
                              CustomJObjectSerializerDelegate<SamplingCapability>?     CustomSamplingCapabilitySerializer      = null,
                              CustomJObjectSerializerDelegate<RootsCapability>?        CustomRootsCapabilitySerializer         = null,
                              CustomJObjectSerializerDelegate<ElicitationCapability>?  CustomElicitationCapabilitySerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",      DefaultJSONLDContext.ToString())
                               : null,

                           Sampling is not null
                               ? new JProperty("sampling",      Sampling.            ToJSON(false,
                                                                                            CustomSamplingCapabilitySerializer))
                               : null,

                           Roots is not null
                               ? new JProperty("roots",         Roots.               ToJSON(false,
                                                                                            CustomRootsCapabilitySerializer))
                               : null,

                           Elicitation is not null
                               ? new JProperty("elicitation",   Elicitation.         ToJSON(false,
                                                                                            CustomElicitationCapabilitySerializer))
                               : null

                       );

            return CustomClientCapabilitiesSerializer is not null
                       ? CustomClientCapabilitiesSerializer(this, json)
                       : json;

        }

        #endregion


    }

}
