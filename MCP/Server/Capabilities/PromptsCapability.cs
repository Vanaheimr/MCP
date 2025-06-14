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

    public delegate ValueTask<GetPromptResult>   GetPromptResultDelegate   (RequestContext<GetPromptRequestParams>?    GetPromptRequestParams,
                                                                            CancellationToken                          CancellationToken);

    public delegate ValueTask<ListPromptsResult> ListPromptsHandlerDelegate(RequestContext<ListPromptsRequestParams>?  ListPromptsRequestParams,
                                                                            CancellationToken                          CancellationToken);

    /// <summary>
    /// Represents the server's capability to provide predefined prompt templates that clients can use.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The prompts capability allows a server to expose a collection of predefined prompt templates that clients
    /// can discover and use. These prompts can be static (defined in the <see cref="PromptCollection"/>) or
    /// dynamically generated through handlers.
    /// </para>
    /// <para>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </para>
    /// </remarks>
    public class PromptsCapability : ACustomData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/promptsCapability");


        /// <summary>
        /// Gets or sets a collection of prompts that will be served by the server.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="PromptCollection"/> contains the predefined prompts that clients can request from the server.
        /// This collection works in conjunction with <see cref="ListPromptsHandler"/> and <see cref="GetPromptHandler"/>
        /// when those are provided:
        /// </para>
        /// <para>
        /// - For <see cref="RequestMethods.PromptsList"/> requests: The server returns all prompts from this collection 
        ///   plus any additional prompts provided by the <see cref="ListPromptsHandler"/> if it's set.
        /// </para>
        /// <para>
        /// - For <see cref="RequestMethods.PromptsGet"/> requests: The server first checks this collection for the requested prompt.
        ///   If not found, it will invoke the <see cref="GetPromptHandler"/> as a fallback if one is set.
        /// </para>
        /// </remarks>
        [JsonIgnore]
        public MCPServerPrimitiveCollection<McpServerPrompt>?  PromptCollection    { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether this server supports notifications for changes to the prompt list.
        /// </summary>
        /// <remarks>
        /// When set to <see langword="true"/>, the server will send notifications using 
        /// <see cref="NotificationMethods.PromptListChangedNotification"/> when prompts are added, 
        /// removed, or modified. Clients can register handlers for these notifications to
        /// refresh their prompt cache. This capability enables clients to stay synchronized with server-side changes 
        /// to available prompts.
        /// </remarks>
        [JsonPropertyName("listChanged")]
        public Boolean?  ListChanged    { get; }

        #endregion

        #region Handlers

        /// <summary>
        /// Gets or sets the handler for <see cref="RequestMethods.PromptsGet"/> requests.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This handler is invoked when a client requests details for a specific prompt by name and provides arguments 
        /// for the prompt if needed. The handler receives the request context containing the prompt name and any arguments, 
        /// and should return a <see cref="GetPromptResult"/> with the prompt messages and other details.
        /// </para>
        /// <para>
        /// This handler will be invoked if the requested prompt name is not found in the <see cref="PromptCollection"/>,
        /// allowing for dynamic prompt generation or retrieval from external sources.
        /// </para>
        /// </remarks>
        [JsonIgnore]
        public GetPromptResultDelegate?     GetPromptHandler      { get; }

        /// <summary>
        /// Gets or sets the handler for <see cref="RequestMethods.PromptsList"/> requests.
        /// </summary>
        /// <remarks>
        /// This handler is invoked when a client requests a list of available prompts from the server
        /// via a <see cref="RequestMethods.PromptsList"/> request. Results from this handler are returned
        /// along with any prompts defined in <see cref="PromptCollection"/>.
        /// </remarks>
        [JsonIgnore]
        public ListPromptsHandlerDelegate?  ListPromptsHandler    { get; }

        #endregion

        #region Constructor(s)

        public PromptsCapability(Boolean?                                        ListChanged          = null,
                                 JObject?                                        CustomData           = null,

                                 GetPromptResultDelegate?                        GetPromptHandler     = null,
                                 ListPromptsHandlerDelegate?                     ListPromptsHandler   = null,

                                 MCPServerPrimitiveCollection<McpServerPrompt>?  PromptCollection     = null)

            : base(CustomData)

        {

            this.ListChanged         = ListChanged;

            this.GetPromptHandler    = GetPromptHandler;
            this.ListPromptsHandler  = ListPromptsHandler;

            this.PromptCollection    = PromptCollection;

        }

        #endregion


        #region Documentation

        // {
        //     "listChanged": true
        // }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of a PromptsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomPromptsCapabilityParser">A delegate to parse custom LoggingCapabilities.</param>
        public static PromptsCapability Parse(JObject                                          JSON,
                                              CustomJObjectParserDelegate<PromptsCapability>?  CustomPromptsCapabilityParser   = null)
        {

            if (TryParse(JSON,
                         out var promptsCapability,
                         out var errorResponse,
                         CustomPromptsCapabilityParser))
            {
                return promptsCapability;
            }

            throw new ArgumentException("The given JSON representation of a PromptsCapability is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out PromptsCapability, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a PromptsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="PromptsCapability">The parsed PromptsCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                      JSON,
                                       [NotNullWhen(true)]  out PromptsCapability?  PromptsCapability,
                                       [NotNullWhen(false)] out String?             ErrorResponse)

            => TryParse(JSON,
                        out PromptsCapability,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a PromptsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="PromptsCapability">The parsed PromptsCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomPromptsCapabilityParser">A delegate to parse custom LoggingCapabilities.</param>
        public static Boolean TryParse(JObject                                          JSON,
                                       [NotNullWhen(true)]  out PromptsCapability?      PromptsCapability,
                                       [NotNullWhen(false)] out String?                 ErrorResponse,
                                       CustomJObjectParserDelegate<PromptsCapability>?  CustomPromptsCapabilityParser   = null)
        {

            ErrorResponse = null;

            try
            {

                PromptsCapability = null;

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


                PromptsCapability = new PromptsCapability(
                                        listChanged,
                                        customData
                                    );

                if (CustomPromptsCapabilityParser is not null)
                    PromptsCapability = CustomPromptsCapabilityParser(JSON,
                                                                      PromptsCapability);

                return true;

            }
            catch (Exception e)
            {
                PromptsCapability  = null;
                ErrorResponse      = "The given JSON representation of a PromptsCapability is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomPromptsCapabilitySerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomPromptsCapabilitySerializer">A delegate to serialize custom LoggingCapabilities.</param>
        public JObject ToJSON(Boolean                                              IncludeJSONLDContext                = false,
                              CustomJObjectSerializerDelegate<PromptsCapability>?  CustomPromptsCapabilitySerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",      DefaultJSONLDContext.ToString())
                               : null,

                           ListChanged.HasValue
                               ? new JProperty("listChanged",   ListChanged.Value)
                               : null

                       );

            return CustomPromptsCapabilitySerializer is not null
                       ? CustomPromptsCapabilitySerializer(this, json)
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
