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
using System.Xml;

namespace OpenXML.Silverlight.Spreadsheet.Elements
{
    public class TableStyleInfo : SpreadsheetElement
    {
        protected override void SaveContent(XmlWriter writer)
        {
            writer.WriteAttributeString("showFirstColumn", ShowFirstColumn ? "1" : "0");
            writer.WriteAttributeString("showLastColumn", ShowLastColumn ? "1" : "0");
            writer.WriteAttributeString("showRowStripes", ShowRowStripes ? "1" : "0");
            writer.WriteAttributeString("showColumnStripes", ShowColumnStripes ? "1" : "0");
            writer.WriteAttributeString("defaultTableStyle", "TableStyleMedium2");
            writer.WriteAttributeString("defaultPivotStyle", "PivotStyleLight16");
        }

        public bool ShowFirstColumn
        {
            get;
            set;
        }
        public bool ShowLastColumn
        {
            get;
            set;
        }
        public bool ShowRowStripes
        {
            get;
            set;
        }
        public bool ShowColumnStripes
        {
            get;
            set;
        }

        protected override string NodeName
        {
            get { return "tableStyleInfo"; }
        }
    }
}
