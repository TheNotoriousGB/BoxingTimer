using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BoxingTimer.Models;
using SQLite;
using Microsoft.Maui.Controls;

namespace BoxingTimer
{
    public class LocalDbService
    {
        private const string DB_Name = "BoxingTimer.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_Name));
            _connection.CreateTableAsync<TimeModel>().Wait(); // Erstelle die Tabelle, falls sie noch nicht existiert
        }

        // Speichert ein neues Zeitmodell in der Datenbank
        public async Task SaveTime(TimeModel timeModel)
        {
            await _connection.InsertAsync(timeModel);
        }

        // Gibt alle Zeitmodelle aus der Datenbank zurück
        public async Task<List<TimeModel>> GetTimes()
        {
            return await _connection.Table<TimeModel>().ToListAsync();
        }

        // Setzt die Datenbank zurück
        public async Task ResetDatabase()
        {
            await _connection.DropTableAsync<TimeModel>(); // Löscht die Tabelle
            await _connection.CreateTableAsync<TimeModel>(); // Erstellt die Tabelle neu
        }
    }
}
