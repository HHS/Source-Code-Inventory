using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SourceCodeInventory.Models
{

    public class json
    {
        [JsonProperty("version")]
        public string Headerversion { get; set; }
        [JsonProperty("agency")]
        public string agency { get; set; }

        public measurement measurementType { get; set; }

        public class measurement
        {
            public string method { set; get; }
        }
        public List<MetaData> releases { get; set; }

    }
    public class MetaData
    {

        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; } = "Development";
        public Permissions permissions { get; set; } = new Permissions();
        // public string version { get; set; }
        public string organization { get; set; }
        public Uri homepageURL { get; set; }
        public Uri downloadURL { get; set; }
        // public Uri disclaimerURL { get; set; }
        public Uri repositoryURL { get; set; }
        public string vcs { get; set; } = "git";
        public double laborHours { get; set; }
        public List<string> languages { get; set; } = new List<string>();
        public DateDef date { get; set; }
        public List<string> tags { get; set; } = new List<string>();
        public ContactDef contact { get; set; }
        public class DateDef
        {
            [JsonConverter(typeof(ESDateTimeConverter))]
            public DateTimeOffset created { get; set; }
            [JsonConverter(typeof(ESDateTimeConverter))]
            public DateTimeOffset lastModified { get; set; }
            [JsonConverter(typeof(ESDateTimeConverter))]
            public DateTime metadataLastUpdated { get; set; }
        }
        public class Permissions
        {
            public string usageType { get; set; } = "governmentWideReuse";


            public List< licenses> licenses { get; set; } =null;
        }

        public class licenses
        {
            public Uri URL { get; set; }
            public string name { get; set; }
        }
        public class ContactDef
        {
            public string email { get; set; }
            public string name { get; set; }
        }

        public class ESDateTimeConverter : IsoDateTimeConverter
        {
            public ESDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

    }
}
