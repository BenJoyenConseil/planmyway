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
using System.Collections.Generic;
using OpenXML.Silverlight.Spreadsheet.Elements;
using System.Linq;

namespace OpenXML.Silverlight.Spreadsheet.Parts
{
    public class WorksheetPart : PackagePart
    {
        int _sheetID;
        List<ColumnSizeDefinition> _columnSizeDefinition;
        List<TablePart> _tables = new List<TablePart>();

        protected override void Initialize()
        {
            Margin = new Thickness(.7, .75, .7, .75);
            HeaderSpace = .3;
            FooterSpace = .3;
            SheetFormat = CreateElement<SheetFormatProperties>();
            Rows = new DictionaryCollection<Row>(this);
            DefinedNames = new Dictionary<string, CellDefinition>();
            _columnSizeDefinition = new List<ColumnSizeDefinition>();
            AddRequiredNamespace(FiftyNine.Ag.OpenXML.Common.Constants.Namespaces.Relationships);
        }
        internal void SetUp(int sheetId)
        {
            _sheetID = sheetId;
        }

        protected override void SaveContent(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("worksheet", SpreadsheetNamespaces.Main.Value);
            WriteRequiredNamespaces(writer);

            writer.WriteStartElement("dimension");
            int maxCol;
            int maxRow;
            GetBottomRightCoordinate(out maxCol, out maxRow);
            string f = Formatting.ColRowFormat(maxCol, maxRow);
            if (f != "A1")
            {
                f = "A1:" + f;
            }
            writer.WriteAttributeString("ref", string.Format("{0}", f));
            writer.WriteEndElement();

            if (_columnSizeDefinition.Count > 0)
            {
                writer.WriteStartElement("cols");
                foreach (var colDef in _columnSizeDefinition)
                {
                    colDef.Save(writer);
                }
                writer.WriteEndElement();
            }

            writer.WriteStartElement("sheetData");
            foreach (var index in Rows.Indexes)
            {
                Row row = Rows[index];
                row.Save(writer, maxCol);
            }
            writer.WriteEndElement();

            writer.WriteStartElement("pageMargins");
            writer.WriteAttributeString("left", Margin.Left.ToString());
            writer.WriteAttributeString("right", Margin.Right.ToString());
            writer.WriteAttributeString("top", Margin.Top.ToString());
            writer.WriteAttributeString("bottom", Margin.Bottom.ToString());
            writer.WriteAttributeString("header", HeaderSpace.ToString());
            writer.WriteAttributeString("footer", FooterSpace.ToString());
            writer.WriteEndElement();

            if (_tables.Count > 0)
            {
                writer.WriteStartElement("tableParts");
                writer.WriteAttributeString("count", _tables.Count.ToString());
                var tableRelsQuery = from r in Relationships
                                     where r.RelationshipType == SpreadsheetRelationshipTypes.Table
                                     select r;
                foreach (var rel in tableRelsQuery)
                {
                    writer.WriteStartElement("tablePart");
                    writer.WriteAttributeString(FiftyNine.Ag.OpenXML.Common.Constants.Namespaces.Relationships.Prefix,
                        "id", FiftyNine.Ag.OpenXML.Common.Constants.Namespaces.Relationships.Value,
                        rel.ID);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
        private void GetBottomRightCoordinate(out int x, out int y)
        {
            if (Rows.Indexes.Count() == 0)
            {
                x = 1;
                y = 0;
                return;
            }
            y = Rows.Indexes.Max();
            x = 0;
            foreach (var index in Rows.Indexes)
            {
                int max = Rows[index].Cells.Indexes.Max();
                if (max > x)
                    x = max;
            }
            x++;
        }

        public TablePart AddTable(string tableName, string displayName, Cell topLeft, Cell bottomRight)
        {
            string partName = string.Format("/xl/tables/table{0}.xml", Tables.Count() + 1);
            //string partName = string.Format("/xl/tables/table_{0}_{1}.xml", SheetID, Tables.Count() + 1);
            TablePart table = Package.CreatePart<TablePart>(partName);
            table.SetUp(_tables.Count + 1, tableName, displayName, topLeft, bottomRight);
            AddRelationship(table);
            _tables.Add(table);
            return table;
        }
        public ColumnSizeDefinition AddColumnSizeDefinition(int fromIndex, int toIndex, double width)
        {
            ColumnSizeDefinition colDef = CreateElement<ColumnSizeDefinition>();
            colDef.FromIndex = fromIndex;
            colDef.ToIndex = toIndex;
            colDef.Width = width;
            _columnSizeDefinition.Add(colDef);
            return colDef;
        }

        public int SheetID
        {
            get { return _sheetID; }
        }
        public DictionaryCollection<Row> Rows
        {
            get;
            private set;
        }
        public Dictionary<string, CellDefinition> DefinedNames
        {
            get;
            private set;
        }
        public Thickness Margin
        {
            get;
            set;
        }
        public double HeaderSpace
        {
            get;
            set;
        }
        public double FooterSpace
        {
            get;
            set;
        }
        public SheetFormatProperties SheetFormat
        {
            get;
            private set;
        }
        public IEnumerable<ColumnSizeDefinition> ColumnSizes
        {
            get { return _columnSizeDefinition.AsEnumerable(); }
        }
        public IEnumerable<TablePart> Tables
        {
            get { return _tables.AsEnumerable(); }
        }

        protected override string ContentType
        {
            get { return SpreadsheetContentTypes.Worksheet; }
        }
        protected override string RelationshipType
        {
            get { return SpreadsheetRelationshipTypes.Worksheet; }
        }
    }
}
