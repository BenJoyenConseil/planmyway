using FiftyNine.Ag.OpenXML.Common.Storage;
using PlanMyWay.Lib.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using OpenXML.Silverlight.Spreadsheet;
using OpenXML.Silverlight.Spreadsheet.Elements;
using OpenXML.Silverlight.Spreadsheet.Parts;

namespace PlanMyWay.Lib.Util
{
    public class SpreadSheetRoadmapGenerator
    {
        static Cell gasCostDefinition;
        static Cell consoDefinition;

        static public void GenerateXLS(string fileName, List<RoadMap> roadmaps, double conso, double gasCost)
        {

            Total total = new Total();
            SpreadsheetDocument doc = new SpreadsheetDocument();
            doc.ApplicationName = "PlanMyWay";
            doc.Creator = "SuperBen";
            doc.Company = "PlanMyWay & Co.";


            doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows[0].Cells[1].SetValue("Consommation moyenne du véhicule");
            doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows[1].Cells[1].SetValue("Prix du carburant");
            consoDefinition = doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows[0].Cells[3];
            gasCostDefinition = doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows[1].Cells[3];
            consoDefinition.SetValue(new Decimal(conso));
            gasCostDefinition.SetValue(new Decimal(gasCost));

            for (int i = 0; i< roadmaps.Count; i++)
            {
                if (i > 0)
                {
                    total.Distance += "+";
                    total.Conso += "+";
                    total.CoutTotal += "+";
                }
                total += SpreadSheetRoadmapGenerator.AddTableXLS(ref doc, roadmaps[i]);

            }
            SpreadSheetRoadmapGenerator.AddTotalXLS(ref doc, total);

            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            using (Stream writeFile = new IsolatedStorageFileStream(fileName, FileMode.Create, file))
            using (IStreamProvider storage = new ZipStreamProvider(writeFile))
            {
                doc.Save(storage);
            }
        }

        static private Total AddTableXLS(ref SpreadsheetDocument doc, RoadMap roadmap)
        {
            Total total = new Total();
            int countRows = doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows.Indexes.Count();
            doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows[countRows].Cells[0].SetValue(String.Empty);
            countRows = doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows.Indexes.Count();
            doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows[countRows].Cells[1].SetValue("Feuille de route du "+ roadmap.Date);

            Debug.WriteLine("tableau " + roadmap.Meetings.FirstOrDefault().DateTime.ToShortDateString());
            List<SharedStringDefinition> strList = new List<SharedStringDefinition>();
            strList.Add(doc.Workbook.SharedStrings.AddString("Départ"));
            strList.Add(doc.Workbook.SharedStrings.AddString("Arrivée"));
            strList.Add(doc.Workbook.SharedStrings.AddString("Distance(km)"));
            strList.Add(doc.Workbook.SharedStrings.AddString("Consomation(Litre)"));
            strList.Add(doc.Workbook.SharedStrings.AddString("Coût(€)"));

            countRows = doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows.Indexes.Count();
            Debug.WriteLine("countRows 1 : "+countRows);
            // Entêtes de colonne
            for (var i=0; i<strList.Count; i++)
            {
                doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows[countRows].Cells[i + 1].SetValue(strList[i]);
            }
            // 1 ligne par trip
            foreach (var trip in roadmap.Trips)
            {
                countRows = doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows.Indexes.Count();
                doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[1].SetValue(trip.Start.City);
                doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[2].SetValue(trip.End.City);
                //Consommmation d'essence en fonction de la conso moyenne du véhicule
                int distance = (int)Math.Round(trip.Distance, 0);
                Cell distanceCell =  doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[3];
                Cell ConsumptionCell = doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[4];
                distanceCell.SetValue(distance);
                //double consumption = distance * conso / 100;
                ConsumptionCell.Formula = distanceCell.CellName + "*" + consoDefinition.CellName + "/100";
                //Prix du trajet en fonction de la consommation pendant le trajet et le prix du carburant
                //double cost = consumption * gasCost;
                doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[5].Formula = ConsumptionCell.CellName + "*" + gasCostDefinition.CellName;
            }
            //Taille des colonnes
            doc.Workbook.Sheets[0].Sheet.AddColumnSizeDefinition(0, 1, 30);
            doc.Workbook.Sheets[0].Sheet.AddColumnSizeDefinition(1, 5, 22);
            //Mise à jour du prochain numéro de ligne libre
            countRows = doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows.Indexes.Count();
            Debug.WriteLine("countRows 3 : " + countRows);
            //Récupération du numéro de ligne de début et de fin du tableau courrant
            int idRowTableStart, idRowTableEnd;
            idRowTableStart = countRows - roadmap.Trips.Count;
            idRowTableEnd = countRows - 1;
            Debug.WriteLine("idRowTableStart : " + idRowTableStart);
            Debug.WriteLine("idRowTableEnd : " + idRowTableEnd);
            Row rowTop = doc.Workbook.Sheets[0].Sheet.Rows[idRowTableStart];
            Row rowBottom = doc.Workbook.Sheets[0].Sheet.Rows[idRowTableEnd];
            //// Total
            doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[1].SetValue("Total");
            doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[3].Formula = "SUM(" + rowTop.Cells[3].CellName + ":" + rowBottom.Cells[3].CellName + ")";
            doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[4].Formula = "SUM(" + rowTop.Cells[4].CellName + ":" + rowBottom.Cells[4].CellName + ")";
            doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[5].Formula = "SUM(" + rowTop.Cells[5].CellName + ":" + rowBottom.Cells[5].CellName + ")";
            //Incrémentation du total
            total.Distance += "SUM(" + rowTop.Cells[3].CellName + ":" + rowBottom.Cells[3].CellName + ")";
            total.Conso += "SUM(" + rowTop.Cells[4].CellName + ":" + rowBottom.Cells[4].CellName + ")";
            total.CoutTotal += "SUM(" + rowTop.Cells[5].CellName + ":" + rowBottom.Cells[5].CellName + ")";


            Debug.WriteLine(roadmap.Meetings.FirstOrDefault().DateTime.ToShortDateString());
            //Ajout d'un tableau
            Row rowCol = doc.Workbook.Sheets[0].Sheet.Rows[idRowTableStart - 1];
            Row rowTotal = doc.Workbook.Sheets[0].Sheet.Rows[idRowTableEnd + 1];
            Debug.WriteLine("rowCol : " + (idRowTableStart - 1));
            Debug.WriteLine("rowTotal : " + (idRowTableEnd + 1));
            TablePart table = doc.Workbook.Sheets[0].Sheet.AddTable(
                "tableau 1",
                "rapport du " + roadmap.Meetings.FirstOrDefault().DateTime.ToShortDateString(),
                rowCol.Cells[1],
                rowTotal.Cells[5]
                );
            //Précision explicite des colonnes du tableau
            for (var i = 0; i < strList.Count; i++)
            {
                table.TableColumns[i].Name = strList[i].String;
            }
            table.ShowTotalsRow = true;
            table.TableStyle.ShowColumnStripes = true;
            table.TableStyle.ShowRowStripes = true;
            return total;
        }

        static private void AddTotalXLS(ref SpreadsheetDocument doc, Total total)
        {
            int countRows = doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows.Indexes.Count();
            doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows[countRows].Cells[0].SetValue(String.Empty);
            List<SharedStringDefinition> strList = new List<SharedStringDefinition>();
            strList.Add(doc.Workbook.SharedStrings.AddString("Distance totale(km)"));
            strList.Add(doc.Workbook.SharedStrings.AddString("Consomation totale(Litre)"));
            strList.Add(doc.Workbook.SharedStrings.AddString("Coût total(€)"));

            countRows = doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows.Indexes.Count();
            // Entêtes de colonne
            for (var i = 0; i < strList.Count; i++)
            {
                doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows[countRows].Cells[i + 3].SetValue(strList[i]);
            }
            countRows = doc.Workbook.Sheets.FirstOrDefault().Sheet.Rows.Indexes.Count();
            doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[2].SetValue("Récapitulatif");
            doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[3].Formula = total.Distance;
            doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[4].Formula = total.Conso;
            doc.Workbook.Sheets[0].Sheet.Rows[countRows].Cells[5].Formula = total.CoutTotal;
        }

        class Total
        {
            String _distance = string.Empty;
            String _conso = string.Empty;
            String _coutTotal = string.Empty;

            public String CoutTotal
            {
                get { return _coutTotal; }
                set { _coutTotal = value; }
            }

            public String Conso
            {
                get { return _conso; }
                set { _conso = value; }
            }

            public String Distance
            {
                get { return _distance; }
                set { _distance = value; }
            }

            public static Total operator +(Total a, Total b)
            {
                return new Total() { Conso = a.Conso + b.Conso, Distance = a.Distance + b.Distance, CoutTotal = a.CoutTotal + b.CoutTotal };
            }
        }
    }
}
