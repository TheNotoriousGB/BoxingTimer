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
        private int _rounds;
        private int _currentRound; 
        private System.Timers.Timer _timer;
        private TimeSpan _roundTime;
        private TimeSpan _restTime;
        private TimeSpan _timeLeft;
        private bool _isRestTime;
        private readonly IAudioManager audioManager;

        public TimerPage(TimeModel timeModel)
        {
            audioManager = new AudioManager();
            InitializeComponent();
            LoadSettings(timeModel); // Lädt die Einstellungen aus dem übergebenen TimeModel
        }

        private void LoadSettings(TimeModel timeModel)
        {
            _rounds = timeModel.Rounds; // Anzahl der Runden setzen
            _roundTime = TimeSpan.FromSeconds(timeModel.RoundTime);
            _restTime = TimeSpan.FromSeconds(timeModel.RestTime);
            _timeLeft = _roundTime; // Setzt die verbleibende Zeit auf die Rundenzeit
            UpdateTimerLabel(); // Aktualisiert die Anzeige mit der geladenen Zeit
        }

        private async void OnStartTimerClicked(object sender, EventArgs e)
        {
            if (_timer != null || _currentRound >= _rounds) return; // Überprüfen, ob der Timer bereits läuft oder keine Runden mehr vorhanden sind

            _currentRound = 0; // Zurücksetzen auf die erste Runde
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
                    _isRestTime = true; // Wechsel zu Ruhezeit
                    _timeLeft = _restTime; // Setzt die verbleibende Zeit auf die Ruhezeit
                }
                else
                {
                    _currentRound++; // Nächste Runde
                    if (_currentRound < _rounds)
                    {
                        _isRestTime = false; // Zurück zur Rundenzeit
                        _timeLeft = _roundTime; // Setzt die verbleibende Zeit auf die nächste Rundenzeit
                    }
                    else
                    {
                        StopTimer(); // Stoppt den Timer, wenn alle Runden vorbei sind
                    }
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

        private async void OnSwipeLeft(object sender, SwipedEventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}
