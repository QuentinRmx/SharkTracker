using System;
using System.Threading.Tasks;

namespace SharkTrackerCore
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            SharkTracker tracker = SharkTracker.New();
            await tracker.UpdateFromRiot((sender, eventArgs) =>
            {
                try
                {
                    if (!(sender is RiotDownloader dl))
                        return;
                    Console.WriteLine(
                        $"Current progress: {dl.ProgressPercent:F2}% ({dl.Progress}/{dl.TotalProgress} files)");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }
}