using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SharkTracker.Controls
{
    public partial class MainMenuControl : UserControl
    {
        private static readonly Uri PATH_OPEN =
            new Uri("../Resources/icons/menu_icon_open_style.png", UriKind.RelativeOrAbsolute);

        private static readonly Uri PATH_COLLAPSE =
            new Uri("../Resources/icons/menu_icon_collapse_style.png", UriKind.RelativeOrAbsolute);

        public static readonly DependencyProperty CurrentSelectionProperty =
            DependencyProperty.Register("CurrentSelection", typeof(string), typeof(MainMenuControl),
                new PropertyMetadata(default(string)));

        private double _defaultWidth;

        private double _collapsedWidth = 24;

        public MainMenuControl()
        {
            InitializeComponent();
            CurrentSelection = "../Controls/CollectionControl.xaml";
            _defaultWidth = Width;
        }

        private bool _isCollapsed = false;

        public string CurrentSelection
        {
            get => (string) GetValue(CurrentSelectionProperty);
            set => SetValue(CurrentSelectionProperty, value);
        }

        private void ButtonCollapse_OnClick(object sender, RoutedEventArgs e)
        {
            _isCollapsed = !_isCollapsed;
            ButtonTracker.Visibility = (_isCollapsed) ? Visibility.Collapsed : Visibility.Visible;
            ButtonCollection.Visibility = (_isCollapsed) ? Visibility.Collapsed : Visibility.Visible;
            ImgCollapse.Source = new BitmapImage((_isCollapsed) ? PATH_COLLAPSE : PATH_OPEN);
            Width = _isCollapsed ? _collapsedWidth : _defaultWidth;
        }

        private void ButtonTracker_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentSelection = "../Controls/TrackerControl.xaml";
        }

        private void ButtonCollection_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentSelection = "../Controls/CollectionControl.xaml";
        }
    }
}