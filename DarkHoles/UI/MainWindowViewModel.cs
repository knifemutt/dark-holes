using DarkHoles.Model.BPlug;
using DarkHoles.Model.MemoryUtilities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DarkHoles.UI
{
    public interface IMainWindowViewModel
    {
    }

    public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
    {
        public static string WorldChrManAobString = "48 8B 1D ?? ?? ?? 04 48 8B F9 48 85 DB ?? ?? 8B 11 85 D2 ?? ?? 8D";
        private readonly IDarkSoulsIIIMemoryUtilities _darkSoulsIIIMemUtils;

        public MainWindowViewModel(IDarkSoulsIIIMemoryUtilities darkSoulsIIIMemoryValues, IVibeAdmin vibeAdmin)
        {
            _darkSoulsIIIMemUtils = darkSoulsIIIMemoryValues;

            _darkSoulsIIIMemUtils.OpenProcess();

            // Do something if not connected to process?

            vibeAdmin.Initialize();

            Task.Run(() =>
            {
                while (true)
                {
                    CharacterHP = _darkSoulsIIIMemUtils.GetCurrentHP().ToString();
                    vibeAdmin.NotifyOfChange(_darkSoulsIIIMemUtils.GetCurrentHP(), _darkSoulsIIIMemUtils.GetMaxHP());
                    Thread.Sleep(100);
                }
            });
        }

        private string _characterHP = "???";
        public string CharacterHP
        {
            get => _characterHP;
            set { _characterHP = value; FirePropertyChanged("CharacterHP"); }
        }

        // will this not work if the user closes the game after opening it?
        public bool IsConnectedToProcess => _darkSoulsIIIMemUtils.IsConnectedToProcess;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void FirePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (PropertyChanged is not null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
