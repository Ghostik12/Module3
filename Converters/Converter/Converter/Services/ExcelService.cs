using OfficeOpenXml;
using System.Data;
using System.IO;

namespace Converter.Services
{
    public class ExcelService
    {
        public DataTable LoadPreview(string filePath, int maxRows = 10)
        {
            var dataTable = new DataTable();

            using var package = new ExcelPackage(new FileInfo(filePath));
            var ws = package.Workbook.Worksheets[0];

            // Заголовки
            int colCount = ws.Dimension.Columns;
            for (int c = 1; c <= colCount; c++)
            {
                string header = ws.Cells[1, c].GetValue<string>() ?? $"Столбец {c}";
                dataTable.Columns.Add(header);
            }

            // Данные
            int rowCount = Math.Min(ws.Dimension.Rows, maxRows + 1); // +1 заголовок
            for (int r = 2; r <= rowCount; r++)
            {
                var row = dataTable.NewRow();
                for (int c = 1; c <= colCount; c++)
                {
                    row[c - 1] = ws.Cells[r, c].GetValue<string>() ?? "";
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public List<string[]> GetSelectedRows(string filePath, List<int> selectedCols, int startRow = 2)
        {
            var result = new List<string[]>();

            using var package = new ExcelPackage(new FileInfo(filePath));
            var ws = package.Workbook.Worksheets[0];
            int rowCount = ws.Dimension.Rows;

            for (int r = startRow; r <= rowCount; r++)
            {
                var row = selectedCols.Select(colIdx =>
                    ws.Cells[r, colIdx + 1].GetValue<string>() ?? ""
                ).ToArray();
                result.Add(row);
            }

            return result;
        }

        public List<string> GetHeaders(string filePath)
        {
            using var package = new ExcelPackage(new FileInfo(filePath));
            var ws = package.Workbook.Worksheets[0];
            var headers = new List<string>();

            int colCount = ws.Dimension.Columns;
            for (int c = 1; c <= colCount; c++)
            {
                headers.Add(ws.Cells[1, c].GetValue<string>() ?? $"Столбец {c}");
            }

            return headers;
        }
    }
}
