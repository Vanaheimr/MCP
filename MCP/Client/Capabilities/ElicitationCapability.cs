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

    public delegate ValueTask<ElicitResult> ElicitationHandlerDelegate(ElicitRequestParams?  ElicitRequestParams,
                                                                       CancellationToken     CancellationToken);


    /// <summary>
    /// Represents the capability for a client to provide server-requested additional information during interactions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This capability enables the MCP client to respond to elicitation requests from an MCP server.
    /// </para>
    /// <para>
    /// When this capability is enabled, an MCP server can request the client to provide additional information
    /// during interactions. The client must set a <see cref="ElicitationHandler"/> to process these requests.
    /// </para>
    /// </remarks>
    public class ElicitationCapability : ACustomData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/elicitationCapability");

        #endregion

        #region Properties

        // Currently empty in the spec, but may be extended in the future.

        /// <summary>
        /// Gets or sets the handler for processing <see cref="RequestMethods.ElicitationCreate"/> requests.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This handler function is called when an MCP server requests the client to provide additional
        /// information during interactions. The client must set this property for the elicitation capability to work.
        /// </para>
        /// <para>
        /// The handler receives message parameters and a cancellation token.
        /// It should return a <see cref="ElicitResult"/> containing the response to the elicitation request.
        /// </para>
        /// </remarks>
        [JsonIgnore]
        public ElicitationHandlerDelegate?  ElicitationHandler   { get; }

        #endregion

        #region Constructor(s)

        public ElicitationCapability(JObject?                     CustomData           = null,
                                     ElicitationHandlerDelegate?  ElicitationHandler   = null)

            : base(CustomData)

        {

            this.ElicitationHandler = ElicitationHandler;

        }

        #endregion


        #region Documentation

        // { }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of an ElicitationCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomElicitationCapabilityParser">A delegate to parse custom ElicitationCapabilities.</param>
        public static ElicitationCapability Parse(JObject                                              JSON,
                                                  CustomJObjectParserDelegate<ElicitationCapability>?  CustomElicitationCapabilityParser   = null)
        {

            if (TryParse(JSON,
                         out var elicitationCapability,
                         out var errorResponse,
                         CustomElicitationCapabilityParser))
            {
                return elicitationCapability;
            }

            throw new ArgumentException("The given JSON representation of an ElicitationCapability is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out ElicitationCapability, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of an ElicitationCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ElicitationCapability">The parsed ElicitationCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                          JSON,
                                       [NotNullWhen(true)]  out ElicitationCapability?  ElicitationCapability,
                                       [NotNullWhen(false)] out String?                 ErrorResponse)

            => TryParse(JSON,
                        out ElicitationCapability,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of an ElicitationCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ElicitationCapability">The parsed ElicitationCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomElicitationCapabilityParser">A delegate to parse custom ElicitationCapabilities.</param>
        public static Boolean TryParse(JObject                                              JSON,
                                       [NotNullWhen(true)]  out ElicitationCapability?      ElicitationCapability,
                                       [NotNullWhen(false)] out String?                     ErrorResponse,
                                       CustomJObjectParserDelegate<ElicitationCapability>?  CustomElicitationCapabilityParser   = null)
        {

            ErrorResponse = null;

            try
            {

                ElicitationCapability = null;

                #region CustomData    [optional]

                var customData = ParseCustomData(
                                     JSON,
                                     [ ]
                                 );

                #endregion


                ElicitationCapability = new ElicitationCapability(
                                            customData
                                        );

                if (CustomElicitationCapabilityParser is not null)
                    ElicitationCapability = CustomElicitationCapabilityParser(JSON,
                                                                              ElicitationCapability);

                return true;

            }
            catch (Exception e)
            {
                ElicitationCapability  = null;
                ErrorResponse          = "The given JSON representation of an ElicitationCapability is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomElicitationCapabilitySerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomElicitationCapabilitySerializer">A delegate to serialize custom ElicitationCapabilities.</param>
        public JObject ToJSON(Boolean                                                  IncludeJSONLDContext                    = false,
                              CustomJObjectSerializerDelegate<ElicitationCapability>?  CustomElicitationCapabilitySerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",   DefaultJSONLDContext.ToString())
                               : null

                       );

            return CustomElicitationCapabilitySerializer is not null
                       ? CustomElicitationCapabilitySerializer(this, json)
                       : json;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Returns a text representation of this object.
        /// </summary>
        public override String ToString()

            => new String[] {

                   //ListChanged.HasValue
                   //    ? $"listChanged: {ListChanged.Value}"
                   //    : ""

               }.AggregateWith(", ");

        #endregion

    }

}
