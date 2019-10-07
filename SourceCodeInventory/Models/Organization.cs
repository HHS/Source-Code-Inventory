using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SourceCodeInventory.Models
{
    public class Organization
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("repos_url")]
        public Uri ReposUrl { get; set; }

        [JsonProperty("events_url")]
        public Uri EventsUrl { get; set; }

        [JsonProperty("hooks_url")]
        public Uri HooksUrl { get; set; }

        [JsonProperty("issues_url")]
        public Uri IssuesUrl { get; set; }

        [JsonProperty("members_url")]
        public string MembersUrl { get; set; }

        [JsonProperty("public_members_url")]
        public string PublicMembersUrl { get; set; }

        [JsonProperty("avatar_url")]
        public Uri AvatarUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("company")]
        public object Company { get; set; }

        [JsonProperty("blog")]
        public Uri Blog { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }

        [JsonProperty("has_organization_projects")]
        public bool HasOrganizationProjects { get; set; }

        [JsonProperty("has_repository_projects")]
        public bool HasRepositoryProjects { get; set; }

        [JsonProperty("public_repos")]
        public long PublicRepos { get; set; }

        [JsonProperty("public_gists")]
        public long PublicGists { get; set; }

        [JsonProperty("followers")]
        public long Followers { get; set; }

        [JsonProperty("following")]
        public long Following { get; set; }

        [JsonProperty("html_url")]
        public Uri HtmlUrl { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
