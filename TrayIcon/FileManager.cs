using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrayIcon
{
    class FileManager
    {

        private JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public List<MouseProfile> LoadProfiles() {
            string path = @"../../Data/Profiles.json";
            List<MouseProfile> jsonread = new List<MouseProfile>();
            try {
                string jsonFromFile = File.ReadAllText(path);
                jsonread = JsonConvert.DeserializeObject<List<MouseProfile>>(jsonFromFile, settings);
            } catch {

            }
            return jsonread;

        }
        public async Task<List<MouseProfile>> LoadProfilesFromWeb() {
            List<MouseProfile> profiles = new List<MouseProfile>();
            HttpClient httpclient = new HttpClient();
            var response = await httpclient.GetAsync("https://viskoro16.sps-prosek.cz/restapi/json.php");
            
            try {
                string content = await response.Content.ReadAsStringAsync();
                profiles = JsonConvert.DeserializeObject<List<MouseProfile>>(content, settings);
            } catch {

            }
            return profiles;

        }

        public void SaveProfiles(List<MouseProfile> Profiles) {
            string path = @"../../Data/Profiles.json";
            string jsonToFilet = JsonConvert.SerializeObject(Profiles, settings);
            File.WriteAllText(path, jsonToFilet);
        }
        public void SaveProfiles2(List<MouseProfile> Profiles) {
            string path = @"../../Data/Profiles2.json";
            string jsonToFilet = JsonConvert.SerializeObject(Profiles, settings);
            File.WriteAllText(path, jsonToFilet);
        }

        public async void SaveProfilesToWeb(string Name, int MouseSpeed, int ScrollSpeed, int DoubleSpeed) {
            

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://viskoro16.sps-prosek.cz/restapi/jsontosql.php");
           
            var keyValues = new List<KeyValuePair<string, string>>();

            keyValues.Add(new KeyValuePair<string, string>("name", Name));
            keyValues.Add(new KeyValuePair<string, string>("mspeed", MouseSpeed.ToString()));
            keyValues.Add(new KeyValuePair<string, string>("dspeed", DoubleSpeed.ToString()));
            keyValues.Add(new KeyValuePair<string, string>("sspeed", ScrollSpeed.ToString()));

           
            request.Content = new FormUrlEncodedContent(keyValues);
            
            var response = await client.SendAsync(request);
            
            string responseContent = await response.Content.ReadAsStringAsync();
        }
        public async void UpdateProfilesToWeb(string Name, int MouseSpeed, int ScrollSpeed, int DoubleSpeed) {


            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://viskoro16.sps-prosek.cz/restapi/jsontosqlupdate.php");

            var keyValues = new List<KeyValuePair<string, string>>();

            keyValues.Add(new KeyValuePair<string, string>("name", Name));
            keyValues.Add(new KeyValuePair<string, string>("mspeed", MouseSpeed.ToString()));
            keyValues.Add(new KeyValuePair<string, string>("dspeed", DoubleSpeed.ToString()));
            keyValues.Add(new KeyValuePair<string, string>("sspeed", ScrollSpeed.ToString()));
            

            request.Content = new FormUrlEncodedContent(keyValues);

            var response = await client.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();
        }
    }
}
