using OfficeOpenXml;
using QuestPDF.Infrastructure;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Converter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Лицензии
            QuestPDF.Settings.License = LicenseType.Community;
            ExcelPackage.License.SetNonCommercialPersonal("Ghostik");
        }
    }

}
