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

using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Illias;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// An abstract generic request.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    public abstract class ARequest<TRequest> : JSONRPCRequest,
                                               IEquatable<TRequest>

        where TRequest : class, IRequest

    {

        #region Data

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public static readonly TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(15);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets metadata related to the request that provides additional protocol-level information.
        /// </summary>
        /// <remarks>
        /// This can include progress tracking tokens and other protocol-specific properties
        /// that are not part of the primary request parameters.
        /// </remarks>
        [JsonPropertyName("_meta")]
        public RequestParamsMetadata?  Meta                 { get; }

        /// <summary>
        /// The timestamp of the request message creation.
        /// </summary>
        [Mandatory]
        public DateTime                RequestTimestamp     { get; }

        /// <summary>
        /// The timeout of this request.
        /// </summary>
        [Mandatory]
        public TimeSpan                RequestTimeout       { get; }

        /// <summary>
        /// An event tracking identification for correlating this request with other events.
        /// </summary>
        [Mandatory]
        public EventTracking_Id        EventTrackingId      { get; }

        /// <summary>
        /// An optional token to cancel this request.
        /// </summary>
        public CancellationToken       CancellationToken    { get; }

        #endregion

        #region Constructor(s)

        public ARequest(Request_Id         Id,
                        String             Method,
                        JObject?           parameters          = null,
                        String?            JSONRPCVersion      = null,
                        JObject?           CustomData          = null,

                        DateTime?          RequestTimestamp    = null,
                        TimeSpan?          RequestTimeout      = null,
                        EventTracking_Id?  EventTrackingId     = null,
                        CancellationToken  CancellationToken   = default)

            : base(Id,
                   Method,
                   parameters is not null
                       ? JsonNode.Parse(parameters.ToString())
                       : null,
                   JSONRPCVersion,
                   CustomData)

        {

            this.RequestTimestamp   = RequestTimestamp ?? Timestamp.Now;
            this.RequestTimeout     = RequestTimeout   ?? DefaultRequestTimeout;
            this.EventTrackingId    = EventTrackingId  ?? EventTracking_Id.New;
            this.CancellationToken  = CancellationToken;

            unchecked
            {

                hashCode = this.Id.              GetHashCode() * 11 ^
                           this.Method.          GetHashCode() *  7 ^
                           this.RequestTimestamp.GetHashCode() *  5 ^
                           this.RequestTimeout.  GetHashCode() *  3 ^
                           this.EventTrackingId. GetHashCode();

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


        #region IEquatable<TRequest> Members

        /// <summary>
        /// Compare two abstract generic requests for equality.
        /// </summary>
        /// <param name="TRequest">Another abstract generic request.</param>
        public abstract Boolean Equals(TRequest? TRequest);

        #endregion

        #region GenericEquals(ARequest)

        /// <summary>
        /// Compare two abstract generic requests for equality.
        /// </summary>
        /// <param name="ARequest">Another abstract generic request.</param>
        public Boolean GenericEquals(ARequest<TRequest>? ARequest)

            => ARequest is not null &&

               Id.              Equals(ARequest.Id)               &&
               Method.          Equals(ARequest.Method)           &&
               RequestTimestamp.Equals(ARequest.RequestTimestamp) &&
               RequestTimeout.  Equals(ARequest.RequestTimeout)   &&
               EventTrackingId. Equals(ARequest.EventTrackingId);//           &&

            // ((CustomData is     null && ARequest.CustomData is     null) ||
            //  (CustomData is not null && ARequest.CustomData is not null && CustomData.Equals(ARequest.CustomData)));

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

            => $"{Method} [Id: {Id}]";

        #endregion


    }

}
