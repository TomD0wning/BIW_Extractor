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
            string user = "DBIWMigration";
            string pass = "X0YF2430XON01";
            string projectHeader = "ID,Name,Code,Display Name,Active";
            string metaDataHeader = "IdDocument,IdDocumentSubmissions,IdDocumentRegister,IdProject,ProjectName,ReceivedDate,Description,IdCompany,IssuedDate,DocumentStatus,Working,IssueNumber,RevisionLetter,PurposeOfIssue,DownloadLink,FileName,FileSize,FileType,DocumentRegisterName";

            //Read from config & set app settings


            //setup network credentials

            NetworkCredential auth = RequestHandler.CreateCredentials(user, pass);

            //get project list
            var projectList = RequestHandler.ParseProjectRequest(RequestHandler.HttpRequest("https://uk-api.myconject.com/api/101/Project/?$top=10", auth,"GET"));
                
            //write project list to disk
            FileOperations.WriteCsv("/Users/tomp.downing/Documents/BIWMigration/ProjectList.csv", projectList, projectHeader,"");

            foreach (var project in projectList)
            {
                //create top level project structure based on project id
                FileOperations.CreateFolder("/Users/tomp.downing/Documents/BIWMigration/" + project.id);
                //Take the project ID and make request for folders
                var docRegList = RequestHandler.ParseRegisterRequest(RequestHandler.HttpRequest($"https://uk-api.myconject.com/api/101/{project.id}/DocumentRegisters", auth, "GET"));
                //create folder based on docRegList
                FileOperations.CreateFolderStructure("/Users/tomp.downing/Documents/BIWMigration/" + project.id, docRegList);

                //get the project docs based on the docRegList
                foreach (var doc in docRegList)
                {
                    var documentList = RequestHandler.ParseDocumentJsonResponse(RequestHandler.HttpRequest($"https://uk-api.myconject.com/api/101/{project.id}/{doc.Id}/Documents", auth, "GET"));
                    try
                    {
                        //Download each document from the doclist for the current DocumentRegister
                        RequestHandler.DownloadDocuments($"/Users/tomp.downing/Documents/BIWMigration/{project.id}/{doc.Id}/", documentList, auth);

                    }catch(Exception e){
                        System.Console.WriteLine(e);
                    }   
                    FileOperations.WriteCsv("/Users/tomp.downing/Documents/BIWMigration/DocumentMetadataList.csv", documentList, metaDataHeader,doc.Name);
                }
            }
            System.Console.WriteLine("fin");
        }
    }
}