using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace TsTimeline
{
    public class SelectableBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseDown += (s, e) =>
            {
                if (AssociatedObject.DataContext is ISelectable selectable)
                {
                    SelectorService.Default.ClearSelect();
                    selectable.IsSelected = true;
                    SelectorService.Default.RaiseSelectionChanged();
                    e.Handled = true;
                }
            };
        }
    }
}