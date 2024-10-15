using System;
using BoxingTimer.Models;
using SQLite;


namespace BoxingTimer
{

public class LocalDbService
    {
        private const string DB_Name = "BoxingTimer.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_Name));
            _connection.CreateTableAsync<TimeModel>();
        }

        public async Task SaveTime(TimeModel timeModel)
        {
            await _connection.InsertAsync(timeModel);
        }

        public async Task<List<TimeModel>> GetTimes()
        {
            var times = await _connection.Table<TimeModel>().ToListAsync();
            return times;
        }

    }
}
