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
using OpenXML.Silverlight.Spreadsheet.Elements;
using FiftyNine.Ag.OpenXML.Common.Packaging;

namespace OpenXML.Silverlight.Spreadsheet.Elements
{
    public class ColumnSizeDefinition : SpreadsheetElement
    {
        protected override void SaveContent(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("min", (FromIndex + 1).ToString());
            writer.WriteAttributeString("max", (ToIndex + 1).ToString());
            writer.WriteAttributeString("width", Width.ToString());
            writer.WriteAttributeString("customWidth", "1");
            if (BestFit)
                writer.WriteAttributeString("bestFit", "1");
        }

        public int FromIndex
        {
            get;
            internal set;
        }
        public int ToIndex
        {
            get;
            internal set;
        }
        public double Width
        {
            get;
            internal set;
        }
        public bool BestFit
        {
            get;
            set;
        }
        protected override string NodeName
        {
            get { return "col"; }
        }
    }
}
