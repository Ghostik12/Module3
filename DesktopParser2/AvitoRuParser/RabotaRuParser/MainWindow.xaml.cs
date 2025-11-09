using HtmlAgilityPack;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using AvitoRuParser.Data;
using AvitoRuParser.Models;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.IO;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using AvitoRuParser.Services;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AvitoRuParser
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient  httpClient;
        private readonly AppDbContext dbContext;
        private CancellationTokenSource cts; // Для остановки парсинга
        private CancellationTokenSource autoCts;
        private bool isAutoParsing = false;
        private int currentPage = 1;
        private int pageSize = 100; // Значение по умолчанию (соответствует SelectedIndex="1")
        private int totalItems = 0;
        private int totalPages = 1;
        private bool isInitialized = false; // Флаг инициализации
        private readonly Dictionary<string, string> cityToSubdomainMap;
        private bool onlyWithPhones = false; // Переменная для хранения состояния флага
        private readonly BlacklistService _blacklistService;

        private readonly WindowsVpnManager _vpnManager = new();
        private bool _isVpnConnected;

        private readonly DbContextOptions<AppDbContext> _dbOptions;

        private const string AvitoKey = "af0deccbgcgidddjgnvljitntccdduijhdinfgjgfjir";
        private const string AvitoCookie = "__cfduid=da6b6b5b9f01fd022f219ed53ac3935791610912291; sessid=ef757cc130c5cd228be88e869369c654.1610912291; _ga=GA1.2.559434019.1610912292; _gid=GA1.2.381990959.1610912292; _fbp=fb.1.1610912292358.1831979940; u=2oiycodt.1oaavs8.dyu0a4x7fxw0; v=1610912321; buyer_laas_location=641780; buyer_location_id=641780; luri=novosibirsk; buyer_selected_search_radius4=0_general; buyer_local_priority_v2=0; sx=H4sIAAAAAAACAxXLQQqAIBAF0Lv8dYvRLEdvU0MIBU0iKCHePXr71z7Gfefd1W5RLYick2kSakiB2VETclpf85n19RJMSp4vJOSlM%2F2BMOBDNaigE9taM8QH0oydNVAAAAA%3D%3D; dfp_group=100; _ym_uid=1610912323905107257; _ym_d=1610912323; _ym_visorc_34241905=b; _ym_isad=2; _ym_visorc_419506=w; _ym_visorc_188382=w; __gads=ID=2cff056a4e50a953-22d0341a94b900a6:T=1610912323:S=ALNI_MZMbOe0285QjW7EVvsYtSa-RA_Vpg; f=5.8696cbce96d2947c36b4dd61b04726f1a816010d61a371dda816010d61a371dda816010d61a371dda816010d61a371ddbb0992c943830ce0bb0992c943830ce0bb0992c943830ce0a816010d61a371dd2668c76b1faaa358c08fe24d747f54dc0df103df0c26013a0df103df0c26013a2ebf3cb6fd35a0ac0df103df0c26013a8b1472fe2f9ba6b978e38434be2a23fac7b9c4258fe3658d831064c92d93c3903815369ae2d1a81d04dbcad294c152cb0df103df0c26013a20f3d16ad0b1c5462da10fb74cac1eab2da10fb74cac1eab3c02ea8f64acc0bdf0c77052689da50d2da10fb74cac1eab2da10fb74cac1eab2da10fb74cac1eab2da10fb74cac1eab91e52da22a560f5503c77801b122405c48ab0bfc8423929a6d7a5083cc1669877def5708993e2ca678f1dc04f891d61e35b0929bad7c1ea5dec762b46b6afe81f200c638bc3d18ce60768b50dd5e12c30e37135e8f7c6b64dc9f90003c0354a346b8ae4e81acb9fa46b8ae4e81acb9fa02c68186b443a7acf8b817f3dc0c3f21c1eac53cc61955882da10fb74cac1eab2da10fb74cac1eab5e5aa47e7d07c0f95e1e792141febc9cb841da6c7dc79d0b";

        public MainWindow()
        {
            InitializeComponent();
            httpClient = new HttpClient();
            ConfigureAvitoHttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36");
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AvitoParser.db");
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            dbContext = new AppDbContext(_dbOptions);
            dbContext.Database.EnsureCreated();
            // Инициализация маппинга городов
            cityToSubdomainMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Москва", "msk" },
                { "Санкт-Петербург", "spb" },
                { "Нижний Новгород", "nizhegorodskaya_oblast" },
                { "Екатеринбург", "eburg" },
                { "Казань", "kazan" },
                { "Новосибирск", "nsk" },
                { "Самара", "samara" },
                { "Челябинск", "chel" },
                { "Абакан", "abakan" },
                { "Анадырь", "anadyr" },
                { "Архангельск", "arkhangelsk" },
                { "Астрахань", "astrakhan" },
                { "Барнаул", "barnaul" },
                { "Белгород", "belgorod" },
                { "Биробиджан", "birobidjan" },
                { "Брянск", "bryansk" },
                { "Великий Новгород", "novgorod" },
                { "Владивосток", "vladivostok" },
                { "Владикавказ", "vladikavkaz" },
                { "Владимир", "vladimir" },
                { "Волгоград", "volgograd" },
                { "Воронеж", "voronezh" },
                { "Грозный", "grozniy" },
                { "Иваново", "ivanovo" },
                { "Ижевск", "izhevsk" },
                { "Иркутск", "irkutsk" },
                { "Йошкар-Ола", "yola" },
                { "Калининград", "kaliningrad" },
                { "Калуга", "kaluga" },
                { "Кемерово", "kemerovo" },
                { "Киров", "kirov" },
                { "Королев", "korolev" },
                { "Кострома", "kostroma" },
                { "Краснодар", "krasnodar" },
                { "Красноярск", "krasnoyarsk" },
                { "Курган", "kurgan" },
                { "Курск", "kursk" },
                { "Кызыл", "kyzyl" },
                { "Липецк", "lipetsk" },
                { "Магадан", "magadan" },
                { "Магнитогорск", "magnitogorsk" },
                { "Махачкала", "mahachkala" },
                { "Омск", "omsk" },
                { "Пермь", "perm" },
                { "Ростов-на-Дону", "rostov" },
            };
            vpnRotateAllCheckBox.Checked += VpnRotateCheckBox_Changed;
            vpnRotateAllCheckBox.Unchecked += VpnRotateCheckBox_Changed;
            vpnSelectCountryCheckBox.Checked += VpnCountryCheckBox_Changed;
            vpnSelectCountryCheckBox.Unchecked += VpnCountryCheckBox_Changed;
            LoadVpnSettings();
            _blacklistService = new BlacklistService(dbContext);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing; // Добавляем обработчик закрытия окна
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            isInitialized = true;

            pageSize = 100;
            LoadVacancies();

            if (dateToPicker != null)
            {
                dateToPicker.SelectedDate = DateTime.Today;
            }

            if (CityComboBox.Items.Count > 0)
            {
                CityComboBox.SelectedIndex = 0; // Выбираем первый город (например, "Москва")
            }

            // Загружаем сохранённые настройки
            onlyWithPhones = Properties.Settings.Default.OnlyWithPhones;
            OnlyWithPhonesCheckBox.IsChecked = onlyWithPhones;

            isAutoParsing = Properties.Settings.Default.AutoParseEnabled;
            if (autoParseCheckBox != null) // Если у тебя есть AutoParseCheckBox
            {
                autoParseCheckBox.IsChecked = isAutoParsing;
            }

            // Загружаем сохранённые значения для TextBox
            VacancyTitleTextBox.Text = Properties.Settings.Default.VacancyTitle;
            waitTimeTextBox.Text = Properties.Settings.Default.WaitTime;
            parseTimeTextBox.Text = Properties.Settings.Default.ParseTime;

            // Автоподключение VPN если было сохранено
            if (Properties.Settings.Default.VpnEnabled)
            {
                string countryCode = "US"; // Значение по умолчанию

                if (Properties.Settings.Default.VpnRotate)
                {
                    // Ротация - выбираем случайную страну
                    string[] countries = { "US", "CN", "JP", "RC", "RM" };
                    countryCode = countries[new Random().Next(0, countries.Length)];
                }
                else if (Properties.Settings.Default.VpnCountryIndex >= 0 &&
                         Properties.Settings.Default.VpnCountryIndex < vpnCountryComboBox.Items.Count)
                {
                    // Конкретная страна из сохраненных настроек
                    var selectedItem = vpnCountryComboBox.Items[Properties.Settings.Default.VpnCountryIndex] as ComboBoxItem;
                    countryCode = selectedItem?.Tag?.ToString() ?? "US";
                }
            }
        }

        // Новый метод для загрузки настроек
        private void LoadVpnSettings()
        {
            vpnRotateAllCheckBox.IsChecked = Properties.Settings.Default.VpnRotate;
            vpnSelectCountryCheckBox.IsChecked = Properties.Settings.Default.VpnEnabled &&
                                               !Properties.Settings.Default.VpnRotate;
            vpnCountryComboBox.SelectedIndex = Properties.Settings.Default.VpnCountryIndex;

            // Принудительно обновляем состояние
            VpnRotateCheckBox_Changed(null, null);
            VpnCountryCheckBox_Changed(null, null);
        }

        // Обработчик выбора количества записей на странице
        private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInitialized) // Пропускаем вызов во время инициализации
            {
                return;
            }

            if (pageSizeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                pageSize = int.Parse(selectedItem.Content.ToString());
                currentPage = 1; // Сбрасываем на первую страницу
                LoadVacancies();
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Сохраняем настройки при закрытии приложения
            Properties.Settings.Default.OnlyWithPhones = onlyWithPhones;
            Properties.Settings.Default.AutoParseEnabled = isAutoParsing;

            // Сохраняем значения TextBox
            Properties.Settings.Default.VacancyTitle = VacancyTitleTextBox.Text;
            Properties.Settings.Default.WaitTime = waitTimeTextBox.Text;
            Properties.Settings.Default.ParseTime = parseTimeTextBox.Text;

            SaveVpnSettings();

            Properties.Settings.Default.Save();

            dbContext?.Dispose();
            httpClient?.Dispose();
        }

        private void OnlyWithPhonesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            onlyWithPhones = true;
            Properties.Settings.Default.OnlyWithPhones = true;
            Properties.Settings.Default.Save();
            Dispatcher.Invoke(() => logTextBox.Text += "Фильтр 'Только с телефонами' включён.");
        }

        private void OnlyWithPhonesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            onlyWithPhones = false;
            Properties.Settings.Default.OnlyWithPhones = false;
            Properties.Settings.Default.Save();
            Dispatcher.Invoke(() => logTextBox.Text += "Фильтр 'Только с телефонами' выключён.");
        }

        // Если есть AutoParseCheckBox
        private void AutoParseCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            isAutoParsing = true;
            Properties.Settings.Default.AutoParseEnabled = true;
            Properties.Settings.Default.Save();
            Dispatcher.Invoke(() => logTextBox.Text += "Автоматический парсинг включён.");
            StartAutoParsing();
        }

        private void AutoParseCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            isAutoParsing = false;
            Properties.Settings.Default.AutoParseEnabled = false;
            Properties.Settings.Default.Save();
            Dispatcher.Invoke(() => logTextBox.Text += "Автоматический парсинг выключён.");
            if (autoCts != null)
            {
                autoCts.Cancel();
                autoCts.Dispose();
                autoCts = null;
            }
        }

        private void VacancyTitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            Properties.Settings.Default.VacancyTitle = VacancyTitleTextBox.Text;
            Properties.Settings.Default.Save();
            CheckStartButtonEnabled();
        }

        private void WaitTimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.WaitTime = waitTimeTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void ParseTimeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.ParseTime = parseTimeTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void LoadVacancies()
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    var vacancies = dbContext.Vacancies.ToList();

                    // Фильтрация по датам
                    DateTime? dateFrom = dateFromPicker.SelectedDate;
                    DateTime? dateTo = dateToPicker.SelectedDate;

                    if (dateFrom.HasValue && dateTo.HasValue)
                    {
                        vacancies = vacancies.Where(v => v.ParseDate.Date >= dateFrom.Value.Date && v.ParseDate.Date <= dateTo.Value.Date).ToList();
                    }
                    else if (dateFrom.HasValue)
                    {
                        vacancies = vacancies.Where(v => v.ParseDate.Date >= dateFrom.Value.Date).ToList();
                    }
                    else if (dateTo.HasValue)
                    {
                        vacancies = vacancies.Where(v => v.ParseDate.Date <= dateTo.Value.Date).ToList();
                    }

                    // Обновляем общее количество записей
                    totalItems = vacancies.Count;
                    totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                    totalPages = totalPages == 0 ? 1 : totalPages; // Минимум 1 страница
                    if (currentPage > totalPages) currentPage = totalPages;

                    // Загружаем записи для текущей страницы
                    var viewModels = vacancies
                        .Skip((currentPage - 1) * pageSize)
                        .Take(pageSize)
                        .Select((v, index) => new VacancyViewModel(v, (currentPage - 1) * pageSize + index + 1))
                        .ToList();

                    var collectionView = CollectionViewSource.GetDefaultView(viewModels);

                    dataGrid.ItemsSource = collectionView;
                    AdjustColumnWidths(dataGrid);

                    // Обновляем UI
                    currentPageTextBox.Text = currentPage.ToString();
                    totalPagesLabel.Content = $"{totalPages}";
                    recordsCountLabel.Content = totalItems.ToString();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"1111 {ex.InnerException}");
            }
        }

        private void AdjustColumnWidths(DataGrid dataGrid)
        {
            foreach (var column in dataGrid.Columns)
            {
                // Устанавливаем ширину на Auto, чтобы изначально подогнать под содержимое
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);

                // Принудительно обновляем макет, чтобы получить реальную ширину
                dataGrid.UpdateLayout();

                // Получаем текущую ширину (по содержимому)
                double maxWidth = column.ActualWidth;

                // Добавляем небольшой отступ (например, 20 пикселей) для читаемости
                column.Width = new DataGridLength(maxWidth + 20);
            }
        }

        private void CurrentPageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    // Пробуем распарсить введённое значение
                    if (!int.TryParse(currentPageTextBox.Text, out int newPage))
                    {
                        MessageBox.Show("Пожалуйста, введите корректное число.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        currentPageTextBox.Text = currentPage.ToString(); // Возвращаем предыдущее значение
                        return;
                    }

                    // Вычисляем общее количество страниц
                    var totalRecords = dbContext.Vacancies.Count();
                    var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                    // Проверяем, что введённое число в допустимом диапазоне
                    if (newPage < 1 || newPage > totalPages)
                    {
                        MessageBox.Show($"Пожалуйста, введите число от 1 до {totalPages}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        currentPageTextBox.Text = currentPage.ToString(); // Возвращаем предыдущее значение
                        return;
                    }

                    // Если валидация пройдена, переходим на новую страницу
                    currentPage = newPage;
                    LoadVacancies();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при переходе на страницу: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    currentPageTextBox.Text = currentPage.ToString(); // Возвращаем предыдущее значение
                }
            }
        }

        private void CurrentPageTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _); // Разрешаем ввод только цифр
        }

        // Обработчики пагинации
        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage = 1;
            LoadVacancies();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadVacancies();
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadVacancies();
            }
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage = totalPages;
            LoadVacancies();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadVacancies();
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckStartButtonEnabled();
        }

        private void CheckStartButtonEnabled()
        {
            Dispatcher.Invoke(() =>
            {
                var vacancyTextBox = VacancyTitleTextBox;
                if (string.IsNullOrWhiteSpace(vacancyTextBox.Text) || CityComboBox.SelectedItem == null)
                {
                    vacancyTextBox.BorderBrush = string.IsNullOrWhiteSpace(vacancyTextBox.Text) ? Brushes.Red : Brushes.Gray;
                    startButton.IsEnabled = false;
                }
                else
                {
                    vacancyTextBox.BorderBrush = Brushes.Gray;
                    startButton.IsEnabled = true;
                }
            });
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Останавливаем предыдущий парсинг, если он идёт
            if (cts != null)
            {
                cts.Cancel();
                Dispatcher.Invoke(() =>
                {
                    stopButton.IsEnabled = false;
                    logTextBox.Text += "Предыдущий парсинг остановлен.\n";
                });
                // Ждём завершения предыдущего парсинга
                await Task.Delay(1000); // Даём время на завершение
            }

            string searchKeyword = string.Empty;
            string city = string.Empty;
            Dispatcher.Invoke(() =>
            {
                startButton.IsEnabled = false;
                stopButton.IsEnabled = true; // Активируем "Стоп"
                logTextBox.Text = "Парсинг начался...\n";
                searchKeyword = VacancyTitleTextBox.Text.Trim();
                city = CityComboBox?.SelectedItem is ComboBoxItem selectedItem ? selectedItem.Content?.ToString() : string.Empty;
            });

            cts = new CancellationTokenSource(); // Создаём токен отмены
            try
            {
                await ParseAllPages(searchKeyword, city, cts.Token);
            }
            catch (OperationCanceledException)
            {
                Dispatcher.Invoke(() => logTextBox.Text += "Парсинг остановлен пользователем.\n");
            }
            finally
            {

            }

            Dispatcher.Invoke(() =>
            {
                logTextBox.Text += "Парсинг завершён.\n";
                LoadVacancies();
                startButton.IsEnabled = true;
                stopButton.IsEnabled = false; // Деактивируем "Стоп"
            });
            cts.Dispose();
            cts = null;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel();
                Dispatcher.Invoke(() =>
                {
                    stopButton.IsEnabled = false;
                    logTextBox.Text += "Остановка парсинга инициирована...\n";
                });
            }
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = false;

            Dispatcher.InvokeAsync(() =>
            {
                var view = dataGrid.ItemsSource as ICollectionView;
                var items = view?.Cast<VacancyViewModel>().ToList();
                if (items != null)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        items[i].RowNumber = i + 1;
                    }
                    dataGrid.Items.Refresh();
                }
            }, DispatcherPriority.Background);
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj);
        }

        private async void StartAutoParsing()
        {
            autoCts = new CancellationTokenSource();
            try
            {
                while (isAutoParsing)
                {
                    autoCts.Token.ThrowIfCancellationRequested();

                    string parseTimeRange = parseTimeTextBox.Text;
                    var times = parseTimeRange.Split(',');
                    if (times.Length != 2 ||
                        !TimeSpan.TryParseExact(times[0].Trim(), "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan startTime) ||
                        !TimeSpan.TryParseExact(times[1].Trim(), "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan endTime))
                    {
                        Dispatcher.Invoke(() => logTextBox.Text += "Неверный формат времени. Используйте ЧЧ:ММ,ЧЧ:ММ (например, 14:00,23:00).\n");
                        break;
                    }

                    if (startTime >= endTime)
                    {
                        Dispatcher.Invoke(() => logTextBox.Text += "Начальное время должно быть меньше конечного.\n");
                        break;
                    }

                    DateTime now = DateTime.Now;
                    Random random = new Random();
                    TimeSpan randomTimeSpan = startTime + TimeSpan.FromMinutes(random.Next((int)(endTime - startTime).TotalMinutes));
                    DateTime nextRun = now.Date.Add(randomTimeSpan);
                    if (nextRun < now)
                    {
                        nextRun = nextRun.AddDays(1);
                    }

                    TimeSpan delay = nextRun - now;
                    Dispatcher.Invoke(() => logTextBox.Text += $"Следующий автоматический парсинг в {nextRun:dd.MM.yyyy HH:mm}.\n");
                    await Task.Delay(delay, autoCts.Token);

                    if (!isAutoParsing) break;

                    string searchKeyword = string.Empty;
                    string city = string.Empty;
                    Dispatcher.Invoke(() =>
                    {
                        startButton.IsEnabled = false;
                        stopButton.IsEnabled = true;
                        logTextBox.Text += $"Автоматический парсинг начался в {DateTime.Now:dd.MM.yyyy HH:mm}...\n";
                        searchKeyword = VacancyTitleTextBox.Text.Trim();
                        city = CityComboBox?.SelectedItem is ComboBoxItem selectedItem ? selectedItem.Content?.ToString() : string.Empty;
                    });

                    cts = new CancellationTokenSource();
                    try
                    {
                        await Task.Run(() => ParseAllPages(searchKeyword, city, cts.Token));
                    }
                    catch (OperationCanceledException)
                    {
                        Dispatcher.Invoke(() => logTextBox.Text += "Автоматический парсинг остановлен.\n");
                    }
                    finally
                    {

                    }

                    Dispatcher.Invoke(() =>
                    {
                        logTextBox.Text += "Автоматический парсинг завершён.\n";
                        LoadVacancies();
                        startButton.IsEnabled = true;
                        stopButton.IsEnabled = false;
                    });
                    cts.Dispose();
                    cts = null;
                }
            }
            catch (OperationCanceledException)
            {
                Dispatcher.Invoke(() => logTextBox.Text += "Автоматический парсинг отключён.\n");
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            var view = dataGrid.ItemsSource as ICollectionView;
            var viewModels = view?.Cast<VacancyViewModel>().ToList();
            if (viewModels == null || !viewModels.Any())
            {
                MessageBox.Show("Нет данных для экспорта.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            DateTime? dateFrom = dateFromPicker.SelectedDate;
            DateTime? dateTo = dateToPicker.SelectedDate;

            if (dateFrom.HasValue && dateTo.HasValue)
            {
                viewModels = viewModels.Where(vm => vm.ParseDate.Date >= dateFrom.Value.Date && vm.ParseDate.Date <= dateTo.Value.Date).ToList();
            }
            else if (dateFrom.HasValue)
            {
                viewModels = viewModels.Where(vm => vm.ParseDate.Date >= dateFrom.Value.Date).ToList();
            }
            else if (dateTo.HasValue)
            {
                viewModels = viewModels.Where(vm => vm.ParseDate.Date <= dateTo.Value.Date).ToList();
            }

            if (!viewModels.Any())
            {
                MessageBox.Show("Нет данных для экспорта в указанном диапазоне дат.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Вакансии");
                worksheet.Cells[1, 1].Value = "№п/п";
                worksheet.Cells[1, 2].Value = "Дата парсинга";
                worksheet.Cells[1, 3].Value = "Дата на сайте";
                worksheet.Cells[1, 4].Value = "ID на сайте";
                worksheet.Cells[1, 5].Value = "Ссылка";
                worksheet.Cells[1, 6].Value = "Название сайта";
                worksheet.Cells[1, 7].Value = "Телефон";
                worksheet.Cells[1, 8].Value = "Вакансия";
                worksheet.Cells[1, 9].Value = "Адрес";
                worksheet.Cells[1, 10].Value = "Компания";
                worksheet.Cells[1, 11].Value = "ФИО";

                for (int i = 0; i < viewModels.Count; i++)
                {
                    var vm = viewModels[i];
                    worksheet.Cells[i + 2, 1].Value = vm.RowNumber;
                    worksheet.Cells[i + 2, 2].Value = vm.ParseDate.ToString("dd.MM.yyyy");
                    worksheet.Cells[i + 2, 3].Value = vm.Date.ToString("dd.MM.yyyy");
                    worksheet.Cells[i + 2, 4].Value = vm.SiteId;
                    worksheet.Cells[i + 2, 5].Value = vm.Link;
                    worksheet.Cells[i + 2, 6].Value = vm.Domain;
                    worksheet.Cells[i + 2, 7].Value = vm.Phone;
                    worksheet.Cells[i + 2, 8].Value = vm.Title;
                    worksheet.Cells[i + 2, 9].Value = vm.Address;
                    worksheet.Cells[i + 2, 10].Value = vm.Company;
                    worksheet.Cells[i + 2, 11].Value = vm.ContactName;
                }

                worksheet.Cells.AutoFitColumns();

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    DefaultExt = "xlsx",
                    FileName = $"Vacancies_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, package.GetAsByteArray());
                    MessageBox.Show($"Файл успешно сохранён: {saveFileDialog.FileName}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ConfigureAvitoHttpClient()
        {
            httpClient.DefaultRequestHeaders.Add("authority", "m.avito.ru");
            httpClient.DefaultRequestHeaders.Add("pragma", "no-cache");
            httpClient.DefaultRequestHeaders.Add("cache-control", "no-cache");
            httpClient.DefaultRequestHeaders.Add("upgrade-insecure-requests", "1");
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.66 Mobile Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "none");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "navigate");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-user", "?1");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "document");
            httpClient.DefaultRequestHeaders.Add("accept-language", "ru-RU,ru;q=0.9");
            httpClient.DefaultRequestHeaders.Add("cookie", AvitoCookie);
        }

        private async Task ParseAllPages(string searchKeyword, string city, CancellationToken token)
        {
            try
            {
                int waitTime = int.TryParse(waitTimeTextBox.Text, out int seconds) ? seconds : 5;
                string country = null;
                bool useRotation = false;

                // Определяем режим работы прокси
                if (vpnSelectCountryCheckBox.IsChecked == true)
                {
                    // Настройка прокси для выбранной страны
                }
                else if (vpnRotateAllCheckBox.IsChecked == true)
                {
                    // Настройка ротации прокси
                }

                // Выбираем режим парсинга - Avito или другой сайт
                if (cityToSubdomainMap.TryGetValue(city, out string subdomain))
                {
                    if (subdomain == "msk")
                    {
                        await ParseRabotaRu(searchKeyword, token);
                    }
                    else
                    {
                        await ParseAvito(searchKeyword, city, token);
                    }
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => logTextBox.Text += $"Критическая ошибка: {ex.Message}\n");
                await Task.Delay(5000);
            }
            finally
            {
                if (vpnRotateAllCheckBox.IsChecked == true || vpnSelectCountryCheckBox.IsChecked == true)
                {
                    await _vpnManager.Disconnect();
                }
            }
        }

        private async Task ParseAvito(string searchKeyword, string city, CancellationToken token)
        {
            try
            {
                // Инициализация сессии
                await httpClient.GetAsync("https://m.avito.ru/", token);

                // Параметры запроса
                var searchParams = new Dictionary<string, string>
            {
                {"categoryId", "14"},
                {"params[30]", "4969"},
                {"locationId", GetAvitoLocationId(city)},
                {"searchRadius", "200"},
                {"priceMin", "2000"},
                {"priceMax", "450000"},
                {"params[110275]", "426645"},
                {"sort", "priceDesc"},
                {"withImagesOnly", "true"},
                {"lastStamp", DateTimeOffset.Now.ToUnixTimeSeconds().ToString()},
                {"display", "list"},
                {"limit", "50"},
                {"query", searchKeyword},
                {"key", AvitoKey}
            };

                var items = await GetAllAvitoItems(searchParams, token);

                foreach (var item in items)
                {
                    token.ThrowIfCancellationRequested();

                    var adId = item.value.id.ToString();
                    var itemDetails = await GetAvitoItemDetails(adId, token);

                    if (itemDetails != null)
                    {
                        var vacancy = new Vacancy
                        {
                            Title = itemDetails.title,
                            Link = $"https://www.avito.ru/{city}/obyavlenie/{adId}",
                            Domain = "avito.ru",
                            ParseDate = DateTime.UtcNow,
                            Address = itemDetails.address,
                            Company = itemDetails.seller?.name,
                            SiteId = adId
                        };

                        var phone = await GetAvitoPhoneNumber(adId, token);
                        vacancy.Phone = phone ?? "Нет телефона";

                        // Обработка вакансии как в вашем исходном коде
                        await ProcessVacancy(vacancy, token);
                    }
                    int waitTime = int.TryParse(waitTimeTextBox.Text, out int second) ? second : 5;
                    await Task.Delay(waitTime * 1000, token); // Используем заданное время ожидания
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => logTextBox.Text += $"Ошибка при парсинге Avito: {ex.Message}\n");
            }
        }

        private async Task ProcessVacancy(Vacancy vacancy, CancellationToken token)
        {
            // Проверяем, установлен ли флаг "Только с телефонами"
            if (onlyWithPhones && string.IsNullOrEmpty(vacancy.Phone))
            {
                Dispatcher.Invoke(() => logTextBox.Text += $"Вакансия ID {vacancy.SiteId} пропущена: отсутствует телефон.\n");
                return;
            }

            if (_blacklistService.IsBlacklisted(vacancy))
            {
                Dispatcher.Invoke(() => logTextBox.Text += $"Пропущена вакансия от {vacancy.Company} (в черном списке)\n");
                return;
            }

            if (!IsDuplicate(vacancy))
            {
                try
                {
                    await dbContext.Vacancies.AddAsync(vacancy, token);
                    await dbContext.SaveChangesAsync(token);

                    Dispatcher.Invoke(() => logTextBox.Text += $"Вакансия ID {vacancy.SiteId} добавлена.\n");
                }
                catch (DbUpdateException ex)
                {
                    Dispatcher.Invoke(() => logTextBox.Text += $"Ошибка сохранения (SQLite): {ex.InnerException?.Message}\n");
                    await Task.Delay(3000, token);
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => logTextBox.Text += $"Общая ошибка: {ex.Message}\n");
                    await Task.Delay(3000, token);
                }
            }
            else
            {
                Dispatcher.Invoke(() => logTextBox.Text += $"Дубликат вакансии: {vacancy.Title}\n");
            }
        }

        private async Task<List<AvitoItem>> GetAllAvitoItems(Dictionary<string, string> searchParams, CancellationToken token)
        {
            var items = new List<AvitoItem>(); // Тип коллекции изменен на AvitoItem
            var url = "https://m.avito.ru/api/9/items";
            int page = 1;
            bool hasMorePages = true;

            while (hasMorePages && !token.IsCancellationRequested)
            {
                searchParams["page"] = page.ToString();
                var queryString = string.Join("&", searchParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

                var response = await httpClient.GetAsync($"{url}?{queryString}", token);
                if (!response.IsSuccessStatusCode)
                {
                    Dispatcher.Invoke(() => logTextBox.Text += $"Ошибка запроса: {response.StatusCode}\n");
                    break;
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AvitoApiResponse>(content);

                if (result?.status != "ok")
                {
                    Dispatcher.Invoke(() => logTextBox.Text += $"Ошибка API: {result?.result}\n");
                    break;
                }

                var pageItems = result.result.items
                    .Where(i => i.type == "item")
                    .ToList();

                items.AddRange(pageItems); // Теперь типы совпадают

                if (pageItems.Count < int.Parse(searchParams["limit"]))
                    hasMorePages = false;

                page++;
                await Task.Delay(3000, token);
            }

            return items;
        }

        private async Task<AvitoItemDetails> GetAvitoItemDetails(string itemId, CancellationToken token)
        {
            var url = $"https://m.avito.ru/api/15/items/{itemId}?key={AvitoKey}";
            var response = await httpClient.GetAsync(url, token);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AvitoItemDetails>(content);
        }

        private async Task<string> GetAvitoPhoneNumber(string itemId, CancellationToken token)
        {
            var url = $"https://m.avito.ru/api/1/items/{itemId}/phone?key={AvitoKey}";
            var response = await httpClient.GetAsync(url, token);

            if (!response.IsSuccessStatusCode)
                return "Не удалось получить телефон";

            var content = await response.Content.ReadAsStringAsync();
            var phoneResponse = JsonConvert.DeserializeObject<AvitoPhoneResponse>(content);

            if (phoneResponse?.status == "ok")
            {
                var uri = phoneResponse.result.action.uri;
                var phone = uri.Split("number=")[1];
                return Uri.UnescapeDataString(phone);
            }

            return phoneResponse?.result?.message ?? "Телефон не указан";
        }

        private string GetAvitoLocationId(string city)
        {
            // Маппинг городов на locationId Avito
            var cityLocationMap = new Dictionary<string, string>
        {
            {"москва", "637640"},
            {"санкт-петербург", "4079"},
            {"новосибирск", "641780"},
            // Добавьте другие города по необходимости
        };

            return cityLocationMap.TryGetValue(city.ToLower(), out string locationId) ? locationId : "641780"; // По умолчанию Новосибирск
        }

        private async Task ParseRabotaRu(string searchKeyword, CancellationToken token)
        {
            // Ваш существующий код парсинга rabota.ru
            // ...
        }

        // Классы для десериализации JSON ответов Avito
        public class AvitoApiResponse
        {
            public string status { get; set; }
            public AvitoResultData result { get; set; } // Убрали дублирование поля result
        }

        public class AvitoResultData
        {
            public List<AvitoItem> items { get; set; }
        }

        public class AvitoItem
        {
            public string type { get; set; }
            public AvitoItemValue value { get; set; } // Исправлено на value (с маленькой буквы)
        }

        public class AvitoItemValue
        {
            public int id { get; set; }
            // Добавьте другие необходимые поля
        }

        private class AvitoItemDetails
        {
            public string title { get; set; }
            public string price { get; set; }
            public string address { get; set; }
            public AvitoSeller seller { get; set; }
            public string description { get; set; }
        }

        private class AvitoSeller
        {
            public string name { get; set; }
        }

        private class AvitoPhoneResponse
        {
            public string status { get; set; }
            public AvitoPhoneResult result { get; set; }
        }

        private class AvitoPhoneResult
        {
            public string message { get; set; }
            public AvitoPhoneAction action { get; set; }
        }

        private class AvitoPhoneAction
        {
            public string uri { get; set; }
        }

        private HashSet<string> blacklist = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private void ParseWithSelenium(string searchUrl)
        {
            // Логика парсинга через Selenium как в предыдущем варианте
            // Добавь сюда старый метод ParseAllPages, если нужно
        }

        private bool IsDuplicate(Vacancy newVacancy)
        {
            // Приводим Date к UTC, если оно не null
            var allVacancies = dbContext.Vacancies
        .AsNoTracking()
        .AsEnumerable() // Переносим обработку на клиент
        .ToList();

            var newVacancyDateUtc = newVacancy.Date.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(newVacancy.Date, DateTimeKind.Utc)
                : newVacancy.Date.ToUniversalTime();

            return allVacancies.Any(v =>
                (v.Company == newVacancy.Company &&
                 v.ShortTitle == newVacancy.ShortTitle &&
                 Math.Abs((v.Date - newVacancyDateUtc).Days) <= 40) ||
                (v.Phone == newVacancy.Phone &&
                 v.Title.StartsWith(newVacancy.Title?.Split(' ')[0] ?? "") &&
                 Math.Abs((v.Date - newVacancyDateUtc).Days) <= 40));
        }

        private DateTime ParseDate(string dateText)
        {
            if (string.IsNullOrEmpty(dateText)) return DateTime.Today;
            if (dateText.Contains("сегодня")) return DateTime.Today;
            if (dateText.Contains("вчера")) return DateTime.Today.AddDays(-1);
            return DateTime.TryParse(dateText, out var date) ? date : DateTime.Today;
        }

        protected override void OnClosed(EventArgs e)
        {
            //driver?.Quit();
            dbContext?.Dispose();
            httpClient?.Dispose();
            base.OnClosed(e);
        }

        // Обновленные обработчики для новых чекбоксов
        private void VpnRotateCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            // Если включена ротация - отключаем выбор страны
            if (vpnRotateAllCheckBox.IsChecked == true)
            {
                vpnSelectCountryCheckBox.IsChecked = false;
            }

            vpnSelectCountryCheckBox.IsEnabled = !vpnRotateAllCheckBox.IsChecked == true;
            vpnCountryComboBox.IsEnabled = vpnSelectCountryCheckBox.IsChecked == true &&
                                         !vpnRotateAllCheckBox.IsChecked == true;
            SaveVpnSettings();
        }

        private void VpnCountryCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            // Если включаем выбор страны - отключаем ротацию
            if (vpnSelectCountryCheckBox.IsChecked == true)
            {
                vpnRotateAllCheckBox.IsChecked = false;
            }

            vpnCountryComboBox.IsEnabled = vpnSelectCountryCheckBox.IsChecked == true;
            testVpnButton.IsEnabled = vpnSelectCountryCheckBox.IsChecked == true ||
                                     vpnRotateAllCheckBox.IsChecked == true;

            // Активируем кнопку теста если выбран любой режим VPN
            testVpnButton.IsEnabled = vpnRotateAllCheckBox.IsChecked == true ||
                                    vpnSelectCountryCheckBox.IsChecked == true;

            SaveVpnSettings();
        }

        // Новый метод для сохранения настроек VPN
        private void SaveVpnSettings()
        {
            Properties.Settings.Default.VpnEnabled = vpnRotateAllCheckBox.IsChecked == true ||
                                                  vpnSelectCountryCheckBox.IsChecked == true;
            Properties.Settings.Default.VpnRotate = vpnRotateAllCheckBox.IsChecked == true;
            Properties.Settings.Default.VpnCountryIndex = vpnCountryComboBox.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private async void TestVpnButton_Click(object sender, RoutedEventArgs e)
        {
            //testVpnButton.IsEnabled = false;
            //logTextBox.AppendText("\n=== Тест VPN ===\n");

            //try
            //{
            //    var ip = await GetCurrentIp();
            //    logTextBox.AppendText($"Ip: {ip}");
            //    await _vpnManager.Connect(@"C:\Program Files\OpenVPN\config\vpnbook-de220-tcp443.ovpn");

            //    await Task.Delay(15000);

            //    // Проверяем IP
            //    using var client = new HttpClient();
            //    string ip1 = await client.GetStringAsync("https://api.ipify.org");

            //    logTextBox.AppendText($"Ip: {ip1}");
            //    await _vpnManager.Disconnect();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка: {ex.Message}\nПопробуйте другой сервер");
            //}
        }

        private async Task<string> GetCurrentIp()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                return await client.GetStringAsync("https://api.ipify.org");
            }
            catch
            {
                return "Не удалось определить IP";
            }
        }

        private void LoadBlacklistButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                Title = "Выберите чёрный список"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var lines = File.ReadAllLines(dialog.FileName);
                    blacklist = new HashSet<string>(lines.Where(line => !string.IsNullOrWhiteSpace(line)));
                    Dispatcher.Invoke(() => logTextBox.Text += $"Загружен чёрный список: {blacklist.Count} записей\n");
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => logTextBox.Text += $"Ошибка загрузки: {ex.Message}\n");
                }
            }
        }
    }
}