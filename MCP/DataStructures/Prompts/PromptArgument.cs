﻿

using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents an argument that a prompt can accept for templating and customization.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="PromptArgument"/> class defines metadata for arguments that can be provided
    /// to a prompt. These arguments are used to customize or parameterize prompts when they are 
    /// retrieved using <see cref="RequestMethods.PromptsGet"/> requests.
    /// </para>
    /// <para>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </para>
    /// </remarks>
    public class PromptArgument
    {

        /// <summary>
        /// Gets or sets the name of the argument used for referencing in prompt templates.
        /// </summary>
        [JsonPropertyName("name")]
        public String Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a human-readable description of the argument's purpose and expected values.
        /// </summary>
        /// <remarks>
        /// This description helps developers understand what information should be provided
        /// for this argument and how it will affect the generated prompt.
        /// </remarks>
        [JsonPropertyName("description")]
        public String? Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an indication as to whether this argument must be provided when requesting the prompt.
        /// </summary>
        /// <remarks>
        /// When set to <see langword="true"/>, the client must include this argument when making a <see cref="RequestMethods.PromptsGet"/> request.
        /// If a required argument is missing, the server should respond with an error.
        /// </remarks>
        [JsonPropertyName("required")]
        public Boolean? Required { get; set; }

    }

}
