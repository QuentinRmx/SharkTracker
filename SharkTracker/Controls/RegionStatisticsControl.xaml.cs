using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SharkTracker.Models;
using SharkTrackerCore.Models;

namespace SharkTracker.Controls
{
    public partial class RegionStatisticsControl : UserControl
    {
        public static readonly DependencyProperty RegionCardsProperty = DependencyProperty.Register("RegionCards",
            typeof(List<Card>), typeof(RegionStatisticsControl), new PropertyMetadata(default(List<Card>)));

        public static readonly DependencyProperty NbChampionsProperty = DependencyProperty.Register("NbChampions",
            typeof(int), typeof(RegionStatisticsControl), new PropertyMetadata(default(int)));

        public RegionStatisticsControl()
        {
            InitializeComponent();
            UpdateStats();
        }

        public List<Card> RegionCards
        {
            get => (List<Card>) GetValue(RegionCardsProperty);
            set
            {
                SetValue(RegionCardsProperty, value);
                UpdateStats();
            }
        }

        public int NbChampions
        {
            get => (int) GetValue(NbChampionsProperty);
            set
            {
                SetValue(NbChampionsProperty, value);
                UpdateStats();
            }
        }

        private void UpdateStats()
        {
            // if (RegionCards == null)
            //     return;
            LabelTotalChampions.Content = NbChampions.ToString();
        }
    }
}