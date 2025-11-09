using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Converter.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;

namespace Converter.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ExcelService _excelService = new();
        private readonly PdfService _pdfService = new();
        private readonly ConverterService _converter = new();

        // === Форматы ===
        public ObservableCollection<string> SourceFormats { get; } = new() { "Excel", "PDF", "XML", "CSV", "Word" };
        public ObservableCollection<string> TargetFormats { get; } = new() { "PDF", "Excel", "XML", "CSV", "Word" };

        [ObservableProperty] private string _sourceFormat = "Excel";
        [ObservableProperty] private string _targetFormat = "PDF";

        // === Пути ===
        [ObservableProperty] private string _sourcePath = "";
        [ObservableProperty] private string _targetPath = "";

        // === Превью ===
        [ObservableProperty] private DataView _previewData = new DataTable().DefaultView;
        [ObservableProperty] private ObservableCollection<ColumnItem> _columns = new();
        [ObservableProperty] private string _conversionStatus = "";

        // === Включение UI ===
        public bool IsColumnSelectionEnabled => SourceFormat == "Excel" && TargetFormat == "PDF";
        public bool CanConvertPair => IsSupportedConversion(SourceFormat, TargetFormat);

        public MainViewModel()
        {
            BrowseSourceCommand = new RelayCommand(BrowseSource);
            BrowseTargetCommand = new RelayCommand(BrowseTarget);
            ConvertCommand = new RelayCommand(Convert, CanConvert);

            // Реакция на смену формата
            OnSourceFormatChanged(SourceFormat);
            OnTargetFormatChanged(TargetFormat);
            UpdateConversionStatus();
        }

        public IRelayCommand BrowseSourceCommand { get; }
        public IRelayCommand BrowseTargetCommand { get; }
        public IRelayCommand ConvertCommand { get; }


        private void BrowseSource()
        {
            var filter = GetFileFilter(SourceFormat, isOpen: true);
            var ofd = new OpenFileDialog
            {
                Filter = filter,
                Title = $"Выберите {SourceFormat} файл"
            };

            if (ofd.ShowDialog() == true)
            {
                SourcePath = ofd.FileName;
                LoadPreviewIfNeeded();
            }
        }

        private void BrowseTarget()
        {
            var filter = GetFileFilter(TargetFormat, isOpen: false);
            var ext = GetDefaultExtension(TargetFormat);
            var sfd = new SaveFileDialog
            {
                Filter = filter,
                FileName = $"output{ext}",
                Title = $"Сохранить как {TargetFormat}"
            };

            if (sfd.ShowDialog() == true)
            {
                TargetPath = sfd.FileName;
            }
        }

        private string GetFileFilter(string format, bool isOpen)
        {
            return format switch
            {
                "Excel" => "Excel Files|*.xlsx;*.xls",
                "PDF" => "PDF Files|*.pdf",
                "XML" => "XML Files|*.xml",
                "CSV" => "CSV Files|*.csv",
                "Word" => "Word Files|*.docx;*.doc",
                _ => "All Files|*.*"
            };
        }

        private string GetDefaultExtension(string format)
        {
            return format switch
            {
                "Excel" => ".xlsx",
                "PDF" => ".pdf",
                "XML" => ".xml",
                "CSV" => ".csv",
                "Word" => ".docx",
                _ => ""
            };
        }

        private void LoadPreviewIfNeeded()
        {
            if (SourceFormat == "Excel" && File.Exists(SourcePath))
            {
                try
                {
                    var headers = _excelService.GetHeaders(SourcePath);
                    Columns.Clear();
                    foreach (var h in headers)
                        Columns.Add(new ColumnItem { Name = h, IsSelected = true });

                    var dataTable = _excelService.LoadPreview(SourcePath, 10);
                    PreviewData = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка чтения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Columns.Clear();
                PreviewData = new DataTable().DefaultView;
            }
        }

        private void Convert()
        {
            if (!CanConvertPair)
            {
                MessageBox.Show(
                    $"Конвертация из {SourceFormat} в {TargetFormat} пока не поддерживается.\n\n" +
                    "Скоро добавим!",
                    "Недоступно",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            try
            {
                _converter.Convert(SourcePath, TargetPath, SourceFormat, TargetFormat);
                MessageBox.Show($"Файл успешно конвертирован!\n{TargetPath}", "Готово!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanConvert() =>
            !string.IsNullOrEmpty(SourcePath) &&
            !string.IsNullOrEmpty(TargetPath) &&
            CanConvertPair &&
            (SourceFormat != "Excel" || TargetFormat != "PDF" || Columns.Any(c => c.IsSelected));

        // === Реакция на смену ===
        partial void OnSourceFormatChanged(string value)
        {
            SourcePath = "";
            LoadPreviewIfNeeded();
            UpdateConversionStatus(); // ← вместо OnPropertyChanged
        }
        partial void OnTargetFormatChanged(string value)
        {
            TargetPath = "";
            UpdateConversionStatus(); // ← вместо OnPropertyChanged
        }

        private void UpdateConversionStatus()
        {
            if (CanConvertPair)
            {
                ConversionStatus = "";
            }
            else
            {
                ConversionStatus = $"Конвертация {SourceFormat} → {TargetFormat} пока не поддерживается";
            }

            // Обновляем кнопку
            ConvertCommand.NotifyCanExecuteChanged();
        }

        private static bool IsSupportedConversion(string from, string to)
        {
            var supported = new HashSet<(string, string)>
            {
                // Excel
                ("Excel", "PDF"), ("Excel", "XML"), ("Excel", "CSV"), ("Excel", "Word"),
                // CSV
                ("CSV", "Excel"), ("CSV", "XML"),
                // XML
                ("XML", "Excel"), ("XML", "CSV"),
                // PDF
                ("PDF", "Excel"),
                // Word
                ("Word", "Excel")
            };

            return supported.Contains((from, to));
        }

        partial void OnSourcePathChanged(string value) => ConvertCommand.NotifyCanExecuteChanged();
        partial void OnTargetPathChanged(string value) => ConvertCommand.NotifyCanExecuteChanged();
    }

    public partial class ColumnItem : ObservableObject
    {
        public string Name { get; set; } = "";
        [ObservableProperty] private bool _isSelected;
    }
}