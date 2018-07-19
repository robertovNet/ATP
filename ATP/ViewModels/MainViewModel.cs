using ATP.Engine;

namespace ATP.Wpf.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            var watcher = new MarketWatcher();
        }
    }
}
