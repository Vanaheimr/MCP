﻿

using System.Text.Json;
using System.Text.Json.Serialization;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents a known resource template that the server is capable of reading.
    /// </summary>
    /// <remarks>
    /// Resource templates provide metadata about resources available on the server,
    /// including how to construct URIs for those resources.
    /// </remarks>
    public class ResourceTemplate
    {

        /// <summary>
        /// Gets or sets the URI template (according to RFC 6570) that can be used to construct resource URIs.
        /// </summary>
        [JsonPropertyName("uriTemplate")]
        public required String UriTemplate { get; init; }

        /// <summary>
        /// Gets or sets a human-readable name for this resource template.
        /// </summary>
        [JsonPropertyName("name")]
        public required String Name { get; init; }

        /// <summary>
        /// Gets or sets a description of what this resource template represents.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This description helps clients understand the purpose and content of resources
        /// that can be generated from this template. It can be used by client applications
        /// to provide context about available resource types or to display in user interfaces.
        /// </para>
        /// <para>
        /// For AI models, this description can serve as a hint about when and how to use
        /// the resource template, enhancing the model's ability to generate appropriate URIs.
        /// </para>
        /// </remarks>
        [JsonPropertyName("description")]
        public String? Description { get; init; }

        /// <summary>
        /// Gets or sets the MIME type of this resource template, if known.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Specifies the expected format of resources that can be generated from this template.
        /// This helps clients understand what type of content to expect when accessing resources
        /// created using this template.
        /// </para>
        /// <para>
        /// Common MIME types include "text/plain" for plain text, "application/pdf" for PDF documents,
        /// "image/png" for PNG images, or "application/json" for JSON data.
        /// </para>
        /// </remarks>
        [JsonPropertyName("mimeType")]
        public String? MimeType { get; init; }

        /// <summary>
        /// Gets or sets optional annotations for the resource template.
        /// </summary>
        /// <remarks>
        /// These annotations can be used to specify the intended audience (<see cref="Role.User"/>, <see cref="Role.Assistant"/>, or both)
        /// and the priority level of the resource template. Clients can use this information to filter
        /// or prioritize resource templates for different roles.
        /// </remarks>
        [JsonPropertyName("annotations")]
        public Annotations? Annotations { get; init; }

        /// <summary>Gets whether <see cref="UriTemplate"/> contains any template expressions.</summary>
        [JsonIgnore]
        public bool IsTemplated => UriTemplate.Contains('{');

        /// <summary>Converts the <see cref="ResourceTemplate"/> into a <see cref="Resource"/>.</summary>
        /// <returns>A <see cref="Resource"/> if <see cref="IsTemplated"/> is <see langword="false"/>; otherwise, <see langword="null"/>.</returns>
        public Resource? AsResource()
        {

            if (IsTemplated)
                return null;

            return new() {
                Uri          = UriTemplate,
                Name         = Name,
                Description  = Description,
                MimeType     = MimeType,
                Annotations  = Annotations,
            };

        }

    }

}
