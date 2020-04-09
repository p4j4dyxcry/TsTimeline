using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace TsTimeline
{
    public class ThumbDragToMousePointConverter
    {
        private Point _prevPoint = new Point(0,0);
        private readonly Thumb _thumb;
        
        public ThumbDragToMousePointConverter(Thumb thumb)
        {
            _thumb = thumb;
            _thumb.DragStarted += (s, e) =>
            {
                _prevPoint = Mouse.GetPosition(thumb);
            };
        }

        public void BindDragDelta(Action<Vector> function)
        {
            _thumb.DragDelta += (s, e) =>
            {
                var point = Mouse.GetPosition(_thumb);
                function?.Invoke(point - _prevPoint);
            };
        }
    }
}