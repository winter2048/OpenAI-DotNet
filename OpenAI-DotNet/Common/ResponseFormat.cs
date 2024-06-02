// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.Serialization;

namespace OpenAI
{
    public enum ChatResponseFormat
    {
        [EnumMember(Value = "auto")]
        Auto = 0,
        [EnumMember(Value = "text")]
        Text,
        [EnumMember(Value = "json_object")]
        Json
    }
}
