using System.Windows;
using System.Windows.Input;

namespace SharkTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Draggable window by clicking anywhere.
            this.MouseDown += delegate { DragMove(); };
            // Exiting the application with the Escape key.
            this.KeyDown += delegate(object sender, KeyEventArgs args)
            {
                if (args.Key == Key.Escape)
                {
                    this.Close();
                }
            };
        }
    }
}