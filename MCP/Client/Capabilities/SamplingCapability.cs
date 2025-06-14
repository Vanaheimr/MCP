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

    public delegate ValueTask<CreateMessageResult> SamplingHandlerDelegate(CreateMessageRequestParams?           CreateMessageRequestParams,
                                                                           IProgress<ProgressNotificationValue>  ProgressNotificationValue,
                                                                           CancellationToken                     CancellationToken);


    /// <summary>
    /// Represents the capability for a client to generate text or other content using an AI model.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This capability enables the MCP client to respond to sampling requests from an MCP server.
    /// </para>
    /// <para>
    /// When this capability is enabled, an MCP server can request the client to generate content
    /// using an AI model. The client must set a <see cref="SamplingHandler"/> to process these requests.
    /// </para>
    /// </remarks>
    public class SamplingCapability : ACustomData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/samplingCapability");

        #endregion

        #region Properties

        // Currently empty in the spec, but may be extended in the future

        #endregion

        #region Handlers

        /// <summary>
        /// Gets or sets the handler for processing <see cref="RequestMethods.SamplingCreateMessage"/> requests.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This handler function is called when an MCP server requests the client to generate content
        /// using an AI model. The client must set this property for the sampling capability to work.
        /// </para>
        /// <para>
        /// The handler receives message parameters, a progress reporter for updates, and a 
        /// cancellation token. It should return a <see cref="CreateMessageResult"/> containing the 
        /// generated content.
        /// </para>
        /// <para>
        /// You can create a handler using the <see cref="McpClientExtensions.CreateSamplingHandler"/> extension
        /// method with any implementation of <see cref="IChatClient"/>.
        /// </para>
        /// </remarks>
        [JsonIgnore]
        public SamplingHandlerDelegate?  SamplingHandler   { get; }

        #endregion

        #region Constructor(s)

        public SamplingCapability(JObject?                  CustomData        = null,
                                  SamplingHandlerDelegate?  SamplingHandler   = null)

            : base(CustomData)

        {

            this.SamplingHandler  = SamplingHandler;

        }

        #endregion


        #region Documentation

        // { }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of a SamplingCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomSamplingCapabilityParser">A delegate to parse custom SamplingCapabilities.</param>
        public static SamplingCapability Parse(JObject                                           JSON,
                                               CustomJObjectParserDelegate<SamplingCapability>?  CustomSamplingCapabilityParser   = null)
        {

            if (TryParse(JSON,
                         out var samplingCapability,
                         out var errorResponse,
                         CustomSamplingCapabilityParser))
            {
                return samplingCapability;
            }

            throw new ArgumentException("The given JSON representation of a SamplingCapability is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out SamplingCapability, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a SamplingCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="SamplingCapability">The parsed SamplingCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                       JSON,
                                       [NotNullWhen(true)]  out SamplingCapability?  SamplingCapability,
                                       [NotNullWhen(false)] out String?              ErrorResponse)

            => TryParse(JSON,
                        out SamplingCapability,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a SamplingCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="SamplingCapability">The parsed SamplingCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomSamplingCapabilityParser">A delegate to parse custom SamplingCapabilities.</param>
        public static Boolean TryParse(JObject                                           JSON,
                                       [NotNullWhen(true)]  out SamplingCapability?      SamplingCapability,
                                       [NotNullWhen(false)] out String?                  ErrorResponse,
                                       CustomJObjectParserDelegate<SamplingCapability>?  CustomSamplingCapabilityParser   = null)
        {

            ErrorResponse = null;

            try
            {

                SamplingCapability = null;

                #region CustomData    [optional]

                var customData = ParseCustomData(
                                     JSON,
                                     [ ]
                                 );

                #endregion


                SamplingCapability = new SamplingCapability(
                                         customData
                                     );

                if (CustomSamplingCapabilityParser is not null)
                    SamplingCapability = CustomSamplingCapabilityParser(JSON,
                                                                        SamplingCapability);

                return true;

            }
            catch (Exception e)
            {
                SamplingCapability  = null;
                ErrorResponse       = "The given JSON representation of SamplingCapability is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomSamplingCapabilitySerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomSamplingCapabilitySerializer">A delegate to serialize custom SamplingCapabilities.</param>
        public JObject ToJSON(Boolean                                            IncludeJSONLDContext              = false,
                              CustomJObjectSerializerDelegate<SamplingCapability>?  CustomSamplingCapabilitySerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",      DefaultJSONLDContext.ToString())
                               : null

                       );

            return CustomSamplingCapabilitySerializer is not null
                       ? CustomSamplingCapabilitySerializer(this, json)
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
