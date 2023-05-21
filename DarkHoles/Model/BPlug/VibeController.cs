/*using Buttplug;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Buttplug.Client;
using Buttplug.Core;
using Buttplug.Client.Connectors.WebsocketConnector;
using Buttplug.Core.Messages;

namespace SoundPlug
{
    public class VibeController
    {
        private ButtplugClient _client;
        private ButtplugClientDevice _device;

        private float _deviceGranularity;

        //public float CurrentVibrationLevel { get; set; }
        //public event Action CurrentVibrationLevelChanged = delegate { };

        public void Initialize()
        {
            Setup().Wait();

            _device = _client.Devices.First();
            _device?.SendVibrateCmd(0.0).Wait();

            SendVibrateCmd(.5).Wait();
            Thread.Sleep(100);
            SendVibrateCmd(0).Wait();
            Thread.Sleep(100);
            SendVibrateCmd(.5).Wait();
            Thread.Sleep(100);
            SendVibrateCmd(0).Wait();

            var vibeCmdInfo = _device.AllowedMessages.First(m => m.Key.ToString()
    .Contains("vib", StringComparison.InvariantCultureIgnoreCase)).Value;
            _deviceGranularity = 1 / (float)vibeCmdInfo.StepCount.First();
        }

        private async Task Setup()
        {
            var connector = new ButtplugEmbeddedConnectorOptions();
            connector.ServerName = "Sound Plug Server";

            _client = new ButtplugClient("sound plug client");
            _client.ConnectAsync(connector).Wait();

            await _client.StartScanningAsync();

            while (_client.Devices.Count() < 1)
            {
                Thread.Sleep(100);
            }

            await _client.StopScanningAsync();
        }

        private float _previousVibeInterval = 0;

        public async void SendVibeCmd(float vibeAmt)
        {
            if (_client?.Devices is null)
                return;

            //System.Diagnostics.Debug.WriteLine($"RECEIVED        : {vibeAmt}");

            var vibeInterval = Math.Round(vibeAmt / _deviceGranularity) * _deviceGranularity; // Lock to intervals of 1 / _vibeInterval (close to)

            // Prevent values over 1 or under 0
            vibeInterval = Math.Min(vibeInterval, 1.0);
            vibeInterval = Math.Max(vibeInterval, 0.0);

            //System.Diagnostics.Debug.WriteLine($"Sending : {vibeInterval}");

            if (vibeInterval == _previousVibeInterval)
                return;

            _previousVibeInterval = (float)vibeInterval;

            if (vibeInterval == 0)
                Thread.Sleep(50);

            await SendVibrateCmd(vibeInterval);
            //CurrentVibrationLevel = (float)vibeInterval;
            //CurrentVibrationLevelChanged.Invoke();
        }

        private async Task SendVibrateCmd(double vibrateLevel)
        {
            foreach(var device in _client.Devices)
            {
                await device.SendVibrateCmd(vibrateLevel);
            }
        }
    }
}
*/