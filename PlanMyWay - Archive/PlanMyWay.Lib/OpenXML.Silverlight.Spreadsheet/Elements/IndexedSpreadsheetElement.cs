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
    public abstract class IndexedSpreadsheetElement : SpreadsheetElement
    {
        internal void SetUp(int index)
        {
            Index = index;
        }

        protected int Index
        {
            get;
            private set;
        }
    }
}
