using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FiftyNine.Ag.OpenXML.Common.Packaging;
using FiftyNine.Ag.OpenXML.Common.Parts;
using OpenXML.Silverlight.Spreadsheet.Parts;

namespace OpenXML.Silverlight.Spreadsheet
{
    public class SpreadsheetDocument : Package
    {
        public SpreadsheetDocument()
        {
            Workbook = CreatePart<WorkbookPart>("/xl/workbook.xml");
            AppPart = CreatePart<AppPart>("/docProps/app.xml");
            CorePart = CreatePart<CorePart>("/docProps/core.xml");

            AddRelationship(Workbook);
            AddRelationship(AppPart);
            AddRelationship(CorePart);
        }

        public WorkbookPart Workbook
        {
            get;
            private set;
        }
        private AppPart AppPart
        {
            get;
            set;
        }
        private CorePart CorePart
        {
            get;
            set;
        }
        public string ApplicationName
        {
            get { return AppPart.Application; }
            set { AppPart.Application = value; }
        }
        public string Company
        {
            get { return AppPart.Company; }
            set { AppPart.Company = value; }
        }
        public string Creator
        {
            get { return CorePart.Creator; }
            set { CorePart.Creator = value; }
        }
        public DateTime Created
        {
            get { return CorePart.Created; }
            set { CorePart.Created = value; }
        }
    }
}
