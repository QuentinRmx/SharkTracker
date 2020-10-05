using System.Windows;
using System.Windows.Input;

namespace SharkTracker.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SizeToContent = SizeToContent.WidthAndHeight;
            // Draggable window by clicking anywhere.
            this.MouseDown += TryDragMove;
            // Exiting the application with the Escape key.
            this.KeyDown += delegate(object sender, KeyEventArgs args)
            {
                if (args.Key == Key.Escape)
                {
                    Application.Current.Shutdown();
                }
            };
            
            MainWindowFrame.KeyDown += delegate(object sender, KeyEventArgs args)
            {
                if (args.Key == Key.Escape)
                {
                    Close();
                }
            };

            MainWindowFrame.MouseDown += TryDragMove;
        }
        
        public void TryDragMove(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}