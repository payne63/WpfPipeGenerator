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

namespace WpfPipeGenerator.Pages
{
    /// <summary>
    /// Logique d'interaction pour PagePipe.xaml
    /// </summary>
    public partial class PagePipe : Page
    {
        private List<int> Pipes;
        public PagePipe()
        {
            InitializeComponent();
            DataContext = this;
            Pipes = new List<int> { 100,300 ,600,200,1200 };
            initPipe();
        }

        private void initPipe()
        {
            StackPanel pipePanel = new StackPanel() { Orientation = Orientation.Horizontal};
            foreach (var pipe in Pipes)
            {
                Button button = new Button() { Content = pipe.ToString(), Width = pipe };
                pipePanel.Children.Add(button);
            }
            sPanel.Children.Add(pipePanel);
        }
    }
}
