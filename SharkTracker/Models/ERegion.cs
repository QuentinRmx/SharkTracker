using System.Windows.Markup;

namespace SharkTracker.Models
{
    public enum ERegion
    {
        Bilgewater,
        Demacia,
        Freljord,
        Ionia,
        Noxus,
        PnZ,
        Si,
        ALL
    }

    public static class ERegionExtensions
    {
        public static string ToLongName(this ERegion region)
        {
            return region switch
            {
                ERegion.Bilgewater => "Bilgewater",
                ERegion.Demacia => "Demacia",
                ERegion.Freljord => "Freljord",
                ERegion.Ionia => "Ionia",
                ERegion.Noxus => "Noxus",
                ERegion.PnZ => "Piltover & Zaun",
                ERegion.Si => "Shadow Isles",
                ERegion.ALL => "Collection",
                _ => "ALL"
            };
        }
    }
}