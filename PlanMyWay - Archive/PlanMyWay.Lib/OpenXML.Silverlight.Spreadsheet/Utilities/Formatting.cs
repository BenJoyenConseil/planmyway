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
    public static class Formatting
    {
        public static string ColRowFormat(int col, int row)
        {
            return string.Format(string.Format("{0}{1}", (char)(col + 64), row + 1));
        }
    }
}
