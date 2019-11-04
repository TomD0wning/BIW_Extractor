using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BIW_Extractor
{
    public class Config
    {
        public Config(){

            ConfigList = new Dictionary<string, string>();
        }

        public Dictionary<string, string> ConfigList { get; set; }

        public void ReadConfigFile(){   
                    
            using (StreamReader sr = new StreamReader("devConfig.json"))
            {
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(sr));

                foreach (var item in o)
                {
                    ConfigList.Add(item.Key, item.Value.ToString());
                }
            }
        }
    }
}
