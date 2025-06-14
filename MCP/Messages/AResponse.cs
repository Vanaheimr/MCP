﻿/*
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

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// An abstract generic response.
    /// </summary>
    public abstract class AResponse<TRequest, TResponse> : AResponse<TResponse>

        where TRequest  : class, IRequest
        where TResponse : class, IResponse//<Result>

    {

        #region Properties

        /// <summary>
        /// The request leading to this response.
        /// </summary>
        public TRequest  Request    { get; }

        #endregion

        #region Constructor(s)

        #region AResponse(Request, Result, ResponseTimestamp = null, ...)

        /// <summary>
        /// Create a new abstract generic response.
        /// </summary>
        /// <param name="Request">The request leading to this result.</param>
        /// <param name="Result">A generic result.</param>
        /// <param name="ResponseTimestamp">An optional response timestamp.</param>
        /// 
        /// <param name="Destination">The destination networking node identification or source routing path.</param>
        /// 
        /// <param name="SignKeys">An optional enumeration of keys to be used for signing this response.</param>
        /// <param name="SignInfos">An optional enumeration of information to be used for signing this response.</param>
        /// <param name="Signatures">An optional enumeration of cryptographic signatures.</param>
        /// 
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public AResponse(TRequest                 Request,
                       //  Result                   Result,
                         DateTime?                ResponseTimestamp     = null,

                         String?                  JSONRPCVersion        = null,
                         JObject?                 CustomData            = null,

                         //IEnumerable<KeyPair>?    SignKeys              = null,
                         //IEnumerable<SignInfo>?   SignInfos             = null,
                         //IEnumerable<Signature>?  Signatures            = null,

                         CancellationToken        CancellationToken     = default)

            : this(Request,
               //    Result,
                   ResponseTimestamp ?? Timestamp.Now,

                   JSONRPCVersion,
                   CustomData,

                   //SignKeys,
                   //SignInfos,
                   //Signatures,

                   CancellationToken)

        { }

        #endregion

        #region AResponse(Request, Result, ResponseTimestamp, ...)

        /// <summary>
        /// Create a new abstract generic response.
        /// </summary>
        /// <param name="Request">The request leading to this result.</param>
        /// <param name="Result">A generic result.</param>
        /// <param name="ResponseTimestamp">The response timestamp.</param>
        /// 
        /// <param name="SignKeys">An optional enumeration of keys to be used for signing this response.</param>
        /// <param name="SignInfos">An optional enumeration of information to be used for signing this response.</param>
        /// <param name="Signatures">An optional enumeration of cryptographic signatures.</param>
        /// 
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public AResponse(TRequest                 Request,
                         //Result                   Result,
                         DateTime                 ResponseTimestamp,

                         String?                  JSONRPCVersion        = null,
                         JObject?                 CustomData            = null,

                         //IEnumerable<KeyPair>?    SignKeys              = null,
                         //IEnumerable<SignInfo>?   SignInfos             = null,
                         //IEnumerable<Signature>?  Signatures            = null,

                         CancellationToken        CancellationToken     = default)

            : base(//Result,
                   Request.Id,
                   ResponseTimestamp,
                   ResponseTimestamp - Request.RequestTimestamp,

                   JSONRPCVersion,
                   CustomData,

                   //SignKeys,
                   //SignInfos,
                   //Signatures,

                   CancellationToken)

        {

            this.Request = Request;

            unchecked
            {
                hashCode = this.Request.GetHashCode() * 3 ^
                           base.        GetHashCode();
            }

        }

        #endregion

        #endregion


        #region GenericEquals(AResponse)

        /// <summary>
        /// Compares two abstract generic responses for equality.
        /// </summary>
        /// <param name="AResponse">An abstract generic response to compare with.</param>
        public Boolean GenericEquals(AResponse<TRequest, TResponse> AResponse)

            => AResponse is not null &&

             ((Request is     null && AResponse.Request is     null) ||
              (Request is not null && AResponse.Request is not null && Request.Equals(AResponse.Request))) &&

               Runtime.        Equals(AResponse.Runtime) &&

               base.BaseGenericEquals(AResponse);

        #endregion


        public JObject ToAbstractJSON(JObject RequestData, JObject ResponseData)
        {

            var json = JSONObject.Create(

                           new JProperty("eventTrackingId",   Request.EventTrackingId.  ToString()),

                           new JProperty("request",           JSONObject.Create(
                               new JProperty("id",                Request.Id.              ToString()),
                               new JProperty("timestamp",         Request.RequestTimestamp.ToISO8601()),
                               new JProperty("timeout",           Request.RequestTimeout.  TotalSeconds),
                               new JProperty("method",            Request.Method),
                               new JProperty("data",              RequestData)
                           )),

                           new JProperty("response",          JSONObject.Create(
                               new JProperty("timestamp",         ResponseTimestamp.       ToISO8601()),
                               new JProperty("runtime",           Runtime.TotalMilliseconds),
                               new JProperty("data",              ResponseData)
                           ))

                       );

            return json;

        }


        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => hashCode ^
               base.GetHashCode();

        #endregion

    }


    /// <summary>
    /// An abstract generic response.
    /// </summary>
    public abstract class AResponse<TResponse> : JSONRPCResponse,
                                                 IEquatable<TResponse>

        where TResponse : class, IResponse//<Result>

    {

        #region Properties

        /// <summary>
        /// The machine-readable result code.
        /// </summary>
        //[Mandatory]
        //public Result                Result                 { get; }

        /// <summary>
        /// The timestamp of the response message.
        /// </summary>
        [Mandatory]
        public DateTime              ResponseTimestamp      { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        [Mandatory]
        public TimeSpan              Runtime                { get; }

        /// <summary>
        /// The Cancellation Token.
        /// </summary>
        [Mandatory]
        public CancellationToken     CancellationToken      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new abstract generic response.
        /// </summary>
        /// <param name="Result">A generic result.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response.</param>
        /// <param name="Runtime">The runtime of the request.</param>
        /// 
        /// <param name="Destination">An alternative source routing path through the overlay network towards the message destination.</param>
        /// <param name="NetworkPath">An networking path of the message through the overlay network.</param>
        /// 
        /// <param name="SignKeys">An optional enumeration of keys to be used for signing this response.</param>
        /// <param name="SignInfos">An optional enumeration of information to be used for signing this response.</param>
        /// <param name="Signatures">An optional enumeration of cryptographic signatures.</param>
        /// 
        /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
        public AResponse(//Result                   Result,
                         Request_Id Id,
                         DateTime                 ResponseTimestamp,
                         TimeSpan                 Runtime,

                         String?                  JSONRPCVersion        = null,
                         JObject?                 CustomData            = null,

                         //IEnumerable<KeyPair>?    SignKeys              = null,
                         //IEnumerable<SignInfo>?   SignInfos             = null,
                         //IEnumerable<Signature>?  Signatures            = null,

                         CancellationToken        CancellationToken     = default)

            : base(Id,
                   new JObject(),//Parameters,
                   JSONRPCVersion,
                   CustomData)
                   //SignKeys,
                   //SignInfos,
                   //Signatures,
                   //
                   //CustomData)

        {

            //this.Result               = Result;
            this.ResponseTimestamp    = ResponseTimestamp;
            this.Runtime              = Runtime;

            this.CancellationToken    = CancellationToken;

            unchecked
            {

                hashCode = //this.Result.           GetHashCode() * 5 ^
                           this.ResponseTimestamp.GetHashCode() * 3 ^
                           this.Runtime.          GetHashCode();

            }

        }

        #endregion


        #region Operator overloading

        #region Operator == (AResponse1, AResponse2)

        /// <summary>
        /// Compares two responses for equality.
        /// </summary>
        /// <param name="AResponse1">A response.</param>
        /// <param name="AResponse2">Another response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AResponse<TResponse>? AResponse1,
                                           AResponse<TResponse>? AResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AResponse1, AResponse2))
                return true;

            // If one is null, but not both, return false.
            if (AResponse1 is null || AResponse2 is null)
                return false;

            return AResponse1.Equals(AResponse2);

        }

        #endregion

        #region Operator != (AResponse1, AResponse2)

        /// <summary>
        /// Compares two responses for inequality.
        /// </summary>
        /// <param name="AResponse1">A response.</param>
        /// <param name="AResponse2">Another response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AResponse<TResponse>? AResponse1,
                                           AResponse<TResponse>? AResponse2)

            => !(AResponse1 == AResponse2);

        #endregion

        #endregion

        #region IEquatable<AResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two abstract generic responses for equality.
        /// </summary>
        /// <param name="Object">An abstract generic response to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is AResponse<TResponse> aResponse &&
                   Equals(aResponse);

        #endregion

        #region BaseGenericEquals(AResponse)

        /// <summary>
        /// Compares two abstract generic responses for equality.
        /// </summary>
        /// <param name="AResponse">An abstract generic response to compare with.</param>
        public Boolean BaseGenericEquals(AResponse<TResponse> AResponse)

            => AResponse is not null &&

               //Result.           Equals(AResponse.Result)            &&
               ResponseTimestamp.Equals(AResponse.ResponseTimestamp);

          //   ((CustomData is     null && AResponse.CustomData is     null) ||
          //    (CustomData is not null && AResponse.CustomData is not null && CustomData.Equals(AResponse.CustomData)));

        #endregion

        #region IEquatable<AResponse> Members

        /// <summary>
        /// Compares two abstract generic responses for equality.
        /// </summary>
        /// <param name="AResponse">An abstract generic response to compare with.</param>
        public abstract Boolean Equals(TResponse? AResponse);

        #endregion

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

            => "";// Result.ToString();

        #endregion

    }

}
