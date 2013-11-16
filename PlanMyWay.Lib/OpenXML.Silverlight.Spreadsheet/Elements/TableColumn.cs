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

namespace OpenXML.Silverlight.Spreadsheet.Elements
{
    public class TableColumn : SpreadsheetElement
    {
        protected override void SaveContent(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("id", ID.ToString());
            writer.WriteAttributeString("name", Name);
        }

        public int ID
        {
            get;
            internal set;
        }
        public string Name
        {
            get;
            set;
        }

        protected override string NodeName
        {
            get { return "tableColumn"; }
        }
    }
}
