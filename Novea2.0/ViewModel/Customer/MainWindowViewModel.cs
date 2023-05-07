using Novea2._0.View;
using Novea2._0.View.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Customer
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ICommand CloseWdCommand { get; set; }
        public ICommand MinimizeWdCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public MainWindowViewModel()
        {
            CloseWdCommand = new RelayCommand<MainWindow>((p) => true, (p) => CloseWd());
            MinimizeWdCommand = new RelayCommand<MainWindow>((p) => true, (p) => MinimizeWd(p));
            LogOutCommand = new RelayCommand<MainWindow>((p) => true, (p) => LogOut(p));
        }
        private void CloseWd()
        {
            Application.Current.Shutdown();
        }
        private void MinimizeWd(MainWindow p)
        {
            p.WindowState = WindowState.Minimized;
        }
        private void LogOut(MainWindow p)
        {
            MainLogin mainLogin = new MainLogin();
            mainLogin.Show();
            p.Close();
        }
    }
}
