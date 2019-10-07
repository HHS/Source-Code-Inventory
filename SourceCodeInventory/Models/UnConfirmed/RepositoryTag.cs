using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SourceCodeInventory.Models.UnConfirmed
{
    public partial class RepositoryTag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("zipball_url")]
        public Uri ZipballUrl { get; set; }

        [JsonProperty("tarball_url")]
        public Uri TarballUrl { get; set; }

        [JsonProperty("commit")]
        public Commit Commit { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }
    }

    public partial class Commit
    {
        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}
