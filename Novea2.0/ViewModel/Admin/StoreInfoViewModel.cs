using Novea2._0.Model;
using Novea2._0.View.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Novea2._0.ViewModel.Admin
{
    public class StoreInfoViewModel : BaseViewModel
    {
        private byte[] imageData;
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand BanAccountCommand { get; set; }
        public ICommand UnbanAccountCommand { get; set; }


        public StoreInfoViewModel()
        {
            Loadwd = new RelayCommand<StoreInfo>((p) => true, (p) => _Loadwd(p));
            Closewd = new RelayCommand<StoreInfo>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<StoreInfo>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<StoreInfo>((p) => true, (p) => moveWindow(p));
            BanAccountCommand = new RelayCommand<StoreInfo>((p) => true, (p) => _BanAccount(p));
            UnbanAccountCommand = new RelayCommand<StoreInfo>((p) => true, (p) => _UnbanAccount(p));
        }

        void _BanAccount(StoreInfo p)
        {
            MessageBoxResult h = MessageBox.Show("Bạn muốn khóa tài khoản này ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                BanWindow banWindow = new BanWindow();
                banWindow.ShowDialog();
            }
            p.Close();
        }

        void _UnbanAccount(StoreInfo p)
        {
            MessageBoxResult h = MessageBox.Show("Bạn muốn mở khóa tài khoản này ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                foreach (CUAHANG a in DataProvider.Ins.DB.CUAHANGs.Where(pa => (pa.MACH == Const.CH.MACH)))
                {
                    a.STATU = true;
                }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Mở khóa tài khoản thành công !", "THÔNG BÁO");
            }
            p.Close();
        }

        void _Loadwd(StoreInfo p)
        {
            DataProvider.Ins.Refresh();
            CUAHANG temp = DataProvider.Ins.DB.CUAHANGs.Where(s => s.MACH == Const.CH.MACH).FirstOrDefault();
            imageData = temp.AVATAR;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageData);
            bitmapImage.EndInit();
            p.HinhAnh1.ImageSource = bitmapImage;
        }
        void moveWindow(StoreInfo p)
        {
            p.DragMove();
        }
        void Close(StoreInfo p)
        {
            p.Close();
        }
        void Minimize(StoreInfo p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}
