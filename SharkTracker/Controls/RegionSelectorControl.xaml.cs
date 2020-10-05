using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SharkTrackerCore.Models;

namespace SharkTracker.Controls
{
    public partial class RegionSelectorControl : UserControl
    {
        public static readonly DependencyProperty OnRegionChangedProperty =
            DependencyProperty.Register("OnRegionChanged", typeof(ICommand), typeof(RegionSelectorControl),
                new PropertyMetadata(default(ICommand)));

        public ERegion SelectedRegion { get; set; }

        public ICommand OnRegionChanged
        {
            get => (ICommand) GetValue(OnRegionChangedProperty);
            set => SetValue(OnRegionChangedProperty, value);
        }

        public RegionSelectorControl()
        {
            InitializeComponent();
        }

        private void Update()
        {
            OnRegionChanged?.Execute(SelectedRegion);
        }

        private void BtnBilgewater_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedRegion = ERegion.Bilgewater;
            Update();
        }

        private void BtnDemacia_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedRegion = ERegion.Demacia;
            Update();
        }

        private void BtnFreljord_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedRegion = ERegion.Freljord;
            Update();
        }

        private void BtnIonia_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedRegion = ERegion.Ionia;
            Update();
        }

        private void BtnNoxus_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedRegion = ERegion.Noxus;
            Update();
        }

        private void BtnPnZ_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedRegion = ERegion.PnZ;
            Update();
        }

        private void BtnSi_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedRegion = ERegion.Si;
            Update();
        }
        
        private void BtnTargon_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedRegion = ERegion.Targon;
            Update();
        }

        private void BtnAll_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedRegion = ERegion.ALL;
            Update();
        }
    }
}