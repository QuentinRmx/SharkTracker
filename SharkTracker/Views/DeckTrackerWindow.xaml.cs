using System.Windows;
using System.Windows.Input;

namespace SharkTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
//        private DispatcherTimer refreshTimer;

        public MainWindow()
        {
            InitializeComponent();
            // Draggable window by clicking anywhere.
            this.MouseDown += TryDragMove;
            // Exiting the application with the Escape key.
            this.KeyDown += delegate(object sender, KeyEventArgs args)
            {
                if (args.Key == Key.Escape)
                {
                    this.Close();
                }
            };
//            refreshTimer = new DispatcherTimer();
//            refreshTimer.Tick += Refresh;
//            refreshTimer.Interval = new TimeSpan(0, 0, 1);
//            refreshTimer.Start();
        }

        private void TryDragMove(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}