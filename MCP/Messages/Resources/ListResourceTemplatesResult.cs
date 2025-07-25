﻿

using System.Text.Json;
using System.Text.Json.Serialization;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents a server's response to a <see cref="RequestMethods.ResourcesTemplatesList"/> request from the client,
    /// containing available resource templates.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This result is returned when a client sends a <see cref="RequestMethods.ResourcesTemplatesList"/> request to discover 
    /// available resource templates on the server.
    /// </para>
    /// <para>
    /// It inherits from <see cref="PaginatedResult"/>, allowing for paginated responses when there are many resource templates.
    /// The server can provide the <see cref="PaginatedResult.NextCursor"/> property to indicate there are more
    /// resource templates available beyond what was returned in the current response.
    /// </para>
    /// <para>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </para>
    /// </remarks>
    public class ListResourceTemplatesResult : PaginatedResult
    {
        /// <summary>
        /// Gets or sets a list of resource templates that the server offers.
        /// </summary>
        /// <remarks>
        /// This collection contains all the resource templates returned in the current page of results.
        /// Each <see cref="ResourceTemplate"/> provides metadata about resources available on the server,
        /// including URI templates, names, descriptions, and MIME types.
        /// </remarks>
        [JsonPropertyName("resourceTemplates")]
        public List<ResourceTemplate> ResourceTemplates { get; set; } = [];
    }

}
