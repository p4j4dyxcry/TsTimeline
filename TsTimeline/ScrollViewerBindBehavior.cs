using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace TsTimeline
{
    public class ScrollViewerBindBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty TargetProperty = 
            DepProp.Register<ScrollViewerBindBehavior,ScrollViewer>(nameof(Target),OnScrollViewerChanged);

        public ScrollViewer Target
        {
            get => (ScrollViewer) GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }
        
        private static void OnScrollViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewerBindBehavior b)
            {
               b.AttachScrollViewer();
            }
        }

        private void AttachScrollViewer()
        {
            AssociatedObject.ScrollChanged += AssociationToSource;
            Target.ScrollChanged += SourceToAssociation;
        }
        
        private void DetachScrollViewer()
        {
            AssociatedObject.ScrollChanged -= AssociationToSource;
            Target.ScrollChanged -= SourceToAssociation;
        }

        static void SyncScrollViewer(ScrollViewer source, ScrollViewer target)
        {
            target.ScrollToHorizontalOffset(source.HorizontalOffset);
            target.ScrollToVerticalOffset(source.VerticalOffset);
        }
        
        void SourceToAssociation( object sender , ScrollChangedEventArgs args)
        {
            SyncScrollViewer(Target,AssociatedObject);
        }
        
        void AssociationToSource( object sender , ScrollChangedEventArgs args)
        {
            SyncScrollViewer(AssociatedObject,Target);
        }

        protected override void OnDetaching()
        {
            DetachScrollViewer();
            base.OnDetaching();
        }
    }
}