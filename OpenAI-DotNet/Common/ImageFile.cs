// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Text.Json.Serialization;

namespace OpenAI
{
    /// <summary>
    /// References an image file in the content of a message.
    /// </summary>
    public sealed class ImageFile
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileId">
        /// The file ID of the image in the message content.
        /// Set purpose='vision' when uploading the file if you need to later display the file content.
        /// </param>
        /// <param name="detail">
        /// Specifies the detail level of the image if specified by the user.
        /// 'low' uses fewer tokens, you can opt in to high resolution using 'high'.
        /// </param>
        public ImageFile(string fileId, ImageDetail detail = ImageDetail.Auto)
        {
            FileId = fileId;
            Detail = detail;
        }

        /// <summary>
        /// The file ID of the image in the message content.
        /// Set purpose='vision' when uploading the file if you need to later display the file content.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("file_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string FileId { get; private set; }

        /// <summary>
        /// Specifies the detail level of the image if specified by the user.
        /// 'low' uses fewer tokens, you can opt in to high resolution using 'high'.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("detail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ImageDetail Detail { get; private set; }
    }
}
