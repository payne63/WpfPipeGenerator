using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfPipeGenerator
{
    internal class TestViewModel : INotifyPropertyChanged
    {
        private Test? _currentTest;
        public Test? CurrentTest { get => _currentTest; private set { _currentTest = value; OnPropertyChanged(); } }
        private Test test1 = new Test() { Description = "d1", Name = "n1" };
        private Test test2 = new Test() { Description = "d2", Name = "n2" };

        private ObservableCollection<Test>? _listTest;
        public ObservableCollection<Test>? listTest { get { return _listTest; } }

        private Test? _selectedItem;
        public Test? SelectedItem { get { return _selectedItem; } set { _selectedItem = value; CurrentTest = value ; OnPropertyChanged(); } }


        public TestViewModel()
        {
            _currentTest = test1;
            _listTest = new ObservableCollection<Test>();
            _listTest.Add(test1);
            _listTest.Add(test2);
            _listTest.Add(new Test() { Description = "d3", Name = "n3"});
        }

        private ICommand? _switchUser;
        public ICommand switchUser
        {
            get
            {
                return _switchUser ?? (_switchUser = new CommandHandler(
                    () =>{CurrentTest = _currentTest == test1 ? test2 : test1;},
                    () => { return true; }
                    ));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    class CommandHandler : ICommand
    {
        private Action _action;
        private Func<bool> _canExecute;

        public CommandHandler(Action action, Func<bool> canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute.Invoke();
        }

        public void Execute(object? parameter)
        {
            _action();
        }
    }
}
