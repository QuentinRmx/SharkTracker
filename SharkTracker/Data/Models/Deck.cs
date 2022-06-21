
using SQLite;

namespace SharkTracker.Data.Models
{
    [Table("deck")]
    public class Deck
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string DeckCode { get; set; }

        public string DeckName { get; set; }

    }
}
