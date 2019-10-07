using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SourceCodeInventory
{
    class SourceCodeInventory
    {
        static void Main(string[] args)
        {
            getDataAsync();
            Console.Read();
        }
        static List<Models.json> data { get; set; } = new List<Models.json>();

        private static async void getDataAsync()
        {
            Console.WriteLine("Using path: " + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.github.com");
                client.DefaultRequestHeaders.Add("User-Agent", "Anything");
                client.DefaultRequestHeaders.Add("Authorization", "token " + ConfigurationManager.AppSettings["Token"]);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Models.json json = new Models.json();
                List<Models.json> jsondata = new List<Models.json>();
                List<Models.MetaData> data1 = new List<Models.MetaData>();

                for (int page = 1; page <=Convert.ToInt32(ConfigurationManager.AppSettings["Page"]); page++)
                {
                    Console.WriteLine("i is: " + page + "; Count: " + data.Count());
                    if (page > 1) { Thread.Sleep(TimeSpan.FromSeconds(3)); }
                    HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["Organization Path"] + "?per_page=100&page=" + page);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine("\n\n>>>: " + response.ToString());
                        return;
                    }
                    response.EnsureSuccessStatusCode();
                    List<Models.Repository> repositories = await response.Content.ReadAsAsync<List<Models.Repository>>();
                    json.Headerversion = response.RequestMessage.Version.ToString();
                    json.agency = ConfigurationManager.AppSettings["Organization"];
                    json.measurementType = new Models.json.measurement();
                    json.measurementType.method = "projects";

                    if (repositories != null && repositories.Count() > 0)
                    {
                        foreach (Models.Repository repository in repositories)
                        {
                            Models.MetaData MetaData = new Models.MetaData();
                            MetaData.name = repository.FullName;
                            string[] partsList = MetaData.name.Split("/");
                            if (partsList.Count() == 2)
                            {
                                MetaData.organization = partsList[0];
                            }
                            MetaData.name = repository.Name;

                            if (repository.Description == null)
                            {
                                MetaData.description = "No description available...";
                            }
                            else
                            {
                                MetaData.description = repository.Description;
                            }

                            MetaData.date = new Models.MetaData.DateDef();
                            MetaData.homepageURL = repository.SvnUrl;
                            MetaData.downloadURL = repository.DownloadsUrl;
                            MetaData.repositoryURL = repository.CloneUrl;
                            MetaData.date.created = repository.CreatedAt;
                            MetaData.date.lastModified = repository.UpdatedAt;
                            MetaData.date.metadataLastUpdated = DateTime.Now;

                            if (!String.IsNullOrWhiteSpace(repository.Language))
                            {
                                /// languages
                                MetaData.languages.Add(repository.Language);
                            }

                            if (repository.Licenses == null)
                            {

                                MetaData.permissions.licenses = null;
                            }
                            else
                            {
                                if (repository.Licenses.Url == null) { MetaData.permissions.licenses = null; }
                                else
                                {
                                    List<Models.MetaData.licenses> licenses = new List<Models.MetaData.licenses>();
                                    Models.MetaData.licenses l = new Models.MetaData.licenses();
                                    l.name = repository.Licenses.Name;
                                    l.URL = repository.Licenses.Url;
                                    licenses.Add(l);
                                    MetaData.permissions.licenses = licenses;
                                }
                            }

                            if (repository.TagsUrl != null)
                            {
                                HttpResponseMessage tagsResponse = await client.GetAsync(repository.TagsUrl);
                                if (tagsResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    List<Models.UnConfirmed.RepositoryTag> tags = await tagsResponse.Content.ReadAsAsync<List<Models.UnConfirmed.RepositoryTag>>();
                                    if (tags != null)
                                    {
                                        Console.WriteLine("Tags: " + tags.Count());
                                        foreach (Models.UnConfirmed.RepositoryTag repoTag in tags)
                                        {
                                            MetaData.tags.Add(repoTag.Name);
                                        }
                                    }
                                }
                            }

                            /// About the Organization Information
                            if (!String.IsNullOrEmpty(MetaData.organization))
                            {
                                HttpResponseMessage repostoryResponse = await client.GetAsync("/orgs/" + MetaData.organization);
                                Models.Organization organization = await repostoryResponse.Content.ReadAsAsync<Models.Organization>();
                                if (organization != null)
                                {
                                    MetaData.contact = new Models.MetaData.ContactDef();
                                    MetaData.contact.email = organization.Email;
                                    MetaData.contact.name = organization.Name;
                                }
                            }

                            /// Get latest release
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            HttpResponseMessage latestReleaseResponse = await client.GetAsync(repository.Url + "/releases");
                            List<Models.UnConfirmed.Release> releases = await latestReleaseResponse.Content.ReadAsAsync<List<Models.UnConfirmed.Release>>();

                            /// Languages
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                            HttpResponseMessage languagesResponse = await client.GetAsync(repository.Url + "/languages");
                            Dictionary<string, string> k = JsonConvert.DeserializeObject<Dictionary<string, string>>(await languagesResponse.Content.ReadAsStringAsync());

                            foreach (KeyValuePair<string, string> entry in k)
                            {
                                // do something with entry.Value or entry.Key
                                if (entry.Key != null && !MetaData.languages.Contains(entry.Key))
                                {
                                    MetaData.languages.Add(entry.Key);
                                }
                            }

                            MetaData.organization = ConfigurationManager.AppSettings["Organization Name"];
                            data1.Add(MetaData);

                            /// WriteFile();

                        }

                        json.releases = data1;

                    }
                    else
                    {
                        break;
                    }
                }
                data.Add(json);

                WriteFile();
            }
            Console.WriteLine("THE END!");
        }

        private static void WriteFile()
        {

            Models.json json1 = new Models.json();
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string json = JToken.Parse(JsonConvert.SerializeObject(data)).ToString();
            Console.WriteLine("\n\n" + json);
            if (File.Exists(path + "\\output.json"))
            {
                File.Delete(path + "\\output.json");
            }
            
                json = json.Remove(0, 1);
                json = json.Remove(json.Length - 1, 1);
                json = json.Remove(json.Length - 1, 1);
            
            System.IO.File.WriteAllText(path + "\\output.json", json);
        }
    }
}
