using System;
using System.Windows;

namespace SharkTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        public static readonly SharkTrackerCore.SharkTracker SharkTracker = SharkTrackerCore.SharkTracker.New();
        
        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
#if DEBUG
            StartupUri = new Uri(@"/Views/MainWindow.xaml", UriKind.Relative);
#elif RELEASE
            StartupUri = new Uri(@"/Views/SplashWindow.xaml", UriKind.Relative);
#endif
        }
    }
}