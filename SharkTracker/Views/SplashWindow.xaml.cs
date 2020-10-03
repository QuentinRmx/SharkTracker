#nullable enable
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace SharkTracker.Views
{
    public partial class SplashWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();

        public SplashWindow()
        {
            // Loaded += SplashWindow_OnLoaded;
            Closing += SplashWindow_OnClosing;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, args) => Timer_Tick();
            InitializeComponent();
            
        }

        // private void SplashWindow_OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        // {
        //     Loaded -= SplashWindow_OnLoaded;
        //     var anim = new DoubleAnimation(1, (Duration) TimeSpan.FromSeconds(2));
        //     anim.Completed += (o, args) =>
        //     {
        //         
        //     };
        //     this.BeginAnimation(OpacityProperty, anim);
        //     
        // }

        private void Timer_Tick()
        {
            timer.Stop();
            MainWindow window = new MainWindow();
            window.Show();
            Close();
        }

        private void SplashWindow_OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            Closing -= SplashWindow_OnClosing;
            cancelEventArgs.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration) TimeSpan.FromSeconds(1));
            anim.Completed += (o, args) =>
            {
                Close();
            };
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void FormFade_OnCompleted(object? sender, EventArgs e)
        {
            timer.Start();
        }
    }
}