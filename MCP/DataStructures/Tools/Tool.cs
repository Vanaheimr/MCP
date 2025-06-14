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

using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents a tool that the server is capable of calling.
    /// </summary>
    public class Tool
    {

        #region Data

        private JsonElement _inputSchema = MCPJSONUtilities.DefaultMCPToolSchema;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the tool.
        /// </summary>
        [JsonPropertyName("name")]
        public String Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a human-readable description of the tool.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This description helps the AI model understand what the tool does and when to use it.
        /// It should be clear, concise, and accurately describe the tool's purpose and functionality.
        /// </para>
        /// <para>
        /// The description is typically presented to AI models to help them determine when
        /// and how to use the tool based on user requests.
        /// </para>
        /// </remarks>
        [JsonPropertyName("description")]
        public String? Description { get; set; }

        /// <summary>
        /// Gets or sets a JSON Schema object defining the expected parameters for the tool.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The schema must be a valid JSON Schema object with the "type" property set to "object".
        /// This is enforced by validation in the setter which will throw an <see cref="ArgumentException"/>
        /// if an invalid schema is provided.
        /// </para>
        /// <para>
        /// The schema typically defines the properties (parameters) that the tool accepts, 
        /// their types, and which ones are required. This helps AI models understand
        /// how to structure their calls to the tool.
        /// </para>
        /// <para>
        /// If not explicitly set, a default minimal schema of <c>{"type":"object"}</c> is used.
        /// </para>
        /// </remarks>
        [JsonPropertyName("inputSchema")]
        public JsonElement InputSchema
        {

            get => _inputSchema;

            set
            {

                if (!MCPJSONUtilities.IsValidMCPToolSchema(value))
                    throw new ArgumentException("The specified document is not a valid MCP tool JSON schema.", nameof(InputSchema));

                _inputSchema = value;

            }

        }

        /// <summary>
        /// Gets or sets optional additional tool information and behavior hints.
        /// </summary>
        /// <remarks>
        /// These annotations provide metadata about the tool's behavior, such as whether it's read-only,
        /// destructive, idempotent, or operates in an open world. They also can include a human-readable title.
        /// Note that these are hints and should not be relied upon for security decisions.
        /// </remarks>
        [JsonPropertyName("annotations")]
        public ToolAnnotations? Annotations { get; set; }

        #endregion




    }

}
