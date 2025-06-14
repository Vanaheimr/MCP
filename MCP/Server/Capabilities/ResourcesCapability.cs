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

    public delegate ValueTask<ReadResourceResult>           ReadResourceDelegate             (RequestContext<ReadResourceRequestParams>?           ReadResourceRequestParams,
                                                                                              CancellationToken                                    CancellationToken);

    public delegate ValueTask<ListResourcesResult>          ListResourcesDelegate            (RequestContext<ListResourcesRequestParams>?          ListResourcesRequestParams,
                                                                                              CancellationToken                                    CancellationToken);

    public delegate ValueTask<ListResourceTemplatesResult>  ListResourceTemplatesDelegate    (RequestContext<ListResourceTemplatesRequestParams>?  ListResourceTemplatesRequestParams,
                                                                                              CancellationToken                                    CancellationToken);

    public delegate ValueTask<EmptyResult>                  SubscribeToResourcesDelegate     (RequestContext<SubscribeRequestParams>?              SubscribeRequestParams,
                                                                                              CancellationToken                                    CancellationToken);

    public delegate ValueTask<EmptyResult>                  UnsubscribeFromResourcesDelegate (RequestContext<UnsubscribeRequestParams>?            UnsubscribeRequestParams,
                                                                                              CancellationToken                                    CancellationToken);


    /// <summary>
    /// Represents the resources capability configuration.
    /// </summary>
    /// <remarks>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </remarks>
    public class ResourcesCapability : ACustomData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/resourcesCapability");


        /// <summary>
        /// Gets or sets a collection of resources served by the server.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Resources specified via <see cref="ResourceCollection"/> augment the <see cref="ListResourcesHandler"/>, <see cref="ListResourceTemplatesHandler"/>
        /// and <see cref="ReadResourceHandler"/> handlers, if provided. Resources with template expressions in their URI templates are considered resource templates
        /// and are listed via ListResourceTemplate, whereas resources without template parameters are considered static resources and are listed with ListResources.
        /// </para>
        /// <para>
        /// ReadResource requests will first check the <see cref="ResourceCollection"/> for the exact resource being requested. If no match is found, they'll proceed to
        /// try to match the resource against each resource template in <see cref="ResourceCollection"/>. If no match is still found, the request will fall back to
        /// any handler registered for <see cref="ReadResourceHandler"/>.
        /// </para>
        /// </remarks>
        [JsonIgnore]
        public MCPServerPrimitiveCollection<McpServerResource>?  ResourceCollection    { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether this server supports subscribing to resource updates.
        /// </summary>
        [JsonPropertyName("subscribe")]
        public Boolean?  Subscribe      { get; }

        /// <summary>
        /// Gets or sets whether this server supports notifications for changes to the resource list.
        /// </summary>
        /// <remarks>
        /// When set to <see langword="true"/>, the server will send notifications using 
        /// <see cref="NotificationMethods.ResourceListChangedNotification"/> when resources are added, 
        /// removed, or modified. Clients can register handlers for these notifications to
        /// refresh their resource cache.
        /// </remarks>
        [JsonPropertyName("listChanged")]
        public Boolean?  ListChanged    { get; }

        #endregion

        #region Handlers

        /// <summary>
        /// Gets or sets the handler for <see cref="RequestMethods.ResourcesRead"/> requests.
        /// </summary>
        /// <remarks>
        /// This handler is responsible for retrieving the content of a specific resource identified by its URI in the Model Context Protocol.
        /// When a client sends a resources/read request, this handler is invoked with the resource URI.
        /// The handler should implement logic to locate and retrieve the requested resource, then return
        /// its contents in a ReadResourceResult object.
        /// </remarks>
        [JsonIgnore]
        public ReadResourceDelegate?              ReadResourceHandler                { get; }

        /// <summary>
        /// Gets or sets the handler for <see cref="RequestMethods.ResourcesList"/> requests.
        /// </summary>
        /// <remarks>
        /// This handler responds to client requests for available resources and returns information about resources accessible through the server.
        /// The implementation should return a <see cref="ListResourcesResult"/> with the matching resources.
        /// </remarks>
        [JsonIgnore]
        public ListResourcesDelegate?             ListResourcesHandler               { get; }

        /// <summary>
        /// Gets or sets the handler for <see cref="RequestMethods.ResourcesTemplatesList"/> requests.
        /// </summary>
        /// <remarks>
        /// This handler is called when clients request available resource templates that can be used
        /// to create resources within the Model Context Protocol server.
        /// Resource templates define the structure and URI patterns for resources accessible in the system,
        /// allowing clients to discover available resource types and their access patterns.
        /// </remarks>
        [JsonIgnore]
        public ListResourceTemplatesDelegate?     ListResourceTemplatesHandler       { get; }

        /// <summary>
        /// Gets or sets the handler for <see cref="RequestMethods.ResourcesSubscribe"/> requests.
        /// </summary>
        /// <remarks>
        /// When a client sends a <see cref="RequestMethods.ResourcesSubscribe"/> request, this handler is invoked with the resource URI
        /// to be subscribed to. The implementation should register the client's interest in receiving updates
        /// for the specified resource.
        /// Subscriptions allow clients to receive real-time notifications when resources change, without
        /// requiring polling.
        /// </remarks>
        [JsonIgnore]
        public SubscribeToResourcesDelegate?      SubscribeToResourcesHandler        { get; }

        /// <summary>
        /// Gets or sets the handler for <see cref="RequestMethods.ResourcesUnsubscribe"/> requests.
        /// </summary>
        /// <remarks>
        /// When a client sends a <see cref="RequestMethods.ResourcesUnsubscribe"/> request, this handler is invoked with the resource URI
        /// to be unsubscribed from. The implementation should remove the client's registration for receiving updates
        /// about the specified resource.
        /// </remarks>
        [JsonIgnore]
        public UnsubscribeFromResourcesDelegate?  UnsubscribeFromResourcesHandler    { get; }

        #endregion

        #region Constructor(s)

        public ResourcesCapability(Boolean?                                          Subscribe                         = null,
                                   Boolean?                                          ListChanged                       = null,
                                   JObject?                                          CustomData                        = null,

                                   ReadResourceDelegate?                             ReadResourceHandler               = null,
                                   ListResourcesDelegate?                            ListResourcesHandler              = null,
                                   ListResourceTemplatesDelegate?                    ListResourceTemplatesHandler      = null,
                                   SubscribeToResourcesDelegate?                     SubscribeToResourcesHandler       = null,
                                   UnsubscribeFromResourcesDelegate?                 UnsubscribeFromResourcesHandler   = null,

                                   MCPServerPrimitiveCollection<McpServerResource>?  ResourceCollection                = null)

            : base(CustomData)

        {

            this.Subscribe                        = Subscribe;
            this.ListChanged                      = ListChanged;

            this.ReadResourceHandler              = ReadResourceHandler;
            this.ListResourcesHandler             = ListResourcesHandler;
            this.ListResourceTemplatesHandler     = ListResourceTemplatesHandler;
            this.SubscribeToResourcesHandler      = SubscribeToResourcesHandler;
            this.UnsubscribeFromResourcesHandler  = UnsubscribeFromResourcesHandler;

        }

        #endregion


        #region Documentation

        // {
        //     "subscribe":   true
        //     "listChanged": true
        // }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of a ResourcesCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomResourcesCapabilityParser">A delegate to parse custom LoggingCapabilities.</param>
        public static ResourcesCapability Parse(JObject                                            JSON,
                                                CustomJObjectParserDelegate<ResourcesCapability>?  CustomResourcesCapabilityParser   = null)
        {

            if (TryParse(JSON,
                         out var resourcesCapability,
                         out var errorResponse,
                         CustomResourcesCapabilityParser))
            {
                return resourcesCapability;
            }

            throw new ArgumentException("The given JSON representation of a ResourcesCapability is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out ResourcesCapability, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a ResourcesCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ResourcesCapability">The parsed ResourcesCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                        JSON,
                                       [NotNullWhen(true)]  out ResourcesCapability?  ResourcesCapability,
                                       [NotNullWhen(false)] out String?               ErrorResponse)

            => TryParse(JSON,
                        out ResourcesCapability,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a ResourcesCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ResourcesCapability">The parsed ResourcesCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomResourcesCapabilityParser">A delegate to parse custom LoggingCapabilities.</param>
        public static Boolean TryParse(JObject                                            JSON,
                                       [NotNullWhen(true)]  out ResourcesCapability?      ResourcesCapability,
                                       [NotNullWhen(false)] out String?                   ErrorResponse,
                                       CustomJObjectParserDelegate<ResourcesCapability>?  CustomResourcesCapabilityParser   = null)
        {

            ErrorResponse = null;

            try
            {

                ResourcesCapability = null;

                #region Subscribe      [mandatory]

                if (JSON.ParseOptional("subscribe",
                                       "subscribe resources",
                                       out Boolean? subscribe,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

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
                                     [ "subscribe", "listChanged" ]
                                 );

                #endregion


                ResourcesCapability = new ResourcesCapability(
                                          subscribe,
                                          listChanged,
                                          customData
                                      );

                if (CustomResourcesCapabilityParser is not null)
                    ResourcesCapability = CustomResourcesCapabilityParser(JSON,
                                                                          ResourcesCapability);

                return true;

            }
            catch (Exception e)
            {
                ResourcesCapability  = null;
                ErrorResponse        = "The given JSON representation of a ResourcesCapability is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomResourcesCapabilitySerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomResourcesCapabilitySerializer">A delegate to serialize custom LoggingCapabilities.</param>
        public JObject ToJSON(Boolean                                                IncludeJSONLDContext                  = false,
                              CustomJObjectSerializerDelegate<ResourcesCapability>?  CustomResourcesCapabilitySerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",      DefaultJSONLDContext.ToString())
                               : null,

                           Subscribe.  HasValue
                               ? new JProperty("subscribe",     Subscribe.  Value)
                               : null,

                           ListChanged.HasValue
                               ? new JProperty("listChanged",   ListChanged.Value)
                               : null

                       );

            return CustomResourcesCapabilitySerializer is not null
                       ? CustomResourcesCapabilitySerializer(this, json)
                       : json;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Returns a text representation of this object.
        /// </summary>
        public override String ToString()

            => new String[] {

                   Subscribe.HasValue
                       ? $"subscribe: {Subscribe.Value}"
                       : "",

                   ListChanged.HasValue
                       ? $"listChanged: {ListChanged.Value}"
                       : ""

               }.AggregateWith(", ");

        #endregion

    }

}
