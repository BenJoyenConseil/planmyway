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

namespace OpenXML.Silverlight.Spreadsheet.Utilities
{
    public static class SpreadsheetRelationshipTypes
    {
        public const string Workbook = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument";
        public const string Worksheet = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet";
        public const string SharedStrings = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings";
        public const string Table = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/table";
    }
}
