﻿

using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace org.GraphDefined.Vanaheimr.Hermod.MCP
{

    /// <summary>
    /// Represents the parameters used with a <see cref="RequestMethods.PromptsList"/> request from a client to request
    /// a list of prompts available from the server.
    /// </summary>
    /// <remarks>
    /// The server responds with a <see cref="ListPromptsResult"/> containing the available prompts.
    /// See the <see href="https://github.com/modelcontextprotocol/specification/blob/main/schema/">schema</see> for details.
    /// </remarks>
    public class ListPromptsRequestParams : PaginatedRequestParams;

}
