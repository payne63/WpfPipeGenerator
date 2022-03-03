using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfPipeGenerator
{
    class MenuItemColor : MenuItem
    {
        public MaterialDesignColors.PrimaryColor primaryColor;

        public event EventHandler? ColorChangedSelected;
        public static readonly RoutedEvent? ColorChangeClick;


        public override void BeginInit()
        {
            base.BeginInit();
            Click += MyOnClick;
        }

        void MyOnClick(object sender, RoutedEventArgs e)
        {
            ColorChangedSelected?.Invoke(this,EventArgs.Empty);
            Debug.WriteLine("click");
        }


    }
}
