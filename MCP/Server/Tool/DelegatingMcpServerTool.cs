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

namespace org.GraphDefined.Vanaheimr.Hermod.MCP.Server
{

    /// <summary>
    /// Provides an <see cref="AMCPServerTool"/> that delegates all operations to an inner <see cref="AMCPServerTool"/>.
    /// </summary>
    /// <remarks>
    /// This is recommended as a base type when building tools that can be chained around an underlying <see cref="AMCPServerTool"/>.
    /// The default implementation simply passes each call to the inner tool instance.
    /// </remarks>
    public abstract class DelegatingMCPServerTool : AMCPServerTool
    {

        #region Data

        private readonly AMCPServerTool _innerTool;

        #endregion

        #region Properties

        /// <inheritdoc />
        public override Tool ProtocolTool
            => _innerTool.ProtocolTool;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegatingMCPServerTool"/> class around the specified <paramref name="innerTool"/>.
        /// </summary>
        /// <param name="innerTool">The inner tool wrapped by this delegating tool.</param>
        protected DelegatingMCPServerTool(AMCPServerTool innerTool)
        {
            _innerTool = innerTool;
        }

        #endregion


        #region (override) InvokeAsync(Request, ...)

        /// <inheritdoc />
        public override ValueTask<CallToolResponse> InvokeAsync(RequestContext<CallToolRequestParams>  Request,
                                                                CancellationToken                      CancellationToken = default)

            => _innerTool.InvokeAsync(
                   Request,
                   CancellationToken
               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Returns a text representation of this object.
        /// </summary>
        public override String ToString()

            => _innerTool.ToString();

        #endregion

    }

}
