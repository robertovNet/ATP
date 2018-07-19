using ATP.Wpf.ViewModels;
using System.Windows;

namespace ATP.Wpf.Views
{
    /// <summary>
    /// Lógica de interacción para Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }
    }
}
