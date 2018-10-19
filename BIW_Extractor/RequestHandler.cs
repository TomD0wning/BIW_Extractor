using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace BIW_Extractor
{
    public static class RequestHandler
    {

        public static string HttpRequest(string url, NetworkCredential auth , string requestMethod){

            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(url);
            request.Method = requestMethod;
            request.ContentType = "application/json";
            request.Credentials = auth;

            string response = String.Empty;

            try
            {
                using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    response = sr.ReadToEnd();
                }
            } 
            catch(WebException e)
            {
                Console.WriteLine($"Request: {url} failed \n{e.Message}");
            }

            Console.WriteLine($"Request: {url} successful");
            return response;
        }


        public static void DownloadDocuments(string path, List<Document> fileList, NetworkCredential auth)
        {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = auth;
                    
                    foreach (var item in fileList)
                    {
                    client.DownloadFile(item.Documents.DownloadLink , path + item.Documents.FileName + "." + item.Documents.FileType);
                    }
                }
        }

        //TODO - write the builder
        public static string UrlBuilder(string url)
        {

            return "";
        }

        public static NetworkCredential CreateCredentials(string user, string pass)
        {
            return new NetworkCredential(user, pass);
        }

        public static List<Document> ParseDocumentJsonResponse(string jsonResponse)
        {

            JArray y = JArray.Parse(jsonResponse);

            List<Document> docList = new List<Document>();

            DocumentFiles documentFiles;

            foreach (var item in y)
            {
                JArray DocFiles = (JArray)item["DocumentFiles"];

                Document doc = JsonConvert.DeserializeObject<Document>(item.ToString());

                foreach (var docs in DocFiles)
                {
                    documentFiles = JsonConvert.DeserializeObject<DocumentFiles>(docs.ToString());
                    doc.Documents = documentFiles;
                }

                docList.Add(doc);
            }

            return docList;
        }

        /*
         * [{"Id":1,"Name":"Meeting Minutes"},{"Id":3,"Name":"Project Directories"},{"Id":4,"Name":"Site Photographs"},
         * {"Id":5,"Name":"Drawings"},{"Id":6,"Name":"Spec's & Schedules"},{"Id":9,"Name":"Feasibility"},
         * {"Id":13,"Name":"Client Briefs"},{"Id":18,"Name":"Health & Safety Information"},{"Id":20,"Name":"O&M Manuals"},
         * {"Id":21,"Name":"Reports Surveys & Investigations"},{"Id":22,"Name":"Programmes"},{"Id":24,"Name":"Tender Information"}]
         */

        public static List<DocumentRegister> ParseRegisterRequest(string jsonResponse)
        {

            List<DocumentRegister> register = new List<DocumentRegister>();

            JArray x = JArray.Parse(jsonResponse);

            foreach (var item in x)
            {
                register.Add(JsonConvert.DeserializeObject<DocumentRegister>(item.ToString()));
            }

            return register;
        }


        public static List<Project> ParseProjectRequest(string jsonResponse)
        {

            JObject x = JObject.Parse(jsonResponse);

            JArray Projects = (JArray)x["Items"];

            List<Project> projectList = new List<Project>();

            foreach (var item in Projects)
            {
                projectList.Add(JsonConvert.DeserializeObject<Project>(item.ToString()));
            }

            return projectList;
        }

        public static JArray JsonStringToJArray(string json)
        {
            return JArray.Parse(json);
        }

    }
}
