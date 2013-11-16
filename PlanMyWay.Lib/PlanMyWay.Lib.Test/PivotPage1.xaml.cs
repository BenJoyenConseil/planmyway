using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Live;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using PlanMyWay.Html;
using System.Text;
using DarksideCookie.OXML.Word.Elements;
using DarksideCookie.OXML.Word;
using DarksideCookie.OXML.Packaging;
using OpenXML.Silverlight.Spreadsheet;
using OpenXML.Silverlight.Spreadsheet.Parts;
using FiftyNine.Ag.OpenXML.Common.Storage;
using PlanMyWay.Lib.Util;
using PlanMyWay.Lib.DataModel;
using PlanMyWay.Lib.Manager;
using Microsoft.Phone.Controls.Maps.Platform;

namespace PlanMyWay.Lib.Test
{
    public partial class PivotPage1 : PhoneApplicationPage
    {
        IsolatedStorageFile myIsolatedStorage;
        IsolatedStorageFileStream fileStream;
        StreamReader reader;

        public PivotPage1()
        {
            InitializeComponent();
        }

        private void signInButton_SessionChanged_1(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Status != LiveConnectSessionStatus.Connected)
                return;

            IRoadMapManager manager = new RoadMapManager();

            manager.AllRoadMapsReceived += (o, eventAllRoadMaps) =>
            {
                if (eventAllRoadMaps.RoadMaps.Count  == 0)
                {
                    Debug.WriteLine("Aucun rendez-vous n'existe pour les jours sélectionnés");
                    return;
                }

                
                //SaveTableur();
                SpreadSheetRoadmapGenerator.GenerateXLS("rapport-PlanMyWay.xlsx", eventAllRoadMaps.RoadMaps, 5.5f, 1.6f);
                if (e.Session != null && e.Status == LiveConnectSessionStatus.Connected)
                {
                    myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
                    fileStream = myIsolatedStorage.OpenFile("rapport-PlanMyWay.xlsx", FileMode.Open, FileAccess.Read);
                    reader = new StreamReader(fileStream);
                    App.Session = e.Session;
                    LiveConnectClient client = new LiveConnectClient(e.Session);
                    client.UploadCompleted += client_UploadCompleted;
                    client.UploadAsync("me/skydrive", "rapport-PlanMyWay--2.xlsx", reader.BaseStream, OverwriteOption.Overwrite);

                }
            };
            ReferenceMeeting start = new ReferenceMeeting(new DateTime(2013, 1, 2, 8, 30, 0), new Location()
            {
                Latitude = 48.85693,
                Longitude = 2.3412
            }) { City = "Paris", Subject = "Start" };
            ReferenceMeeting end = start;
            end.Subject = "End";

            manager.GetAllRoadMapsAsync(new DateTime(2013, 1, 2), new DateTime(2013, 1, 4), start, end);

        }

        private void saveFile()
        {
            Element htmlFile = new Element();

            htmlFile.Tag = "html";
            Element head = new Element("head");
            head.Add(new Element("title") { Text = "Test html title" });
            Element body = new Element("body");
            body.Add(new Element("p") { Text = "<span color=\"Red\">Salut comment ça va??</span>" });
            htmlFile.Add(head);
            htmlFile.Add(body);
            try
            {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
                using (Stream writeFile = new IsolatedStorageFileStream("rapport-PlanMyWay.xls", FileMode.Create, file))
                {
                    //writeFile.Write(htmlFile.ToString());
                    //writeFile.Flush();
                    //writeFile.Close();
                    //file.Dispose();
                    //writeFile.Dispose();

                    ExcelWriter writer = new ExcelWriter(writeFile);
                    writer.BeginWrite();
                    writer.WriteCell(0, 0, "ExcelWriter Demo");
                    writer.WriteCell(1, 0, "int");
                    writer.WriteCell(1, 1, 10);
                    writer.WriteCell(2, 0, "double");
                    writer.WriteCell(2, 1, 1.5);
                    writer.WriteCell(3, 0, "empty");
                    writer.WriteCell(3, 1);
                    writer.EndWrite();
                    writeFile.Close();
                }
            }
            catch (Exception e)
            {
            }
        }

        private void SaveTableur()
        {
            SpreadsheetDocument doc = new SpreadsheetDocument();
            doc.ApplicationName = "PlanMyWay";
            doc.Creator = "SuperBen";
            doc.Company = "PlanMyWay & Co.";

            

            SharedStringDefinition str1 = doc.Workbook.SharedStrings.AddString("Column 1");
            SharedStringDefinition str2 = doc.Workbook.SharedStrings.AddString("Column 2");
            SharedStringDefinition str3 = doc.Workbook.SharedStrings.AddString("Column 3");

            doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[0].SetValue(str1);
            doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[1].SetValue(str2);
            doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[2].SetValue(str3);

            doc.Workbook.Sheets[0].Sheet.Rows[1].Cells[0].SetValue("Value 1");
            doc.Workbook.Sheets[0].Sheet.Rows[1].Cells[1].SetValue(1);
            doc.Workbook.Sheets[0].Sheet.Rows[1].Cells[2].SetValue(1001);

            doc.Workbook.Sheets[0].Sheet.Rows[2].Cells[0].SetValue("Value 2");
            doc.Workbook.Sheets[0].Sheet.Rows[2].Cells[1].SetValue(2);
            doc.Workbook.Sheets[0].Sheet.Rows[2].Cells[2].SetValue(1002);

            doc.Workbook.Sheets[0].Sheet.Rows[3].Cells[0].SetValue("Value 3");
            doc.Workbook.Sheets[0].Sheet.Rows[3].Cells[1].SetValue(3);
            doc.Workbook.Sheets[0].Sheet.Rows[3].Cells[2].SetValue(1003);

            doc.Workbook.Sheets[0].Sheet.Rows[4].Cells[0].SetValue("Value 4");
            doc.Workbook.Sheets[0].Sheet.Rows[4].Cells[1].SetValue(4);
            doc.Workbook.Sheets[0].Sheet.Rows[4].Cells[2].SetValue(1004);

            TablePart table = doc.Workbook.Sheets[0].Sheet.AddTable("My Table", "My Table", doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[0], doc.Workbook.Sheets[0].Sheet.Rows[4].Cells[2]);
            table.TableColumns[0].Name = str1.String;
            table.TableColumns[1].Name = str2.String;
            table.TableColumns[2].Name = str3.String;
            doc.Workbook.Sheets[0].Sheet.AddColumnSizeDefinition(0, 2, 20);

            doc.Workbook.Sheets[0].Sheet.Rows[5].Cells[1].SetValue("Sum:");
            doc.Workbook.Sheets[0].Sheet.Rows[5].Cells[2].Formula = "SUM(" + doc.Workbook.Sheets[0].Sheet.Rows[1].Cells[2].CellName + ":" + doc.Workbook.Sheets[0].Sheet.Rows[4].Cells[2].CellName + ")";

            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            using (Stream writeFile = new IsolatedStorageFileStream("rapport-PlanMyWay.xlsx", FileMode.Create, file))
            using (IStreamProvider storage = new ZipStreamProvider(writeFile))
            {
                doc.Save(storage);
            }
        }

        private void SaveDoc()
        {
            WordDocument doc = new WordDocument();
            doc.ApplicationName = "SilverWord";
            doc.Creator = "Chris Klug";
            doc.Company = "59NORTH";

            doc.FontTable.CreateElement<FontDefinition>();
            FontDefinition fontDefinition = doc.FontTable.CreateElement<FontDefinition>();
            fontDefinition.Name = "Calibri";
            fontDefinition.Panose1 = "020F0502020204030204";
            fontDefinition.CharSet = "00";
            fontDefinition.Family = FontFamilyEnumeration.Swiss;
            fontDefinition.Pitch = FontPitchEnumeration.Variable;
            fontDefinition.Signature.UnicodeSignature0 = "E00002FF";
            fontDefinition.Signature.CodePageSignature0 = "0000019F";
            FontReference calibri = doc.FontTable.AddFont(fontDefinition);

            fontDefinition = doc.FontTable.CreateElement<FontDefinition>();
            fontDefinition.Name = "Times New Roman";
            fontDefinition.Panose1 = "02020603050405020304";
            fontDefinition.CharSet = "00";
            fontDefinition.Family = FontFamilyEnumeration.Roman;
            fontDefinition.Pitch = FontPitchEnumeration.Variable;
            fontDefinition.Signature.UnicodeSignature0 = "E0002AFF";
            fontDefinition.Signature.CodePageSignature0 = "000001FF";
            FontReference timesNewRoman = doc.FontTable.AddFont(fontDefinition);

            CharacterStyle style = doc.Styles.CreateElement<CharacterStyle>();
            style.Id = "TitleStyle";
            style.Name = "Title Style";
            style.RunProperties.FontSize = 60;
            style.RunProperties.IsBold = true;
            style.RunProperties.Font.ComplexScript = timesNewRoman;
            style.RunProperties.Font.HighAnsi = timesNewRoman;
            style.RunProperties.Font.ASCII = timesNewRoman;
            style.RunProperties.Font.EastAsia = timesNewRoman;
            doc.Styles.AddStyle(style);

            DarksideCookie.OXML.Word.Elements.Run run =
                doc.Document.CreateElement<DarksideCookie.OXML.Word.Elements.Run>();
            Text t = doc.Document.CreateElement<Text>();
            t.Content = Title.Text;
            run.Content.Add(t);
            run.Properties.Style = style;
            doc.Document.Sections[0].Paragraphs[0].Runs.Add(run);

            DarksideCookie.OXML.Word.Elements.Paragraph p;
            string[] paragraphs = this.Text.Text.Split(new string[] { "\r\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var paragraph in paragraphs)
            {
                string[] lines = paragraph.Split(new string[] { "\r" }, StringSplitOptions.RemoveEmptyEntries);
                p = doc.Document.CreateElement<DarksideCookie.OXML.Word.Elements.Paragraph>();
                run = doc.Document.CreateElement<DarksideCookie.OXML.Word.Elements.Run>();
                run.Properties.Font.HighAnsi = calibri;
                run.Properties.Font.ComplexScript = calibri;
                run.Properties.Font.ASCII = calibri;
                run.Properties.Font.EastAsia = calibri;
                for (int i = 0; i < lines.Length; i++)
                {
                    t = doc.Document.CreateElement<Text>();
                    t.Content = lines[i];
                    run.Content.Add(t);
                    if (i < lines.Length - 1)
                    {
                        run.Content.Add(doc.Document.CreateElement<Break>());
                    }
                }
                p.Runs.Add(run);
                doc.Document.Sections[0].Paragraphs.Add(p);

                DarksideCookie.OXML.Word.Elements.Section section = doc.Document.CreateElement<DarksideCookie.OXML.Word.Elements.Section>();
                section.Paragraphs.Add(p);
                doc.Document.Sections.Add(section);

            }

            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            using (Stream writeFile = new IsolatedStorageFileStream("rapport-PlanMyWay.docx", FileMode.Create, file))
            using (IStreamStorage storage = new ZipStreamStorage(writeFile))
            {
                doc.Save(storage);
            }
        }

        void client_UploadCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            Debug.WriteLine("Upload File completed!");
            myIsolatedStorage.Dispose();
            fileStream.Dispose();
            reader.Dispose();
        }
    }
}