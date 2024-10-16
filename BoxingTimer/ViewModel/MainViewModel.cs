using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BoxingTimer.Models;   
using Microsoft.Maui.Controls;

namespace BoxingTimer.ViewModel
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        private readonly LocalDbService _dbService; // Instance of the LocalDbService class for database operations
        private readonly INavigation _navigation; // Instance of the INavigation interface for navigation operations

        public MainViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _dbService = new LocalDbService(); // Initialize the LocalDbService
            SaveCommand = new Command(async () => await SaveData()); // Initialize the SaveCommand with an async command to save data
        }

        private int rounds; // Number of rounds
        public int Rounds
        {
            get => rounds;
            set
            {
                if (rounds != value)
                {
                    rounds = value;
                    OnPropertyChanged(); // Notify property changed
                }
            }
        }

        private int roundTime; // Duration of each round
        public int RoundTime
        {
            get => roundTime;
            set
            {
                if (roundTime != value)
                {
                    roundTime = value;
                    OnPropertyChanged(); // Notify property changed
                }
            }
        }

        private int restTime; // Duration of rest time between rounds
        public int RestTime
        {
            get => restTime;
            set
            {
                if (restTime != value)
                {
                    restTime = value;
                    OnPropertyChanged(); // Notify property changed
                }
            }
        }

        public ICommand SaveCommand { get; } // Command to save data

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Saves the data to the database and navigates to the TimerPage
        private async Task SaveData()
        {
            var timeModel = new TimeModel
            {
                Rounds = this.Rounds,
                RoundTime = this.RoundTime,
                RestTime = this.RestTime
            };

            await _dbService.SaveTime(timeModel); // Save the time model to the database using the LocalDbService
            await _navigation.PushAsync(new TimerPage()); // Navigate to the TimerPage
        }
    }
}
