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
    public class CustomerInfoViewModel : BaseViewModel
    {
        private byte[] imageData;
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand BanAccountCommand { get; set; }
        public ICommand UnbanAccountCommand { get; set; }


        public CustomerInfoViewModel()
        {
            Loadwd = new RelayCommand<CustomerInfo>((p) => true, (p) => _Loadwd(p));
            Closewd = new RelayCommand<CustomerInfo>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<CustomerInfo>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<CustomerInfo>((p) => true, (p) => moveWindow(p));
            BanAccountCommand = new RelayCommand<CustomerInfo>((p) => true, (p) => _BanAccount(p));
            UnbanAccountCommand = new RelayCommand<CustomerInfo>((p) => true, (p) => _UnbanAccount(p));
        }

        void _BanAccount(CustomerInfo p)
        {
            MessageBoxResult h = MessageBox.Show("Bạn muốn khóa tài khoản này ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                BanWindow banWindow = new BanWindow();
                banWindow.ShowDialog();
            }
            p.Close();
        }

        void _UnbanAccount(CustomerInfo p)
        {
            MessageBoxResult h = MessageBox.Show("Bạn muốn mở khóa tài khoản này ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                foreach (KHACH a in DataProvider.Ins.DB.KHACHes.Where(pa => (pa.MAND == Const.KH.MAND)))
                {
                    a.STATU = true;
                }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Mở khóa tài khoản thành công !", "THÔNG BÁO");
            }
            p.Close();
        }

        void _Loadwd(CustomerInfo p)
        {
            DataProvider.Ins.Refresh();
            KHACH temp = DataProvider.Ins.DB.KHACHes.Where(s => s.MAND == Const.KH.MAND).FirstOrDefault();
            imageData = temp.AVATAR;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageData);
            bitmapImage.EndInit();
            p.HinhAnh1.ImageSource = bitmapImage;
        }
        void moveWindow(CustomerInfo p)
        {
            p.DragMove();
        }
        void Close(CustomerInfo p)
        {
            p.Close();
        }
        void Minimize(CustomerInfo p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}
