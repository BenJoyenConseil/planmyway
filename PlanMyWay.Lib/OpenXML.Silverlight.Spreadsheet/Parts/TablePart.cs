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
using OpenXML.Silverlight.Spreadsheet.Utilities;
using OpenXML.Silverlight.Spreadsheet.Elements;
using System.Collections.Generic;

namespace OpenXML.Silverlight.Spreadsheet.Parts
{
    public class TablePart : PackagePart
    {
        string _tableName;
        int _id;
        Point _tl;
        Point _br;

        protected override void Initialize()
        {
            TableStyle = CreateElement<TableStyleInfo>();
        }
        internal void SetUp(int id, string tableName, string displayName, Cell topLeft, Cell bottomRight)
        {
            _id = id;
            _tableName = tableName.Replace(' ', '_');
            _tl = new Point(topLeft.ColumnIndex, topLeft.RowIndex);
            _br = new Point(bottomRight.ColumnIndex, bottomRight.RowIndex);
            DisplayName = displayName.Replace(' ','_');

            List<TableColumn> tableColumns = new List<TableColumn>();
            int cnt = 1;
            for (int i = (int)_tl.X; i <= (int)_br.X; i++)
            {
                TableColumn col = CreateElement<TableColumn>();
                col.ID = cnt;
                col.Name = "Column" + cnt;
                tableColumns.Add(col);
                cnt++;
            }
            TableColumns = tableColumns.ToArray();
        }

        protected override void SaveContent(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("table", SpreadsheetNamespaces.Main.Value);
            //WriteRequiredNamespaces(writer);

            writer.WriteAttributeString("id", _id.ToString());
            writer.WriteAttributeString("name", _tableName);
            writer.WriteAttributeString("displayName", DisplayName);
            writer.WriteAttributeString("ref", string.Format("{0}:{1}",Formatting.ColRowFormat((int)_tl.X+1,(int)_tl.Y),Formatting.ColRowFormat((int)_br.X+1,(int)_br.Y)));
            writer.WriteAttributeString("totalsRowShown", ShowTotalsRow ? "1" : "0");

            writer.WriteStartElement("autoFilter");
            writer.WriteAttributeString("ref", string.Format("{0}:{1}", Formatting.ColRowFormat((int)_tl.X+1, (int)_tl.Y), Formatting.ColRowFormat((int)_br.X+1, (int)_br.Y)));
            writer.WriteEndElement();

            writer.WriteStartElement("tableColumns");
            writer.WriteAttributeString("count", TableColumns.Length.ToString());
            foreach (var col in TableColumns)
            {
                col.Save(writer);
            }
            writer.WriteEndElement();

            TableStyle.Save(writer);

            writer.WriteEndElement();
        }

        public string DisplayName
        {
            get;
            set;
        }
        public bool ShowTotalsRow
        {
            get;
            set;
        }
        public TableColumn[] TableColumns
        {
            get;
            private set;
        }
        public TableStyleInfo TableStyle
        {
            get;
            private set;
        }

        protected override string ContentType
        {
            get { return SpreadsheetContentTypes.Table; }
        }
        protected override string RelationshipType
        {
            get { return SpreadsheetRelationshipTypes.Table; }
        }
    }
}
