using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TsTimeline
{
    public class MeasureRenderer : Control
    {
        public static readonly DependencyProperty ScrollViewerProperty =
            DepProp.Register<MeasureRenderer, ScrollViewer>(nameof(ScrollViewer),ScrollViewerChanged);

        public static readonly DependencyProperty ScaleProperty = 
            DepProp.Register<MeasureRenderer,double>(nameof(Scale),1d);
        
        public static readonly DependencyProperty ItemHeightProperty = 
            DepProp.Register<MeasureRenderer,double>(nameof(ItemHeight),15d);

        public static readonly DependencyProperty Alter0Property =
            DepProp.Register<MeasureRenderer, Brush>(nameof(Alter0),Brushes.FloralWhite);

        public static readonly DependencyProperty Alter1Property =
            DepProp.Register<MeasureRenderer, Brush>(nameof(Alter1),Brushes.WhiteSmoke);

        public ScrollViewer ScrollViewer
        {
            get => (ScrollViewer) GetValue(ScrollViewerProperty);
            set => SetValue(ScrollViewerProperty, value);
        }
        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }
        public double ItemHeight
        {
            get => (double) GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }
        public Brush Alter0
        {
            get { return (Brush) GetValue(Alter0Property); }
            set { SetValue(Alter0Property, value); }
        }

        public Brush Alter1
        {
            get { return (Brush) GetValue(Alter1Property); }
            set { SetValue(Alter1Property, value); }
        }

        private double _offset = 15;
        
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
            
            void OnScrollChanged(object sender, ScrollChangedEventArgs e)
            {
                RaiseInvalidateVisual();
            }
        }

        private Throttler _throttler;

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
            if (scale >= 0.25)
                return 200;
            if (scale >= 0.125)
                return 400;
            return 800;
        }
        
        protected override void OnRender(DrawingContext drawingContext)
        {
            int maxValue = (int) ((ActualWidth+ 0.5) * (1.0 / Scale) );
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

            DrawBackGround(drawingContext);
            DrawGrid(drawingContext);
        }

        private void DrawBackGround(DrawingContext drawingContext)
        {
            var pen = new Pen();

            var brushes = new [] {Alter0,Alter1};

            var h = (int)ItemHeight;

            var scrollOffset = (int) (-ScrollViewer?.VerticalOffset ?? 0);
            int index = -scrollOffset % (h * 2) > (h - 1) ? 1: 0;
            for (int i = scrollOffset%h + h; i < ActualHeight; i+=h)
            {
                if (i < h)
                {
                    drawingContext.DrawRectangle(brushes[index++%2],pen,new Rect(0,h,ActualWidth,i ));
                }
                else
                {
                    drawingContext.DrawRectangle(brushes[index++%2],pen,new Rect(0,i,ActualWidth,h));                    
                }
            }
        }

        private void DrawGrid(DrawingContext drawingContext)
        {    
            var pen = new Pen()
            {
                Brush = Brushes.LightGray,
                Thickness = 0.5d,
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
                var endY = 15;
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
            return x < offset;
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