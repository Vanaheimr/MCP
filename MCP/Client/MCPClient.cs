///*
// * Copyright (c) 2010-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
// * This file is part of Vanaheimr MCP <https://www.github.com/Vanaheimr/MCP>
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *     http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */

//using System.Text.Json;
//using Microsoft.Extensions.Logging;

//namespace org.GraphDefined.Vanaheimr.Hermod.MCP.Client
//{


//    /// <inheritdoc/>
//    internal sealed partial class MCPClient : MCPEndpoint, IMCPClient
//    {

//        private static ClientInfo DefaultImplementation { get; } = new (
//            Name:     DefaultAssemblyName.Name ?? nameof(MCPClient),
//            Version:  DefaultAssemblyName.Version?.ToString() ?? "1.0.0"
//        );

//        private readonly IClientTransport _clientTransport;
//        private readonly MCPClientOptions _options;

//        private ITransport? _sessionTransport;
//        private CancellationTokenSource? _connectCts;

//        private ServerCapabilities? _serverCapabilities;
//        private ServerInfo? _serverInfo;
//        private string? _serverInstructions;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="MCPClient"/> class.
//        /// </summary>
//        /// <param name="clientTransport">The transport to use for communication with the server.</param>
//        /// <param name="options">Options for the client, defining protocol version and capabilities.</param>
//        /// <param name="loggerFactory">The logger factory.</param>
//        public MCPClient(IClientTransport clientTransport, MCPClientOptions? options, ILoggerFactory? loggerFactory)
//            : base(loggerFactory)
//        {

//            options ??= new();

//            _clientTransport = clientTransport;
//            _options = options;

//            EndpointName = clientTransport.Name;

//            if (options.Capabilities is { } capabilities)
//            {
//                if (capabilities.NotificationHandlers is { } notificationHandlers)
//                {
//                    NotificationHandlers.RegisterRange(notificationHandlers);
//                }

//                if (capabilities.Sampling is { } samplingCapability)
//                {
//                    if (samplingCapability.SamplingHandler is not { } samplingHandler)
//                    {
//                        throw new InvalidOperationException("Sampling capability was set but it did not provide a handler.");
//                    }

//                    RequestHandlers.Set(
//                        RequestMethods.SamplingCreateMessage,
//                        (request, _, cancellationToken) => samplingHandler(
//                            request,
//                            request?.Meta?.ProgressToken is { } token ? new TokenProgress(this, token) : NullProgress.Instance,
//                            cancellationToken),
//                        MCPJSONUtilities.JsonContext.Default.CreateMessageRequestParams,
//                        MCPJSONUtilities.JsonContext.Default.CreateMessageResult);
//                }

//                if (capabilities.Roots is { } rootsCapability)
//                {
//                    if (rootsCapability.RootsHandler is not { } rootsHandler)
//                    {
//                        throw new InvalidOperationException("Roots capability was set but it did not provide a handler.");
//                    }

//                    RequestHandlers.Set(
//                        RequestMethods.RootsList,
//                        (request, _, cancellationToken) => rootsHandler(request, cancellationToken),
//                        MCPJSONUtilities.JsonContext.Default.ListRootsRequestParams,
//                        MCPJSONUtilities.JsonContext.Default.ListRootsResult);
//                }

//                if (capabilities.Elicitation is { } elicitationCapability)
//                {
//                    if (elicitationCapability.ElicitationHandler is not { } elicitationHandler)
//                    {
//                        throw new InvalidOperationException("Elicitation capability was set but it did not provide a handler.");
//                    }

//                    RequestHandlers.Set(
//                        RequestMethods.ElicitationCreate,
//                        (request, _, cancellationToken) => elicitationHandler(request, cancellationToken),
//                        MCPJSONUtilities.JsonContext.Default.ElicitRequestParams,
//                        MCPJSONUtilities.JsonContext.Default.ElicitResult);
//                }
//            }
//        }

//        /// <inheritdoc/>
//        public ServerCapabilities ServerCapabilities => _serverCapabilities ?? throw new InvalidOperationException("The client is not connected.");

//        /// <inheritdoc/>
//        public ServerInfo ServerInfo => _serverInfo ?? throw new InvalidOperationException("The client is not connected.");

//        /// <inheritdoc/>
//        public string? ServerInstructions => _serverInstructions;

//        /// <inheritdoc/>
//        public override string EndpointName { get; }

//        /// <summary>
//        /// Asynchronously connects to an MCP server, establishes the transport connection, and completes the initialization handshake.
//        /// </summary>
//        public async Task ConnectAsync(CancellationToken cancellationToken = default)
//        {
//            _connectCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
//            cancellationToken = _connectCts.Token;

//            try
//            {
//                // Connect transport
//                _sessionTransport = await _clientTransport.ConnectAsync(cancellationToken).ConfigureAwait(false);
//                InitializeSession(_sessionTransport);
//                // We don't want the ConnectAsync token to cancel the session after we've successfully connected.
//                // The base class handles cleaning up the session in DisposeAsync without our help.
//                StartSession(_sessionTransport, fullSessionCancellationToken: CancellationToken.None);

//                // Perform initialization sequence
//                using var initializationCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
//                initializationCts.CancelAfter(_options.InitializationTimeout);

//                try
//                {

//                    // Send initialize request
//                    string requestProtocol = _options.ProtocolVersion ?? MCPSession.LatestProtocolVersion;

//                    var initializeResponse = await this.SendRequestAsync(
//                                                       RequestMethods.Initialize,
//                                                       new InitializeRequestParams {
//                                                           ProtocolVersion = requestProtocol,
//                                                           Capabilities    = _options.Capabilities ?? new ClientCapabilities(),
//                                                           ClientInfo      = _options.ClientInfo   ?? DefaultImplementation,
//                                                       },
//                                                       MCPJSONUtilities.JsonContext.Default.InitializeRequestParams,
//                                                       MCPJSONUtilities.JsonContext.Default.InitializeResult,
//                                                       cancellationToken: initializationCts.Token
//                                                   ).ConfigureAwait(false);

//                    // Store server information
//                    if (_logger.IsEnabled(LogLevel.Information))
//                    {
//                        LogServerCapabilitiesReceived(
//                            EndpointName,
//                            capabilities: JsonSerializer.Serialize(initializeResponse.Capabilities, MCPJSONUtilities.JsonContext.Default.ServerCapabilities),
//                            serverInfo:   JsonSerializer.Serialize(initializeResponse.ServerInfo,   MCPJSONUtilities.JsonContext.Default.Implementation)
//                        );
//                    }

//                    _serverCapabilities = initializeResponse.Capabilities;
//                    _serverInfo         = initializeResponse.ServerInfo;
//                    _serverInstructions = initializeResponse.Instructions;

//                    // Validate protocol version
//                    bool isResponseProtocolValid =
//                        _options.ProtocolVersion is { } optionsProtocol ? optionsProtocol == initializeResponse.ProtocolVersion :
//                        MCPSession.SupportedProtocolVersions.Contains(initializeResponse.ProtocolVersion);
//                    if (!isResponseProtocolValid)
//                    {
//                        LogServerProtocolVersionMismatch(EndpointName, requestProtocol, initializeResponse.ProtocolVersion);
//                        throw new MCPException($"Server protocol version mismatch. Expected {requestProtocol}, got {initializeResponse.ProtocolVersion}");
//                    }

//                    // Send initialized notification
//                    await SendMessageAsync(
//                              new JSONRPCNotification {
//                                  Method = NotificationMethods.InitializedNotification
//                              },
//                              initializationCts.Token
//                          ).ConfigureAwait(false);
//                }
//                catch (OperationCanceledException oce) when (initializationCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
//                {
//                    LogClientInitializationTimeout(EndpointName);
//                    throw new TimeoutException("Initialization timed out", oce);
//                }
//            }
//            catch (Exception e)
//            {
//                LogClientInitializationError(EndpointName, e);
//                await DisposeAsync().ConfigureAwait(false);
//                throw;
//            }
//        }

//        /// <inheritdoc/>
//        public override async ValueTask DisposeUnsynchronizedAsync()
//        {
//            try
//            {
//                if (_connectCts is not null)
//                {
//                    await _connectCts.CancelAsync().ConfigureAwait(false);
//                    _connectCts.Dispose();
//                }

//                await base.DisposeUnsynchronizedAsync().ConfigureAwait(false);
//            }
//            finally
//            {
//                if (_sessionTransport is not null)
//                {
//                    await _sessionTransport.DisposeAsync().ConfigureAwait(false);
//                }
//            }
//        }

//        [LoggerMessage(Level = LogLevel.Information, Message = "{EndpointName} client received server '{ServerInfo}' capabilities: '{Capabilities}'.")]
//        private partial void LogServerCapabilitiesReceived(string endpointName, string capabilities, string serverInfo);

//        [LoggerMessage(Level = LogLevel.Error, Message = "{EndpointName} client initialization error.")]
//        private partial void LogClientInitializationError(string endpointName, Exception exception);

//        [LoggerMessage(Level = LogLevel.Error, Message = "{EndpointName} client initialization timed out.")]
//        private partial void LogClientInitializationTimeout(string endpointName);

//        [LoggerMessage(Level = LogLevel.Error, Message = "{EndpointName} client protocol version mismatch with server. Expected '{Expected}', received '{Received}'.")]
//        private partial void LogServerProtocolVersionMismatch(string endpointName, string expected, string received);

//    }

//}