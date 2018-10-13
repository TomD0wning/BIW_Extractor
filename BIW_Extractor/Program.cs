using System.Net.Http;


namespace BIW_Extractor
{
    class Program
    {
        static void Main(string[] args)
        {
            string user = "DBIWMigration";
            string pass = "X0YF2430XON01";

            //RequestHandler.HttpRequest("https://uk-api.myconject.com/api/101/Project",user,pass,"GET");
            //var res = RequestHandler.HttpRequest("https://uk-api.myconject.com/api/101/9333/Companies", user, pass, "GET");


            //Extract Doc folder list
            var docList = RequestHandler.HttpRequest("https://uk-api.myconject.com/api/101/9333/DocumentRegisters", user, pass, "GET");
            RequestHandler.ParseRegisterRequest(docList);


            //Extract documents

            //var res = RequestHandler.HttpRequest("https://uk-api.myconject.com/api/101/9333/18/Documents", user, pass, "GET");
            //var docTest = RequestHandler.ParseJsonResponse(res);
            //FileOperations.WriteDocumentCsv("../DocumentMetadataList.csv", docTest);
            //System.Console.WriteLine(docTest.ToString());




            //var lst = FileOperations.ReadProjectFile("../BIWProjectRequestResponse.json");
            //FileOperations.WriteProjectCsv("../ProjectList.csv", lst);
        }
    }
}
