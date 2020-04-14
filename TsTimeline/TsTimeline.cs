using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TsTimeline
{
    [TemplatePart(Name="PART_SCROLL_VIEWER", Type=typeof(ScrollViewer))]
    public class TsTimeline : Control
    {
        public static readonly DependencyProperty MaximumProperty =
            DepProp.Register<TsTimeline, double>(nameof(Maximum) , 1000d, FrameworkPropertyMetadataOptions.AffectsMeasure);

        public double Maximum
        {
            get => (double) GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty MinimumProperty =             
            DepProp.Register<TsTimeline, double>(nameof(Minimum) , 0d, FrameworkPropertyMetadataOptions.AffectsMeasure);

        public double Minimum
        {
            get => (double) GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty = 
            DepProp.Register<TsTimeline, double>(nameof(Scale) , 12d, FrameworkPropertyMetadataOptions.AffectsMeasure);

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty TrackHeightProperty = 
            DepProp.Register<TsTimeline, double>(nameof(TrackHeight) , 15d);

        public double TrackHeight
        {
            get => (double) GetValue(TrackHeightProperty);
            set => SetValue(TrackHeightProperty, value);
        }

        public static readonly DependencyProperty TracksProperty =
            DepProp.Register<TsTimeline, IEnumerable>(nameof(Tracks));
        public IEnumerable Tracks
        {
            get => (IEnumerable) GetValue(TracksProperty);
            set => SetValue(TracksProperty, value);
        }

        public static readonly DependencyPropertyKey CanvasActualWidthPropertyKey = 
            DepProp.RegisterReadOnly<TsTimeline, double>(nameof(CanvasActualWidth));

        public static readonly DependencyProperty CanvasActualWidthProperty = CanvasActualWidthPropertyKey.DependencyProperty;
        
        public double CanvasActualWidth
        {
            get => (double) GetValue(CanvasActualWidthProperty);
            private set => SetValue(CanvasActualWidthPropertyKey, value);
        }

        internal static readonly DependencyPropertyKey ScrollViewerPropertyKey = 
            DepProp.RegisterReadOnly<TsTimeline, ScrollViewer>(nameof(ScrollViewer));
        
        public static readonly DependencyProperty ScrollViewerProperty = ScrollViewerPropertyKey.DependencyProperty;
        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer) GetValue(ScrollViewerProperty); }
            private set => SetValue(ScrollViewerPropertyKey, value);
        }

        public static readonly DependencyProperty Alter0Property =
            DepProp.Register<TsTimeline, Brush>(nameof(Alter0),Brushes.FloralWhite);

        public Brush Alter0
        {
            get { return (Brush) GetValue(Alter0Property); }
            set { SetValue(Alter0Property, value); }
        }
        
        public static readonly DependencyProperty Alter1Property =
            DepProp.Register<TsTimeline, Brush>(nameof(Alter1),Brushes.WhiteSmoke);

        public Brush Alter1
        {
            get { return (Brush) GetValue(Alter1Property); }
            set { SetValue(Alter1Property, value); }
        }
        
        public TsTimeline()
        {
            LayoutUpdated += (s, e) =>
            {
                CanvasActualWidth = Maximum * Scale;
            };

            PreviewMouseWheel += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    var delta = 1f + e.Delta * 0.001f;

                    var ss = Scale * delta;

                    Scale = Math.Min(Math.Max(0.125f, ss), 32.0f);
                    //Console.WriteLine(Scale);
                    e.Handled = true;
                }
            };

            PreviewMouseMove += (s, e) => { InvalidateVisual(); };
            MouseLeave += (s, e) => { InvalidateVisual(); };
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            // double start = 0;
            //
            // if(ScrollViewer is null)
            //     ScrollViewer = this.FindChild<ScrollViewer>();
            //
            // if (ScrollViewer != null)
            //     start = ScrollViewer.HorizontalOffset;
            //
            //
            // var pen = new Pen(Brushes.RoyalBlue,1);
            // var mouse = Mouse.GetPosition(this);
            //
            // var x = MathEx.Snap(mouse.X + Scale * 0.5, Scale);
            // var startSnap = MathEx.Snap(mouse.X + start + Scale * 0.5, Scale) - x - start;
            //
            // drawingContext.DrawLine(pen,new Point(x + startSnap,0)  ,new Point(x + startSnap,ActualHeight) );
            // drawingContext.DrawTextEx($"{Math.Ceiling((x + start - 0.5 ) * (1.0 / Scale)) }",mouse.X + 2, mouse.Y - 10);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ScrollViewer = GetTemplateChild("PART_SCROLL_VIEWER") as ScrollViewer;
        }
    }
}