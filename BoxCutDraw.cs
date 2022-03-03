using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfPipeGenerator
{
    internal class BoxCutDraw : FrameworkElement
    {
        private Pen mainPen;
        Brush? brushe;
        Brush? brusheLight;
        Brush? brushePaper;
        Brush? brusheBodyLight;
        Brush? brusheDarkForeground;
        Typeface? typeFaceArial;
        private int scaleFactor = 5;
        private double length;

        public BoxCutDraw() : this(100)
        {
        }

        public BoxCutDraw(double _length)
        {
            length = _length;
            Width = _length / 5;
            mainPen = new Pen(FindResource("PrimaryHueLightForegroundBrush") as Brush, 1.0);
            brushe = FindResource("PrimaryHueMidBrush") as Brush;
            brusheLight = FindResource("PrimaryHueLightBrush") as Brush;
            brushePaper = FindResource("MaterialDesignPaper") as Brush;
            brusheBodyLight = FindResource("MaterialDesignBodyLight") as Brush;
            brusheDarkForeground = FindResource("PrimaryHueDarkForegroundBrush") as Brush;
            typeFaceArial = new Typeface("Arial");
            ToolTip = $"Coupe à réaliser : {length}mm";
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            DrawPipeText(drawingContext, length, new Vector(0, 0));
        }
            void DrawPipeText(DrawingContext drawingContext, double length, Vector location)
        {
            drawingContext.DrawRoundedRectangle(brusheDarkForeground, mainPen, new Rect(location.X, location.Y + 5, length / scaleFactor, 20), 5, 5);
            DrawTextSimplfy(drawingContext, length.ToString(), location + new Vector(-3, 7));
        }

        void DrawPipe(DrawingContext drawingContext, double length, Vector location)
        {
            drawingContext.DrawRoundedRectangle(brushe, mainPen, new Rect(location.X, location.Y, length / scaleFactor, 30), 5, 5);
        }

        void DrawTextSimplfy(DrawingContext drawingContext, string text, Vector location, double size = 14.0)
        {
            drawingContext.DrawText(new FormattedText(text, System.Globalization.CultureInfo.GetCultureInfo("fr-fr"), FlowDirection.LeftToRight, typeFaceArial, size, Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip), new Point(location.X + 5, location.Y));
        }
    }
}
