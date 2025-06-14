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
    /// Used to attribute a type containing methods that should be exposed as <see cref="AMCPServerTool"/>s.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This attribute is used to mark a class containing methods that should be automatically
    /// discovered and registered as <see cref="AMCPServerTool"/>s. When combined with discovery methods like
    /// <see cref="McpServerBuilderExtensions.WithToolsFromAssembly"/>, it enables automatic registration 
    /// of tools without explicitly listing each tool class. The attribute is not necessary when a reference
    /// to the type is provided directly to a method like <see cref="McpServerBuilderExtensions.WithTools{T}"/>.
    /// </para>
    /// <para>
    /// Within a class marked with this attribute, individual methods that should be exposed as
    /// tools must be marked with the <see cref="MCPServerToolAttribute"/>.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MCPServerToolTypeAttribute : Attribute;

}
