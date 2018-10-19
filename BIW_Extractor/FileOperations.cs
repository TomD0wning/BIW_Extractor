using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Mono.Unix;


namespace BIW_Extractor
{
    public static class FileOperations
    {


        public static void ReadConfig()
        {


        }

        /// <summary>
        /// Create the folder if it does not exist
        /// </summary>
        /// <param name="path">path name</param>
        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Create the folder if it does not exist
        /// </summary>
        /// <param name="path">path name</param>
        public static void Createfolder<T>(string path, List<T> listOfFolders)
        {
            foreach (var item in listOfFolders)
            {
                if (!Directory.Exists(path + "/" + item))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        public static void CreateFolderStructure(string path, List<DocumentRegister> documentRegisters)
        {

            foreach (var item in documentRegisters)
            {
                if (!Directory.Exists(path + "/" + item.Name))
                {
                    Directory.CreateDirectory(path + "/" + item.Id);
                    SetFullFolderPermissionsUnix(path + "/" + item.Id);
                }
            }
        }

        public static void SetFullFolderPermissionsUnix(string path)
        {
            var nixFolderInfo = new UnixDirectoryInfo(path)
            {
                FileAccessPermissions = FileAccessPermissions.AllPermissions
            };

        }

        public static void CreateProjectFolder(string filePath)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create();
        }

        /*
         * <Write a generic csv>
         * 
         * <Params: >
         */

        public static void WriteCsv<T>(string outputFile, List<T> metaDataList, string header, string altMetaData)
        {
            var csv = new StringBuilder();

            if (!File.Exists(outputFile))
                csv.AppendLine(header);

            foreach (var item in metaDataList)
            {
                csv.AppendLine($"{item.ToString()},{altMetaData}");
            }

            File.AppendAllText(outputFile, csv.ToString());
        }

        //"Items": [
        //{
        //  "Id": 9333,
        //  "Name": " Cheadle (2304) Checkout Installation 2016",
        //  "Code": "TBC",
        //  "DisplayName": " Cheadle (2304) Checkout Installation 2016",
        //  "Active": true
        //},

        public static List<Project> ReadProjectFile(string file)
        {
            string fileContents;

            using (StreamReader sr = new StreamReader(File.OpenRead(file)))
            {
                fileContents = sr.ReadToEnd();
            }

            JObject x = JObject.Parse(fileContents);

            JArray Projects = (JArray)x["Items"];

            List<Project> projectList = new List<Project>();

            foreach (var item in Projects)
            {
                projectList.Add(JsonConvert.DeserializeObject<Project>(item.ToString()));
            }

            return projectList;
        }
    }
}
