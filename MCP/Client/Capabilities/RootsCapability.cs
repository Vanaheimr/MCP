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

    public delegate ValueTask<ListRootsResult> RootsHandlerDelegate(ListRootsRequestParams?  ListRootsRequestParams,
                                                                    CancellationToken        CancellationToken);


    /// <summary>
    /// Represents a client capability that enables root resource discovery in the Model Context Protocol.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When present in <see cref="RootsCapability"/>, it indicates that the client supports listing
    /// root URIs that serve as entry points for resource navigation.
    /// </para>
    /// <para>
    /// The roots capability establishes a mechanism for servers to discover and access the hierarchical 
    /// structure of resources provided by a client. Root URIs represent top-level entry points from which
    /// servers can navigate to access specific resources.
    /// </para>
    /// <para>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </para>
    /// </remarks>
    public class RootsCapability : ACustomData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/rootsCapability");

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether the client supports notifications for changes to the roots list.
        /// </summary>
        /// <remarks>
        /// When set to <see langword="true"/>, the client can notify servers when roots are added, 
        /// removed, or modified, allowing servers to refresh their roots cache accordingly.
        /// This enables servers to stay synchronized with client-side changes to available roots.
        /// </remarks>
        [JsonPropertyName("listChanged")]
        public Boolean?  ListChanged    { get; }

        #endregion

        #region Handlers

        /// <summary>
        /// Gets or sets the handler for <see cref="RequestMethods.RootsList"/> requests.
        /// </summary>
        /// <remarks>
        /// This handler is invoked when a client sends a <see cref="RequestMethods.RootsList"/> request to retrieve available roots.
        /// The handler receives request parameters and should return a <see cref="ListRootsResult"/> containing the collection of available roots.
        /// </remarks>
        [JsonIgnore]
        public RootsHandlerDelegate?  RootsHandler   { get; }

        #endregion

        #region Constructor(s)

        public RootsCapability(Boolean?               ListChanged    = null,
                               JObject?               CustomData     = null,
                               RootsHandlerDelegate?  RootsHandler   = null)

            : base(CustomData)

        {

            this.ListChanged   = ListChanged;
            this.RootsHandler  = RootsHandler;

        }

        #endregion


        #region Documentation

        // {
        //     "listChanged": true
        // }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of a RootsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomRootsCapabilityParser">A delegate to parse custom RootsCapabilities.</param>
        public static RootsCapability Parse(JObject                                        JSON,
                                            CustomJObjectParserDelegate<RootsCapability>?  CustomRootsCapabilityParser   = null)
        {

            if (TryParse(JSON,
                         out var rootsCapability,
                         out var errorResponse,
                         CustomRootsCapabilityParser))
            {
                return rootsCapability;
            }

            throw new ArgumentException("The given JSON representation of a RootsCapability is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out RootsCapability, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a RootsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RootsCapability">The parsed RootsCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomRootsCapabilityParser">A delegate to parse custom RootsCapabilities.</param>
        public static Boolean TryParse(JObject                                    JSON,
                                       [NotNullWhen(true)]  out RootsCapability?  RootsCapability,
                                       [NotNullWhen(false)] out String?           ErrorResponse)

            => TryParse(JSON,
                        out RootsCapability,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a RootsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RootsCapability">The parsed RootsCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomRootsCapabilityParser">A delegate to parse custom RootsCapabilities.</param>
        public static Boolean TryParse(JObject                                        JSON,
                                       [NotNullWhen(true)]  out RootsCapability?      RootsCapability,
                                       [NotNullWhen(false)] out String?               ErrorResponse,
                                       CustomJObjectParserDelegate<RootsCapability>?  CustomRootsCapabilityParser   = null)
        {

            ErrorResponse = null;

            try
            {

                RootsCapability = null;

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


                RootsCapability = new RootsCapability(
                                      listChanged,
                                      customData
                                  );

                if (CustomRootsCapabilityParser is not null)
                    RootsCapability = CustomRootsCapabilityParser(JSON,
                                                                  RootsCapability);

                return true;

            }
            catch (Exception e)
            {
                RootsCapability  = null;
                ErrorResponse    = "The given JSON representation of a RootsCapability is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomRootsCapabilitySerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomRootsCapabilitySerializer">A delegate to serialize custom RootsCapabilities.</param>
        public JObject ToJSON(Boolean                                            IncludeJSONLDContext              = false,
                              CustomJObjectSerializerDelegate<RootsCapability>?  CustomRootsCapabilitySerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",      DefaultJSONLDContext.ToString())
                               : null,

                           ListChanged.HasValue
                               ? new JProperty("listChanged",   ListChanged.Value)
                               : null

                       );

            return CustomRootsCapabilitySerializer is not null
                       ? CustomRootsCapabilitySerializer(this, json)
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
