using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace TsTimeline
{
    public sealed class BubbleDoubleClickBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty CommandProperty =
            DepProp.Register<BubbleDoubleClickBehavior, ICommand>(nameof(Command));

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
        }

        private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
            {
                var parent = AssociatedObject.FindVisualParentWithType<ClipsControl>();

                var parameter = (int)(parent?.LastMouseDownX ?? e.GetPosition(AssociatedObject).X);

                if (Command?.CanExecute(parameter) is true)
                {
                    Command.Execute(parameter);
                    e.Handled = true;                    
                }
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseDown -= AssociatedObject_PreviewMouseDown;
            base.OnDetaching();
        }
    }
}