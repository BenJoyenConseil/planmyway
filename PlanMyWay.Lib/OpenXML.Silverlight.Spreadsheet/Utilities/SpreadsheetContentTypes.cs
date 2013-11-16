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
    public static class SpreadsheetContentTypes
    {
        public const string Workbook = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml";
        public const string Worksheet = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
        public const string SharedStrings = "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml";
        public const string Table = "application/vnd.openxmlformats-officedocument.spreadsheetml.table+xml";
    }
}
