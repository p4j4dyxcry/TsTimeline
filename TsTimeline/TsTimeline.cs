using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TsTimeline
{
    public class TsTimeline : Control
    {
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(TsTimeline), new FrameworkPropertyMetadata(1000d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double Maximum
        {
            get => (double) GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(TsTimeline), new PropertyMetadata(0d));

        public double Minimum
        {
            get => (double) GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
            "Scale", typeof(double), typeof(TsTimeline), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double Scale
        {
            get => (double) GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty TrackHeightProperty = DependencyProperty.Register(
            "TrackHeight", typeof(double), typeof(TsTimeline), new PropertyMetadata(15d));

        public double TrackHeight
        {
            get => (double) GetValue(TrackHeightProperty);
            set => SetValue(TrackHeightProperty, value);
        }

        public static readonly DependencyProperty TracksProperty = DependencyProperty.Register(
            "Tracks", typeof(IEnumerable), typeof(TsTimeline), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable Tracks
        {
            get => (IEnumerable) GetValue(TracksProperty);
            set => SetValue(TracksProperty, value);
        }

        public static readonly DependencyProperty CanvasActualWidthProperty = DependencyProperty.Register(
            "CanvasActualWidth", typeof(double), typeof(TsTimeline), new FrameworkPropertyMetadata(300d));

        public double CanvasActualWidth
        {
            get => (double) GetValue(CanvasActualWidthProperty);
            set => SetValue(CanvasActualWidthProperty, value);
        }
        
        public ScaleTransform ScaleTransform { get; } = new ScaleTransform(1,1);

        public TsTimeline()
        {
            this.LayoutTransform = ScaleTransform;

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

                    Scale = Math.Min(Math.Max(0.25f, ss), 32.0f);
                    //Console.WriteLine(Scale);
                    e.Handled = true;
                }
            };

            PreviewMouseMove += (s, e) => { InvalidateVisual(); };
            MouseLeave += (s, e) => { InvalidateVisual(); };
        }

        private ScrollViewer _scrollViewer;
        protected override void OnRender(DrawingContext drawingContext)
        {
            double start = 0;
            
            if(_scrollViewer is null)
                _scrollViewer = this.FindChild<ScrollViewer>();

            if (_scrollViewer != null)
                start = _scrollViewer.HorizontalOffset;
            
            
            var pen = new Pen(Brushes.RoyalBlue,1);
            var mouse = Mouse.GetPosition(this);

            var x = MathEx.Snap(mouse.X + Scale * 0.5, Scale);
            var startSnap = MathEx.Snap(mouse.X + start + Scale * 0.5, Scale) - x - start;

            drawingContext.DrawLine(pen,new Point(x + startSnap,0)  ,new Point(x + startSnap,ActualHeight) );
            
            drawingContext.DrawTextEx($"{Math.Ceiling((x + start - 0.5 ) * (1.0 / Scale)) }",mouse.X + 2, mouse.Y - 10,TextAlignment.TopLeft);
        }
    }
}