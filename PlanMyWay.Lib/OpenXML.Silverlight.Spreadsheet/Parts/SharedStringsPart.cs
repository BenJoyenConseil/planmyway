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
    public class SharedStringsPart : PackagePart
    {
        List<SharedStringDefinition> _strings = new List<SharedStringDefinition>();

        protected override void SaveContent(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("sst", SpreadsheetNamespaces.Main.Value);
            int count = Strings.Count();
            writer.WriteAttributeString("count", count.ToString());
            writer.WriteAttributeString("uniqueCount", count.ToString());

            foreach (var str in Strings)
            {
                writer.WriteStartElement("si");
                writer.WriteElementString("t", str.String);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        public SharedStringDefinition AddString(string str)
        {
            SharedStringDefinition def = _strings.FirstOrDefault(ss => ss.String == str);
            if (def == null)
            {
                def = new SharedStringDefinition() { String = str, Index = _strings.Count };
                _strings.Add(def);
            }
            return def;
        }

        public IEnumerable<SharedStringDefinition> Strings
        {
            get { return _strings; }
        }
        protected override string ContentType
        {
            get { return SpreadsheetContentTypes.SharedStrings; }
        }
        protected override string RelationshipType
        {
            get { return SpreadsheetRelationshipTypes.SharedStrings; }
        }
    }

    public class SharedStringDefinition
    {
        public int Index { get; internal set; }
        public string String { get; internal set; }
    }
}
