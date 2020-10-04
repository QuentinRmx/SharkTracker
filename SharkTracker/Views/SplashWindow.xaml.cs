#nullable enable
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using SharkTrackerCore;

namespace SharkTracker.Views
{
    public partial class SplashWindow : Window
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public SplashWindow()
        {
            // Loaded += SplashWindow_OnLoaded;
            Closing += SplashWindow_OnClosing;
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, args) => Timer_Tick();
            SizeToContent = SizeToContent.WidthAndHeight;
            // Draggable window by clicking anywhere.
            this.MouseDown += TryDragMove;
            // Exiting the application with the Escape key.
            this.KeyDown += delegate(object sender, KeyEventArgs args)
            {
                if (args.Key == Key.Escape)
                {
                    Close();
                }
            };
            
            Grid.KeyDown += delegate(object sender, KeyEventArgs args)
            {
                if (args.Key == Key.Escape)
                {
                    Close();
                }
            };

            Grid.MouseDown += TryDragMove;
            
        }
        
        public void TryDragMove(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (mouseButtonEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Timer_Tick()
        {
            if (TbProgress.Text != "Ready")
                return;
            timer.Stop();
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        private void SplashWindow_OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            Closing -= SplashWindow_OnClosing;
            cancelEventArgs.Cancel = true;
            DoubleAnimation anim = new DoubleAnimation(0, TimeSpan.FromSeconds(1));
            anim.Completed += (o, args) => { Close(); };
            BeginAnimation(OpacityProperty, anim);
        }

        private void FormFade_OnCompleted(object? sender, EventArgs e)
        {
            timer.Start();
        }
    }
}