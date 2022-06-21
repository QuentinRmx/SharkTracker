using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SharkTracker.Data.Models
{
    [Table("matchup")]
    public class Matchup
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PlayerDeckId { get; set; }

        public string OpponentDeckName { get; set; }

        public int Win { get; set; }

        public int Lose { get; set; }

        [Ignore]
        public int TotalEncounters => Win + Lose;

        [Ignore]
        public decimal Winrate
        {
            get
            {
                if (TotalEncounters == 0) return 0;
                return (((decimal)Win * 100) / (decimal)TotalEncounters);
            }
        }

        public string EvaluateWinrate()
        {
            if (Winrate < 25) return "text-danger";
            else if (Winrate >= 25 && Winrate < 50) return "text-warning";
            else if (Winrate >= 50 && Winrate < 75) return "text-info";
            else return "text-success";
        }
    }
}
