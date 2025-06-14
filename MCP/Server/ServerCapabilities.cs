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

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents the capabilities that a server may support.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Server capabilities define the features and functionality available when clients connect.
    /// These capabilities are advertised to clients during the initialize handshake.
    /// </para>
    /// <para>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </para>
    /// </remarks>
    public class ServerCapabilities : ACustomData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/serverCapabilities");


        /// <summary>Gets or sets notification handlers to register with the server.</summary>
        /// <remarks>
        /// <para>
        /// When constructed, the server will enumerate these handlers once, which may contain multiple handlers per notification method key.
        /// The server will not re-enumerate the sequence after initialization.
        /// </para>
        /// <para>
        /// Notification handlers allow the server to respond to client-sent notifications for specific methods.
        /// Each key in the collection is a notification method name, and each value is a callback that will be invoked
        /// when a notification with that method is received.
        /// </para>
        /// <para>
        /// Handlers provided via <see cref="NotificationHandlers"/> will be registered with the server for the lifetime of the server.
        /// For transient handlers, <see cref="IMCPEndpoint.RegisterNotificationHandler"/> may be used to register a handler that can
        /// then be unregistered by disposing of the <see cref="IAsyncDisposable"/> returned from the method.
        /// </para>
        /// </remarks>
        [JsonIgnore]
        public IEnumerable<KeyValuePair<String, Func<JSONRPCNotification, CancellationToken, ValueTask>>>? NotificationHandlers { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a server's logging capability, supporting sending log messages to the client.
        /// </summary>
        [JsonPropertyName("logging")]
        public LoggingCapability?           Logging         { get; }

        /// <summary>
        /// Gets or sets a server's prompts capability for serving predefined prompt templates that clients can discover and use.
        /// </summary>
        [JsonPropertyName("prompts")]
        public PromptsCapability?           Prompts         { get; }

        /// <summary>
        /// Gets or sets a server's resources capability for serving predefined resources that clients can discover and use.
        /// </summary>
        [JsonPropertyName("resources")]
        public ResourcesCapability?         Resources       { get; }

        /// <summary>
        /// Gets or sets a server's tools capability for listing tools that a client is able to invoke.
        /// </summary>
        [JsonPropertyName("tools")]
        public ToolsCapability?             Tools           { get; }

        /// <summary>
        /// Gets or sets a server's completions capability for supporting argument auto-completion suggestions.
        /// </summary>
        [JsonPropertyName("completions")]
        public CompletionsCapability?       Completions     { get; }

        /// <summary>
        /// Gets or sets experimental, non-standard capabilities that the server supports.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="Experimental"/> dictionary allows servers to advertise support for features that are not yet 
        /// standardized in the Model Context Protocol specification. This extension mechanism enables 
        /// future protocol enhancements while maintaining backward compatibility.
        /// </para>
        /// <para>
        /// Values in this dictionary are implementation-specific and should be coordinated between client 
        /// and server implementations. Clients should not assume the presence of any experimental capability 
        /// without checking for it first.
        /// </para>
        /// </remarks>
        [JsonPropertyName("experimental")]
        public Dictionary<String, Object>?  Experimental    { get; }

        #endregion

        #region Constructor(s)

        public ServerCapabilities(LoggingCapability?                                                                           Logging                = null,
                                  PromptsCapability?                                                                           Prompts                = null,
                                  ResourcesCapability?                                                                         Resources              = null,
                                  ToolsCapability?                                                                             Tools                  = null,
                                  CompletionsCapability?                                                                       Completions            = null,
                                  Dictionary<String, Object>?                                                                  Experimental           = null,
                                  JObject?                                                                                     CustomData             = null,

                                  IEnumerable<KeyValuePair<String, Func<JSONRPCNotification, CancellationToken, ValueTask>>>?  NotificationHandlers   = null)

            : base(CustomData)

        {

            this.Logging               = Logging;
            this.Prompts               = Prompts;
            this.Resources             = Resources;
            this.Tools                 = Tools;
            this.Completions           = Completions;
            this.Experimental          = Experimental;

            this.NotificationHandlers  = NotificationHandlers;

        }

        #endregion


        #region Documentation

        // {
        //     "logging": {},
        //     "tools": {
        //         "listChanged": true
        //     }
        // }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of ServerCapabilities.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomServerCapabilitiesParser">A delegate to parse custom ServerCapabilities.</param>
        public static ServerCapabilities Parse(JObject                                           JSON,
                                               CustomJObjectParserDelegate<ServerCapabilities>?  CustomServerCapabilitiesParser   = null)
        {

            if (TryParse(JSON,
                         out var serverCapabilities,
                         out var errorResponse,
                         CustomServerCapabilitiesParser))
            {
                return serverCapabilities;
            }

            throw new ArgumentException("The given JSON representation of ServerCapabilities is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out ServerCapabilities, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of ServerCapabilities.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ServerCapabilities">The parsed ServerCapabilities.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                       JSON,
                                       [NotNullWhen(true)]  out ServerCapabilities?  ServerCapabilities,
                                       [NotNullWhen(false)] out String?              ErrorResponse)

            => TryParse(JSON,
                        out ServerCapabilities,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of ServerCapabilities.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ServerCapabilities">The parsed ServerCapabilities.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomServerCapabilitiesParser">A delegate to parse custom ServerCapabilities.</param>
        public static Boolean TryParse(JObject                                           JSON,
                                       [NotNullWhen(true)]  out ServerCapabilities?      ServerCapabilities,
                                       [NotNullWhen(false)] out String?                  ErrorResponse,
                                       CustomJObjectParserDelegate<ServerCapabilities>?  CustomServerCapabilitiesParser   = null)
        {

            ErrorResponse = null;

            try
            {

                ServerCapabilities = null;

                #region LoggingCapability        [optional]

                if (JSON.ParseOptionalJSON("logging",
                                           "logging capability",
                                           LoggingCapability.TryParse,
                                           out LoggingCapability? loggingCapability,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region PromptsCapability        [optional]

                if (JSON.ParseOptionalJSON("prompts",
                                           "prompts capability",
                                           PromptsCapability.TryParse,
                                           out PromptsCapability? promptsCapability,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region ResourcesCapability      [optional]

                if (JSON.ParseOptionalJSON("resources",
                                           "resources capability",
                                           ResourcesCapability.TryParse,
                                           out ResourcesCapability? resourcesCapability,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region ToolsCapability          [optional]

                if (JSON.ParseOptionalJSON("tools",
                                           "tools capability",
                                           ToolsCapability.TryParse,
                                           out ToolsCapability? toolsCapability,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region CompletionsCapability    [optional]

                if (JSON.ParseOptionalJSON("completions",
                                           "completions capability",
                                           CompletionsCapability.TryParse,
                                           out CompletionsCapability? completionsCapability,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region CustomData               [optional]

                var customData = ParseCustomData(
                                     JSON,
                                     [
                                         "logging",
                                         "prompts",
                                         "resources",
                                         "tools",
                                         "completions"
                                     ]
                                 );

                #endregion


                ServerCapabilities = new ServerCapabilities(

                                         loggingCapability,
                                         promptsCapability,
                                         resourcesCapability,
                                         toolsCapability,
                                         completionsCapability,
                                         null,
                                         customData

                                     );

                if (CustomServerCapabilitiesParser is not null)
                    ServerCapabilities = CustomServerCapabilitiesParser(JSON,
                                                                        ServerCapabilities);

                return true;

            }
            catch (Exception e)
            {
                ServerCapabilities  = null;
                ErrorResponse       = "The given JSON representation of ServerCapabilities is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomServerCapabilitiesSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomServerCapabilitiesSerializer">A delegate to serialize custom ServerCapabilities.</param>
        public JObject ToJSON(Boolean                                                  IncludeJSONLDContext                    = false,
                              CustomJObjectSerializerDelegate<ServerCapabilities>?     CustomServerCapabilitiesSerializer      = null,
                              CustomJObjectSerializerDelegate<LoggingCapability>?      CustomLoggingCapabilitySerializer       = null,
                              CustomJObjectSerializerDelegate<PromptsCapability>?      CustomPromptsCapabilitySerializer       = null,
                              CustomJObjectSerializerDelegate<ResourcesCapability>?    CustomResourcesCapabilitySerializer     = null,
                              CustomJObjectSerializerDelegate<ToolsCapability>?        CustomToolsCapabilitySerializer         = null,
                              CustomJObjectSerializerDelegate<CompletionsCapability>?  CustomCompletionsCapabilitySerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",      DefaultJSONLDContext.ToString())
                               : null,

                           Logging      is not null
                               ? new JProperty("logging",       Logging.             ToJSON(false,
                                                                                            CustomLoggingCapabilitySerializer))
                               : null,

                           Prompts      is not null
                               ? new JProperty("prompts",       Prompts.             ToJSON(false,
                                                                                            CustomPromptsCapabilitySerializer))
                               : null,

                           Resources    is not null
                               ? new JProperty("resources",     Resources.           ToJSON(false,
                                                                                            CustomResourcesCapabilitySerializer))
                               : null,

                           Tools        is not null
                               ? new JProperty("tools",         Tools.               ToJSON(false,
                                                                                            CustomToolsCapabilitySerializer))
                               : null,

                           Completions  is not null
                               ? new JProperty("completions",   Completions.         ToJSON(false,
                                                                                            CustomCompletionsCapabilitySerializer))
                               : null,

                           Experimental is not null
                               ? new JProperty("experimental",  JSONObject.Create(
                                                                    Experimental.Select(exp => new JProperty(exp.Key, exp.Value.ToString()))
                                                                ))
                               : null

                       );

            return CustomServerCapabilitiesSerializer is not null
                       ? CustomServerCapabilitiesSerializer(this, json)
                       : json;

        }

        #endregion


    }

}
