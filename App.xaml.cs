using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfPipeGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //var mWin = new MainWindow();
            if (e.Args.Length>0)
            {
                foreach (var arg in e.Args)
                {
                    MessageBox.Show(arg);
                }
               // mWin.Show();
            }
        }
    }
}
