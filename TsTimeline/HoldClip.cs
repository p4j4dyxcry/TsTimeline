using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TsTimeline
{
    [TemplatePart(Name="PART_LEFT", Type=typeof(Thumb))]
    [TemplatePart(Name="PART_RIGHT", Type=typeof(Thumb))]
    [TemplatePart(Name="PART_CENTER", Type=typeof(Thumb))]
    public class HoldClip : Control
    {
        public static readonly DependencyProperty StartValueProperty =
            DepProp.Register<HoldClip, double>(
                nameof(StartValue),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged);
        public double StartValue
        {
            get => (double) GetValue(StartValueProperty);
            set => SetValue(StartValueProperty, value);
        }

        public static readonly DependencyProperty EndValueProperty = 
            DepProp.Register<HoldClip, double>(
                nameof(EndValue),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged);

        public double EndValue
        {
            get => (double) GetValue(EndValueProperty);
            set => SetValue(EndValueProperty, value);
        }

        public static readonly DependencyProperty IsReadOnlyProperty = 
            DepProp.Register<HoldClip, bool>(nameof(IsReadOnly));

        public bool IsReadOnly
        {
            get => (bool) GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty =
            DepProp.Register<HoldClip, double>(nameof(Scale),1, OnValueChanged);

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HoldClip t)
            {
                t.UpdateThumbs();
            }
        }

        private double MaxValue => (int) (ActualWidth * (1.0 / Scale) + 0.5d);

        private void Right_OnDragDelta(Vector vector)
        {
            if (IsReadOnly)
                return;
            
            var change = Math.Ceiling(vector.X * (1.0d / Scale) - 0.5d);

            // 右側のクランプ
            if (EndValue + change > MaxValue)
            {
                change = MaxValue - EndValue;
            }
            // 左側のクランプ
            else if (EndValue + change <= StartValue)
            {
                change =  StartValue - EndValue + 1;
            }
            EndValue += change;
        }

        private void Center_OnDragDelta(Vector vector)
        {
            if (IsReadOnly)
                return;
            var change = Math.Ceiling(vector.X * (1.0d / Scale) - 0.5d);            
            var diff = ClampToCanvasDiff(change);

            StartValue += diff;
            EndValue += diff;
        }

        private void Left_OnDragDelta(Vector vector)
        {
            if (IsReadOnly)
                return;
            
            var change = Math.Ceiling(vector.X * (1.0d / Scale) - 0.5d);
            // 右側のクランプ
            if (StartValue + change >= EndValue)
            {
                change = EndValue - StartValue - 1;
            }
            // 左側のクランプ
            else if (StartValue + change < 0)
            {
                change = -StartValue;
            }

            StartValue += change;
        }

        public double ClampToCanvasDiff(double d)
        {
            if (StartValue + d <= 0)
                return -StartValue;

            if (EndValue + d >= MaxValue)
            {
                return MaxValue - EndValue;
            }

            return d;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TrySetupThumbs();
        }

        private Thumb _left;
        private Thumb _right;
        private Thumb _center;

        private bool TrySetupThumbs()
        {
            if (_left != null && _right != null && _center != null)
                return true;
            
            if (_left is null)
                _left = this.GetTemplateChild("PART_LEFT") as Thumb;
            if (_right is null)
                _right = this.GetTemplateChild("PART_RIGHT") as Thumb;
            if (_center is null)
                _center = this.GetTemplateChild("PART_CENTER") as Thumb;

            var result = _left != null && _right != null && _center != null;

            if (result)
            {
                var leftBinder = new ThumbDragToMousePointConverter(_left);
                leftBinder.BindDragDelta(Left_OnDragDelta);
                
                var rightBinder = new ThumbDragToMousePointConverter(_right);
                rightBinder.BindDragDelta(Right_OnDragDelta);
                
                var centerBinder = new ThumbDragToMousePointConverter(_center);
                centerBinder.BindDragDelta(Center_OnDragDelta);
                
                Loaded += (s, e) =>
                {
                    UpdateThumbs();
                };
            }
            
            return result;
        }
        
        private void UpdateThumbs()
        {
            if (TrySetupThumbs() is false)
                return;
            
            Canvas.SetLeft(_left,StartValue * Scale - _left.ActualWidth / 2);
            Canvas.SetLeft(_right,EndValue * Scale - _right.ActualWidth / 2);
            Canvas.SetLeft(_center,StartValue * Scale );
            
            var w = EndValue * Scale - StartValue * Scale;

            if(w > 0)
                _center.Width = w;
        }
    }
}