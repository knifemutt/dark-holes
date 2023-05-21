using System.Windows;
using DarkHoles.UI;

namespace DarkHoles
{
    public interface IMainWindowView
    {
        void Show();
    }

    public partial class MainWindowView : Window, IMainWindowView
    {
        public MainWindowView(IMainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = mainWindowViewModel;
        }
    }
}
