using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using PipeCalculator;
using System.Diagnostics;
using MaterialDesignThemes.Wpf;
using System.Windows.Markup;
using System.Linq;

namespace WpfPipeGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DocumentViewer? documentViewer1;
        private readonly PaletteHelper _paletteHelper = new();


        public MainWindow()
        {
            //var Spath = System.AppDomain.CurrentDomain.BaseDirectory;

            InitializeComponent();
            CreateListingEntryCuts();

            //menuItemTheme.UpdateLayout();
        }



        private void CreateListingEntryCuts()
        {
            var listEntryCuts = new ListingEntryCuts();
            listEntryCuts.DeleteMe += ListEntryCuts_DeleteMe;
            stackPanelPipeRef.Children.Add(listEntryCuts);

        }

        private void ListEntryCuts_DeleteMe(object? sender, EventArgs e)
        {
            stackPanelPipeRef.Children.Remove(sender as ListingEntryCuts);
        }

        private void PrintClick(object sender, RoutedEventArgs e)
        {
            CreateMyWPFControlReport(ResolveStackPanel);
            SaveCurrentDocument2();
            ;
        }

        private void PrintClickWPF(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new();
            printDlg.PrintVisual(ResolveStackPanel, "Pipe Printing");
        }


        public void CreateMyWPFControlReport(UIElement usefulData) // était HorizontalAxis
        {
            //Set up the WPF Control to be printed
            FrameworkElement controlToPrint = new();
            controlToPrint.DataContext = usefulData;
            controlToPrint.RenderTransform = new ScaleTransform(0.65, 0.65);

            FixedDocument fixedDoc = new();
            PageContent pageContent = new();
            FixedPage fixedPage = new();

            //Create first page of document
            fixedPage.Children.Add(controlToPrint);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);
            //Create any other required pages here
            documentViewer1 = new DocumentViewer
            {
                //View the document
                Document = fixedDoc
            };
        }

        public void SaveCurrentDocument2()
        {
            if (documentViewer1 == null) return;
            FixedDocument doc = (FixedDocument)documentViewer1.Document;
            foreach (var page in doc.Pages)
            {
                Console.WriteLine(page);
            }
            var dialog = new PrintDialog();
            var resultDialog = dialog.ShowDialog();
            if (resultDialog == true) dialog.PrintDocument(doc.DocumentPaginator, "Print");
        }

        private void ButtonSolveClick(object sender, RoutedEventArgs e)
        {
            DataToDraw.ClearAll();
            foreach (ListingEntryCuts item in stackPanelPipeRef.Children)
            {
                int baseLength = int.Parse(item.pipeBaseLength.Text);
                var listCut = new List<int>();
                if (item == null) continue;
                foreach (SelectionCut cut in item.listBoxCuts.Children)
                {
                    if (string.IsNullOrEmpty(cut.textBoxQt.Text) || string.IsNullOrEmpty(cut.textBoxLength.Text)) break;
                    var qt = int.Parse(cut.textBoxQt.Text);
                    var length = int.Parse(cut.textBoxLength.Text);
                    if (baseLength < length)
                    {
                        //MessageBox.Show("La longueur de coupe est supérieur à la longueur de base. Resolution des découpes impossible", "Erreur",MessageBoxButton.OK, MessageBoxImage.Stop);
                        bool? Result = new MessageBoxCustom("La longueur de coupe est supérieur à la longueur de base.\nCalcul des débits impossible", MessageType.Error, MessageButtons.Ok).ShowDialog();
                        DataToDraw.ClearAll(); // détruit le rendu de calcul
                        return;
                    }
                    for (int i = 0; i < qt; i++)
                    {
                        listCut.Add(length);
                    }

                }

                var description = string.IsNullOrEmpty(item.pipeDescription.Text) ? "sans Description" : item.pipeDescription.Text;
                var pipeManager = new PipeManager(description, baseLength, listCut);
                pipeManager.Process();
                _=new DataToDraw(description, baseLength, pipeManager.GetEnumerable());
                DataToDraw.BuildSolution(ResolveStackPanel);

            }
        }

        private void ButtonAddListingEntryCutsClick(object sender, RoutedEventArgs e)
        {
            CreateListingEntryCuts();
        }

        //gestion souris sur le scrollview
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is not ScrollViewer scrollViewer)
            {
                return;
            }

            if (e.Delta > 0)
            {
                scrollViewer.LineUp();
                scrollViewer.LineUp();
            }
            else
            {
                scrollViewer.LineDown();
                scrollViewer.LineDown();
            }
            e.Handled = true;
        }

        private void MenuItem_Click_Print(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg2 = new() { PageRangeSelection = PageRangeSelection.AllPages, UserPageRangeEnabled = true };
            bool? printResult = printDlg2.ShowDialog();
            if (printResult == true)
            {
                try
                {
                    printDlg2.PrintVisual(ResolveStackPanel, "Pipe Printing");
                }
                catch (Exception)
                {
                    bool? result = new MessageBoxCustom("impossible d'imprimer,\nerreur d'impression", MessageType.Error, MessageButtons.Ok).ShowDialog();
                }
            }
        }


        private void MenuItem_Click_Print2(object sender, RoutedEventArgs e)
        {
            //var pageSize = new Size(8.26*96*1.78, 22.69*96*1.78);
            var pageSize = new Size(1412,1998);
            var fixedDocument = new FixedDocument();
            fixedDocument.DocumentPaginator.PageSize = pageSize;
            //scroll.Content = null;
            UIElement[] elements = new UIElement[ResolveStackPanel.Children.Count];
            ResolveStackPanel.Children.CopyTo(elements, 0);
            ResolveStackPanel.Children.Clear();

            
            var fixedPage = new FixedPage() { Width = pageSize.Width, Height = pageSize.Height };
            //fixedPage.Measure(pageSize);
            //fixedPage.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));
            //fixedPage.UpdateLayout();
            PageContent pageContent;
            double totalHeight = 0;
            foreach (var child in elements)
            {
                totalHeight += ((FrameworkElement)child).ActualHeight;
                Debug.WriteLine(totalHeight);
                if (totalHeight < 500)
                {
                    
                    fixedPage.Children.Add(child);
                }
                else
                {
                    pageContent = new PageContent();
                    ((IAddChild)pageContent).AddChild(fixedPage);
                    fixedDocument.Pages.Add(pageContent);
                    fixedPage = new FixedPage() { Width = pageSize.Width, Height = pageSize.Height };
                    //fixedPage.Measure(pageSize);
                    //fixedPage.Arrange(new Rect(0, 0, pageSize.Width, pageSize.Height));
                    //fixedPage.UpdateLayout();
                    fixedPage.Children.Add(child);
                    totalHeight = ((FrameworkElement)child).ActualHeight;
                    Debug.WriteLine("new page");
                }
            }
            
            pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(fixedPage);
            fixedDocument.Pages.Add(pageContent);

            var printDlg = new PrintDialog();
            printDlg.PrintDocument(fixedDocument.DocumentPaginator, "Impression découpe");
            //fixedPage.Children.Clear();
            //foreach (var child in elements)
            //{
            //    ResolveStackPanel.Children.Add(child);
            //}
        }



        private void MenuItem_Click_New(object sender, RoutedEventArgs e)
        {
            stackPanelPipeRef.Children.Clear();
            CreateListingEntryCuts();
        }

        private void MenuItem_Click_Quit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            bool? result = new MessageBoxCustom("Logiciel de préparation de débit\nVersion du 05/03/2022", MessageType.About, MessageButtons.Ok).ShowDialog();
        }


        private void DarkMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = DarkMenuItem.IsChecked ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

    }


}
