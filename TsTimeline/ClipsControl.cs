using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TsTimeline
{
    public class ClipsControl : ItemsControl , ISelectable
    {
        public SelectorService SelectorService => SelectorService.Default;
        
        public static readonly DependencyProperty LastMouseDownXProperty =
            DepProp.Register<ClipsControl, double>(nameof(LastMouseDownX));

        public double LastMouseDownX
        {
            get => (double) GetValue(LastMouseDownXProperty);
            set => SetValue(LastMouseDownXProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty =
            DepProp.Register<ClipsControl, double>(nameof(Scale));

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DepProp.Register<ClipsControl, bool>(nameof(IsSelected) , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault , SelectedChanged);

        private static void SelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ClipsControl c)
                c.OnSelectedChanged();
        }

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public ClipsControl()
        {
            PreviewMouseDown += (s, e) =>
            {
                LastMouseDownX = e.GetPosition(this).X * (1.0 / Scale);
            };
        }
        
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            OnMouseDownSelectedChanged();
            base.OnMouseDown(e);
            e.Handled = true;
        }

        private void OnSelectedChanged()
        {
            SelectorService.UpdateSelectedItems(this);
        }

        private void OnMouseDownSelectedChanged()
        {
            SelectorService.MouseDownSelectionChanged(this);
        }
    }
}