using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TsTimeline
{
    public class MeasureRenderer : Control
    {
        public static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.Register(
            "ScrollViewer", typeof(ScrollViewer), typeof(MeasureRenderer),
            new PropertyMetadata(default(ScrollViewer), ScrollViewerChanged));

        public ScrollViewer ScrollViewer
        {
            get => (ScrollViewer) GetValue(ScrollViewerProperty);
            set => SetValue(ScrollViewerProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(MeasureRenderer), new PropertyMetadata(1d));

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        private static void ScrollViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MeasureRenderer m)
                m.OnScrollViewerChanged(e.NewValue as ScrollViewer, e.OldValue as ScrollViewer);
        }

        private void OnScrollViewerChanged(ScrollViewer newValue, ScrollViewer oldValue)
        {
            if (newValue != null)
                newValue.ScrollChanged += OnScrollChanged;
            if (oldValue != null)
                oldValue.ScrollChanged -= OnScrollChanged;

            RaiseInvalidateVisual();
        }

        private Throttler _throttler;
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (Math.Abs(e.HorizontalChange) > 0)
                RaiseInvalidateVisual();
        }

        private void RaiseInvalidateVisual()
        {
            InvalidateVisual();
            if(_throttler is null)
                _throttler = new Throttler(TimeSpan.FromMilliseconds(0) , InvalidateVisual);
            _throttler.Invoke();
        }

        public MeasureRenderer()
        {
            this.SizeChanged += (s, e) =>
            {
                RaiseInvalidateVisual();
            };
        }

        int scale_factor(double scale)
        {
            if (scale >= 16)
                return 1;
            if (scale >= 8)
                return 5;
            if (Scale >= 4)
                return 10;
            if (scale >= 2)
                return 25;
            if (scale >= 1)
                return 50;
            if (scale >= 0.5)
                return 100;
            return 200;
        }
        
        protected override void OnRender(DrawingContext drawingContext)
        {
            int maxValue = (int) (ActualWidth * (1.0 / Scale) + 0.5);
            var factor = scale_factor(Scale);
            
            for (int i = 0; i <= maxValue; i+= factor)
            {
                var x = (i * Scale);

                // 画面サイズ以上の描画はキャンセル
                if (IsLeftOutside(x))
                    continue;

                if (IsRightOutSide(x))
                    break;

                var alignment = TextAlignment.TopCenter;
                if (i == 0)
                    alignment = TextAlignment.TopLeft;

                var drawPoint = OffsetPoint(x, 0);
                
                drawingContext.DrawTextEx($"{i}", drawPoint.X, drawPoint.Y, alignment);
            }

            RenderLine(drawingContext);
        }

        private void RenderLine(DrawingContext drawingContext)
        {    
            var pen = new Pen()
            {
                Brush = Brushes.Black,
                Thickness = 0.1d,
            };

            var lineInterval = 10;
            var maxValue = ActualWidth * (1.0f / Scale);
            var interval = Math.Max((int) MathEx.Snap(lineInterval * (1.0 / Scale), 1) , 1);

            for (double i = 0; i < maxValue; i += interval)
            {
                double x = i * Scale;

                // 画面サイズ以上の描画はキャンセル
                if (IsLeftOutside(x))
                    continue;

                if (IsRightOutSide(x))
                    break;

                var beginY = ActualHeight;
                var endY = 10;
                drawingContext.DrawLine(pen, OffsetPoint(x,beginY), OffsetPoint(x,endY) );
            }
        }

        private Point OffsetPoint(double x, double y)
        {
            if (ScrollViewer != null)
            {
                return new Point(x - ScrollViewer.HorizontalOffset , y);                
            }
            return new Point(x,y);
        }

        bool IsLeftOutside(double x)
        {
            var offset = 0d;
            if (ScrollViewer != null)
            {
                offset = ScrollViewer.HorizontalOffset;                
            }
            return x <= offset;
        }
        
        bool IsRightOutSide(double x)
        {
            var offset = ActualWidth;
            if (ScrollViewer != null)
            {
                offset = ScrollViewer.HorizontalOffset + ScrollViewer.ActualWidth;
            }
            return x >= offset;
        }
        
    }
}