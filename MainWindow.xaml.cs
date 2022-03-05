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
            //foreach (var col in  (MaterialDesignColors.PrimaryColor[])Enum.GetValues( typeof(MaterialDesignColors.PrimaryColor)))
            //{
            //    var menuItemColor = new MenuItemColor()
            //    {
            //        Header = col.ToString(),
            //        Icon = PackIconKind.Boat,
            //        primaryColor = col,
            //    };
            //    menuItemColor.Click += MenuItemColor_ColorChangedSelected;
            //    menuItemTheme.Items.Add(menuItemColor);

            //}
            //menuItemTheme.UpdateLayout();
        }

        //private void MenuItemColor_ColorChangedSelected(object? sender, EventArgs e)
        //{
        //    Debug.WriteLine(((MenuItemColor)sender).primaryColor.ToString());
        //    if (sender == null) return;
        //    ITheme theme = _paletteHelper.GetTheme();
        //    //theme.PrimaryMid = ((MenuItemColor)sender).primaryColor;
        //    theme.SetPrimaryColor(System.Windows.Media.Color.FromRgb(200, 0, 0)); //red
        //    _paletteHelper.SetTheme(theme);
        //}

        private void CreateListingEntryCuts()
        {
            var listEntryCuts = new ListingEntryCuts();
            listEntryCuts.DeleteMe += ListEntryCuts_DeleteMe;
            stackPanelPipeRef.Children.Add(listEntryCuts);
            //ITheme theme ;

            //PaletteHelper p = new PaletteHelper();
            //p.SetTheme(theme);
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
            //controlToPrint = new HorizontalAxis();
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
                        MessageBox.Show("La longueur de coupe est supérieur à la longueur de base. Resolution des découpes impossible", "Erreur",MessageBoxButton.OK, MessageBoxImage.Stop);
                        DataToDraw.ClearAll();
                        return;
                    }
                    for (int i = 0; i < qt; i++)
                    {
                        listCut.Add(length);
                    }

                }

                var description = string.IsNullOrEmpty(item.pipeDescription.Text) ? "????" : item.pipeDescription.Text;
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
            PrintDialog printDlg2 = new();
            try
            {
                printDlg2.PrintVisual(ResolveStackPanel, "Pipe Printing");
            }
            catch (Exception)
            {
                MessageBox.Show("impossible d'imprimer", "erreur d'impression", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //scroll.ScrollToHome();
            //PrintDialog printDlg = new PrintDialog();
            //bool? printResult = printDlg.ShowDialog();
            //if (printResult != null)
            //{
            //    bool p = printResult.Value;
            //    if (p)
            //    {
            //        try
            //        {
            //            FrameworkElement controlToPrint = new FrameworkElement();
            //            controlToPrint.DataContext = ResolveStackPanel;
            //            printDlg.PrintVisual(controlToPrint, "Grid Printing");
            //            //printDlg.PrintVisual(ResolveStackPanel, "Grid Printing.");
            //        }
            //        catch (Exception)
            //        {
            //            MessageBox.Show("impossible d'imprimer", "erreur d'impression", MessageBoxButton.OK, MessageBoxImage.Error);
            //        }
            //    }
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
            MessageBox.Show("Logiciel de préparation de débit - Version du 01/31/2022", "A propos",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        //private void MenuITem_Click_ChangeTheme(object sender, RoutedEventArgs e)
        //{
        //    ITheme theme = _paletteHelper.GetTheme();
        //    theme.SetPrimaryColor(System.Windows.Media.Color.FromRgb(200, 0, 0)); //red
        //    _paletteHelper.SetTheme(theme);
        //}

        private void DarkMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = DarkMenuItem.IsChecked ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        //private void ColorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        //{
        //    ITheme theme = _paletteHelper.GetTheme();
        //    theme.SetPrimaryColor(e.NewValue); //red
        //    _paletteHelper.SetTheme(theme);
        //    Debug.WriteLine("color change");
        //    //BundledTheme b = new();
        //    //b.PrimaryColor = MaterialDesignColors.PrimaryColor.Red;
        //}
    }


}
