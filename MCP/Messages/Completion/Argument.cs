
using System.Text.Json.Serialization;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents an argument used in completion requests to provide context for auto-completion functionality.
    /// </summary>
    /// <remarks>
    /// This class is used when requesting completion suggestions for a particular field or parameter.
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </remarks>
    public class Argument
    {

        /// <summary>
        /// Gets or sets the name of the argument being completed.
        /// </summary>
        [JsonPropertyName("name")]
        public String Name { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the current partial text value for which completion suggestions are requested.
        /// </summary>
        /// <remarks>
        /// This represents the text that has been entered so far and for which completion
        /// options should be generated.
        /// </remarks>
        [JsonPropertyName("value")]
        public String Value { get; set; } = String.Empty;

    }

}
