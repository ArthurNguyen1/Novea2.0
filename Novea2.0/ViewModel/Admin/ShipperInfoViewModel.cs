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
    public class ShipperInfoViewModel : BaseViewModel
    {
        private byte[] imageData;
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand BanAccountCommand { get; set; }
        public ICommand UnbanAccountCommand { get; set; }


        public ShipperInfoViewModel()
        {
            Loadwd = new RelayCommand<ShipperInfo>((p) => true, (p) => _Loadwd(p));
            Closewd = new RelayCommand<ShipperInfo>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<ShipperInfo>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<ShipperInfo>((p) => true, (p) => moveWindow(p));
            BanAccountCommand = new RelayCommand<ShipperInfo>((p) => true, (p) => _BanAccount(p));
            UnbanAccountCommand = new RelayCommand<ShipperInfo>((p) => true, (p) => _UnbanAccount(p));
        }

        void _BanAccount(ShipperInfo p)
        {
            MessageBoxResult h = MessageBox.Show("Bạn muốn khóa tài khoản này ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                BanWindow banWindow = new BanWindow();
                banWindow.ShowDialog();
            }
            p.Close();
        }

        void _UnbanAccount(ShipperInfo p)
        {
            MessageBoxResult h = MessageBox.Show("Bạn muốn mở khóa tài khoản này ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                foreach (SHIPPER a in DataProvider.Ins.DB.SHIPPERs.Where(pa => (pa.MAND == Const.SHP.MAND)))
                {
                    a.STATU = true;
                }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Mở khóa tài khoản thành công !", "THÔNG BÁO");
            }
            p.Close();
        }

        void _Loadwd(ShipperInfo p)
        {
            DataProvider.Ins.Refresh();
            SHIPPER temp = DataProvider.Ins.DB.SHIPPERs.Where(s => s.MAND == Const.SHP.MAND).FirstOrDefault();
            imageData = temp.AVATAR;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageData);
            bitmapImage.EndInit();
            p.HinhAnh1.ImageSource = bitmapImage;
        }
        void moveWindow(ShipperInfo p)
        {
            p.DragMove();
        }
        void Close(ShipperInfo p)
        {
            p.Close();
        }
        void Minimize(ShipperInfo p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}
