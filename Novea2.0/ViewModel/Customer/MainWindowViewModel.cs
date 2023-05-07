using Novea2._0.View;
using Novea2._0.View.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Customer
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ICommand CloseWdCommand { get; set; }
        public ICommand MinimizeWdCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand SwitchTabCommand { get; set; }
        public ICommand GetIdTab { get; set; }

        private int buttonIndex;
        public MainWindowViewModel()
        {
            CloseWdCommand = new RelayCommand<MainWindow>((p) => true, (p) => CloseWd());
            MinimizeWdCommand = new RelayCommand<MainWindow>((p) => true, (p) => MinimizeWd(p));
            LogOutCommand = new RelayCommand<MainWindow>((p) => true, (p) => LogOut(p));
            GetIdTab = new RelayCommand<RadioButton>((p) => true, (p) => buttonIndex = int.Parse(p.Uid));
            SwitchTabCommand = new RelayCommand<MainWindow>((p) => true, (p) => SwitchTab(p));
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
        private void SwitchTab(MainWindow p)
        {
            switch (buttonIndex)
            {
                case 0:
                    p.MainFrame.NavigationService.Navigate(new Home());
                    break;
                case 1:
                    p.MainFrame.NavigationService.Navigate(new Cart());
                    break;
                case 2:
                    p.MainFrame.NavigationService.Navigate(new OrderHistory());
                    break;
                case 3:
                    p.MainFrame.NavigationService.Navigate(new Setting());
                    break;
                default:
                    break;
            }
        }
    }
}
