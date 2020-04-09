using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace TsTimeline
{
    public class DrawerControl : Control
    {
        private readonly Action<DrawingContext> _render;
        public DrawerControl(Action<DrawingContext> render)
        {
            _render = render;
            UseLayoutRounding = true;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            _render?.Invoke(drawingContext);
        }
    }
}