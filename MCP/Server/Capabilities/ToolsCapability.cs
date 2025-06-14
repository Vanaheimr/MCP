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
using org.GraphDefined.Vanaheimr.Hermod.MCP.Server;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    public delegate ValueTask<CallToolResponse>  CallToolDelegate   (RequestContext<CallToolRequestParams>?   CallToolRequestParams,
                                                                     CancellationToken                        CancellationToken);

    public delegate ValueTask<ListToolsResult>   ListToolsDelegate  (RequestContext<ListToolsRequestParams>?  ListToolsRequestParams,
                                                                     CancellationToken                        CancellationToken);


    /// <summary>
    /// Represents the tools capability configuration.
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </summary>
    public class ToolsCapability : ACustomData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/toolsCapability");


        /// <summary>
        /// Gets or sets a collection of tools served by the server.
        /// </summary>
        /// <remarks>
        /// Tools will specified via <see cref="ToolCollection"/> augment the <see cref="ListToolsHandler"/> and
        /// <see cref="CallToolHandler"/>, if provided. ListTools requests will output information about every tool
        /// in <see cref="ToolCollection"/> and then also any tools output by <see cref="ListToolsHandler"/>, if it's
        /// non-<see langword="null"/>. CallTool requests will first check <see cref="ToolCollection"/> for the tool
        /// being requested, and if the tool is not found in the <see cref="ToolCollection"/>, any specified <see cref="CallToolHandler"/>
        /// will be invoked as a fallback.
        /// </remarks>
        [JsonIgnore]
        public MCPServerPrimitiveCollection<AMCPServerTool>? ToolCollection { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether this server supports notifications for changes to the tool list.
        /// </summary>
        /// <remarks>
        /// When set to <see langword="true"/>, the server will send notifications using 
        /// <see cref="NotificationMethods.ToolListChangedNotification"/> when tools are added, 
        /// removed, or modified. Clients can register handlers for these notifications to
        /// refresh their tool cache. This capability enables clients to stay synchronized with server-side 
        /// changes to available tools.
        /// </remarks>
        [JsonPropertyName("listChanged")]
        public Boolean?  ListChanged    { get; }

        #endregion

        #region Handlers

        /// <summary>
        /// Gets or sets the handler for <see cref="RequestMethods.ToolsCall"/> requests.
        /// </summary>
        /// <remarks>
        /// This handler is invoked when a client makes a call to a tool that isn't found in the <see cref="ToolCollection"/>.
        /// The handler should implement logic to execute the requested tool and return appropriate results. 
        /// It receives a <see cref="RequestContext{CallToolRequestParams}"/> containing information about the tool 
        /// being called and its arguments, and should return a <see cref="CallToolResponse"/> with the execution results.
        /// </remarks>
        [JsonIgnore]
        public CallToolDelegate?   CallToolHandler     { get; }

        /// <summary>
        /// Gets or sets the handler for <see cref="RequestMethods.ToolsList"/> requests.
        /// </summary>
        /// <remarks>
        /// The handler should return a list of available tools when requested by a client.
        /// It supports pagination through the cursor mechanism, where the client can make
        /// repeated calls with the cursor returned by the previous call to retrieve more tools.
        /// When used in conjunction with <see cref="ToolCollection"/>, both the tools from this handler
        /// and the tools from the collection will be combined to form the complete list of available tools.
        /// </remarks>
        [JsonIgnore]
        public ListToolsDelegate?  ListToolsHandler    { get; }

        #endregion

        #region Constructor(s)

        public ToolsCapability(Boolean?                                      ListChanged        = null,
                               JObject?                                      CustomData         = null,

                               CallToolDelegate?                             CallToolHandler    = null,
                               ListToolsDelegate?                            ListToolsHandler   = null,

                               MCPServerPrimitiveCollection<AMCPServerTool>?  ToolCollection     = null)

            : base(CustomData)

        {

            this.ListChanged       = ListChanged;

            this.CallToolHandler   = CallToolHandler;
            this.ListToolsHandler  = ListToolsHandler;

            this.ToolCollection    = ToolCollection;

        }

        #endregion


        #region Documentation

        // {
        //     "listChanged": true
        // }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of a ToolsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomToolsCapabilityParser">A delegate to parse custom LoggingCapabilities.</param>
        public static ToolsCapability Parse(JObject                                          JSON,
                                              CustomJObjectParserDelegate<ToolsCapability>?  CustomToolsCapabilityParser   = null)
        {

            if (TryParse(JSON,
                         out var toolsCapability,
                         out var errorResponse,
                         CustomToolsCapabilityParser))
            {
                return toolsCapability;
            }

            throw new ArgumentException("The given JSON representation of a ToolsCapability is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out ToolsCapability, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a ToolsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ToolsCapability">The parsed ToolsCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                    JSON,
                                       [NotNullWhen(true)]  out ToolsCapability?  ToolsCapability,
                                       [NotNullWhen(false)] out String?           ErrorResponse)

            => TryParse(JSON,
                        out ToolsCapability,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a ToolsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ToolsCapability">The parsed ToolsCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomToolsCapabilityParser">A delegate to parse custom LoggingCapabilities.</param>
        public static Boolean TryParse(JObject                                        JSON,
                                       [NotNullWhen(true)]  out ToolsCapability?      ToolsCapability,
                                       [NotNullWhen(false)] out String?               ErrorResponse,
                                       CustomJObjectParserDelegate<ToolsCapability>?  CustomToolsCapabilityParser   = null)
        {

            ErrorResponse = null;

            try
            {

                ToolsCapability = null;

                #region ListChanged    [mandatory]

                if (JSON.ParseOptional("listChanged",
                                       "list changed",
                                       out Boolean? listChanged,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region CustomData     [optional]

                var customData = ParseCustomData(
                                     JSON,
                                     [ "listChanged" ]
                                 );

                #endregion


                ToolsCapability = new ToolsCapability(
                                      listChanged,
                                      customData
                                  );

                if (CustomToolsCapabilityParser is not null)
                    ToolsCapability = CustomToolsCapabilityParser(JSON,
                                                                  ToolsCapability);

                return true;

            }
            catch (Exception e)
            {
                ToolsCapability  = null;
                ErrorResponse    = "The given JSON representation of a ToolsCapability is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomToolsCapabilitySerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomToolsCapabilitySerializer">A delegate to serialize custom LoggingCapabilities.</param>
        public JObject ToJSON(Boolean                                            IncludeJSONLDContext              = false,
                              CustomJObjectSerializerDelegate<ToolsCapability>?  CustomToolsCapabilitySerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",      DefaultJSONLDContext.ToString())
                               : null,

                           ListChanged.HasValue
                               ? new JProperty("listChanged",   ListChanged.Value)
                               : null

                       );

            return CustomToolsCapabilitySerializer is not null
                       ? CustomToolsCapabilitySerializer(this, json)
                       : json;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Returns a text representation of this object.
        /// </summary>
        public override String ToString()

            => new String[] {

                   ListChanged.HasValue
                       ? $"listChanged: {ListChanged.Value}"
                       : ""

               }.AggregateWith(", ");

        #endregion

    }

}
