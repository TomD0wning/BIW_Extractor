using System;
using System.Collections.Generic;

namespace BIW_Extractor
{
    public class Document
    {
        /*
        *"IdDocument": 602076,
        *"IdDocumentSubmission": 928742,
        *"IdDocumentRegister": 18,
        *"IdProject": 9333,
        *"ProjectName": " Cheadle (2304) Checkout Installation 2016",
        *"ReceivedDate": "2016-11-16T10:26:51",
        *"Description": "Audit Index Section 4 - Certificates",
        *"IdCompany": 364,
        *"CompanyName": "Air Link Systems Limited",
        *"IssuedDate": "2016-11-16T10:26:51.15",
        *"DocumentStatus": "Information",
        *"Working": false,
        *"IssueNumber": 2,
        *"RevisionLetter": "B",
        *"PurposeOfIssue": "",
        *"DocumentFiles": [
            {
                "DownloadLink": "https://uk-api.myconject.com/api/101/9333/DocumentDownload/928742?documentFileId=1357581",
                "FileName": "Audit Index Section 4 - Certificates",
                "FileSize": 115712,
                "FileType": "doc"
         * 
         */

        public int IdDocument;
        public int IdDocumentSubmission;
        public int IdDocumentRegister;
        public int IdProject;
        public string ProjectName;
        public string ReceivedDate;
        public string Description;
        public int IdCompany;
        public string CompanyName;
        public string IssuedDate;
        public string DocumentStatus;
        public bool? Working;
        public int IssueNumber;
        public string RevisionLetter;
        public string PurposeOfIssue;
        public List<DocumentFiles> Documents;


        public override string ToString()
        {
            return $"{this.IdDocument},{this.IdDocumentSubmission},{this.IdDocumentRegister},{this.IdProject},{this.ProjectName.Replace(',',' ').Trim()},{this.ReceivedDate}," +
                $"{this.Description},{this.IdCompany},{this.CompanyName.Replace(',',' ').Trim()},{this.IssuedDate},{this.DocumentStatus},{this.Working},{this.IssueNumber},{this.RevisionLetter},{this.PurposeOfIssue}";
        }   
    }

    public class DocumentFiles
    {
        public string DownloadLink;
        public string FileName;
        public int FileSize;
        public string FileType;

        public override string ToString()
        {
            return $"{this.DownloadLink},{this.FileName.Replace(',',' ')},{this.FileSize},{this.FileType}";
        }
    }


    public class DocumentRegister
    {
        public int Id;
        public string Name;

        public override string ToString()
        {
            return $"{this.Id},{this.Name}";
        }

    }

}
