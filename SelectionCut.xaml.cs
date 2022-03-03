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
    /// Logique d'interaction pour SelectionCut.xaml
    /// </summary>
    public partial class SelectionCut : UserControl
    {
        public event EventHandler? ValidationDone;
        public event EventHandler? DeleteMe;
        public event EventHandler? AddNext;

        public int Length { get; set; }
        public int Qt { get; set; }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("MyText", typeof(string), typeof(SelectionCut));

        public string MyText { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }

        public SelectionCut()
        {
            InitializeComponent();
        }


        private void TextBoxQt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxQt.Text))
            {
                textBoxQt.Text = "quantite";
                return;
            }
            if (int.TryParse(textBoxQt.Text, out int tempQt)) Qt = tempQt;
            if (Length != 0 && Qt != 0) ValidationDone?.Invoke(this, EventArgs.Empty);
        }

        private void TextBoxLength_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxLength.Text))
            {
                textBoxLength.Text = "longueur";
                return;
            }
            if (int.TryParse(textBoxLength.Text, out int tempLength)) Length = tempLength;
            Debug.WriteLine($"length has been set");
            if (Length != 0 && Qt != 0) ValidationDone?.Invoke(this, EventArgs.Empty);
        }

        // bloque les saisies aux nombres seulement
        private void textBoxLength_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");

        //leve l'évenement de supression
        private void Button_Delete(object sender, RoutedEventArgs e) => DeleteMe?.Invoke(this, EventArgs.Empty);

        private void TextBoxLength_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) textBoxQt.Focus();
        }
        private void TextBoxQt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddNext?.Invoke(this, EventArgs.Empty);
            }
        }

        private void textBoxQt_LostFocus_1(object sender, RoutedEventArgs e)
        {
            AddNext?.Invoke(this, EventArgs.Empty);
            
        }


    }
}
