
using SharkTracker.Data.LoRDeckCodes;
using SQLite;

namespace SharkTracker.Data.Models
{
    [Table("deck")]
    public class Deck
    {
        /// <summary>
        /// Unique row identifier in the db.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Actual deck identifier (FK for other table)
        /// </summary>
        public int DeckId { get; set; }

        public string DeckCode { get; set; }

        public string DeckName { get; set; }

        /// <summary>
        /// Gets incremented each time a new version is created. Version 1 is the first version and so on.
        /// </summary>
        public int Version { get; set; }

        [Ignore]
        public List<CardCodeAndCount> Cards { get; set; }
    }
}
