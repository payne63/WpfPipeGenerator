using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfPipeGenerator
{
    internal class PipeCuts : ObservableCollection<PipeCuts> , INotifyPropertyChanged
    {
        private ObservableCollection<int>? _cuts;

        public ObservableCollection<int>? Cuts { get { return _cuts; } set { _cuts = value; OnPropertyChanged(); } }

        public new event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
