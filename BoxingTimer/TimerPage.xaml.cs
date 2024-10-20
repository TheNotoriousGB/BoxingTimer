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
        private System.Timers.Timer _timer; // Timer-Objekt zur Verfolgung der verstrichenen Zeit
        private TimeSpan _roundTime; // Dauer jeder Runde
        private TimeSpan _restTime; // Dauer der Ruhepause
        private TimeSpan _timeLeft; // Verbleibende Zeit f�r die aktuelle Runde oder Ruhepause
        private bool _isRestTime; // Flag zur Anzeige, ob es sich um die Ruhepause handelt
        private readonly IAudioManager audioManager; // Audio-Manager zum Abspielen von Audio-Clips

        public TimerPage()
        {
            audioManager = new AudioManager(); // Initialisiert den Audio-Manager
            InitializeComponent(); // Initialisiert die Benutzeroberfl�che
            LoadSettings(); // L�dt die Einstellungen (Runden- und Ruhezeiten)
        }

        // L�dt die Einstellungen aus der Datenbank
        private async void LoadSettings()
        {
            var dbService = new LocalDbService(); // Instanziiert den Datenbankdienst
            var times = await dbService.GetTimes(); // Holt die Zeiten aus der Datenbank

            if (times.Any()) // �berpr�ft, ob es Zeiten gibt
            {
                var timeModel = times.First(); // Nimmt das erste Zeitmodell
                _roundTime = TimeSpan.FromSeconds(timeModel.RoundTime); // Setzt die Rundenzeit
                _restTime = TimeSpan.FromSeconds(timeModel.RestTime); // Setzt die Ruhezeit
                _timeLeft = _roundTime; // Setzt die verbleibende Zeit auf die Rundenzeit
                UpdateTimerLabel(); // Aktualisiert die Timer-Anzeige
            }
        }

        // Startet den Timer und spielt einen Audio-Clip ab
        private async void OnStartTimerClicked(object sender, EventArgs e)
        {
            if (_timer != null) return; // �berpr�ft, ob der Timer bereits l�uft

            _timer = new System.Timers.Timer(1000); // Timer mit einem Intervall von 1 Sekunde
            _timer.Elapsed += OnTimerElapsed; // F�gt den Ereignishandler f�r den Timer hinzu
            _timer.Start(); // Startet den Timer

            var player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("fighte-clip.wav")); // Erstellt einen Spieler f�r den Audio-Clip
            player.Play(); // Spielt den Audio-Clip ab
        }

        // Stoppt den Timer
        private void OnStopTimerClicked(object sender, EventArgs e)
        {
            StopTimer(); // Ruft die StopTimer-Methode auf
        }

        // Wird bei jedem Timer-Intervall aufgerufen
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_timeLeft.TotalSeconds > 0) // �berpr�ft, ob noch Zeit verbleibt
            {
                _timeLeft = _timeLeft.Add(TimeSpan.FromSeconds(-1)); // Verringert die verbleibende Zeit um 1 Sekunde
                UpdateTimerLabel(); // Aktualisiert die Timer-Anzeige
            }
            else
            {
                if (!_isRestTime) // �berpr�ft, ob es sich nicht um die Ruhepause handelt
                {
                    _isRestTime = true; // Setzt das Flag auf Ruhezeit
                    _timeLeft = _restTime; // Setzt die verbleibende Zeit auf die Ruhezeit
                    _timer.Start(); // Startet den Timer erneut (f�r die Ruhepause)
                }
                else
                {
                    StopTimer(); // Stoppt den Timer, wenn die Ruhezeit abgelaufen ist
                }
            }
        }

        // Aktualisiert das Timer-Label in der Benutzeroberfl�che
        private void UpdateTimerLabel()
        {
            Dispatcher.Dispatch(() =>
            {
                TimerLabel.Text = _timeLeft.ToString(@"mm\:ss"); // Formatiert die verbleibende Zeit als "mm:ss"
            });
        }

        // Stoppt den Timer und setzt die Variablen zur�ck
        private void StopTimer()
        {
            if (_timer == null) return; // �berpr�ft, ob der Timer nicht null ist

            _timer.Stop(); // Stoppt den Timer
            _timer.Dispose(); // Gibt die Ressourcen des Timers frei
            _timer = null; // Setzt den Timer auf null
            _isRestTime = false; // Setzt das Ruhezeit-Flag zur�ck
        }

        private async void OnSwipeUp(object sender, SwipedEventArgs e)
        {
            // Navigiere zu Page2
            await Navigation.PushAsync(new MainPage());
        }
    }
}
