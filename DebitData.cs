using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfPipeGenerator
{
    internal class DebitData : INotifyPropertyChanged
    {
        private int length;
        private int qt;
        public int Length { get { return length; } set { length = value ; OnPropertyChange("Length"); } }
        public int Qt { get { return qt; } set { qt = value; OnPropertyChange("Qt"); } }

        public event PropertyChangedEventHandler? PropertyChanged;

        public static ObservableCollection<DebitData> DebitDatas = new ObservableCollection<DebitData>();

        protected void OnPropertyChange( string name )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public DebitData(int length, int qt)
        {
            Length = length;
            Qt = qt;
        }
        public override string ToString()
        {

            return $"Length = {Length}"; 
        }
    }
}
