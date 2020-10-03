namespace SharkTracker.Models.Filters
{
    public enum ECollectionFilter : short
    {
    
        ALL = 0,
        Champions = 1,
        Epics = 2,
        Rares = 4,
        Commons = 8,
        Incomplete = 16
    }
}