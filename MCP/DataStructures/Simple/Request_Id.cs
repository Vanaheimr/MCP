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
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Illias;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents a JSON-RPC request identifier, which can be either a string or an integer.
    /// </summary>
    [JsonConverter(typeof(Converter))]
    public readonly struct Request_Id : IEquatable<Request_Id>
    {

        /// <summary>The id, either a string or a boxed long or null.</summary>
        private readonly object? _id;

        /// <summary>Initializes a new instance of the <see cref="Request_Id"/> with a specified value.</summary>
        /// <param name="value">The required ID value.</param>
        private Request_Id(String value)
        {
            _id = value;
        }

        /// <summary>Initializes a new instance of the <see cref="Request_Id"/> with a specified value.</summary>
        /// <param name="value">The required ID value.</param>
        private Request_Id(Int64 value)
        {
            // Box the long. Request IDs are almost always strings in practice, so this should be rare.
            _id = value;
        }

        /// <summary>Gets the underlying object for this id.</summary>
        /// <remarks>This will either be a <see cref="string"/>, a boxed <see cref="long"/>, or <see langword="null"/>.</remarks>
        public object? Id => _id;




        #region (static) NewRandom(Length = 30, IsLocal = false)

        /// <summary>
        /// Create a new random request identification.
        /// </summary>
        /// <param name="Length">The expected length of the request identification.</param>
        /// <param name="IsLocal">The request identification was generated locally and not received via network.</param>
        public static Request_Id NewRandom(Byte      Length    = 30,
                                           Boolean?  IsLocal   = false)

            => new ((IsLocal == true ? "Local:" : "") +
                    RandomExtensions.RandomString(Length));

        #endregion



        public static Request_Id Parse(Int64 Value)
            => new (Value);

        public static Boolean TryParse(Int64 Value, out Request_Id RequestId)
        {
            RequestId = new Request_Id(Value);
            return true;
        }


        public static Request_Id Parse(String Value)
            => new (Value);

        public static Boolean TryParse(String Value, out Request_Id RequestId)
        {
            RequestId = new Request_Id(Value);
            return true;
        }



        public JToken? AsJSONToken()

            => _id is null
                   ? null
                   : _id switch {
                         String str       => (JToken) str,
                         Int64  longValue => (JToken) longValue,
                         _ => null
                     };




        /// <inheritdoc />
        public override String ToString() =>
            _id is String  stringValue ? stringValue :
            _id is Int64   longValue   ? longValue.ToString(CultureInfo.InvariantCulture) :
            String.Empty;

        /// <inheritdoc />
        public bool Equals(Request_Id other) => Equals(_id, other._id);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Request_Id other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => _id?.GetHashCode() ?? 0;

        /// <inheritdoc />
        public static bool operator ==(Request_Id left, Request_Id right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(Request_Id left, Request_Id right) => !left.Equals(right);

        /// <summary>
        /// Provides a <see cref="JsonConverter"/> for <see cref="Request_Id"/> that handles both string and number values.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public sealed class Converter : JsonConverter<Request_Id>
        {

            /// <inheritdoc />
            public override Request_Id Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return reader.TokenType switch {
                           JsonTokenType.String => Request_Id.Parse(reader.GetString()!),
                           JsonTokenType.Number => Request_Id.Parse(reader.GetInt64()),
                           _ => throw new JsonException("requestId must be a string or an integer"),
                       };
            }

            public override void Write(Utf8JsonWriter         writer,
                                       Request_Id              value,
                                       JsonSerializerOptions  options)
            {
                switch (value._id)
                {

                    case string str:
                        writer.WriteStringValue(str);
                        return;

                    case long longValue:
                        writer.WriteNumberValue(longValue);
                        return;

                    case null:
                        writer.WriteStringValue(String.Empty);
                        return;

                }
            }

        }

    }

}
