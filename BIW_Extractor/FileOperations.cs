using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace BIW_Extractor
{
    public static class FileOperations
    {


        public static void ReadConfig()
        {


        }


        public static void WriteResponse(string file, HttpWebResponse response)
        {

        }

        public static void WriteProjectCsv(string outputFile, List<Project> proj){

            var csv = new StringBuilder();

            string header = "ID,Name,Code,Display Name,Active";

            csv.AppendLine(header);

            foreach (Project item in proj)
            {
                csv.AppendLine(item.ToString());
            }


            File.WriteAllText(outputFile, csv.ToString());
        }

        public static void WriteDocumentCsv(string outputFile, List<Document> docs)
        {
            var csv = new StringBuilder();

            string header = "IdDocument,IdDocumentSubmissions,IdDocumentRegister,IdProject,ProjectName,ReceivedDate,Description,IdCompany,IssuedDate,DocumentStatus,Working,IssueNumber,RevisionLetter,PurposeOfIssue,DownloadLink,FileName,FileSize,FileType,SuccessfulDL,Location";
            csv.AppendLine(header);

            foreach(Document d in docs)
            {
                csv.AppendLine(d.ToString());
            }

            File.WriteAllText(outputFile, csv.ToString());
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
