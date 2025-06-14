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

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace org.GraphDefined.Vanaheimr.Hermod.MCP.Server
{

    /// <summary>Provides a thread-safe collection of <typeparamref name="T"/> instances, indexed by their names.</summary>
    /// <typeparam name="T">Specifies the type of primitive stored in the collection.</typeparam>
    public class MCPServerPrimitiveCollection<T> : ICollection<T>,
                                                   IReadOnlyCollection<T>

        where T : IMCPServerPrimitive

    {

        /// <summary>
        /// Concurrent dictionary of primitives, indexed by their names.
        /// </summary>
        private readonly ConcurrentDictionary<String, T> _primitives = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="MCPServerPrimitiveCollection{T}"/> class.
        /// </summary>
        public MCPServerPrimitiveCollection()
        {
        }

        /// <summary>Occurs when the collection is changed.</summary>
        /// <remarks>
        /// By default, this is raised when a primitive is added or removed. However, a derived implementation
        /// may raise this event for other reasons, such as when a primitive is modified.
        /// </remarks>
        public event EventHandler? Changed;

        /// <summary>Gets the number of primitives in the collection.</summary>
        public Int32 Count => _primitives.Count;

        /// <summary>Gets whether there are any primitives in the collection.</summary>
        public Boolean IsEmpty => _primitives.IsEmpty;

        /// <summary>Raises <see cref="Changed"/> if there are registered handlers.</summary>
        protected void RaiseChanged() => Changed?.Invoke(this, EventArgs.Empty);

        /// <summary>Gets the <typeparamref name="T"/> with the specified <paramref name="name"/> from the collection.</summary>
        /// <param name="name">The name of the primitive to retrieve.</param>
        /// <returns>The <typeparamref name="T"/> with the specified name.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException">An primitive with the specified name does not exist in the collection.</exception>
        public T this[String name]
        {
            get
            {
                return _primitives[name];
            }
        }

        /// <summary>Clears all primitives from the collection.</summary>
        public virtual void Clear()
        {
            _primitives.Clear();
            RaiseChanged();
        }

        /// <summary>Adds the specified <typeparamref name="T"/> to the collection.</summary>
        /// <param name="primitive">The primitive to be added.</param>
        /// <exception cref="ArgumentNullException"><paramref name="primitive"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">A primitive with the same name as <paramref name="primitive"/> already exists in the collection.</exception>
        public void Add(T primitive)
        {
            if (!TryAdd(primitive))
                throw new ArgumentException($"A primitive with the same name '{primitive.Id}' already exists in the collection.", nameof(primitive));
        }

        /// <summary>Adds the specified <typeparamref name="T"/> to the collection.</summary>
        /// <param name="primitive">The primitive to be added.</param>
        /// <returns><see langword="true"/> if the primitive was added; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="primitive"/> is <see langword="null"/>.</exception>
        public virtual bool TryAdd(T primitive)
        {

            var added = _primitives.TryAdd(primitive.Id, primitive);

            if (added)
                RaiseChanged();

            return added;

        }

        /// <summary>Removes the specified primitivefrom the collection.</summary>
        /// <param name="primitive">The primitive to be removed from the collection.</param>
        /// <returns>
        /// <see langword="true"/> if the primitive was found in the collection and removed; otherwise, <see langword="false"/> if it couldn't be found.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="primitive"/> is <see langword="null"/>.</exception>
        public virtual Boolean Remove(T primitive)
        {

            var removed = ((ICollection<KeyValuePair<String, T>>) _primitives).Remove(new (primitive.Id,
                                                                                           primitive));

            if (removed)
                RaiseChanged();

            return removed;

        }

        /// <summary>Attempts to get the primitive with the specified name from the collection.</summary>
        /// <param name="name">The name of the primitive to retrieve.</param>
        /// <param name="primitive">The primitive, if found; otherwise, <see langword="null"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the primitive was found in the collection and return; otherwise, <see langword="false"/> if it couldn't be found.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        public virtual bool TryGetPrimitive(String                      name,
                                            [NotNullWhen(true)] out T?  primitive)

            => _primitives.TryGetValue(name,
                                       out primitive);

        /// <summary>Checks if a specific primitive is present in the collection of primitives.</summary>
        /// <param name="primitive">The primitive to search for in the collection.</param>
        /// <see langword="true"/> if the primitive was found in the collection and return; otherwise, <see langword="false"/> if it couldn't be found.
        /// <exception cref="ArgumentNullException"><paramref name="primitive"/> is <see langword="null"/>.</exception>
        public virtual Boolean Contains(T primitive)
        {
            return ((ICollection<KeyValuePair<String, T>>) _primitives).Contains(new (primitive.Id,
                                                                                      primitive));
        }

        /// <summary>Gets the names of all of the primitives in the collection.</summary>
        public virtual ICollection<String> PrimitiveNames
            => _primitives.Keys;

        /// <summary>Creates an array containing all of the primitives in the collection.</summary>
        /// <returns>An array containing all of the primitives in the collection.</returns>
        public virtual T[] ToArray()
            => _primitives.Values.ToArray();

        /// <inheritdoc/>
        public virtual void CopyTo(T[] array, Int32 arrayIndex)
        {
            _primitives.Values.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public virtual IEnumerator<T> GetEnumerator()
        {
            foreach (var entry in _primitives)
            {
                yield return entry.Value;
            }
        }

        /// <inheritdoc/>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <inheritdoc/>
        Boolean ICollection<T>.IsReadOnly
            => false;

    }

}
