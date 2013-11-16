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
using FiftyNine.Ag.OpenXML.Common.Packaging;
using OpenXML.Silverlight.Spreadsheet.Utilities;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace OpenXML.Silverlight.Spreadsheet.Elements
{
    public class Row : IndexedSpreadsheetElement
    {
        int _colSpan;

        protected override void Initialize()
        {
            Cells = new DictionaryCollection<Cell>(this.Part);
            Cells.ItemCreated += (o, e) => { 
                e.Payload.RowIndex = Index; 
            };
        }

        internal void Save(XmlWriter writer, int columnSpan)
        {
            _colSpan = columnSpan;
            base.Save(writer);
        }
        protected override void SaveContent(XmlWriter writer)
        {
            writer.WriteAttributeString("r", (Index + 1).ToString());
            writer.WriteAttributeString("spans", string.Format("{0}:{1}", 1, _colSpan));
            if (IsHidden)
                writer.WriteAttributeString("hidden", "1");

            if (Height != default(double))
            {
                writer.WriteAttributeString("ht", Height.ToString());
                writer.WriteAttributeString("customHeight", "1");
            }

            foreach (var index in Cells.Indexes.OrderBy(k => k))
            {
                Cells[index].Save(writer);
            }
        }

        public DictionaryCollection<Cell> Cells
        {
            get;
            private set;
        }
        public bool IsHidden
        {
            get;
            set;
        }
        public double Height
        {
            get;
            set;
        }

        protected override string NodeName
        {
            get { return "row"; }
        }
    }
}
