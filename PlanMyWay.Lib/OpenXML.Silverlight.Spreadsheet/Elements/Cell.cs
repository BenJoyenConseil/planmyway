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
using System.Xml;
using OpenXML.Silverlight.Spreadsheet.Parts;
using OpenXML.Silverlight.Spreadsheet.Utilities;

namespace OpenXML.Silverlight.Spreadsheet.Elements
{
    public class Cell : IndexedSpreadsheetElement
    {
        protected override void Initialize()
        {
            Type = CellValueType.Number;
        }

        public void SetValue(SharedStringDefinition sharedString)
        {
            Value = sharedString;
            Type = CellValueType.SharedString;
        }
        public void SetValue(decimal value)
        {
            Value = value;
            Type = CellValueType.Number;
        }
        public void SetValue(string str)
        {
            Value = str;
            Type = CellValueType.InlineString;
        }
        public void SetValue(Exception ex)
        {
            Value = ex.Message;
            Type = CellValueType.Error;
        }
        public void SetValue(DateTime date)
        {
            Value = date;
            Type = CellValueType.Date;
        }
        public void SetValue(bool value)
        {
            Value = value;
            Type = CellValueType.Boolean;
        }

        protected override void SaveContent(XmlWriter writer)
        {
            writer.WriteAttributeString("r", CellName);
            if (!string.IsNullOrEmpty(Formula))
            {
                writer.WriteElementString("f", Formula);
                if (Value == null)
                {
                    Type = CellValueType.Number;
                    Value = 0;
                }
            }
            if (Value == null)
            {
                return;
            }

            switch (Type)
            {
                case CellValueType.SharedString:
                    writer.WriteAttributeString("t", "s");
                    writer.WriteElementString("v", ((SharedStringDefinition)Value).Index.ToString());
                    break;
                case CellValueType.Number:
                    writer.WriteElementString("v", Value.ToString().Replace(",", "."));
                    break;
                case CellValueType.InlineString:
                    writer.WriteAttributeString("t", "inlineStr");
                    writer.WriteStartElement("is");
                    writer.WriteElementString("t", (string)Value);
                    writer.WriteEndElement();
                    break;
                case CellValueType.Error:
                    writer.WriteAttributeString("t", "e");
                    writer.WriteElementString("v", (string)Value);
                    break;
                case CellValueType.Date:
                    writer.WriteAttributeString("t", "1");
                    DateTime d = (DateTime)Value;
                    double val = ConvertDateToJulian(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
                    writer.WriteElementString("v", ((int)val).ToString());
                    break;
                case CellValueType.Boolean:
                    writer.WriteAttributeString("t", "b");
                    writer.WriteElementString("v", (bool)Value ? "1" : "0");
                    break;
            }
        }

        private double ConvertDateToJulian(int y, int m, int d, int h, int mn, int s)
        {
            double jy;
            double ja;
            double jm;

            if (m > 2)
            {
                jy = y;
                jm = m + 1;
            }
            else
            {
                jy = y - 1;
                jm = m + 13;
            }

            double intgr = Math.Floor(Math.Floor(365.25 * jy) + Math.Floor(30.6001 * jm) + d + 1720995);

            //check for switch to Gregorian calendar   
            int gregcal = 15 + 31 * (10 + 12 * 1582);
            if (d + 31 * (m + 12 * y) >= gregcal)
            {
                ja = Math.Floor(0.01 * jy);
                intgr += 2 - ja + Math.Floor(0.25 * ja);
            }

            //correct for half-day offset   
            double dayfrac = h / 24.0 - 0.5;
            if (dayfrac < 0.0)
            {
                dayfrac += 1.0;
                --intgr;
            }

            //now set the fraction of a day   
            double frac = dayfrac + (mn + s / 60.0) / 60.0 / 24.0;

            //round to nearest second   
            double jd0 = (intgr + frac) * 100000;
            double jd = Math.Floor(jd0);
            if (jd0 - jd > 0.5) ++jd;

            return jd / 100000;
        }  

        internal int RowIndex
        {
            get;
            set;
        }
        internal int ColumnIndex
        {
            get { return Index; }
        }
        public string CellName
        {
            get { return string.Format("{0}{1}", (char)(ColumnIndex + 65), RowIndex + 1); }
        }
        public CellValueType Type
        {
            get;
            private set;
        }
        public object Value
        {
            get;
            private set;
        }
        public string Formula
        {
            get;
            set;
        }

        protected override string NodeName
        {
            get { return "c"; }
        }
    }

    public enum CellValueType
    {
        Boolean,
        Date,
        Error,
        InlineString,
        Number,
        SharedString
    }
}
