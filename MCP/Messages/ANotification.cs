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

using System.Text.Json.Nodes;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// An abstract generic notification.
    /// </summary>
    /// <typeparam name="TNotification">The type of the notification.</typeparam>
    public abstract class ANotification<TNotification> : JSONRPCNotification,
                                                         IEquatable<TNotification>

        where TNotification : class, INotification

    {

        #region Properties

        ///// <summary>
        ///// Gets or sets metadata related to the request that provides additional protocol-level information.
        ///// </summary>
        ///// <remarks>
        ///// This can include progress tracking tokens and other protocol-specific properties
        ///// that are not part of the primary request parameters.
        ///// </remarks>
        //[JsonPropertyName("_meta")]
        //public RequestParamsMetadata?  Meta                 { get; }

        /// <summary>
        /// The timestamp of the notification message creation.
        /// </summary>
        [Mandatory]
        public DateTime           NotificationTimestamp    { get; }

        /// <summary>
        /// An event tracking identification for correlating this notification with other events.
        /// </summary>
        [Mandatory]
        public EventTracking_Id   EventTrackingId          { get; }

        /// <summary>
        /// An optional token to cancel this request.
        /// </summary>
        public CancellationToken  CancellationToken        { get; }

        #endregion

        #region Constructor(s)

        public ANotification(String             Method,
                             JObject?           parameters              = null,
                             String?            JSONRPCVersion          = null,
                             JObject?           CustomData              = null,

                             DateTime?          NotificationTimestamp   = null,
                             EventTracking_Id?  EventTrackingId         = null,
                             CancellationToken  CancellationToken       = default)

            : base(Method,
                   parameters is not null
                       ? JsonNode.Parse(parameters.ToString())
                       : null,
                   JSONRPCVersion,
                   CustomData)

        {

            this.NotificationTimestamp  = NotificationTimestamp ?? Timestamp.Now;
            this.EventTrackingId        = EventTrackingId       ?? EventTracking_Id.New;
            this.CancellationToken      = CancellationToken;

            unchecked
            {

                hashCode = this.Method.               GetHashCode() * 5 ^
                           this.NotificationTimestamp.GetHashCode() * 3 ^
                           this.EventTrackingId.      GetHashCode();

            }

        }

        #endregion


        //public JObject ToAbstractJSON(Object RequestData)

        //    => ToAbstractJSON(null, RequestData);

        //public JObject ToAbstractJSON(IWebSocketConnection  Connection,
        //                              Object                RequestData)
        //{

        //    var json = JSONObject.Create(
        //                   new JProperty("id",               RequestId.       ToString()),
        //                   new JProperty("timestamp",        RequestTimestamp.ToISO8601()),
        //                   new JProperty("eventTrackingId",  EventTrackingId. ToString()),
        //                 //  new JProperty("connection",       Connection?.     ToJSON()),
        //                   new JProperty("destinationId",    DestinationId.   ToString()),
        //                   new JProperty("networkPath",      NetworkPath.     ToJSON()),
        //                   new JProperty("timeout",          RequestTimeout.  TotalSeconds),
        //                   new JProperty("action",           Action),
        //                   new JProperty("data",             RequestData)
        //               );

        //    return json;

        //}


        #region IEquatable<TNotification> Members

        /// <summary>
        /// Compare two abstract generic notifications for equality.
        /// </summary>
        /// <param name="TNotification">Another abstract generic notification.</param>
        public abstract Boolean Equals(TNotification? TNotification);

        #endregion

        #region GenericEquals(ANotification)

        /// <summary>
        /// Compare two abstract generic notifications for equality.
        /// </summary>
        /// <param name="ANotification">Another abstract generic notification.</param>
        public Boolean GenericEquals(ANotification<TNotification>? ANotification)

            => ANotification is not null &&

               Method.               Equals(ANotification.Method)                &&
               NotificationTimestamp.Equals(ANotification.NotificationTimestamp) &&
               EventTrackingId.      Equals(ANotification.EventTrackingId);//           &&

            // ((CustomData is     null && ANotification.CustomData is     null) ||
            //  (CustomData is not null && ANotification.CustomData is not null && CustomData.Equals(ANotification.CustomData)));

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => hashCode ^
               base.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"{Method}";

        #endregion


    }

}
