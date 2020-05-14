using System.Windows;
using System.Windows.Controls;

namespace SharkTracker.Behaviors
{
    public class ScrollViewerBehavior
    {
        // ATTRIBUTES

        // CONSTRUCTORS

        // METHODS
        public static bool GetAutoScrollToTop(DependencyObject obj)
        {
            return (bool) obj.GetValue(AutoScrollToTopProperty);
        }

        public static void SetAutoScrollToTop(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToTopProperty, value);
        }

        public static readonly DependencyProperty AutoScrollToTopProperty =
            DependencyProperty.RegisterAttached("AutoScrollToTop", typeof(bool), typeof(ScrollViewerBehavior),
                new PropertyMetadata(false, (o, e) =>
                {
                    if (!(o is ScrollViewer scrollViewer))
                    {
                        return;
                    }

                    if ((bool) e.NewValue)
                    {
                        scrollViewer.ScrollToTop();
                        SetAutoScrollToTop(o, false);
                    }
                }));
    }
}