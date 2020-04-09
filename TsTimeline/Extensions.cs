using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace TsTimeline
{

    public static class VisualTreeExtensions
    {
        public static T FindChild<T>(this FrameworkElement root, Func<FrameworkElement, bool> compare = null) 
            where T : FrameworkElement
        {
            if (compare is null)
                compare = x => true;
            
            var children = Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(root)).Select(x => VisualTreeHelper.GetChild(root, x)).OfType<FrameworkElement>().ToArray();

            foreach (var child in children)
            {
                if (child is T t && compare(child))
                    return t;

                t = child.FindChild<T>(compare);
                if (t != null)
                    return t;
            }
            return null;
        }
    }
    
    public enum TextAlignment
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        TopCenter,
        BottomCenter,
        LeftCenter,
        RightCenter,
        Center
    }
    
    public static class DrawingContextExtensions
    {
        public static void DrawTextEx(this DrawingContext @this , string text , double x, double y , TextAlignment alignment = TextAlignment.TopLeft)
        {
            var typeface = new Typeface(SystemFonts.MessageFontFamily , FontStyles.Normal, FontWeights.Regular, FontStretches.Normal);
            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                8,
                Brushes.Black,
                1);

            var w = text.Length * 4.5;
            var h = 8;
            
            switch (alignment)
            {
                case TextAlignment.TopLeft:
                    break;
                case TextAlignment.TopRight:
                    x -= w;
                    break;
                case TextAlignment.BottomLeft:
                    y += h;
                    break;
                case TextAlignment.BottomRight:
                    x -= w;
                    y += h;
                    break;
                case TextAlignment.TopCenter:
                    x -= w / 2d;
                    break;
                case TextAlignment.BottomCenter:
                    x -= w / 2d;
                    y += h;
                    break;
                case TextAlignment.LeftCenter:
                    y += h / 2d;
                    break;
                case TextAlignment.RightCenter:
                    x -= w;
                    y += h / 2d;
                    break;
                case TextAlignment.Center:
                    x -= w / 2d;
                    y += h / 2d;
                    break;
            }
            
            @this.DrawText(formattedText,new Point(x,y));
        }
    }

    public static class MathEx
    {
        public static int SnapInt(double d, int multiple)
        {
            return (int)(d ) / multiple * multiple;
        }

        public static double Snap(double d, double multiple)
        {
            return  (int)((d)/multiple) * multiple;
        }

        public static int Mod(double d, int value)
        {
            return (int)(d) % value;
        }
    }
    
}