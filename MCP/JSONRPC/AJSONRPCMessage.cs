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

#region Usings

using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents any JSON-RPC message used in the Model Context Protocol (MCP).
    /// </summary>
    /// <remarks>
    /// This interface serves as the foundation for all message types in the JSON-RPC 2.0 protocol
    /// used by MCP, including requests, responses, notifications, and errors. JSON-RPC is a stateless,
    /// lightweight remote procedure call (RPC) protocol that uses JSON as its data format.
    /// </remarks>
    /// <remarks>
    /// Create a new JSON-RPC message with the optional specified JSON-RPC version.
    /// </remarks>
    /// <param name="JSONRPCVersion">The optional JSON-RPC version, default is "2.0".</param>
    [JsonConverter(typeof(Converter))]
    public abstract class AJSONRPCMessage(String?   JSONRPCVersion   = null,
                                          JObject?  CustomData       = null)

        : ACustomData(CustomData)

    {

        #region Data

        public const String DefaultJSONRPCVersion = "2.0";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the JSON-RPC protocol version used.
        /// </summary>
        /// <inheritdoc />
        [JsonPropertyName("jsonrpc")]
        public String       JSONRPCVersion     { get; } = JSONRPCVersion ?? DefaultJSONRPCVersion;

        /// <summary>
        /// Gets or sets the transport the <see cref="AJSONRPCMessage"/> was received on or should be sent over.
        /// </summary>
        /// <remarks>
        /// This is used to support the Streamable HTTP transport where the specification states that the server
        /// SHOULD include JSON-RPC responses in the HTTP response body for the POST request containing
        /// the corresponding JSON-RPC request. It may be <see langword="null"/> for other transports.
        /// </remarks>
        [JsonIgnore]
        public ITransport?  RelatedTransport   { get; set; }

        #endregion


        /// <summary>
        /// Provides a <see cref="JsonConverter"/> for <see cref="AJSONRPCMessage"/> messages,
        /// handling polymorphic deserialization of different message types.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This converter is responsible for correctly deserializing JSON-RPC messages into their appropriate
        /// concrete types based on the message structure. It analyzes the JSON payload and determines if it
        /// represents a request, notification, successful response, or error response.
        /// </para>
        /// <para>
        /// The type determination rules follow the JSON-RPC 2.0 specification:
        /// <list type="bullet">
        /// <item><description>Messages with "method" and "id" properties are deserialized as <see cref="JSONRPCRequest"/>.</description></item>
        /// <item><description>Messages with "method" but no "id" property are deserialized as <see cref="JSONRPCNotification"/>.</description></item>
        /// <item><description>Messages with "id" and "result" properties are deserialized as <see cref="JSONRPCResponse"/>.</description></item>
        /// <item><description>Messages with "id" and "error" properties are deserialized as <see cref="JSONRPCError"/>.</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed class Converter : JsonConverter<AJSONRPCMessage>
        {

            /// <inheritdoc/>
            public override AJSONRPCMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {

                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException("Expected StartObject token");

                using var doc = JsonDocument.ParseValue(ref reader);
                var root = doc.RootElement;

                // All JSON-RPC messages must have a jsonrpc property with value "2.0"
                if (!root.TryGetProperty("jsonrpc", out var versionProperty) ||
                    versionProperty.GetString() != "2.0")
                {
                    throw new JsonException("Invalid or missing jsonrpc version");
                }

                // Determine the message type based on the presence of id, method, and error properties
                var hasId     = root.TryGetProperty("id",     out _);
                var hasMethod = root.TryGetProperty("method", out _);
                var hasError  = root.TryGetProperty("error",  out _);

                var rawText   = root.GetRawText();

                // Messages with an id but no method are responses
                if (hasId && !hasMethod)
                {

                    // Messages with an error property are error responses
                    if (hasError)
                        return JsonSerializer.Deserialize(rawText, options.GetTypeInfo<JSONRPCError>());

                    // Messages with a result property are success responses
                    if (root.TryGetProperty("result", out _))
                        return JsonSerializer.Deserialize(rawText, options.GetTypeInfo<JSONRPCResponse>());

                    throw new JsonException("Response must have either result or error");

                }

                // Messages with a method but no id are notifications
                if (hasMethod && !hasId)
                    return JsonSerializer.Deserialize(rawText, options.GetTypeInfo<JSONRPCNotification>());

                // Messages with both method and id are requests
                if (hasMethod && hasId)
                    return JsonSerializer.Deserialize(rawText, options.GetTypeInfo<JSONRPCRequest>());

                throw new JsonException("Invalid JSON-RPC message format");

            }


            /// <inheritdoc/>
            public override void Write(Utf8JsonWriter writer, AJSONRPCMessage value, JsonSerializerOptions options)
            {
                switch (value)
                {

                    case JSONRPCRequest request:
                        JsonSerializer.Serialize(writer, request,      options.GetTypeInfo<JSONRPCRequest>());
                        break;

                    case JSONRPCNotification notification:
                        JsonSerializer.Serialize(writer, notification, options.GetTypeInfo<JSONRPCNotification>());
                        break;

                    case JSONRPCResponse response:
                        JsonSerializer.Serialize(writer, response,     options.GetTypeInfo<JSONRPCResponse>());
                        break;

                    case JSONRPCError error:
                        JsonSerializer.Serialize(writer, error,        options.GetTypeInfo<JSONRPCError>());
                        break;

                    default:
                        throw new JsonException($"Unknown JSON-RPC message type: {value.GetType()}");

                }
            }

        }

    }

}
