using System;
using System.Linq;
using System.Timers;
using Microsoft.Maui.Controls;
using BoxingTimer.Models;
using Plugin.Maui.Audio;

namespace BoxingTimer
{
    public partial class TimerPage : ContentPage
    {
        private System.Timers.Timer _timer;
        private TimeSpan _roundTime;
        private TimeSpan _restTime;
        private TimeSpan _timeLeft;
        private bool _isRestTime;
        private readonly IAudioManager audioManager;

        public TimerPage()
        {
            audioManager = new AudioManager(); 
            InitializeComponent();
            LoadSettings();
            
        }

        private async void LoadSettings()
        {
            var dbService = new LocalDbService();
            var times = await dbService.GetTimes();

            if (times.Any())
            {
                var timeModel = times.First();
                _roundTime = TimeSpan.FromSeconds(timeModel.RoundTime);
                _restTime = TimeSpan.FromSeconds(timeModel.RestTime);
                _timeLeft = _roundTime;
                UpdateTimerLabel();
            }
        }

        private async void OnStartTimerClicked(object sender, EventArgs e)
        {
            if (_timer != null) return;

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();

            var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("fighte-clip.wav"));
            player.Play();
        }

        private void OnStopTimerClicked(object sender, EventArgs e)
        {
            StopTimer();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_timeLeft.TotalSeconds > 0)
            {
                _timeLeft = _timeLeft.Add(TimeSpan.FromSeconds(-1));
                UpdateTimerLabel();
            }
            else
            {
                if (!_isRestTime)
                {
                    _isRestTime = true;
                    _timeLeft = _restTime;
                }
                else
                {
                    StopTimer();
                    DisplayAlert("Timer", "Die Zeit ist abgelaufen!", "OK");
                }
            }
        }

        private void UpdateTimerLabel()
        {
            Dispatcher.Dispatch(() =>
            {
                TimerLabel.Text = _timeLeft.ToString(@"mm\:ss");
            });
        }

        private void StopTimer()
        {
            if (_timer == null) return;

            _timer.Stop();
            _timer.Dispose();
            _timer = null;
            _isRestTime = false;
        }
    }
    }


