using Novea2._0.Model;
using Novea2._0.View;
using Novea2._0.View.Customer;
using Novea2._0.View.Store_Owner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Novea2._0.ViewModel.Customer
{
    public class MainWindowViewModel : BaseViewModel
    {
        private BitmapImage ava;
        public BitmapImage Ava { get => ava; set { ava = value; OnPropertyChanged(); } }
        private string hoten;
        public string Hoten { get => hoten; set { hoten = value; OnPropertyChanged(); } }
        public ICommand CloseWdCommand { get; set; }
        public ICommand MinimizeWdCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand SwitchTabCommand { get; set; }
        public ICommand GetIdTab { get; set; }
        public ICommand LoadGuestwdCommand { get; set; }
        public ICommand MoveWindowCommand { get; set; }

        private int buttonIndex;
        public MainWindowViewModel()
        {
            CloseWdCommand = new RelayCommand<View.Customer.MainWindow>((p) => true, (p) => CloseWd());
            MinimizeWdCommand = new RelayCommand<View.Customer.MainWindow>((p) => true, (p) => MinimizeWd(p));
            LogOutCommand = new RelayCommand<View.Customer.MainWindow>((p) => true, (p) => LogOut(p));
            GetIdTab = new RelayCommand<RadioButton>((p) => true, (p) => buttonIndex = int.Parse(p.Uid));
            SwitchTabCommand = new RelayCommand<View.Customer.MainWindow>((p) => true, (p) => SwitchTab(p));
            LoadGuestwdCommand = new RelayCommand<View.Customer.MainWindow>((p) => true, (p) => LoadGuestwd(p));
            MoveWindowCommand = new RelayCommand<View.Customer.MainWindow>((p) => true, (p) => MoveWindow(p));
        }
        private void MoveWindow(View.Customer.MainWindow p)
        {
            p.DragMove();
        }
        void LoadGuestwd(View.Customer.MainWindow p)
        {
            if (Const.IsLogin)
            {
                byte[] imageData = Const.KH.AVATAR;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(imageData);
                bitmapImage.EndInit();
                Ava = bitmapImage;
                Hoten = string.Join(" ", Const.KH.HOTEN.Split().Reverse().Take(2).Reverse());
            }
        }
        private void CloseWd()
        {
            Application.Current.Shutdown();
        }
        private void MinimizeWd(View.Customer.MainWindow p)
        {
            p.WindowState = WindowState.Minimized;
        }
        private void LogOut(View.Customer.MainWindow p)
        {
            MainLogin mainLogin = new MainLogin();
            mainLogin.Show();
            p.Close();
        }
        private void SwitchTab(View.Customer.MainWindow p)
        {
            switch (buttonIndex)
            {
                case 0:
                    p.MainFrame.NavigationService.Navigate(new View.Customer.Home());
                    break;
                case 1:
                    p.MainFrame.NavigationService.Navigate(new Cart());
                    break;
                case 2:
                    p.MainFrame.NavigationService.Navigate(new OrderHistory());
                    break;
                case 3:
                    p.MainFrame.NavigationService.Navigate(new View.Customer.Setting());
                    break;
                default:
                    break;
            }
        }
    }
}
