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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// The name of the notification sent from the client to the server after initialization has finished.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This notification is sent by the client after it has received and processed the server's response to the 
    /// <see cref="RequestMethods.Initialize"/> request. It signals that the client is ready to begin normal operation 
    /// and that the initialization phase is complete.
    /// </para>
    /// <para>
    /// After receiving this notification, the server can begin sending notifications and processing
    /// further requests from the client.
    /// </para>
    /// </remarks>
    public class InitializedNotification : ANotification<InitializedNotification>,
                                           INotification
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://graphdefined.com/context/mcp/initializedNotification");


        public const String MethodName = "notifications/initialized";

        #endregion

        #region Constructor(s)

        public InitializedNotification() : base(MethodName)
        { }

        #endregion


        #region Documentation

        // {
        //     "jsonrpc":  "2.0",
        //     "method":   "notifications/initialized"
        // }

        #endregion

        #region (static) Parse    (JSON, ...)

        /// <summary>
        /// Parse the given JSON representation of an InitializedNotification.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomInitializedNotificationParser">A delegate to parse custom InitializedNotifications.</param>
        public static InitializedNotification Parse(JObject                                                JSON,
                                                    DateTime?                                              RequestTimestamp                      = null,
                                                    EventTracking_Id?                                      EventTrackingId                       = null,
                                                    CustomJObjectParserDelegate<InitializedNotification>?  CustomInitializedNotificationParser   = null)
        {

            if (TryParse(JSON,
                         out var initializedNotification,
                         out var errorResponse,
                         RequestTimestamp,
                         EventTrackingId,
                         CustomInitializedNotificationParser))
            {
                return initializedNotification;
            }

            throw new ArgumentException("The given JSON representation of an InitializedNotification is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse (JSON, out InitializedNotification, out ErrorResponse, ...)

        /// <summary>
        /// Try to parse the given JSON representation of an InitializedNotification.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="InitializedNotification">The parsed InitializedNotification.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CustomInitializedNotificationParser">A delegate to parse custom InitializedNotifications.</param>
        public static Boolean TryParse(JObject                                                JSON,
                                       [NotNullWhen(true)]  out InitializedNotification?      InitializedNotification,
                                       [NotNullWhen(false)] out String?                       ErrorResponse,
                                       DateTime?                                              RequestTimestamp                      = null,
                                       EventTracking_Id?                                      EventTrackingId                       = null,
                                       CustomJObjectParserDelegate<InitializedNotification>?  CustomInitializedNotificationParser   = null)
        {

            ErrorResponse = null;

            try
            {

                InitializedNotification = null;

                #region JSON-RPC Version    [mandatory]

                if (!JSON.ParseMandatoryText("jsonrpc",
                                             "JSON-RPC version",
                                             out String? jsonRPCVersion,
                                             out ErrorResponse))
                {
                    return false;
                }

                if (jsonRPCVersion != "2.0")
                {
                    ErrorResponse = "The JSON-RPC version must be '2.0'!";
                    return false;
                }

                #endregion

                #region Method              [mandatory]

                if (!JSON.ParseMandatoryText("method",
                                             "notification method",
                                             out String? method,
                                             out ErrorResponse))
                {
                    return false;
                }

                if (method != MethodName)
                {
                    ErrorResponse = $"The method must be '{MethodName}'!";
                    return false;
                }

                #endregion


                #region Signatures          [optional]

                //if (JSON.ParseOptionalHashSet("signatures",
                //                              "cryptographic signatures",
                //                              Signature.TryParse,
                //                              out HashSet<Signature> Signatures,
                //                              out ErrorResponse))
                //{
                //    if (ErrorResponse is not null)
                //        return false;
                //}

                #endregion

                #region CustomData          [optional]

                //if (JSON.ParseOptionalJSON("customData",
                //                           "custom data",
                //                           WWCP.CustomData.TryParse,
                //                           out CustomData? CustomData,
                //                           out ErrorResponse))
                //{
                //    if (ErrorResponse is not null)
                //        return false;
                //}

                #endregion


                InitializedNotification = new InitializedNotification();

                if (CustomInitializedNotificationParser is not null)
                    InitializedNotification = CustomInitializedNotificationParser(JSON,
                                                                                  InitializedNotification);

                return true;

            }
            catch (Exception e)
            {
                InitializedNotification  = null;
                ErrorResponse            = "The given JSON representation of an InitializedNotification is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomInitializedNotificationSerializer = null, CustomSignatureSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomInitializedNotificationSerializer">A delegate to serialize custom InitializedNotifications.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(Boolean                                                    IncludeJSONLDContext                      = false,
                              CustomJObjectSerializerDelegate<InitializedNotification>?  CustomInitializedNotificationSerializer   = null)
                              //CustomJObjectSerializerDelegate<Signature>?          CustomSignatureSerializer           = null)
        {

            var json = JSONObject.Create(

                           IncludeJSONLDContext
                               ? new JProperty("@context",   DefaultJSONLDContext.ToString())
                               : null,

                                 new JProperty("jsonrpc",    JSONRPCVersion),
                                 new JProperty("method",     Method)

                       //Signatures.Any()
                       //    ? new JProperty("signatures",         new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                       //                                                                                                     CustomCustomDataSerializer))))
                       //    : null

                       );

            return CustomInitializedNotificationSerializer is not null
                       ? CustomInitializedNotificationSerializer(this, json)
                       : json;

        }

        #endregion


        public override Boolean Equals(InitializedNotification? TRequest)
        {
            throw new NotImplementedException();
        }


    }


}
