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

namespace OpenXML.Silverlight.Spreadsheet.Elements
{
    public class CellDefinition
    {
        public CellDefinition(string col, string row)
        {
            Column = col;
            Row = row;
        }

        public string Column
        {
            get;
            private set;
        }
        public string Row
        {
            get;
            private set;
        }
    }
}
