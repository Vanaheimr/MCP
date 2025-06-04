
using System.Text.Json;
using System.Text.Json.Serialization;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents the parameters used with a <see cref="RequestMethods.PromptsGet"/> request from a client to get a prompt provided by a server.
    /// </summary>
    /// <remarks>
    /// The server will respond with a <see cref="GetPromptResult"/> containing the resulting prompt.
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </remarks>
    public class GetPromptRequestParams : RequestParams
    {

        /// <summary>
        /// Gets or sets the name of the prompt.
        /// </summary>
        [JsonPropertyName("name")]
        public required String Name { get; init; }

        /// <summary>
        /// Gets or sets arguments to use for templating the prompt when retrieving it from the server.
        /// </summary>
        /// <remarks>
        /// Typically, these arguments are used to replace placeholders in prompt templates. The keys in this dictionary
        /// should match the names defined in the prompt's <see cref="Prompt.Arguments"/> list. However, the server may
        /// choose to use these arguments in any way it deems appropriate to generate the prompt.
        /// </remarks>
        [JsonPropertyName("arguments")]
        public IReadOnlyDictionary<String, JsonElement>? Arguments { get; init; }

    }

}
