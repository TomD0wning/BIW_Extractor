using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using Mono.Unix;


namespace BIW_Extractor
{
    public static class FileOperations
    {

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
                if (!Directory.Exists($@"{path}/{item}"))
                {
                    Directory.CreateDirectory($@"{path}{item}");
                }
            }
        }

        public static void CreateFolderStructure(string path, List<DocumentRegister> documentRegisters)
        {
            string pathToCreate = string.Empty;

            foreach (var item in documentRegisters)
            {
                pathToCreate = $@"{path}/{item.Name}";
                if (!Directory.Exists(pathToCreate))
                {
                    Directory.CreateDirectory(pathToCreate);
                    SetFullFolderPermissionsUnix(pathToCreate);
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

        public static List<string> ReadFile(string file)
        {
            string[] fileContents;

            List<string> lst = new List<string>();

            using (StreamReader sr = new StreamReader(File.OpenRead(file)))
            {
                fileContents = sr.ReadToEnd().Split('\n');
            }

            return fileContents.ToList();
        }

        public static void CreateProjectFolder(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                System.IO.FileInfo file = new System.IO.FileInfo(filePath);
                file.Directory.Create();
            }
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

        public static void OutputObj(string outputFile, object obj){


            var line = new StringBuilder();

            line.AppendLine(obj.ToString());

            File.AppendAllText(outputFile, line.ToString());
        }

        public static void WriteDocumentMetaDataCsv(string outputFile, Document document, DocumentFiles documentFile, string header, string friendlyName, string fileID)
        {
            var csv = new StringBuilder();
            if (!File.Exists(outputFile))
            {
              csv.AppendLine(header);
            }
                    csv.AppendLine($"{document.IdDocument},{document.IdDocumentSubmission},{fileID},{document.IdDocumentRegister},{friendlyName.Replace(',', ' ').Trim()},{document.IdProject}," +
                   $"{document.ProjectName.Replace(',', ' ').Trim()},{document.ReceivedDate},{document.Description.Replace(',',' ').Trim()},{document.IdCompany}," +
                   $"{document.CompanyName.Replace(',', ' ').Trim()},{document.IssuedDate},{document.DocumentStatus},{document.Working}," +
                   $"{document.IssueNumber},{document.RevisionLetter},{document.PurposeOfIssue},{documentFile.ToString()}");

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
