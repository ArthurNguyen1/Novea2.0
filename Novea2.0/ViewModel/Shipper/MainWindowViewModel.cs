using Novea2._0.Model;
using Novea2._0.View.Shipper;
using Novea2._0.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;

namespace Novea2._0.ViewModel.Shipper
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
        public ICommand LoadAdminwdCommand { get; set; }
        public ICommand MoveWindowCommand { get; set; }

        private int buttonIndex;
        public MainWindowViewModel()
        {
            CloseWdCommand = new RelayCommand<MainWindow>((p) => true, (p) => CloseWd());
            MinimizeWdCommand = new RelayCommand<MainWindow>((p) => true, (p) => MinimizeWd(p));
            LogOutCommand = new RelayCommand<MainWindow>((p) => true, (p) => LogOut(p));
            GetIdTab = new RelayCommand<RadioButton>((p) => true, (p) => buttonIndex = int.Parse(p.Uid));
            SwitchTabCommand = new RelayCommand<MainWindow>((p) => true, (p) => SwitchTab(p));
            LoadAdminwdCommand = new RelayCommand<MainWindow>((p) => true, (p) => Loadwd(p));
            MoveWindowCommand = new RelayCommand<MainWindow>((p) => true, (p) => MoveWindow(p));
        }
        private void MoveWindow(MainWindow p)
        {
            p.DragMove();
        }
        void Loadwd(MainWindow p)
        {
            if (Const.IsLogin)
            {
                byte[] imageData = Const.SHP.AVATAR;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(imageData);
                bitmapImage.EndInit();
                Ava = bitmapImage;
                Hoten = string.Join(" ", Const.SHP.HOTEN.Split().Reverse().Take(2).Reverse());
            }
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
                    p.MainFrame.NavigationService.Navigate(new Order());
                    break;
                case 2:
                    p.MainFrame.NavigationService.Navigate(new Setting());
                    break;
                default:
                    break;
            }
        }
    }
}
