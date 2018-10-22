using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace BIW_Extractor
{
    class Program
    {
        static void Main(string[] args)
        {



            //Read from config & set app settings
            Dictionary<string, string> config = FileOperations.ReadConfig();

            //set up variables from config that need to be used multiple times.
            string rootFolderLocation = config.GetValueOrDefault("ProjectDriveLocation");
            string metaDataFileLocation = config.GetValueOrDefault("DocumentListCsv");
            string metaDataHeader = config.GetValueOrDefault("MetaDataHeader");
            string logFileLocation = config.GetValueOrDefault("LogFileLocation");

            //Create log file and start logging
            Logging.CreateLogFile(config.GetValueOrDefault("LogFileLocation"));

            //setup network credentials
            NetworkCredential auth = RequestHandler.CreateCredentials(config.GetValueOrDefault("User"), config.GetValueOrDefault("Pwd"));

            //get project list
            //var projectList = RequestHandler.ParseProjectRequest(RequestHandler.HttpRequest("https://uk-api.myconject.com/api/101/Project/?$top=10", auth,"GET", logFileLocation));
            var projectList = FileOperations.ReadProjectFile("../BIWProjectRequestResponse.json");


            //write project list to disk
            FileOperations.WriteCsv(config.GetValueOrDefault("ProjectListCsv"), projectList, config.GetValueOrDefault("ProjectHeader"),"");


            foreach (var project in projectList)
            {
                //create top level project structure based on project id
                FileOperations.CreateFolder(rootFolderLocation + project.id);
                //Take the project ID and make request for folders
                var docRegList = RequestHandler.ParseRegisterRequest(RequestHandler.HttpRequest($"https://uk-api.myconject.com/api/101/{project.id}/DocumentRegisters", auth, "GET", logFileLocation));
                //create folder based on docRegList
                FileOperations.CreateFolderStructure(rootFolderLocation + project.id, docRegList);

                //get the project docs based on the docRegList
                foreach (var doc in docRegList)
                {
                    var documentList = RequestHandler.ParseDocumentJsonResponse(RequestHandler.HttpRequest($"https://uk-api.myconject.com/api/101/{project.id}/{doc.Id}/Documents", auth, "GET", logFileLocation));
                    try
                    {
                        //Download each document from the doclist for the current DocumentRegister
                        RequestHandler.DownloadDocuments($"{rootFolderLocation}/{project.id}/{doc.Id}/", documentList, auth, logFileLocation);
                        //Add metadata to CSV
                        FileOperations.WriteCsv(metaDataFileLocation, documentList, metaDataHeader, doc.Name);
                    }
                    catch(Exception e){
                        System.Console.WriteLine(e);
                        Logging.LogOperation("DocDownload", doc.ToString(), "Unsuccessful: " + e.Message, logFileLocation);
                    }   

                }
            }
            System.Console.WriteLine("fin");
        }
    }
}