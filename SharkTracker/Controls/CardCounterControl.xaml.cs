using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SharkTrackerCore.Observation;

namespace SharkTracker.Controls
{
    public partial class CardCounterControl : UserControl, IObservable
    {
        public static readonly DependencyProperty QuantityProperty = DependencyProperty.Register("Quantity",
            typeof(int), typeof(CardCounterControl), new PropertyMetadata(default(int)));


        public int Minimum { get; set; } = 0;

        public int Maximum { get; set; } = 3;

        public int Quantity
        {
            get => (int) GetValue(QuantityProperty);
            set
            {
                SetValue(QuantityProperty, value);
                UpdateText();
            }
        }

        public CardCounterControl()
        {
            InitializeComponent();
            Quantity = 0;
        }

        private void Decrement(object sender, RoutedEventArgs e)
        {
            Quantity--;
            if (Quantity <= Minimum)
            {
                Quantity = Minimum;
            }
            NotifyAll();
        }

        private void Increment(object sender, RoutedEventArgs e)
        {
            Quantity++;
            if (Quantity >= Maximum)
            {
                Quantity = Maximum;
            }
            NotifyAll();
        }

        public void UpdateText()
        {
            quantityLabel.Content = Quantity.ToString();
        }
        
        private readonly List<Observer> _observers = new List<Observer>();

        /// <inheritdoc />
        public void Register(Observer o)
        {
            if (!_observers.Contains(o))
                _observers.Add(o);
        }

        /// <inheritdoc />
        public void Unregister(Observer o)
        {
            if (_observers.Contains(o))
                _observers.Remove(o);
        }

        /// <inheritdoc />
        public void NotifyAll()
        {
            _observers.ForEach(o => o.Notify());
        }
    }
}