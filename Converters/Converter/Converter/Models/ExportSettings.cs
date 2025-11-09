

namespace Converter.Models
{
    public class ExportSettings
    {
        public string ExcelPath { get; set; } = "";
        public string PdfPath { get; set; } = "";
        public List<int> SelectedColumnIndices { get; set; } = new();
        public List<string> ColumnHeaders { get; set; } = new();
    }
}
