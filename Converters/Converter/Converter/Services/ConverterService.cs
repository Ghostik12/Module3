using CsvHelper;
using CsvHelper.Configuration;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml;
using System.Data;
using System.Globalization;
using System.IO;
using UglyToad.PdfPig;

namespace Converter.Services
{
    public class ConverterService
    {
        private readonly ExcelService _excelService = new();
        private readonly PdfService _pdfService = new();

        public void Convert(string sourcePath, string targetPath, string sourceFormat, string targetFormat)
        {
            // Читаем данные в универсальный формат
            var data = ReadSource(sourcePath, sourceFormat);

            // Записываем в целевой формат
            WriteTarget(data, targetPath, targetFormat, Path.GetFileName(sourcePath));
        }

        private DataTable ReadSource(string path, string format)
        {
            return format switch
            {
                "Excel" => ReadExcel(path),
                "CSV" => ReadCsv(path),
                "XML" => ReadXml(path),
                "PDF" => ReadPdf(path),
                "Word" => ReadWord(path),
                _ => throw new NotSupportedException($"Формат {format} не поддерживается для чтения")
            };
        }

        private void WriteTarget(DataTable data, string path, string format, string title)
        {
            switch (format)
            {
                case "PDF": _pdfService.GeneratePdf(path, title, GetHeaders(data), GetRows(data)); break;
                case "Excel": WriteExcel(data, path); break;
                case "XML": WriteXml(data, path); break;
                case "CSV": WriteCsv(data, path); break;
                case "Word": WriteWord(data, path, title); break;
                default: throw new NotSupportedException($"Формат {format} не поддерживается для записи");
            }
        }

        // === ЧТЕНИЕ ===
        private DataTable ReadExcel(string path)
        {
            var dt = new DataTable();
            using var package = new ExcelPackage(new FileInfo(path));
            var ws = package.Workbook.Worksheets[0];
            foreach (var cell in ws.Cells[1, 1, 1, ws.Dimension.Columns])
                dt.Columns.Add(cell.GetValue<string>() ?? $"Column{cell.Start.Column}");
            for (int r = 2; r <= ws.Dimension.Rows; r++)
            {
                var row = dt.NewRow();
                for (int c = 1; c <= ws.Dimension.Columns; c++)
                    row[c - 1] = ws.Cells[r, c].GetValue<string>() ?? "";
                dt.Rows.Add(row);
            }
            return dt;
        }

        private DataTable ReadCsv(string path)
        {
            var dt = new DataTable();
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true });
            using var dr = new CsvDataReader(csv);
            dt.Load(dr);
            return dt;
        }

        private DataTable ReadXml(string path)
        {
            var dt = new DataTable();
            dt.ReadXml(path);
            return dt;
        }

        private DataTable ReadPdf(string path)
        {
            var dt = new DataTable();
            dt.Columns.Add("Text");
            using var doc = PdfDocument.Open(path);
            foreach (var page in doc.GetPages())
            {
                var text = page.Text;
                foreach (var line in text.Split('\n'))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        dt.Rows.Add(line.Trim());
                }
            }
            return dt;
        }

        private DataTable ReadWord(string path)
        {
            var dt = new DataTable();
            dt.Columns.Add("Paragraph");
            using var doc = WordprocessingDocument.Open(path, false);
            var body = doc.MainDocumentPart?.Document.Body;
            foreach (var para in body?.Elements<Paragraph>() ?? [])
            {
                var text = para.InnerText;
                if (!string.IsNullOrWhiteSpace(text))
                    dt.Rows.Add(text.Trim());
            }
            return dt;
        }

        // === ЗАПИСЬ ===
        private void WriteExcel(DataTable data, string path)
        {
            using var package = new ExcelPackage(new FileInfo(path));
            var ws = package.Workbook.Worksheets.Add("Sheet1");
            for (int c = 0; c < data.Columns.Count; c++)
                ws.Cells[1, c + 1].Value = data.Columns[c].ColumnName;
            for (int r = 0; r < data.Rows.Count; r++)
                for (int c = 0; c < data.Columns.Count; c++)
                    ws.Cells[r + 2, c + 1].Value = data.Rows[r][c];
            package.Save();
        }

        private void WriteCsv(DataTable data, string path)
        {
            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            foreach (DataColumn col in data.Columns)
                csv.WriteField(col.ColumnName);
            csv.NextRecord();
            foreach (DataRow row in data.Rows)
            {
                for (int i = 0; i < data.Columns.Count; i++)
                    csv.WriteField(row[i]);
                csv.NextRecord();
            }
        }

        private void WriteXml(DataTable data, string path)
        {
            data.WriteXml(path, XmlWriteMode.WriteSchema);
        }

        private void WriteWord(DataTable data, string path, string title)
        {
            using var doc = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document);
            var mainPart = doc.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = mainPart.Document.AppendChild(new Body());

            var titlePara = body.AppendChild(new Paragraph());
            var titleRun = titlePara.AppendChild(new Run());
            titleRun.AppendChild(new Text(title) { Space = SpaceProcessingModeValues.Preserve });
            titleRun.RunProperties = new RunProperties { Bold = new Bold() };

            foreach (DataRow row in data.Rows)
            {
                var para = body.AppendChild(new Paragraph());
                var run = para.AppendChild(new Run());
                run.AppendChild(new Text(string.Join(" | ", row.ItemArray)));
            }

            mainPart.Document.Save();
        }

        // === Вспомогательные ===
        private List<string> GetHeaders(DataTable dt) => dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
        private List<string[]> GetRows(DataTable dt) => dt.Rows.Cast<DataRow>().Select(r => r.ItemArray.Select(o => o?.ToString() ?? "").ToArray()).ToList();
    }
}
