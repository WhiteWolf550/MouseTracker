using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public void SaveProfiles(List<MouseProfile> Profiles) {
            string path = @"../../Data/Profiles.json";
            string jsonToFilet = JsonConvert.SerializeObject(Profiles, settings);
            File.WriteAllText(path, jsonToFilet);
        }
    }
}
