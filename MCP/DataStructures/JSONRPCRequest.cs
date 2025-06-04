using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// A request message in the JSON-RPC protocol.
    /// </summary>
    /// <remarks>
    /// Requests are messages that require a response from the receiver. Each request includes a unique ID
    /// that will be included in the corresponding response message (either a success response or an error).
    /// 
    /// The receiver of a request message is expected to execute the specified method with the provided parameters
    /// and return either a <see cref="JsonRpcResponse"/> with the result, or a <see cref="JSONRPCError"/>
    /// if the method execution fails.
    /// </remarks>
    public class JSONRPCRequest : AJSONRPCMessageWithId
    {

        /// <summary>
        /// Name of the method to invoke.
        /// </summary>
        [JsonPropertyName("method")]
        public required String     Method    { get; init; }

        /// <summary>
        /// Optional parameters for the method.
        /// </summary>
        [JsonPropertyName("params")]
        public          JsonNode?  Params    { get; init; }

        internal JSONRPCRequest WithId(RequestId id)

            => new() {
                   JsonRpc           = JsonRpc,
                   Id                = id,
                   Method            = Method,
                   Params            = Params,
                   RelatedTransport  = RelatedTransport,
               };

    }

}
