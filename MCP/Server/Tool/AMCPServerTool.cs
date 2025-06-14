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

using System.Text.Json;
using System.Reflection;

using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP.Server
{

    /// <summary>
    /// Represents an invocable tool used by Model Context Protocol clients and servers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="AMCPServerTool"/> is an abstract base class that represents an MCP tool for use in the server (as opposed
    /// to <see cref="Tool"/>, which provides the protocol representation of a tool, and <see cref="McpClientTool"/>, which
    /// provides a client-side representation of a tool). Instances of <see cref="AMCPServerTool"/> can be added into a
    /// <see cref="IServiceCollection"/> to be picked up automatically when <see cref="McpServerFactory"/> is used to create
    /// an <see cref="IMcpServer"/>, or added into a <see cref="MCPServerPrimitiveCollection{McpServerTool}"/>.
    /// </para>
    /// <para>
    /// Most commonly, <see cref="AMCPServerTool"/> instances are created using the static <see cref="M:McpServerTool.Create"/> methods.
    /// These methods enable creating an <see cref="AMCPServerTool"/> for a method, specified via a <see cref="Delegate"/> or 
    /// <see cref="MethodInfo"/>, and are what are used implicitly by <see cref="McpServerBuilderExtensions.WithToolsFromAssembly"/> and
    /// <see cref="M:McpServerBuilderExtensions.WithTools"/>. The <see cref="M:McpServerTool.Create"/> methods
    /// create <see cref="AMCPServerTool"/> instances capable of working with a large variety of .NET method signatures, automatically handling
    /// how parameters are marshaled into the method from the JSON received from the MCP client, and how the return value is marshaled back
    /// into the <see cref="CallToolResponse"/> that's then serialized and sent back to the client.
    /// </para>
    /// <para>
    /// By default, parameters are sourced from the <see cref="CallToolRequestParams.Arguments"/> dictionary, which is a collection
    /// of key/value pairs, and are represented in the JSON schema for the function, as exposed in the returned <see cref="AMCPServerTool"/>'s
    /// <see cref="ProtocolTool"/>'s <see cref="Tool.InputSchema"/>. Those parameters are deserialized from the
    /// <see cref="JsonElement"/> values in that collection. There are a few exceptions to this:
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///       <see cref="CancellationToken"/> parameters are automatically bound to a <see cref="CancellationToken"/> provided by the
    ///       <see cref="IMcpServer"/> and that respects any <see cref="CancelledNotification"/>s sent by the client for this operation's
    ///       <see cref="Request_Id"/>. The parameter is not included in the generated JSON schema.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="IServiceProvider"/> parameters are bound from the <see cref="RequestContext{CallToolRequestParams}"/> for this request,
    ///       and are not included in the JSON schema.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="IMcpServer"/> parameters are not included in the JSON schema and are bound directly to the <see cref="IMcpServer"/>
    ///       instance associated with this request's <see cref="RequestContext{CallToolRequestParams}"/>. Such parameters may be used to understand
    ///       what server is being used to process the request, and to interact with the client issuing the request to that server.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       <see cref="IProgress{ProgressNotificationValue}"/> parameters accepting <see cref="ProgressNotificationValue"/> values
    ///       are not included in the JSON schema and are bound to an <see cref="IProgress{ProgressNotificationValue}"/> instance manufactured
    ///       to forward progress notifications from the tool to the client. If the client included a <see cref="ProgressToken"/> in their request, 
    ///       progress reports issued to this instance will propagate to the client as <see cref="NotificationMethods.ProgressNotification"/> notifications with
    ///       that token. If the client did not include a <see cref="ProgressToken"/>, the instance will ignore any progress reports issued to it.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       When the <see cref="AMCPServerTool"/> is constructed, it may be passed an <see cref="IServiceProvider"/> via 
    ///       <see cref="MCPServerToolCreateOptions.Services"/>. Any parameter that can be satisfied by that <see cref="IServiceProvider"/>
    ///       according to <see cref="IServiceProviderIsService"/> will not be included in the generated JSON schema and will be resolved 
    ///       from the <see cref="IServiceProvider"/> provided to <see cref="InvokeAsync"/> rather than from the argument collection.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///       Any parameter attributed with <see cref="FromKeyedServicesAttribute"/> will similarly be resolved from the 
    ///       <see cref="IServiceProvider"/> provided to <see cref="InvokeAsync"/> rather than from the argument
    ///       collection, and will not be included in the generated JSON schema.
    ///     </description>
    ///   </item>
    /// </list>
    /// </para>
    /// <para>
    /// All other parameters are deserialized from the <see cref="JsonElement"/>s in the <see cref="CallToolRequestParams.Arguments"/> dictionary, 
    /// using the <see cref="JsonSerializerOptions"/> supplied in <see cref="MCPServerToolCreateOptions.SerializerOptions"/>, or if none was provided, 
    /// using <see cref="MCPJSONUtilities.DefaultOptions"/>.
    /// </para>
    /// <para>
    /// In general, the data supplied via the <see cref="CallToolRequestParams.Arguments"/>'s dictionary is passed along from the caller and
    /// should thus be considered unvalidated and untrusted. To provide validated and trusted data to the invocation of the tool, consider having 
    /// the tool be an instance method, referring to data stored in the instance, or using an instance or parameters resolved from the <see cref="IServiceProvider"/>
    /// to provide data to the method.
    /// </para>
    /// <para>
    /// Return values from a method are used to create the <see cref="CallToolResponse"/> that is sent back to the client:
    /// </para>
    /// <list type="table">
    ///   <item>
    ///     <term><see langword="null"/></term>
    ///     <description>Returns an empty <see cref="CallToolResponse.Content"/> list.</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="AIContent"/></term>
    ///     <description>Converted to a single <see cref="Content"/> object using <see cref="AIContentExtensions.ToContent(AIContent)"/>.</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="string"/></term>
    ///     <description>Converted to a single <see cref="Content"/> object with <see cref="Content.Text"/> set to the string value and <see cref="Content.Type"/> set to "text".</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="Content"/></term>
    ///     <description>Returned as a single-item <see cref="Content"/> list.</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="IEnumerable{String}"/> of <see cref="string"/></term>
    ///     <description>Each <see cref="string"/> is converted to a <see cref="Content"/> object with <see cref="Content.Text"/> set to the string value and <see cref="Content.Type"/> set to "text".</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="IEnumerable{AIContent}"/> of <see cref="AIContent"/></term>
    ///     <description>Each <see cref="AIContent"/> is converted to a <see cref="Content"/> object using <see cref="AIContentExtensions.ToContent(AIContent)"/>.</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="IEnumerable{Content}"/> of <see cref="Content"/></term>
    ///     <description>Returned as the <see cref="Content"/> list.</description>
    ///   </item>
    ///   <item>
    ///     <term><see cref="CallToolResponse"/></term>
    ///     <description>Returned directly without modification.</description>
    ///   </item>
    ///   <item>
    ///     <term>Other types</term>
    ///     <description>Serialized to JSON and returned as a single <see cref="Content"/> object with <see cref="Content.Type"/> set to "text".</description>
    ///   </item>
    /// </list>
    /// </remarks>
    public abstract class AMCPServerTool : IMCPServerPrimitive
    {

        #region Properties

        /// <inheritdoc />
        String IMCPServerPrimitive.Id
            => ProtocolTool.Name;

        /// <summary>
        /// Gets the protocol <see cref="Tool"/> type for this instance.
        /// </summary>
        public abstract Tool  ProtocolTool    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="AMCPServerTool"/> class.
        /// </summary>
        protected AMCPServerTool()
        { }

        #endregion


        #region Create (method, options = null)

        /// <summary>
        /// Creates an <see cref="AMCPServerTool"/> instance for a method, specified via a <see cref="Delegate"/> instance.
        /// </summary>
        /// <param name="method">The method to be represented via the created <see cref="AMCPServerTool"/>.</param>
        /// <param name="options">Optional options used in the creation of the <see cref="AMCPServerTool"/> to control its behavior.</param>
        /// <returns>The created <see cref="AMCPServerTool"/> for invoking <paramref name="method"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="method"/> is <see langword="null"/>.</exception>
        public static AMCPServerTool Create(Delegate                     method,
                                           MCPServerToolCreateOptions?  options   = null)

            => AIFunctionMCPServerTool.Create(
                   method,
                   options
               );

        #endregion

        #region Create (method, target = null, options = null)

        /// <summary>
        /// Creates an <see cref="AMCPServerTool"/> instance for a method, specified via a <see cref="Delegate"/> instance.
        /// </summary>
        /// <param name="method">The method to be represented via the created <see cref="AMCPServerTool"/>.</param>
        /// <param name="target">The instance if <paramref name="method"/> is an instance method; otherwise, <see langword="null"/>.</param>
        /// <param name="options">Optional options used in the creation of the <see cref="AMCPServerTool"/> to control its behavior.</param>
        /// <returns>The created <see cref="AMCPServerTool"/> for invoking <paramref name="method"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="method"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="method"/> is an instance method but <paramref name="target"/> is <see langword="null"/>.</exception>
        public static AMCPServerTool Create(MethodInfo                   method,
                                           Object?                      target    = null,
                                           MCPServerToolCreateOptions?  options   = null)

            => AIFunctionMCPServerTool.Create(
                   method,
                   target,
                   options
               );

        #endregion

        #region Create (method, createTargetFunc = null, options = null)

        /// <summary>
        /// Creates an <see cref="AMCPServerTool"/> instance for a method, specified via an <see cref="MethodInfo"/> for
        /// and instance method, along with a <see cref="Type"/> representing the type of the target object to
        /// instantiate each time the method is invoked.
        /// </summary>
        /// <param name="method">The instance method to be represented via the created <see cref="AIFunction"/>.</param>
        /// <param name="createTargetFunc">
        /// Callback used on each function invocation to create an instance of the type on which the instance method <paramref name="method"/>
        /// will be invoked. If the returned instance is <see cref="IAsyncDisposable"/> or <see cref="IDisposable"/>, it will
        /// be disposed of after method completes its invocation.
        /// </param>
        /// <param name="options">Optional options used in the creation of the <see cref="AMCPServerTool"/> to control its behavior.</param>
        /// <returns>The created <see cref="AIFunction"/> for invoking <paramref name="method"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="method"/> is <see langword="null"/>.</exception>
        public static AMCPServerTool Create(MethodInfo                                           method,
                                           Func<RequestContext<CallToolRequestParams>, Object>  createTargetFunc,
                                           MCPServerToolCreateOptions?                          options   = null)

            => AIFunctionMCPServerTool.Create(
                   method,
                   createTargetFunc,
                   options
               );

        #endregion

        #region Create (function, options = null)

        /// <summary>Creates an <see cref="AMCPServerTool"/> that wraps the specified <see cref="AIFunction"/>.</summary>
        /// <param name="function">The function to wrap.</param>
        /// <param name="options">Optional options used in the creation of the <see cref="AMCPServerTool"/> to control its behavior.</param>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// Unlike the other overloads of Create, the <see cref="AMCPServerTool"/> created by <see cref="Create(AIFunction, MCPServerToolCreateOptions)"/>
        /// does not provide all of the special parameter handling for MCP-specific concepts, like <see cref="IMcpServer"/>.
        /// </remarks>
        public static AMCPServerTool Create(AIFunction                   function,
                                           MCPServerToolCreateOptions?  options   = null)

            => AIFunctionMCPServerTool.Create(
                   function,
                   options
               );

        #endregion


        #region InvokeAsync(request, ...)

        /// <summary>Invokes the <see cref="AMCPServerTool"/>.</summary>
        /// <param name="request">The request information resulting in the invocation of this tool.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation requests. The default is <see cref="CancellationToken.None"/>.</param>
        /// <returns>The call response from invoking the tool.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request"/> is <see langword="null"/>.</exception>
        public abstract ValueTask<CallToolResponse>

            InvokeAsync(RequestContext<CallToolRequestParams>  request,
                        CancellationToken                      cancellationToken   = default);

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Returns a text representation of this object.
        /// </summary>
        public override String ToString()

            => ProtocolTool.Name;

        #endregion

    }

}
