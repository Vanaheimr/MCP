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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    public interface IResponse //: ISignableMessage
    {

        /// <summary>
        /// The timestamp of the response.
        /// </summary>
        [Mandatory]
        DateTime  ResponseTimestamp    { get; }


        ///// <summary>
        ///// The event tracking identification for correlating this request with other events.
        ///// </summary>
        //[Mandatory]
        //EventTracking_Id   EventTrackingId      { get; }

        /// <summary>
        /// The runtime of the request.
        /// </summary>
        [Mandatory]
        TimeSpan  Runtime              { get; }

    }


    /// <summary>
    /// The common interface of all response messages.
    /// </summary>
    public interface IResponse<T> : IResponse
        where T : IResult
    {

        /// <summary>
        /// The machine-readable result code.
        /// </summary>
        [Mandatory]
        T         Result               { get; }

    }

}
