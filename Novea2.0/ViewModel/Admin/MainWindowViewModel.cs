using Novea2._0.Model;
using Novea2._0.View;
using Novea2._0.View.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Novea2._0.ViewModel.Admin
{
    public class MainWindowViewModel : BaseViewModel
    {
        private BitmapImage _Ava;
        public BitmapImage Ava { get => _Ava; set { _Ava = value; OnPropertyChanged(); } }
        public string Name;
        int buttonIndex;

        public ICommand MinimizeWd { get; set; }
        public ICommand CloseWd { get; set; }
        public ICommand SwitchTab { get; set; }
        public ICommand GetIdTab { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand TenDangNhap_Loaded { get; set; }
        public MainWindowViewModel()
        {
            MinimizeWd = new RelayCommand<MainWindow>((p) => true, (p) => minimizeWindow(p));
            CloseWd = new RelayCommand<MainWindow>((p) => true, (p) => closeWindow());
            GetIdTab = new RelayCommand<RadioButton>((p) => true, (p) => buttonIndex = int.Parse(p.Uid));
            SwitchTab = new RelayCommand<MainWindow>((p) => true, (p) => switchTab(p));
            LogOutCommand = new RelayCommand<MainWindow>((p) => true, (p) => logOut(p));
            Loadwd = new RelayCommand<MainWindow>((p) => true, (p) => _Loadwd(p));
            MoveWindow = new RelayCommand<MainWindow>((p) => true, (p) => moveWindow(p));
            TenDangNhap_Loaded = new RelayCommand<MainWindow>((p) => true, (p) => LoadTenAD(p));
        }
        void _Loadwd(MainWindow p)
        {
            if (Const.IsLogin)
            {
                byte[] imageData = Const.ADM.AVATAR;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(imageData);
                bitmapImage.EndInit();
                Ava = bitmapImage;
                LoadTenAD(p);
            }
        }
        public void LoadTenAD(MainWindow p)
        {
            p.TenDangNhap.Text = string.Join(" ", Const.CH.TENCH.Split().Reverse().Take(2).Reverse());
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
                    p.MainFrame.NavigationService.Navigate(new Store());
                    break;
                case 2:
                    p.MainFrame.NavigationService.Navigate(new View.Admin.Customer());
                    break;
                case 3:
                    p.MainFrame.NavigationService.Navigate(new Shipper());
                    break;
                case 4:
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
        public void moveWindow(MainWindow p)
        {
            p.DragMove();
        }
    }
}
