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

    public delegate ValueTask<CompleteResult>  CompleteDelegate (RequestContext<CompleteRequestParams>?  CompleteRequestParams,
                                                                 CancellationToken                       CancellationToken);


    /// <summary>
    /// Represents the completions capability for providing auto-completion suggestions
    /// for prompt arguments and resource references.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When enabled, this capability allows a Model Context Protocol server to provide 
    /// auto-completion suggestions. This capability is advertised to clients during the initialize handshake.
    /// </para>
    /// <para>
    /// The primary function of this capability is to improve the user experience by offering
    /// contextual suggestions for argument values or resource identifiers based on partial input.
    /// </para>
    /// <para>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </para>
    /// </remarks>
    public class CompletionsCapability : ACustomData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/completionsCapability");

        #endregion

        #region Properties

        // Currently empty in the spec, but may be extended in the future.

        #endregion

        #region Handlers

        /// <summary>
        /// Gets or sets the handler for completion requests.
        /// </summary>
        /// <remarks>
        /// This handler provides auto-completion suggestions for prompt arguments or resource references in the Model Context Protocol.
        /// The handler receives a reference type (e.g., "ref/prompt" or "ref/resource") and the current argument value,
        /// and should return appropriate completion suggestions.
        /// </remarks>
        [JsonIgnore]
        public CompleteDelegate?  CompleteHandler    { get; }

        #endregion

        #region Constructor(s)

        public CompletionsCapability(JObject?           CustomData        = null,
                                     CompleteDelegate?  CompleteHandler   = null)

            : base(CustomData)

        {

            this.CompleteHandler = CompleteHandler;

        }

        #endregion


        #region Documentation

        // { }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of a CompletionsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomCompletionsCapabilityParser">A delegate to parse custom LoggingCapabilities.</param>
        public static CompletionsCapability Parse(JObject                                              JSON,
                                                  CustomJObjectParserDelegate<CompletionsCapability>?  CustomCompletionsCapabilityParser   = null)
        {

            if (TryParse(JSON,
                         out var completionsCapability,
                         out var errorResponse,
                         CustomCompletionsCapabilityParser))
            {
                return completionsCapability;
            }

            throw new ArgumentException("The given JSON representation of a CompletionsCapability is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out CompletionsCapability, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a CompletionsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CompletionsCapability">The parsed CompletionsCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                          JSON,
                                       [NotNullWhen(true)]  out CompletionsCapability?  CompletionsCapability,
                                       [NotNullWhen(false)] out String?                 ErrorResponse)

            => TryParse(JSON,
                        out CompletionsCapability,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a CompletionsCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CompletionsCapability">The parsed CompletionsCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomCompletionsCapabilityParser">A delegate to parse custom LoggingCapabilities.</param>
        public static Boolean TryParse(JObject                                              JSON,
                                       [NotNullWhen(true)]  out CompletionsCapability?      CompletionsCapability,
                                       [NotNullWhen(false)] out String?                     ErrorResponse,
                                       CustomJObjectParserDelegate<CompletionsCapability>?  CustomCompletionsCapabilityParser   = null)
        {

            ErrorResponse = null;

            try
            {

                CompletionsCapability = null;

                #region CustomData     [optional]

                var customData = ParseCustomData(
                                     JSON,
                                     [ "listChanged" ]
                                 );

                #endregion


                CompletionsCapability = new CompletionsCapability(
                                            customData
                                        );

                if (CustomCompletionsCapabilityParser is not null)
                    CompletionsCapability = CustomCompletionsCapabilityParser(JSON,
                                                                              CompletionsCapability);

                return true;

            }
            catch (Exception e)
            {
                CompletionsCapability  = null;
                ErrorResponse          = "The given JSON representation of a CompletionsCapability is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomCompletionsCapabilitySerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomCompletionsCapabilitySerializer">A delegate to serialize custom LoggingCapabilities.</param>
        public JObject ToJSON(Boolean                                                  IncludeJSONLDContext                    = false,
                              CustomJObjectSerializerDelegate<CompletionsCapability>?  CustomCompletionsCapabilitySerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",  DefaultJSONLDContext.ToString())
                               : null

                       );

            return CustomCompletionsCapabilitySerializer is not null
                       ? CustomCompletionsCapabilitySerializer(this, json)
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
