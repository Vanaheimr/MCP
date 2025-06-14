using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// A JSON converter for enums that allows customizing the serialized string value of enum members
    /// using the <see cref="JsonStringEnumMemberNameAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to convert.</typeparam>
    /// <remarks>
    /// This is a temporary workaround for lack of System.Text.Json's JsonStringEnumConverter&lt;T&gt;
    /// 9.x support for custom enum member naming. It will be replaced by the built-in functionality
    /// once .NET 9 is fully adopted.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class CustomizableJsonStringEnumConverter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TEnum>

        : JsonStringEnumConverter<TEnum>

        where TEnum : struct, Enum

    {

    }

    /// <summary>
    /// Represents the type of role in the Model Context Protocol conversation.
    /// </summary>
    [JsonConverter(typeof(CustomizableJsonStringEnumConverter<Role>))]
    public enum Role
    {

        /// <summary>
        /// Corresponds to a human user in the conversation.
        /// </summary>
        [JsonStringEnumMemberName("user")]
        User,

        /// <summary>
        /// Corresponds to the AI assistant in the conversation.
        /// </summary>
        [JsonStringEnumMemberName("assistant")]
        Assistant

    }

}
