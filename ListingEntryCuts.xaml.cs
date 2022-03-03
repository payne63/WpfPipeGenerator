using PipeCalculator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logique d'interaction pour ListingEntryCuts.xaml
    /// </summary>
    public partial class ListingEntryCuts : UserControl
    {

        public event EventHandler? DeleteMe;

        public ListingEntryCuts()
        {
            InitializeComponent();
            CreateSelectionCut();
        }


        private void CreateSelectionCut()
        {
            var newSelectionCut = new SelectionCut();
            newSelectionCut.DeleteMe += DeleteSelectionCut;
            newSelectionCut.AddNext += AddNewSelectionCutToList;
            listBoxCuts.Children.Add(newSelectionCut);
         }

        private void DeleteSelectionCut(object? sender, EventArgs e)
        {
            if (sender == null) return;
            listBoxCuts.Children.Remove(sender as UIElement);
            if (listBoxCuts.Children.Count < 1) CreateSelectionCut();

        }

        private void AddNewSelectionCutToList(object? sender, EventArgs e) => CreateSelectionCut();

        private void textBoxLength_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = Regex.IsMatch(e.Text, "[^0-9.]+");
        private void addSelectionCutItem_StartResolve(object sender, EventArgs e)
        {
            var listCut = new List<int>();
            foreach (var item in listBoxCuts.Children)
            {
                switch (item)
                {
                    case SelectionCut cut:
                        if (string.IsNullOrEmpty( cut.textBoxQt.Text)  || string.IsNullOrEmpty(cut.textBoxLength.Text)) break;
                        var qt = int.Parse(cut.textBoxQt.Text);
                        var length = int.Parse(cut.textBoxLength.Text);
                        for (int i = 0; i < qt; i++)
                        {
                            listCut.Add(length);
                        }
                        break;
                    default:
                        break;
                }
            }
            Debug.WriteLine("start resolve");
            var description = string.IsNullOrEmpty(pipeDescription.Text)? "????": pipeDescription.Text;
            var pipeManager = new PipeManager(description, 6000, listCut);
            pipeManager.Process();
            var result = pipeManager.GetEnumerable();
            Debug.WriteLine($"description :{description}");
            foreach (var pipe in result)
            {
                foreach (var cut in pipe)
                {
                    Debug.Write($"-{cut}");
                }
                Debug.WriteLine("");
            }
        }

        private void ButtonDeleteListEntryCuts_Click(object sender, RoutedEventArgs e)
        {
            DeleteMe?.Invoke(this, EventArgs.Empty);
        }
    }
}
