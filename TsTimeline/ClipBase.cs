using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TsTimeline
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
    
    public class ClipBase : Control , ISelectable
    {
        // 実装速度優先でstaticで扱う。将来的にはTimelineControlから注入する形にする
        // こうしないとUIで2か所以上でTimeLineControlが使いづらくなる。
        public static readonly ClipSelectorService ClipSelectorService = new ClipSelectorService();
        
        public static readonly DependencyProperty IsSelectedProperty =
            DepProp.Register<ClipBase, bool>(nameof(IsSelected) , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,IsSelectedChanged);
        
        public static readonly DependencyProperty IsReadOnlyProperty =
            DepProp.Register<ClipBase, bool>(nameof(IsReadOnly));

        public static readonly DependencyProperty ScaleProperty =
            DepProp.Register<ClipBase, double>(nameof(Scale), 1, ValueChanged);
        
        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
        
        public bool IsReadOnly
        {
            get => (bool) GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        protected Canvas PART_Canvas;
        
        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClipBase t)
                t.OnScaleChanged();
        }
        
        private static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClipBase t)
                t.OnSelectedChanged();
        }

        protected virtual void OnScaleChanged()
        {
            
        }
        
        protected virtual void OnSelectedChanged()
        {
            ClipSelectorService.UpdateSelectedItems(this);
        }

        protected void OnMouseDownSelectedChanged()
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                IsSelected = true;
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                IsSelected ^= true;
            }
            else
            {
                ClipSelectorService.SingleSelect(this);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
//            PART_Canvas = this.GetTemplateChild("PART_CANVAS") as Canvas;
        }
    }
}