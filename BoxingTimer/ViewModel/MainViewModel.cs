using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BoxingTimer.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private int rounds;

        [ObservableProperty]
        private string roundTime;

        [ObservableProperty]
        private string restTime;

        public MainViewModel()
        {
            SaveCommand = new RelayCommand(SaveData);
        }

        public RelayCommand SaveCommand { get; }

        private void SaveData()
        {
            // Navigiere zur TimerPage, wenn das Routing und Shell korrekt konfiguriert sind.
            Shell.Current.GoToAsync("TimerPage");
        }
    }
}
