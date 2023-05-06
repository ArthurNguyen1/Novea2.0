using Novea2._0.View;
using Novea2._0.View.Store_Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Store_Owner
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ICommand MinimizeWd { get; set; }
        public ICommand CloseWd { get; set; }
        public ICommand SwitchTab { get; set; }
        public ICommand GetIdTab { get; set; }
        public ICommand LogOutCommand { get; set; }
        int buttonIndex;
        public MainWindowViewModel()
        {
            MinimizeWd = new RelayCommand<MainWindow>((p) => true, (p) => minimizeWindow(p));
            CloseWd = new RelayCommand<MainWindow>((p) => true, (p) => closeWindow());
            GetIdTab = new RelayCommand<RadioButton>((p) => true, (p) => buttonIndex = int.Parse(p.Uid));
            SwitchTab = new RelayCommand<MainWindow>((p) => true, (p) => switchTab(p));
            LogOutCommand = new RelayCommand<MainWindow>((p) => true, (p) => logOut(p));
        }
        private void minimizeWindow(MainWindow p)
        {
            p.WindowState = WindowState.Minimized;
        }
        private void closeWindow()
        {
            Application.Current.Shutdown();
        }
        private void switchTab(MainWindow p)
        {
            switch (buttonIndex)
            {
                case 0:
                    p.MainFrame.NavigationService.Navigate(new Home());
                    break;
                case 1:
                    p.MainFrame.NavigationService.Navigate(new Order());
                    break;
                case 2:
                    p.MainFrame.NavigationService.Navigate(new Product());
                    break;
                case 3:
                    p.MainFrame.NavigationService.Navigate(new Customer());
                    break;
                case 4:
                    p.MainFrame.NavigationService.Navigate(new History());
                    break;
                case 5:
                    p.MainFrame.NavigationService.Navigate(new Setting());
                    break;
                default:
                    break;
            }
        }
        private void logOut(MainWindow p)
        {
            MainLogin login = new MainLogin();
            login.Show();
            p.Close();
        }
    }
}
