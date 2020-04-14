using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace TsTimeline
{
    /// <summary>
    /// Thumbのドラッグデルタをドラッグ開始地点からの差分に変換してイベントにバインドします。
    /// </summary>
    public class ThumbDragToMousePointConverter
    {
        private Point _prevPoint = new Point(0,0);
        private readonly Thumb _thumb;
        
        public ThumbDragToMousePointConverter(Thumb thumb , Action mouseDown)
        {
            _thumb = thumb;
            _thumb.DragStarted += (s, e) =>
            {
                _prevPoint = Mouse.GetPosition(thumb);
                mouseDown?.Invoke();
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