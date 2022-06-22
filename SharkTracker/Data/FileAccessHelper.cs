namespace SharkTracker.Data
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            return Path.Combine(FileSystem.Current.AppDataDirectory, filename);
        }
    }
}
