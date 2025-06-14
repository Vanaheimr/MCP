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

    public delegate ValueTask<EmptyResult> SetLoggingLevelHandlerDelegate(RequestContext<SetLevelRequestParams>?  SetLevelRequestParams,
                                                                          CancellationToken                       CancellationToken);

    /// <summary>
    /// Represents the logging capability configuration for a Model Context Protocol server.
    /// </summary>
    /// <remarks>
    /// This capability allows clients to set the logging level and receive log messages from the server.
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </remarks>
    public class LoggingCapability : ACustomData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/loggingCapability");

        #endregion

        #region Properties

        // Currently empty in the spec, but may be extended in the future

        #endregion

        #region Handlers

        /// <summary>
        /// Gets or sets the handler for set logging level requests from clients.
        /// </summary>
        [JsonIgnore]
        public SetLoggingLevelHandlerDelegate?  SetLoggingLevelHandler    { get; }

        #endregion

        #region Constructor(s)

        public LoggingCapability(JObject?                         CustomData               = null,
                                 SetLoggingLevelHandlerDelegate?  SetLoggingLevelHandler   = null)

            : base(CustomData)

        {

            this.SetLoggingLevelHandler = SetLoggingLevelHandler;

        }

        #endregion


        #region Documentation

        // { }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of a LoggingCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomLoggingCapabilityParser">A delegate to parse custom LoggingCapabilities.</param>
        public static LoggingCapability Parse(JObject                                          JSON,
                                              CustomJObjectParserDelegate<LoggingCapability>?  CustomLoggingCapabilityParser   = null)
        {

            if (TryParse(JSON,
                         out var loggingCapability,
                         out var errorResponse,
                         CustomLoggingCapabilityParser))
            {
                return loggingCapability;
            }

            throw new ArgumentException("The given JSON representation of a LoggingCapability is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out LoggingCapability, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a LoggingCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="LoggingCapability">The parsed LoggingCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                      JSON,
                                       [NotNullWhen(true)]  out LoggingCapability?  LoggingCapability,
                                       [NotNullWhen(false)] out String?             ErrorResponse)

            => TryParse(JSON,
                        out LoggingCapability,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a LoggingCapability.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="LoggingCapability">The parsed LoggingCapability.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomLoggingCapabilityParser">A delegate to parse custom LoggingCapabilities.</param>
        public static Boolean TryParse(JObject                                          JSON,
                                       [NotNullWhen(true)]  out LoggingCapability?      LoggingCapability,
                                       [NotNullWhen(false)] out String?                 ErrorResponse,
                                       CustomJObjectParserDelegate<LoggingCapability>?  CustomLoggingCapabilityParser   = null)
        {

            ErrorResponse = null;

            try
            {

                LoggingCapability = null;

                #region CustomData    [optional]

                var customData = ParseCustomData(
                                     JSON,
                                     [ ]
                                 );

                #endregion


                LoggingCapability = new LoggingCapability(
                                        customData
                                    );

                if (CustomLoggingCapabilityParser is not null)
                    LoggingCapability = CustomLoggingCapabilityParser(JSON,
                                                                      LoggingCapability);

                return true;

            }
            catch (Exception e)
            {
                LoggingCapability  = null;
                ErrorResponse      = "The given JSON representation of a LoggingCapability is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomLoggingCapabilitySerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomLoggingCapabilitySerializer">A delegate to serialize custom LoggingCapabilities.</param>
        public JObject ToJSON(Boolean                                              IncludeJSONLDContext                = false,
                              CustomJObjectSerializerDelegate<LoggingCapability>?  CustomLoggingCapabilitySerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",   DefaultJSONLDContext.ToString())
                               : null

                       );

            return CustomLoggingCapabilitySerializer is not null
                       ? CustomLoggingCapabilitySerializer(this, json)
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
