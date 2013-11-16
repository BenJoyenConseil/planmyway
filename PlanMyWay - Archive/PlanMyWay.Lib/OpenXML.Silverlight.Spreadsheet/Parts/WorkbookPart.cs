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
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using OpenXML.Silverlight.Spreadsheet.Elements;

namespace OpenXML.Silverlight.Spreadsheet.Parts
{
    public class WorkbookPart : PackagePart
    {
        List<SheetDefinition> _sheets = new List<SheetDefinition>();

        protected override void Initialize()
        {
            AddRequiredNamespace(FiftyNine.Ag.OpenXML.Common.Constants.Namespaces.Relationships);

            AddSheet("/xl/worksheets/sheet1.xml", "Sheet1");
            AddSheet("/xl/worksheets/sheet2.xml", "Sheet2");
            AddSheet("/xl/worksheets/sheet3.xml", "Sheet3");

            SharedStrings = Package.CreatePart<SharedStringsPart>("/xl/sharedStrings.xml");
            AddRelationship(SharedStrings);
        }

        public WorksheetPart AddSheet(string partName, string sheetName)
        {
            WorksheetPart sheet = Package.CreatePart<WorksheetPart>(partName);
            sheet.SetUp(_sheets.Count + 1);
            Relationship rel = AddRelationship(sheet);

            _sheets.Add(new SheetDefinition() { Sheet = sheet, Relationship = rel, Name = sheetName });
            return sheet;
        }

        protected override void SaveContent(XmlWriter writer)
        {
            writer.WriteStartElement("workbook", SpreadsheetNamespaces.Main.Value);
            WriteRequiredNamespaces(writer);

            writer.WriteStartElement("sheets");
            foreach (var sheetDef in _sheets)
            {
                sheetDef.Save(writer);
            }
            writer.WriteEndElement();

            Dictionary<string, KeyValuePair<string, CellDefinition>> defNames = new Dictionary<string, KeyValuePair<string, CellDefinition>>();
            List<string> sheetName = new List<string>();

            foreach (var sheetDef in _sheets)
            {
                foreach (var name in sheetDef.Sheet.DefinedNames)
                {
                    defNames.Add(sheetDef.Name, name);
                }
            }

            if (defNames.Count > 0)
            {
                writer.WriteStartElement("definedNames");
                foreach (var name in defNames.Keys)
                {
                    writer.WriteStartElement("definedName");
                    writer.WriteAttributeString("name", defNames[name].Key);
                    writer.WriteString(string.Format("{0}!${1}${2}", name, defNames[name].Value.Column, defNames[name].Value.Row));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        public SheetDefinition[] Sheets
        {
            get { return _sheets.ToArray(); }
        }
        public SharedStringsPart SharedStrings
        {
            get;
            private set;
        }

        protected override string ContentType
        {
            get { return SpreadsheetContentTypes.Workbook; }
        }
        protected override string RelationshipType
        {
            get { return SpreadsheetRelationshipTypes.Workbook; }
        }

        public class SheetDefinition
        {
            public void Save(XmlWriter writer)
            {
                writer.WriteStartElement("sheet");
                writer.WriteAttributeString("name", Name);
                writer.WriteAttributeString("sheetId", Sheet.SheetID.ToString());
                writer.WriteAttributeString(FiftyNine.Ag.OpenXML.Common.Constants.Namespaces.Relationships.Prefix, "id", FiftyNine.Ag.OpenXML.Common.Constants.Namespaces.Relationships.Value, Relationship.ID);
                writer.WriteEndElement();
            }

            public WorksheetPart Sheet { get; internal set; }
            internal Relationship Relationship { get; set; }
            public string Name { get; set; }
            public int ID { get; private set; }
        }
    }
}
