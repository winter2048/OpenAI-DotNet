// Licensed under the MIT License. See LICENSE in the project root for license information.

using OpenAI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace OpenAI.Threads
{
    /// <summary>
    /// A message created by an Assistant or a user.
    /// Messages can include text, images, and other files.
    /// Messages stored as a list on the Thread.
    /// </summary>
    public sealed class MessageResponse : BaseResponse
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("id")]
        public string Id { get; private set; }

        /// <summary>
        /// The object type, which is always message.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("object")]
        public string Object { get; private set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the message was created.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("created_at")]
        public int CreatedAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnixTimeSeconds).DateTime;

        /// <summary>
        /// The thread ID that this message belongs to.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("thread_id")]
        public string ThreadId { get; private set; }

        /// <summary>
        /// The status of the message, which can be either 'in_progress', 'incomplete', or 'completed'.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumConverter<MessageStatus>))]
        public MessageStatus Status { get; private set; }

        /// <summary>
        /// On an incomplete message, details about why the message is incomplete.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("incomplete_details")]
        public IncompleteDetails IncompleteDetails { get; private set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the message was completed.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("completed_at")]
        public int? CompletedAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime? CompletedAt
            => CompletedAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(CompletedAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the message was marked as incomplete.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("incomplete_at")]
        public int? IncompleteAtUnixTimeSeconds { get; private set; }

        [JsonIgnore]
        public DateTime? IncompleteAt
            => IncompleteAtUnixTimeSeconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(IncompleteAtUnixTimeSeconds.Value).DateTime
                : null;

        /// <summary>
        /// The entity that produced the message. One of user or assistant.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("role")]
        public Role Role { get; private set; }

        /// <summary>
        /// The content of the message in array of text and/or images.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("content")]
        public dynamic Content { get; private set; }

        /// <summary>
        /// If applicable, the ID of the assistant that authored this message.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("assistant_id")]
        public string AssistantId { get; private set; }

        /// <summary>
        /// If applicable, the ID of the run associated with the authoring of this message.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("run_id")]
        public string RunId { get; private set; }

        /// <summary>
        /// A list of file IDs that the assistant should use.
        /// Useful for tools like 'retrieval' and 'code_interpreter' that can access files.
        /// A maximum of 10 files can be attached to a message.
        /// </summary>
        [JsonIgnore]
        [Obsolete("Use Attachments instead.")]
        public IReadOnlyList<string> FileIds => Attachments?.Select(attachment => attachment.FileId).ToList();

        /// <summary>
        /// A list of files attached to the message, and the tools they were added to.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Attachments")]
        public IReadOnlyList<Attachment> Attachments { get; private set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("metadata")]
        public IReadOnlyDictionary<string, string> Metadata { get; private set; }

        public static implicit operator string(MessageResponse message) => message?.ToString();

        public static implicit operator Message(MessageResponse response)
            => response?.Content switch
            {
                string content => new(content, response.Role, response.Attachments, response.Metadata),
                IReadOnlyList<Content> contents => new(contents, response.Role, response.Attachments, response.Metadata),
                _ => null
            };

        public override string ToString() => Id;

        /// <summary>
        /// Formats all of the <see cref="Content"/> items into a single string,
        /// putting each item on a new line.
        /// </summary>
        /// <returns><see cref="string"/> of all <see cref="Content"/>.</returns>
        public string PrintContent()
            => Content switch
            {
                string content => content,
                IReadOnlyList<Content> contents => string.Join("\n", contents.Select(content => content?.ToString())),
                _ => string.Empty
            };
    }
}
