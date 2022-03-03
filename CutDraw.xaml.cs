using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPipeGenerator
{
    /// <summary>
    /// Logique d'interaction pour SolvePipe.xaml
    /// </summary>
    public partial class CutDraw : FrameworkElement
    {

        private int scaleFactor = 5;

        private Pen mainPen;
        Brush? brushe;
        Brush? brusheLight;
        Brush? brushePaper;
        Brush? brusheBodyLight;
        Brush? brusheDarkForeground;
        Typeface? typeFaceArial;

        private double length;

        public CutDraw():this(100)
        {
            
        }

        public CutDraw(double _length)
        {
            length = _length;
            Width = _length /5;
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
            //ImageSource image = new BitmapImage(new Uri(@"C:\Users\Florent\source\repos\WpfPipeGenerator\Image\logo-AVITECH-M-A-blanc.png")) as ImageSource;

            //if (DataToDraw.dataToDraws == null) return;
            DrawPipeText(drawingContext, length, new Vector(0,0));
            //int positionY = 50;
            //foreach (var data in DataToDraw.dataToDraws)
            //{
            //    int positionLocalY = 0;
            //    drawingContext.DrawRoundedRectangle(brusheLight, mainPen, new Rect(0, positionY - 50, 6000 / scaleFactor + 50, (data.cutLength.Count() + 1) * 40), 5, 5);
            //    DrawTextSimplfy(drawingContext, data.description == null ? "?" : data.description, new Vector(0, positionY - 50), 20.0);
            //    foreach (var pipes in data.cutLength)
            //    {
            //        int positionX = 0;
            //        DrawPipe(drawingContext, data.lengthBase + 10 * scaleFactor, new Vector(25 - 5, positionLocalY + positionY - 5));
            //        foreach (var length in pipes)
            //        {
            //            DrawPipeText(drawingContext, length, new Vector(25 + positionX, positionLocalY + positionY));
            //            positionX += length / scaleFactor;
            //        }
            //        positionLocalY += 40;
            //    }

            //    positionY += 50 * (data.cutLength.Count() + 1);
            //}
        }
        void DrawPipeText(DrawingContext drawingContext, double length, Vector location)
        {
            drawingContext.DrawRoundedRectangle(brusheDarkForeground, mainPen, new Rect(location.X, location.Y+5, length / scaleFactor, 20), 5, 5);
            DrawTextSimplfy(drawingContext, length.ToString(), location+ new Vector(-3,7));
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
