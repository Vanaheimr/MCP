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
using System.ComponentModel;

using Microsoft.Extensions.AI;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP.Server
{

    /// <summary>
    /// Provides options for controlling the creation of an <see cref="AMCPServerTool"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These options allow for customizing the behavior and metadata of tools created with
    /// <see cref="M:McpServerTool.Create"/>. They provide control over naming, description,
    /// tool properties, and dependency injection integration.
    /// </para>
    /// <para>
    /// When creating tools programmatically rather than using attributes, these options
    /// provide the same level of configuration flexibility.
    /// </para>
    /// </remarks>
    public sealed class MCPServerToolCreateOptions
    {

        /// <summary>
        /// Gets or sets optional services used in the construction of the <see cref="AMCPServerTool"/>.
        /// </summary>
        /// <remarks>
        /// These services will be used to determine which parameters should be satisifed from dependency injection. As such,
        /// what services are satisfied via this provider should match what's satisfied via the provider passed in at invocation time.
        /// </remarks>
        public IServiceProvider?           Services               { get; set; }

        /// <summary>
        /// Gets or sets the name to use for the <see cref="AMCPServerTool"/>.
        /// </summary>
        /// <remarks>
        /// If <see langword="null"/>, but an <see cref="MCPServerToolAttribute"/> is applied to the method,
        /// the name from the attribute will be used. If that's not present, a name based on the method's name will be used.
        /// </remarks>
        public String?                     Name                   { get; set; }

        /// <summary>
        /// Gets or set the description to use for the <see cref="AMCPServerTool"/>.
        /// </summary>
        /// <remarks>
        /// If <see langword="null"/>, but a <see cref="DescriptionAttribute"/> is applied to the method,
        /// the description from that attribute will be used.
        /// </remarks>
        public String?                     Description            { get; set; }

        /// <summary>
        /// Gets or sets a human-readable title for the tool that can be displayed to users.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The title provides a more descriptive, user-friendly name for the tool than the tool's
        /// programmatic name. It is intended for display purposes and to help users understand
        /// the tool's purpose at a glance.
        /// </para>
        /// <para>
        /// Unlike the tool name (which follows programmatic naming conventions), the title can
        /// include spaces, special characters, and be phrased in a more natural language style.
        /// </para>
        /// </remarks>
        public String?                     Title                  { get; set; }

        /// <summary>
        /// Gets or sets whether the tool may perform destructive updates to its environment.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see langword="true"/>, the tool may perform destructive updates to its environment.
        /// If <see langword="false"/>, the tool performs only additive updates.
        /// This property is most relevant when the tool modifies its environment (ReadOnly = false).
        /// </para>
        /// <para>
        /// The default is <see langword="true"/>.
        /// </para>
        /// </remarks>
        public Boolean?                    Destructive            { get; set; }

        /// <summary>
        /// Gets or sets whether calling the tool repeatedly with the same arguments 
        /// will have no additional effect on its environment.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is most relevant when the tool modifies its environment (ReadOnly = false).
        /// </para>
        /// <para>
        /// The default is <see langword="false"/>.
        /// </para>
        /// </remarks>
        public Boolean?                    Idempotent             { get; set; }

        /// <summary>
        /// Gets or sets whether this tool may interact with an "open world" of external entities.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see langword="true"/>, the tool may interact with an unpredictable or dynamic set of entities (like web search).
        /// If <see langword="false"/>, the tool's domain of interaction is closed and well-defined (like memory access).
        /// </para>
        /// <para>
        /// The default is <see langword="true"/>.
        /// </para>
        /// </remarks>
        public Boolean?                    OpenWorld              { get; set; }

        /// <summary>
        /// Gets or sets whether this tool does not modify its environment.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see langword="true"/>, the tool only performs read operations without changing state.
        /// If <see langword="false"/>, the tool may make modifications to its environment.
        /// </para>
        /// <para>
        /// Read-only tools do not have side effects beyond computational resource usage.
        /// They don't create, update, or delete data in any system.
        /// </para>
        /// <para>
        /// The default is <see langword="false"/>.
        /// </para>
        /// </remarks>
        public Boolean?                    ReadOnly               { get; set; }

        /// <summary>
        /// Gets or sets the JSON serializer options to use when marshalling data to/from JSON.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="MCPJSONUtilities.DefaultOptions"/> if left unspecified.
        /// </remarks>
        public JsonSerializerOptions?      SerializerOptions      { get; set; }

        /// <summary>
        /// Gets or sets the JSON schema options when creating <see cref="AIFunction"/> from a method.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="AIJsonSchemaCreateOptions.Default"/> if left unspecified.
        /// </remarks>
        public AIJsonSchemaCreateOptions?  SchemaCreateOptions    { get; set; }





        /// <summary>
        /// Creates a shallow clone of the current <see cref="MCPServerToolCreateOptions"/> instance.
        /// </summary>
        internal MCPServerToolCreateOptions Clone()

             => new() {
                    Services             = Services,
                    Name                 = Name,
                    Description          = Description,
                    Title                = Title,
                    Destructive          = Destructive,
                    Idempotent           = Idempotent,
                    OpenWorld            = OpenWorld,
                    ReadOnly             = ReadOnly,
                    SerializerOptions    = SerializerOptions,
                    SchemaCreateOptions  = SchemaCreateOptions,
                };

    }

}
