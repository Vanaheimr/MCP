﻿
using Microsoft.Extensions.AI;
using System.Reflection;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP.Server
{

    /// <summary>
    /// Used to attribute a type containing members that should be exposed as <see cref="McpServerResource"/>s.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This attribute is used to mark a class containing members that should be automatically
    /// discovered and registered as <see cref="McpServerResource"/>s. When combined with discovery methods like
    /// <see cref="McpServerBuilderExtensions.WithResourcesFromAssembly"/>, it enables automatic registration 
    /// of resources without explicitly listing each resource class. The attribute is not necessary when a reference
    /// to the type is provided directly to a method like <see cref="McpServerBuilderExtensions.WithResources{T}"/>.
    /// </para>
    /// <para>
    /// Within a class marked with this attribute, individual members that should be exposed as
    /// resources must be marked with the <see cref="McpServerResourceAttribute"/>.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class McpServerResourceTypeAttribute : Attribute;

}
