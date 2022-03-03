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
    /// Logique d'interaction pour AddSelectionCut.xaml
    /// </summary>
    public partial class AddSelectionCut : UserControl
    {
        public event EventHandler? StartResolve;

        public event EventHandler? AddNewSelectionCutToList;
        public AddSelectionCut()
        {
            InitializeComponent();
        }

        private void AddNewSelectionCut(object? sender, EventArgs e)
        {
            AddNewSelectionCutToList?.Invoke(this, EventArgs.Empty);
        }

        private void ResolveSelectionCut(object? sender, EventArgs e)
        {
            StartResolve?.Invoke(this, EventArgs.Empty);
            
        }
    }
}
