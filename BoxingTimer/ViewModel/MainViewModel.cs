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
        private readonly LocalDbService _dbService;
        private readonly INavigation _navigation;

        public MainViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _dbService = new LocalDbService();
            SaveCommand = new Command(async () => await SaveData());
        }

        private int rounds;
        public int Rounds
        {
            get => rounds;
            set
            {
                if (rounds != value)
                {
                    rounds = value;
                    OnPropertyChanged();
                }
            }
        }

        private int roundTime;
        public int RoundTime
        {
            get => roundTime;
            set
            {
                if (roundTime != value)
                {
                    roundTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private int restTime;
        public int RestTime
        {
            get => restTime;
            set
            {
                if (restTime != value)
                {
                    restTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task SaveData()
        {
            var timeModel = new TimeModel
            {
                Rounds = this.Rounds,
                RoundTime = this.RoundTime,
                RestTime = this.RestTime
            };

            await _dbService.SaveTime(timeModel);
            await _navigation.PushAsync(new TimerPage()); // Navigiere zur TimerPage
        }
    }
}
