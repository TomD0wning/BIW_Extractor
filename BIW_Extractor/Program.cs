using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace BIW_Extractor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read from config & set app settings
            //Dictionary<string, string> config = FileOperations.ReadConfig();
            Config conf = new Config();

            conf.ReadConfigFile();

            //set up variables from config that need to be used multiple times.
            string rootFolderLocation = conf.ConfigList.GetValueOrDefault("ProjectDriveLocation");
            string metaDataFileLocation = conf.ConfigList.GetValueOrDefault("DocumentListCsv");
            string metaDataHeader = conf.ConfigList.GetValueOrDefault("MetaDataHeader");
            string logFileLocation = conf.ConfigList.GetValueOrDefault("LogFileLocation");
            string ProjectCheckList = conf.ConfigList.GetValueOrDefault("ProjectCheckList");

            //Create log file and start logging
            Logging.CreateLogFile(conf.ConfigList.GetValueOrDefault("LogFileLocation"));

            //setup network credentials
            NetworkCredential auth = RequestHandler.CreateCredentials(conf.ConfigList.GetValueOrDefault("User"), conf.ConfigList.GetValueOrDefault("Pwd"));

            //get project list
            var projectList = RequestHandler.ParseProjectRequest(RequestHandler.HttpRequest("https://uk-api.myconject.com/api/101/Project/?$top=130000", auth, "GET", logFileLocation));

            List<string> emptyProjects = null;

            if(File.Exists(ProjectCheckList))
                emptyProjects = FileOperations.ReadFile(ProjectCheckList);

            foreach (var project in projectList)
            {
                if (!emptyProjects.Contains(project.id.ToString()))
                {
                    //Take the project ID and make request for folders
                    var docRegList = RequestHandler.ParseRegisterRequest(RequestHandler.HttpRequest($"https://uk-api.myconject.com/api/101/{project.id}/DocumentRegisters", auth, "GET", logFileLocation));

                    int emptyDocRegCount = 0;
                     
                    //get the project docs based on the docRegList
                    foreach (var doc in docRegList)
                    {
                        //see if there is any docs within doc list and if not
                        var documentList = RequestHandler.ParseDocumentJsonResponse(RequestHandler.HttpRequest($"https://uk-api.myconject.com/api/101/{project.id}/{doc.Id}/Documents", auth, "GET", logFileLocation));
                        if (documentList.Count == 0)
                        {
                            emptyDocRegCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if(emptyDocRegCount == docRegList.Count)
                        FileOperations.OutputObj(ProjectCheckList, $"{project.id} - {project.name}");
                }
            }
            System.Console.WriteLine("fin");
        }
    }
}