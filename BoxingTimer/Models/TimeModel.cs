using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxingTimer.Models
{
    [SQLite.Table("TimeModel")]
    public class TimeModel
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        [SQLite.Column("Id")]
        public int Id { get; set; }

        [SQLite.Column("Rounds")]
        public int Rounds { get; set; }

        [SQLite.Column("RoundTime")]
        public int RoundTime { get; set; }

        [SQLite.Column("RestTime")]
        public int RestTime { get; set; }
    }
}
