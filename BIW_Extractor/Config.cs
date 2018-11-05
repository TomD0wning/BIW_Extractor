﻿using System.Collections.Generic;
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


        /*
         *Prod Config: config.json 
         *Dev Config: devConfig.json
         *Local Config: localSysConfig.json
         */


        public void ReadConfigFile(){   
                    
            using (StreamReader sr = new StreamReader("localSysConfig.json"))
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
