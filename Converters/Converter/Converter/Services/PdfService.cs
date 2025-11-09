using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Converter.Services
{
    public class PdfService
    {
        public void GeneratePdf(string outputPath, string title, List<string> headers, List<string[]> data)
        {
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header()
                        .Text(title)
                        .FontSize(14)
                        .SemiBold()
                        .FontColor(Colors.Blue.Darken2);

                    page.Content()
                        .PaddingVertical(10)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var _ in headers)
                                    columns.RelativeColumn();
                            });

                            // Заголовки
                            table.Header(header =>
                            {
                                foreach (var h in headers)
                                {
                                    header.Cell()
                                          .Border(1)
                                          .Padding(4)
                                          .Background(Colors.Grey.Lighten2)
                                          .Text(h)
                                          .FontSize(10)
                                          .SemiBold();
                                }
                            });

                            // Данные
                            foreach (var row in data)
                            {
                                foreach (var cell in row)
                                {
                                    table.Cell()
                                          .Border(1)
                                          .Padding(4)
                                          .Text(cell)
                                          .FontSize(9);
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
            .GeneratePdf(outputPath);
        }
    }
}
