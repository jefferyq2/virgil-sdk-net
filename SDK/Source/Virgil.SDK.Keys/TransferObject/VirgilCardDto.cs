using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Virgil.SDK.Keys.TransferObject
{
    public class VirgilCardDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("data")]
        public Dictionary<string,string> CustomData { get; set; }

        [JsonProperty("is_confirmed")]
        public bool IsConfirmed { get; set; }

        [JsonProperty("hash")]
        public byte[] Hash { get; set; }

        [JsonProperty("public_key")]
        public PublicKeyDto PublicKey { get; set; }  

        [JsonProperty("identity")]
        public VirgilIdentityDto Identity { get; set; }
    }
}