﻿

using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents a prompt that the server offers.
    /// </summary>
    /// <remarks>
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </remarks>
    public class Prompt
    {

        /// <summary>
        /// Gets or sets a list of arguments that this prompt accepts for templating and customization.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This list defines the arguments that can be provided when requesting the prompt.
        /// Each argument specifies metadata like name, description, and whether it's required.
        /// </para>
        /// <para>
        /// When a client makes a <see cref="RequestMethods.PromptsGet"/> request, it can provide values for these arguments
        /// which will be substituted into the prompt template or otherwise used to render the prompt.
        /// </para>
        /// </remarks>
        [JsonPropertyName("arguments")]
        public List<PromptArgument>? Arguments { get; set; }

        /// <summary>
        /// Gets or sets an optional description of what this prompt provides.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This description helps developers understand the purpose and use cases for the prompt.
        /// It should explain what the prompt is designed to accomplish and any important context.
        /// </para>
        /// <para>
        /// The description is typically used in documentation, UI displays, and for providing context
        /// to client applications that may need to choose between multiple available prompts.
        /// </para>
        /// </remarks>
        [JsonPropertyName("description")]
        public String? Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the prompt.
        /// </summary>
        [JsonPropertyName("name")]
        public String Name { get; set; } = String.Empty;

    }

}
