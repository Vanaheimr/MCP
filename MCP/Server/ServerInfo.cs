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
    /// Provides the name and version of an MCP implementation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="ServerInfo"/> class is used to identify MCP clients and servers during the initialization handshake.
    /// It provides version and name information that can be used for compatibility checks, logging, and debugging.
    /// </para>
    /// <para>
    /// Both clients and servers provide this information during connection establishment.
    /// </para>
    /// <para>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </para>
    /// </remarks>
    public class ServerInfo : ACustomData
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/serverInfo");

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the implementation.
        /// </summary>
        /// <remarks>
        /// This is typically the name of the client or server library/application.
        /// </remarks>
        [JsonPropertyName("name")]
        public String  Name       { get; }

        /// <summary>
        /// Gets or sets the version of the implementation.
        /// </summary>
        /// <remarks>
        /// The version is used during client-server handshake to identify implementation versions,
        /// which can be important for troubleshooting compatibility issues or when reporting bugs.
        /// </remarks>
        [JsonPropertyName("version")]
        public String  Version    { get; }

        #endregion

        #region Constructor(s)

        public ServerInfo(String    Name,
                          String    Version,
                          JObject?  CustomData   = null)

            : base(CustomData)

        {

            this.Name     = Name.   Trim();
            this.Version  = Version.Trim();

        }

        #endregion


        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of a ServerInfo.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomServerInfoParser">A delegate to parse custom ServerInfos.</param>
        public static ServerInfo Parse(JObject                                   JSON,
                                       CustomJObjectParserDelegate<ServerInfo>?  CustomServerInfoParser   = null)
        {

            if (TryParse(JSON,
                         out var clientInfo,
                         out var errorResponse,
                         CustomServerInfoParser))
            {
                return clientInfo;
            }

            throw new ArgumentException("The given JSON representation of a ServerInfo is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out ServerInfo, out ErrorResponse, ...)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a ServerInfo.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ServerInfo">The parsed ServerInfo.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomServerInfoParser">A delegate to parse custom ServerInfos.</param>
        public static Boolean TryParse(JObject                               JSON,
                                       [NotNullWhen(true)]  out ServerInfo?  ServerInfo,
                                       [NotNullWhen(false)] out String?      ErrorResponse)

            => TryParse(JSON,
                        out ServerInfo,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a ServerInfo.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ServerInfo">The parsed ServerInfo.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomServerInfoParser">A delegate to parse custom ServerInfos.</param>
        public static Boolean TryParse(JObject                                   JSON,
                                       [NotNullWhen(true)]  out ServerInfo?      ServerInfo,
                                       [NotNullWhen(false)] out String?          ErrorResponse,
                                       CustomJObjectParserDelegate<ServerInfo>?  CustomServerInfoParser   = null)
        {

            ErrorResponse = null;

            try
            {

                ServerInfo = null;

                #region Name          [mandatory]

                if (!JSON.ParseMandatoryText("name",
                                             "name",
                                             out String? name,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Version       [mandatory]

                if (!JSON.ParseMandatoryText("version",
                                             "version",
                                             out String? version,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region CustomData    [optional]

                var customData = ParseCustomData(
                                     JSON,
                                     [ "sampling", "roots", "elicitation" ]
                                 );

                #endregion


                ServerInfo = new ServerInfo(
                                 name,
                                 version,
                                 customData
                             );

                if (CustomServerInfoParser is not null)
                    ServerInfo = CustomServerInfoParser(JSON,
                                                        ServerInfo);

                return true;

            }
            catch (Exception e)
            {
                ServerInfo     = null;
                ErrorResponse  = "The given JSON representation of a ServerInfo is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomServerInfoSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomServerInfoSerializer">A delegate to serialize custom ServerInfos.</param>
        public JObject ToJSON(Boolean                                       IncludeJSONLDContext         = false,
                              CustomJObjectSerializerDelegate<ServerInfo>?  CustomServerInfoSerializer   = null)
        {

            var json = CreateJSON(

                           IncludeJSONLDContext
                               ? new JProperty("@context",   DefaultJSONLDContext.ToString())
                               : null,

                                 new JProperty("name",       Name),
                                 new JProperty("version",    Version)

                       );

            return CustomServerInfoSerializer is not null
                       ? CustomServerInfoSerializer(this, json)
                       : json;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Returns a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"'{Name}'/'{Version}'";

        #endregion

    }

}
