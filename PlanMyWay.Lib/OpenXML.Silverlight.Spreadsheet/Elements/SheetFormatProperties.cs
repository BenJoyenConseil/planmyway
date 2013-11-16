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
using FiftyNine.Ag.OpenXML.Common;
using System.Collections.Generic;
using OpenXML.Silverlight.Spreadsheet.Utilities;
using FiftyNine.Ag.OpenXML.Common.Packaging;

namespace OpenXML.Silverlight.Spreadsheet.Elements
{
    public class SheetFormatProperties : SpreadsheetElement
    {
        protected override void Initialize()
        {
            DefaultRowHeight = 15;
        }

        protected override void SaveContent(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("defaultRowHeight", DefaultRowHeight.ToString());
        }

        public double DefaultRowHeight
        {
            get;
            set;
        }

        protected override string NodeName
        {
            get { return "sheetFormatPr"; }
        }
    }
}
