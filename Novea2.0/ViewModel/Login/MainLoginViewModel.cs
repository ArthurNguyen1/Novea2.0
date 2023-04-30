using Novea2._0.View;
using Novea2._0.View.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Login
{
    public class MainLoginViewModel : BaseViewModel
    {
        public ICommand SwitchTab { get; set; }
        public ICommand GetIdTab { get; set; }
        int buttonIndex;
        public MainLoginViewModel()
        {
            GetIdTab = new RelayCommand<Button>((p) => true, (p) => buttonIndex = int.Parse(p.Uid));
            SwitchTab = new RelayCommand<MainLogin>((p) => true, (p) => switchTab(p));
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
