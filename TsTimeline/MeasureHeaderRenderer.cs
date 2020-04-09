using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TsTimeline
{
    public class MeasureHeaderRenderer : Control
    {
        public static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.Register(
            "ScrollViewer", typeof(ScrollViewer), typeof(MeasureHeaderRenderer),
            new PropertyMetadata(default(ScrollViewer), ScrollViewerChanged));

        public ScrollViewer ScrollViewer
        {
            get => (ScrollViewer) GetValue(ScrollViewerProperty);
            set => SetValue(ScrollViewerProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(MeasureHeaderRenderer), new PropertyMetadata(1d));

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        private static void ScrollViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MeasureHeaderRenderer m)
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

        private Throttler throttler;
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (Math.Abs(e.HorizontalChange) > 0)
                RaiseInvalidateVisual();
        }

        private void RaiseInvalidateVisual()
        {
            if(throttler is null)
                throttler = new Throttler(TimeSpan.FromMilliseconds(10) , InvalidateVisual);
            throttler.Invoke();
        }

        public MeasureHeaderRenderer()
        {

        }
        
        
        protected override void OnRender(DrawingContext drawingContext)
        {
            double offset = 20;
            double prev = -offset;
            int maxValue = (int) (ActualWidth * (1.0 / Scale) + 0.5);

            double startValue = 0;
            double endValue = maxValue;
            
            if (ScrollViewer != null)
            {
                startValue = ScrollViewer.HorizontalOffset;
                endValue = ScrollViewer.ActualWidth;
            }
            
            for (int i = 0; i <= maxValue; i++)
            {
                var snapedScale = MathEx.Snap(i * Scale, 5);


                var x = (i * Scale);

                if (snapedScale - prev < offset)
                    continue;
                                
                if (x < startValue)
                    continue;

                if (x >= startValue + endValue)
                    break;

                prev = snapedScale;

                var alignment = TextAlignment.TopCenter;
                if (i == 0)
                    alignment = TextAlignment.TopLeft;

                drawingContext.DrawTextEx($"{i}", x - startValue, 0, alignment);
            }

            RenderLine(drawingContext);
        }

        private StreamGeometry _streamGeometry = new StreamGeometry();

        private void RenderLine(DrawingContext drawingContext)
        {    
            var pen = new Pen()
            {
                Brush = Brushes.Black,
                Thickness = 0.1d,
            };

            var drawcall = 0;
            var length = ActualWidth;
            if (Scale < 1)
                length = ActualWidth * (1.0f / Scale);

            var interval = (int) MathEx.Snap(10 * (1.0 / Scale), 1);

            if (interval <= 0)
                interval = 1;

            double startValue = 0;
            double endValue = length;

            if (ScrollViewer != null)
            {
                startValue = ScrollViewer.HorizontalOffset;
                endValue = ScrollViewer.ActualWidth;
            }
            for (double i = 0; i < length; i += interval)
            {
                double x = i * Scale;

                // 画面サイズ以上の描画はキャンセル

                if (x < startValue)
                    continue;

                if (x >= startValue + endValue)
                    break;

                var h = ActualHeight;

                if (Parent is FrameworkElement u)
                    h = u.ActualHeight;

                var beginY = h;
                var endY = 0;

                var b = ((int) 10 * 10);
                drawcall++;
                drawingContext.DrawLine(pen, new Point(x - startValue, beginY) , new Point(x - startValue, endY) );
            }

            Console.WriteLine($"drawcall-{drawcall}");
        }
    }
}