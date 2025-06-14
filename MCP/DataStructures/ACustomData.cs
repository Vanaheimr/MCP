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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// An abstract custom data container.
    /// </summary>
    /// <remarks>
    /// Create a new abstract custom data container.
    /// </remarks>
    /// <param name="CustomData">An optional custom data object allowing to store any kind of customer specific data.</param>
    public abstract class ACustomData(JObject? CustomData = null) : IEquatable<ACustomData>
    {

        #region Data

        protected readonly JObject customData = CustomData ?? [];

        #endregion


        #region CreateJSON(JProperties)

        /// <summary>
        /// Create a JSON object using the given JSON properties, but filter null values.
        /// </summary>
        /// <param name="JProperties">JSON properties.</param>
        protected JObject CreateJSON(params JProperty?[] JProperties)
        {

            if (JProperties is null || JProperties.Length == 0)
                return [];

            var data = JProperties.
                           Where(jProperty => jProperty is not null && jProperty.Value.IsNotJSONNullOrEmpty()).
                           Cast<JProperty>().
                           ToArray();

            var json = data.Length > 0
                           ? new JObject(data)
                           : [];

            foreach (var property in customData)
            {
                if (!json.ContainsKey(property.Key))
                    json[property.Key] = property.Value;
            }

            return json;

        }

        #endregion

        #region ParseCustomData(JSON, IgnoreProperties)

        /// <summary>
        /// Parse the custom data from the given JSON object, ignoring specified properties.
        /// </summary>
        /// <param name="JSON">The JSON object to be parsed.</param>
        /// <param name="IgnoreProperties">A hash set of property keys to ignore.</param>
        protected static JObject ParseCustomData(JObject          JSON,
                                                 HashSet<String>  IgnoreProperties)
        {

            var customData = new JObject();

            foreach (var property in JSON.Properties())
            {
                if (!IgnoreProperties.Contains(property.Name))
                    customData.Add(property.Name, property.Value);
            }

            return customData;

        }

        #endregion


        #region IEquatable<ACustomData> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two abstract custom data containers for equality.
        /// </summary>
        /// <param name="Object">An abstract custom data container to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ACustomData aCustomData &&
                   Equals(aCustomData);

        #endregion

        #region Equals(ACustomData)

        /// <summary>
        /// Compares two abstract custom data containers for equality.
        /// </summary>
        /// <param name="ACustomData">An abstract custom data container to compare with.</param>
        public Boolean Equals(ACustomData? ACustomData)

            => ACustomData is not null;

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => customData?.GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => customData?.ToString() ?? "";

        #endregion

    }

}