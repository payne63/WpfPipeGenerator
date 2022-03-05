using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Media;

namespace WpfPipeGenerator
{
    class DataToDraw
    {
        public static List<DataToDraw>? dataToDraws;

        public string? description;
        public double lengthBase;
        public IEnumerable<IEnumerable<int>> cutLength;

        public DataToDraw(string description, double lengthBase, IEnumerable<IEnumerable<int>> cutLength)
        {
            this.description = description;
            this.lengthBase = lengthBase;
            this.cutLength = cutLength;
            if (dataToDraws == null) dataToDraws = new List<DataToDraw>();
            dataToDraws.Add(this);
        }

        internal static void ClearAll()
        {
            if (dataToDraws != null) dataToDraws.Clear();
        }

        public static void BuildSolution(Panel uiElement)
        {
            uiElement.Children.Clear();
            //add Title
            uiElement.Children.Add(new TextBox()
            {
                Text = "Feuille de Débits",
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 25,
                FontStyle = new FontStyle(),
                Margin = new Thickness(3)
            });
            if (dataToDraws == null) return;
            if (dataToDraws.Count == 0) return;
            foreach (var dataToDraw in dataToDraws)
            {
                //add description pipe
                uiElement.Children.Add(new Button()
                {
                    Content = dataToDraw.description,
                    FontSize = 20,
                    Width = 1405,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(2, 10, 2, 0),
                });
                var localStackPanel = new StackPanel() { Orientation = Orientation.Vertical, Margin = new Thickness(1) };
                var counter = 1;
                foreach (var pipe in dataToDraw.cutLength)
                {
                    var localButtonPipe = new Button() { HorizontalAlignment = HorizontalAlignment.Left, Height = 35, Margin = new Thickness(1) };
                    var buttonStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left };
                    //var buttonStackPanel = new DockPanel() { HorizontalAlignment = HorizontalAlignment.Left } ;
                    localButtonPipe.Content = buttonStackPanel;
                    buttonStackPanel.Children.Add(new Button()
                    {
                        Content = counter,
                        FontWeight = FontWeights.Bold,
                        ToolTip = "Numéro du tube",
                        Height = 30,
                        Width = 50,
                        Background = Application.Current.FindResource("PrimaryHueLightBrush") as Brush
                    }); ;
                    foreach (var cut in pipe)
                    {
                        //var buttonCut = new Button() { Content = cut.ToString() ,Width=cut/5 };
                        var buttonCut = new BoxCutDraw(cut);
                        //DockPanel.SetDock(buttonCut, Dock.Left);
                        buttonStackPanel.Children.Add(buttonCut);

                    }

                    var LengthRest = dataToDraw.lengthBase - pipe.Sum();
                    buttonStackPanel.Children.Add(new Button()
                    {
                        Content = $"chute:{LengthRest}mm",
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Background = Application.Current.FindResource("PrimaryHueLightBrush") as Brush,
                        Height = 30,
                        Width = 120 + LengthRest / 5,
                        ToolTip = "reste après découpe"
                    });
                    localStackPanel.Children.Add(localButtonPipe);
                    counter++;
                }

                //add all cuts
                uiElement.Children.Add(localStackPanel);


            }
        }
    }
}
