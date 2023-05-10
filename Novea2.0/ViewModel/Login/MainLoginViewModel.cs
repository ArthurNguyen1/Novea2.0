using Novea2._0.View;
using Novea2._0.View.Login;
using Novea2._0.Model;
using Novea2._0.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xamarin.Forms.PlatformConfiguration;

namespace Novea2._0.ViewModel.Login
{
    public class MainLoginViewModel : BaseViewModel
    {
        public ICommand SwitchTab { get; set; }
        public ICommand GetIdTab { get; set; }
        public ICommand MinimizeLogin { get; set; }
        public ICommand CloseLogin { get; set; }
        public ICommand CloseLG { get; set; }

        int buttonIndex;
        public MainLoginViewModel()
        {
            GetIdTab = new RelayCommand<Button>((p) => true, (p) => buttonIndex = int.Parse(p.Uid));
            SwitchTab = new RelayCommand<MainLogin>((p) => true, (p) => switchTab(p));
            MinimizeLogin = new RelayCommand<MainLogin>((p) => true, (p) => minimizeLogin(p));
            CloseLogin = new RelayCommand<MainLogin>((p) => true, (p) => closeLogin());
            CloseLG = new RelayCommand<MainLogin>((p) => true, (p) => CloseMainLogin(p));
        }
        public void CloseMainLogin(MainLogin p)
        {
            if (Const.IsLogin)
            {
                p.Hide();
            }
        }
        private void closeLogin()
        {
            Application.Current.Shutdown();
        }
        private void minimizeLogin(MainLogin p)
        {
            p.WindowState = WindowState.Minimized;
        }
        private void switchTab(MainLogin p)
        {
            switch (buttonIndex)
            {
                case 0:
                    p.LoginFrame.NavigationService.Navigate(new AdminLogin());
                    break;
                case 1:
                    p.LoginFrame.NavigationService.Navigate(new ClientLogin());
                    break;
                case 2:
                    p.LoginFrame.NavigationService.Navigate(new StoreLogin());
                    break;
                case 3:
                    p.LoginFrame.NavigationService.Navigate(new ShipperLogin());
                    break;
                default:
                    break;
            }
        }
    }
}
