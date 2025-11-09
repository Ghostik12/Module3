using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace ConverterXMLtoPDF
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: false);
            IConfiguration config = builder.Build();
            var myFirstClass = config.GetSection("Settings");

            // Настройка лицензии QuestPDF (для некоммерческого использования)
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            // Настройка лицензии EPPlus (для некоммерческого использования)
            ExcelPackage.License.SetNonCommercialPersonal("Ghostik");

            string excelPath = myFirstClass.GetSection("Input").Value;
            string pdfPath = myFirstClass.GetSection("Output").Value;
            int[] columnsToExport = myFirstClass.GetSection("Columns").Value.Split(' ').Select(x => int.Parse(x)).ToArray();

            var data = ReadExcel(excelPath, columnsToExport);
            GeneratePdf(data, pdfPath, columnsToExport.Select(i => (char)('A' + i)).ToArray());

            Console.WriteLine("Готово! PDF сохранён: " + pdfPath);
        }

        static List<string[]> ReadExcel(string filePath, int[] columns)
        {
            var result = new List<string[]>();

            using var package = new ExcelPackage(new FileInfo(filePath));
            var worksheet = package.Workbook.Worksheets[0]; // первый лист
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 1; row <= rowCount; row++) // с 1, если 1-я строка — заголовки
            {
                var rowData = columns.Select(col =>
                    worksheet.Cells[row, col + 1].GetValue<string>() ?? ""
                ).ToArray();

                result.Add(rowData);
            }

            return result;
        }

        static void GeneratePdf(List<string[]> data, string filePath, char[] columnHeaders)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text("Отчёт из Excel")
                        .SemiBold().FontSize(16).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(10)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var _ in columnHeaders)
                                    columns.RelativeColumn();
                            });

                            // Заголовки
                            table.Header(header =>
                            {
                                foreach (var headerText in columnHeaders)
                                {
                                    header.Cell().Border(1).Padding(5)
                                          .Background(Colors.Grey.Lighten2)
                                          .Text(headerText.ToString())
                                          .SemiBold();
                                }
                            });

                            // Данные
                            foreach (var row in data)
                            {
                                foreach (var cell in row)
                                {
                                    table.Cell().Border(1).Padding(5).Text(cell);
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Страница ");
                            x.CurrentPageNumber();
                        });
                });
            })
            .GeneratePdf(filePath);
        }
    }
}
