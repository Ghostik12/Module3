using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvitoRuParser.Services
{
    public class WindowsVpnManager
    {
        private Process _ovpnProcess;
        private readonly string _openVpnPath;
        private bool _isConnected;

        public WindowsVpnManager()
        {
            // Путь к установленному OpenVPN
            _openVpnPath = @"C:\Program Files\OpenVPN\bin\openvpn.exe";
        }

        public async Task Connect(string configPath)
        {
            //if (!File.Exists(_openVpnPath))
            //    throw new FileNotFoundException("OpenVPN не установлен", _openVpnPath);

            //if (!File.Exists(configPath))
            //    throw new FileNotFoundException("Конфиг не найден", configPath);

            //// Авторизация должна быть в той же папке, что и конфиг
            //var authPath = Path.Combine(
            //    Path.GetDirectoryName(configPath),
            //    "auth.txt"
            //);

            //var startInfo = new ProcessStartInfo
            //{
            //    FileName = _openVpnPath,
            //    Arguments = $"--config \"{configPath}\" --auth-user-pass \"{authPath}\"",
            //    WindowStyle = ProcessWindowStyle.Hidden,
            //    UseShellExecute = false,
            //    CreateNoWindow = true,
            //    RedirectStandardOutput = true
            //};

            //try
            //{
            //    _ovpnProcess = new Process { StartInfo = startInfo };
            //    _ovpnProcess.OutputDataReceived += (s, e) =>
            //    {
            //        if (e.Data?.Contains("Initialization Sequence Completed") == true)
            //            _isConnected = true;
            //    };

            //    _ovpnProcess.Start();
            //    _ovpnProcess.BeginOutputReadLine();

            //    // Ожидание подключения (15 сек)
            //    for (int i = 0; i < 30 && !_isConnected; i++)
            //        await Task.Delay(500);

            //    if (!_isConnected)
            //        throw new TimeoutException("Не удалось подключиться");
            //}
            //catch
            //{
            //    _ovpnProcess?.Kill();
            //    throw;
            //}
        }

        public async Task Disconnect()
        {
            //if (_ovpnProcess != null && !_ovpnProcess.HasExited)
            //{
            //    _ovpnProcess.Kill();
            //    await _ovpnProcess.WaitForExitAsync();
            //    _isConnected = false;
            //}
        }

        public void Dispose() => Disconnect().Wait();
    }
}
