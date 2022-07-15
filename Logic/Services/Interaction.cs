using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.IO;

using Logic.Controllers;
using Logic.Models;

namespace Logic.Services
{
    public static class Interaction
    {
        private static string PathExport = $"{Directory.GetCurrentDirectory()}\\Export.xlsx";

        private static Application ExcelApp;
        private static Worksheet Sheet;

        public static void OpenFile(string path)
        {
            path = $"{Directory.GetCurrentDirectory()}\\Organizations\\{path}";

            if (!Directory.Exists(@path))
            {
                Directory.CreateDirectory(@path);
            }

            Process.Start("explorer", @path);
        }

        public static void Export(string path = "", Organization organization = null)
        {
            if (path == "")
                path = PathExport;

            ExcelApp = new Application();

            ExcelApp.Workbooks.Add();
            ExcelApp.Workbooks[1].Worksheets[3].Delete();
            ExcelApp.Workbooks[1].Worksheets[2].Delete();

            Sheet = ExcelApp.Workbooks[1].Worksheets[1];

            if (organization == null)
                FillSheetOrganizations();
            else
                FillSheetReports(organization);
            
            ExcelApp.Workbooks[1].SaveAs(@path);
            ExcelApp.Workbooks.Close();
            ExcelApp.Quit();
        }

        private static void FillSheetOrganizations()
        {
            int rowIndex = 1;

            foreach (var organization in OrganizationControl.Organizations)
            {
                var contacts = ContactControl.Search(organization.Id);

                foreach (var contact in contacts)
                {
                    Sheet.Cells[rowIndex, 1].Value = $"{organization.Name}";
                    Sheet.Cells[rowIndex, 2].Value = $"{organization.City}";
                    Sheet.Cells[rowIndex, 3].Value = $"{organization.INN}";
                    Sheet.Cells[rowIndex, 4].Value = $"{organization.ContractNumber}";
                    Sheet.Cells[rowIndex, 5].Value = $"{organization.ContractDate}";
                    Sheet.Cells[rowIndex, 6].Value = $"{contact.Name}";
                    Sheet.Cells[rowIndex, 7].Value = $"{contact.Post}";
                    Sheet.Cells[rowIndex, 8].Value = $"{contact.NumberPhone}";
                    Sheet.Cells[rowIndex, 9].Value = $"{contact.Email}";
                    Sheet.Cells[rowIndex, 10].Value = $"{organization.Address}";
                    Sheet.Cells[rowIndex, 11].Value = $"{organization.Website}";
                    Sheet.Cells[rowIndex, 12].Value = $"{organization.Email}";
                    Sheet.Cells[rowIndex, 13].Value = $"{organization.EDM}";

                    rowIndex++;
                }
            }
        }

        private static void FillSheetReports(Organization organization)
        {
            var reports = ReportControl.Search(organization.Id);
            int rowIndex = 1;

            foreach (var report in reports)
            {
                Sheet.Cells[rowIndex, 1].Value = $"{report.DateReport}";
                Sheet.Cells[rowIndex, 2].Value = $"{report.Information.Trim()}";

                rowIndex++;
            }
        }
    }
}
