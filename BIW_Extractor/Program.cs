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
            var projectList = RequestHandler.ParseProjectRequest(RequestHandler.HttpRequest("https://uk-api.myconject.com/api/101/Project/?$top=500", auth, "GET", logFileLocation));
            //var projectList = FileOperations.ReadProjectFile("../BIWProjectRequestResponse.json");

            List<string> completedProjects = null;

            if(File.Exists(ProjectCheckList))
                completedProjects = FileOperations.ReadFile(ProjectCheckList);

            //write project list to disk
            FileOperations.WriteCsv(conf.ConfigList.GetValueOrDefault("ProjectListCsv"), projectList, conf.ConfigList.GetValueOrDefault("ProjectHeader"), "");

            foreach (var project in projectList)
            {
                if (!completedProjects.Contains(project.id.ToString()))
                {
                    //Take the project ID and make request for folders
                    var docRegList = RequestHandler.ParseRegisterRequest(RequestHandler.HttpRequest($"https://uk-api.myconject.com/api/101/{project.id}/DocumentRegisters", auth, "GET", logFileLocation));
                    //create folder based on docRegList
                    FileOperations.CreateFolderStructure($"{rootFolderLocation}/{project.id}", docRegList);

                    //get the project docs based on the docRegList
                    foreach (var doc in docRegList)
                    {
                        var documentList = RequestHandler.ParseDocumentJsonResponse(RequestHandler.HttpRequest($"https://uk-api.myconject.com/api/101/{project.id}/{doc.Id}/Documents", auth, "GET", logFileLocation));
                        if (documentList.Count == 0)
                        {
                            continue;
                        }
                        try
                        {
                            //Download each document from the doclist for the current DocumentRegister
                            RequestHandler.DownloadDocuments($"{rootFolderLocation}/{project.id}/{doc.Name}/", documentList, auth, logFileLocation, doc.Name);
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine(e);
                            Logging.LogOperation("DocumentDownload", doc.ToString(), "Unsuccessful: " + e.Message, logFileLocation);
                        }
                    }
                    FileOperations.OutputObj(ProjectCheckList, project.id);
                }
            }
            System.Console.WriteLine("fin");
        }
    }
}