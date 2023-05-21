using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Buttplug.Client;
using Buttplug.Core;
using Buttplug.Client.Connectors.WebsocketConnector;
using Buttplug.Core.Messages;

namespace DarkHoles.Model.BPlug
{
    public interface IVibeAdmin
    {
        void NotifyOfChange(int health, int maxHealth);
        void Initialize();
        void Stop();
    }

    public class VibeAdmin : IVibeAdmin
    {
        private ButtplugClient _client;
        private ButtplugClientDevice _device;

        public void Initialize()
        {
            Setup();

            _device = _client.Devices.First();

            Task.Run(BuzzOnStartup).Wait();

            Task.Run(RunVibeControlLoop);
        }

        private async Task BuzzOnStartup()
        {
            await _device.VibrateAsync(0.0);

            await _device.VibrateAsync(0.5);
            Thread.Sleep(500);
            await _device.VibrateAsync(0.0);
            Thread.Sleep(500);
            await _device.VibrateAsync(0.5);
            Thread.Sleep(500);
            await _device.VibrateAsync(0.0);
        }

        private void Setup()
        {
            _client = new ButtplugClient("DarkHoles Client");
            var connector = new ButtplugWebsocketConnector(new Uri("ws://127.0.0.1:12345"));

            _client.ConnectAsync(connector).Wait();
           
            //_client.ScanningFinished += (aObj, aScanningFinishedArgs) => System.Diagnostics.Debug.WriteLine("Device scanning is finished!");

            // TODO: Need to do scanning???
            _client.StartScanningAsync().Wait();

            Thread.Sleep(3000);

            System.Diagnostics.Debug.WriteLine("Client currently knows about these devices:");
            foreach (var device in _client.Devices)
            {
                System.Diagnostics.Debug.WriteLine($"- {device.Name}");
            }

            _client.StopScanningAsync().Wait();
        }

        #region Thread-Safe Vars
        private int _health;
        private object _healthLock = new object();
        private int Health {
            get
            {
                lock(_healthLock)
                {
                    return _health;
                }
            }
            set 
            { 
                lock(_healthLock)
                {
                    _health = value;
                }
            }
        }

        private int _maxHealth;
        private object _maxHealthLock = new object();
        private int MaxHealth
        {
            get
            {
                lock (_maxHealthLock)
                {
                    return _maxHealth;
                }
            }
            set
            {
                lock (_maxHealthLock)
                {
                    if (value > 0)
                        _maxHealth = value;
                }
            }
        }
        #endregion

        public void Stop()
        {

            Task.Run(StopDeviceAsync).Wait();
        }

        private async Task StopDeviceAsync()
        {
            await _device.VibrateAsync(0.0);
            await _device.Stop();
        }

        public void NotifyOfChange(int health, int maxHealth)
        {
            Health = health;
            MaxHealth = maxHealth;
        }

        private const int TICKS_PER_SECOND = 10 * 1000 * 1000; // 10 million
        private const int MAX_VIBE_SECONDS = 10;

        private async Task RunVibeControlLoop()
        {
            int lastHp = Health;
            double strength = 0.0, lastStrength = 0.0;
            long ticksLeftInVibeTime = 0;
            long ticksOfLastLoop = DateTime.Now.Ticks;

            var vibeCmdInfo = _device.VibrateAttributes.First(d => d.ActuatorType == ActuatorType.Vibrate);
            var vibrationGranularity = vibeCmdInfo.StepCount;

            while (true)
            {
                // Count down to 0
                var elapsedTicks = DateTime.Now.Ticks - ticksOfLastLoop;

                ticksLeftInVibeTime = Math.Max(0, ticksLeftInVibeTime - elapsedTicks);
                if (ticksLeftInVibeTime <= 0)
                {
                    // Ramp down rather than stop abruptly?

                    // Only sends necessary command once, don't overload!
                    if (strength != 0)
                        await _device.VibrateAsync(0.0);

                    strength = 0;
                }

                var currentHp = Health;

                var damage = lastHp - currentHp;
                if (damage > 0)
                {
                    var damageHpPercent = Math.Max(damage / (double)MaxHealth, (double)1/vibrationGranularity);
                    strength = Math.Min(Math.Max(strength, damageHpPercent), 1.0);

                    // Add percentage of time to vibe time
                    var secondsToVibe = damageHpPercent * MAX_VIBE_SECONDS;
                    ticksLeftInVibeTime += (long)(secondsToVibe * TICKS_PER_SECOND);

                    // Punish if you die >:3c
                    if(currentHp <= 0)
                    {
                        strength = 1;
                        ticksLeftInVibeTime += MAX_VIBE_SECONDS * TICKS_PER_SECOND;
                    }
                  }

                // Only sends necessary command once, don't overload!
                if (strength != lastStrength)
                {
                    await _device.VibrateAsync(strength);
                }

                lastHp = currentHp;
                lastStrength = strength;

                ticksOfLastLoop = DateTime.Now.Ticks;
                Thread.Sleep(10);
            }
        }

        private void SendVibeCmd(double intensity, long ticksLeft)
        {
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now.TimeOfDay} || Str: {intensity}   Time: {ticksLeft}");
            _device?.VibrateAsync(intensity).Wait();
        }

        private async Task SendVibeCmd(double intensity)
        {

        }
    }
}
