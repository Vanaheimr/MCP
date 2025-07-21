/*
 * Copyright (c) 2010-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of Vanaheimr Hermod <https://www.github.com/Vanaheimr/Hermod>
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

using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.MCP;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod
{

    public delegate Task OnMCPRequestDelegate(JSONRPCRequest     Request,
                                              NetworkStream      Stream,
                                              CancellationToken  CancellationToken);


    /// <summary>
    /// A simple Model Context Protocol test server that listens for incoming TCP connections and processes MCP requests, supporting pipelining.
    /// </summary>
    /// <param name="IPAddress">The IP address to listen on. If null, the loopback address will be used.</param>
    /// <param name="TCPPort">The TCP port to listen on. If 0, a random TCP port will be assigned.</param>
    /// <param name="BufferSize">An optional buffer size for the TCP stream. If null, the default buffer size will be used.</param>
    /// <param name="ReceiveTimeout">An optional receive timeout for the TCP stream. If null, the default receive timeout will be used.</param>
    /// <param name="SendTimeout">An optional send timeout for the TCP stream. If null, the default send timeout will be used.</param>
    /// <param name="LoggingHandler">An optional logging handler that will be called for each log message.</param>
    public class MCPTestServer(IIPAddress?              IPAddress        = null,
                               IPPort?                  TCPPort          = null,
                               UInt32?                  BufferSize       = null,
                               TimeSpan?                ReceiveTimeout   = null,
                               TimeSpan?                SendTimeout      = null,
                               TCPEchoLoggingDelegate?  LoggingHandler   = null)

        : AHTTPTestServer(
              IPAddress,
              TCPPort,
              BufferSize,
              ReceiveTimeout,
              SendTimeout,
              LoggingHandler
          )

    {

        #region Events

        /// <summary>
        /// An event fired whenever an MCP JSON RPC request is ready for processing.
        /// </summary>
        public event OnMCPRequestDelegate? OnMCPRequest;

        #endregion


        #region StartNew(...)

        public static async Task<MCPTestServer>

            StartNew(IIPAddress?              IPAddress        = null,
                     IPPort?                  TCPPort          = null,
                     UInt32?                  BufferSize       = null,
                     TimeSpan?                ReceiveTimeout   = null,
                     TimeSpan?                SendTimeout      = null,
                     TCPEchoLoggingDelegate?  LoggingHandler   = null)

        {

            var server = new MCPTestServer(
                             IPAddress,
                             TCPPort,
                             BufferSize,
                             ReceiveTimeout,
                             SendTimeout,
                             LoggingHandler
                         );

            await server.Start();

            return server;

        }

        #endregion


        #region (override) ProcessHTTPRequest(Request, Stream, CancellationToken = default)

        protected async override Task

            ProcessHTTPRequest(HTTPRequest        Request,
                               NetworkStream      Stream,
                               CancellationToken  CancellationToken   = default)

        {

            #region Check for content type JSON

            if (Request.ContentType != HTTPContentType.Application.JSON_UTF8)
            {

                await SendResponse(
                          Stream,
                          new HTTPResponse.Builder(Request) {
                              HTTPStatusCode  = HTTPStatusCode.UnsupportedMediaType,
                              ContentType     = HTTPContentType.Text.PLAIN,
                              Content         = $"Unsupported content type: '{Request.ContentType}'!".ToUTF8Bytes(),
                              Connection      = ConnectionType.Close
                          },
                          CancellationToken
                      );

                return;

            }

            #endregion

            #region Check for Conent with length or chunked transfer encoding

            if ((!Request.ContentLength.HasValue || Request.ContentLength <= 0) &&
                 !Request.IsChunkedTransferEncoding)
            {

                await SendResponse(
                          Stream,
                          new HTTPResponse.Builder(Request) {
                              HTTPStatusCode  = HTTPStatusCode.BadRequest,
                              ContentType     = HTTPContentType.Text.PLAIN,
                              Content         = $"Missing or invalid HTTP body!".ToUTF8Bytes(),
                              Connection      = ConnectionType.Close
                          },
                          CancellationToken
                      );

                return;

            }

            #endregion

            #region Check for content is valid JSON object

            var jsonRPC = Request.HTTPBodyAsJSONObject;

            if (jsonRPC is null)
            {

                await SendResponse(
                          Stream,
                          new HTTPResponse.Builder(Request) {
                              HTTPStatusCode  = HTTPStatusCode.BadRequest,
                              ContentType     = HTTPContentType.Text.PLAIN,
                              Content         = $"Invalid JSON body!".ToUTF8Bytes(),
                              Connection      = ConnectionType.Close
                          },
                          CancellationToken
                      );

                return;

            }

            #endregion

            #region Try to parse the JSON RPC request

            if (!JSONRPCRequest.TryParse(jsonRPC,
                                         out var jsonRPCRequest,
                                         out var errorResponse,
                                         null,
                                         Request.EventTrackingId,
                                         Request.Timeout,
                                         null,
                                         CancellationToken))
            {
                await SendResponse(
                          Stream,
                          new HTTPResponse.Builder(Request) {
                              HTTPStatusCode  = HTTPStatusCode.BadRequest,
                              ContentType     = HTTPContentType.Text.PLAIN,
                              Content         = $"The JSON RPC request could not be parsed: {errorResponse}".ToUTF8Bytes(),
                              Connection      = ConnectionType.Close
                          },
                          CancellationToken
                      );
                return;
            }

            #endregion


            await LogEvent(
                      OnMCPRequest,
                      loggingDelegate => loggingDelegate.Invoke(
                          jsonRPCRequest,
                          Stream,
                          CancellationToken
                      )
                  );

        }

        #endregion


        #region (private) LogEvent (Logger, LogHandler, ...)

        private Task LogEvent<TDelegate>(TDelegate?                                         Logger,
                                         Func<TDelegate, Task>                              LogHandler,
                                         [CallerArgumentExpression(nameof(Logger))] String  EventName     = "",
                                         [CallerMemberName()]                       String  OICPCommand   = "")

            where TDelegate : Delegate

            => LogEvent(
                   nameof(MCPTestServer),
                   Logger,
                   LogHandler,
                   EventName,
                   OICPCommand
               );

        #endregion


    }

}
