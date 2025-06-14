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

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Provides constants with the names of common request methods used in the MCP protocol.
    /// </summary>
    public static class RequestMethods
    {

        /// <summary>
        /// The name of the request method sent from the client to request a list of the server's tools.
        /// </summary>
        public const String ToolsList = "tools/list";

        /// <summary>
        /// The name of the request method sent from the client to request that the server invoke a specific tool.
        /// </summary>
        public const String ToolsCall = "tools/call";

        /// <summary>
        /// The name of the request method sent from the client to request a list of the server's prompts.
        /// </summary>
        public const String PromptsList = "prompts/list";

        /// <summary>
        /// The name of the request method sent by the client to get a prompt provided by the server.
        /// </summary>
        public const String PromptsGet = "prompts/get";

        /// <summary>
        /// The name of the request method sent from the client to request a list of the server's resources.
        /// </summary>
        public const String ResourcesList = "resources/list";

        /// <summary>
        /// The name of the request method sent from the client to read a specific server resource.
        /// </summary>
        public const String ResourcesRead = "resources/read";

        /// <summary>
        /// The name of the request method sent from the client to request a list of the server's resource templates.
        /// </summary>
        public const String ResourcesTemplatesList = "resources/templates/list";

        /// <summary>
        /// The name of the request method sent from the client to request <see cref="NotificationMethods.ResourceUpdatedNotification"/> 
        /// notifications from the server whenever a particular resource changes.
        /// </summary>
        public const String ResourcesSubscribe = "resources/subscribe";

        /// <summary>
        /// The name of the request method sent from the client to request unsubscribing from <see cref="NotificationMethods.ResourceUpdatedNotification"/> 
        /// notifications from the server.
        /// </summary>
        public const String ResourcesUnsubscribe = "resources/unsubscribe";

        /// <summary>
        /// The name of the request method sent from the server to request a list of the client's roots.
        /// </summary>
        public const String RootsList = "roots/list";

        /// <summary>
        /// The name of the request method sent by either endpoint to check that the connected endpoint is still alive.
        /// </summary>
        public const String Ping = "ping";

        /// <summary>
        /// The name of the request method sent from the client to the server to adjust the logging level.
        /// </summary>
        /// <remarks>
        /// This request allows clients to control which log messages they receive from the server
        /// by setting a minimum severity threshold. After processing this request, the server will
        /// send log messages with severity at or above the specified level to the client as
        /// <see cref="NotificationMethods.LoggingMessageNotification"/> notifications.
        /// </remarks>
        public const String LoggingSetLevel = "logging/setLevel";

        /// <summary>
        /// The name of the request method sent from the client to the server to ask for completion suggestions.
        /// </summary>
        /// <remarks>
        /// This is used to provide autocompletion-like functionality for arguments in a resource reference or a prompt template.
        /// The client provides a reference (resource or prompt), argument name, and partial value, and the server 
        /// responds with matching completion options.
        /// </remarks>
        public const String CompletionComplete = "completion/complete";

        /// <summary>
        /// The name of the request method sent from the server to sample an large language model (LLM) via the client.
        /// </summary>
        /// <remarks>
        /// This request allows servers to utilize an LLM available on the client side to generate text or image responses
        /// based on provided messages. It is part of the sampling capability in the Model Context Protocol and enables servers to access
        /// client-side AI models without needing direct API access to those models.
        /// </remarks>
        public const String SamplingCreateMessage = "sampling/createMessage";

        /// <summary>
        /// The name of the request method sent from the client to the server to elicit additional information from the user via the client.
        /// </summary>
        /// <remarks>
        /// This request is used when the server needs more information from the client to proceed with a task or interaction.
        /// Servers can request structured data from users, with optional JSON schemas to validate responses.
        /// </remarks>
        public const String ElicitationCreate = "elicitation/create";

        /// <summary>
        /// The name of the request method sent from the client to the server when it first connects, asking it initialize.
        /// </summary>
        /// <remarks>
        /// The initialize request is the first request sent by the client to the server. It provides client information
        /// and capabilities to the server during connection establishment. The server responds with its own capabilities
        /// and information, establishing the protocol version and available features for the session.
        /// </remarks>
        public const String Initialize = "initialize";

    }

}
