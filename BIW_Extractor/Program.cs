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

            //Create log file and start logging, based on the current config context
            Logging.CreateLogFile(conf.ConfigList.GetValueOrDefault("LogFileLocation"));

            //setup network credentials
            NetworkCredential auth = RequestHandler.CreateCredentials(conf.ConfigList.GetValueOrDefault("User"), conf.ConfigList.GetValueOrDefault("Pwd"));

            //get project list from making a HTTP call to the BIW API for all project. $Top=100000 is used to get very project.
            var projectList = RequestHandler.ParseProjectRequest(RequestHandler.HttpRequest("https://uk-api.myconject.com/api/101/Project/?$top=100000", auth, "GET", logFileLocation));

            //Uncomment the below & change the configuration file path to read from a file for the project to download.
            //var projectList = FileOperations.ReadProjectsFromCSV("/Volumes/Kracken/BIW_Migration/UpdatedProjects.txt");

            List<string> completedProjects = null;

            //Obtain a list of already completed project in order to avoid redownloading. Useful for failover and dealing with the flaky BIW API
            if(File.Exists(ProjectCheckList))
                completedProjects = FileOperations.ReadFile(ProjectCheckList);

            //write project list to disk, un comment to write the full list of projects to a CSV. Only required in the first instance of running.
            //FileOperations.WriteCsv(conf.ConfigList.GetValueOrDefault("ProjectListCsv"), projectList, conf.ConfigList.GetValueOrDefault("ProjectHeader"), "");


            /*
             *The below foreahc loop is the main part of the extraction, due to the API constraints and nesting of projects, actions need to be taken sequentally
             *first by getting the Document Register, which responds with the list of created folders within the project, then make a call based on the project & the DocumentRegister
             *to ascertain if there are any documents to download, then download the projects and log to metadata & save to the file system.
             */
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