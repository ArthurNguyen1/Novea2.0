using Novea2._0.Model;
using Novea2._0.View.Store_Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Novea2._0.ViewModel.Store_Owner
{
    public class DetailOrderViewModel : BaseViewModel
    {
        private ObservableCollection<CTHD> _listCTHD;
        public ObservableCollection<CTHD> listCTHD { get => _listCTHD; set { _listCTHD = value; OnPropertyChanged(); } }
        public ICommand Closewd { get; set; }
        public ICommand Minimizewd { get; set; }
        public ICommand MoveWindow { get; set; }
        public ICommand Loadwd { get; set; }
        public ICommand FinishOrderCommand { get; set; }
        private int _TongTien;
        public int TongTien { get => _TongTien; set { _TongTien = value; OnPropertyChanged(); } }
        public ICommand GetSoHD { get; set; }
        private string SoHD_Now;
        public DetailOrderViewModel()
        {
            GetSoHD = new RelayCommand<DetailOrder>((p) => true, (p) => _GetSoHD(p));
            Loadwd = new RelayCommand<DetailOrder>((p) => true, (p) => _Loadwd(p));
            Closewd = new RelayCommand<DetailOrder>((p) => true, (p) => Close(p));
            Minimizewd = new RelayCommand<DetailOrder>((p) => true, (p) => Minimize(p));
            MoveWindow = new RelayCommand<DetailOrder>((p) => true, (p) => moveWindow(p));
            FinishOrderCommand = new RelayCommand<DetailOrder>((p) => true, (p) => _FinishOrderCommand(p));

        }
        void _FinishOrderCommand(DetailOrder p)
        {
            MessageBoxResult h = System.Windows.MessageBox.Show("Bạn xác nhận hoàn thành đơn hàng này ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                var uRow = DataProvider.Ins.DB.HOADONs.Where(w => w.SOHD == SoHD_Now).FirstOrDefault();
                uRow.STATU = "Đang giao hàng";
                DataProvider.Ins.DB.SaveChanges();

                p.Close();
            }
        }
        void _GetSoHD(DetailOrder p)
        {
            SoHD_Now = p.SoHD.Text;
        }

        void _Loadwd(DetailOrder p)
        {
            DataProvider.Ins.Refresh();
            listCTHD = new ObservableCollection<CTHD>(DataProvider.Ins.DB.CTHDs.Where(pa => pa.SOHD == SoHD_Now));
            HOADON hd_temp = DataProvider.Ins.DB.HOADONs.Where(pa => pa.SOHD == SoHD_Now).FirstOrDefault();
            TongTien = (int)hd_temp.TONGTIEN;
        }
        void moveWindow(DetailOrder p)
        {
            p.DragMove();
        }
        void Close(DetailOrder p)
        {
            p.Close();
        }
        void Minimize(DetailOrder p)
        {
            p.WindowState = WindowState.Minimized;
        }
    }
}
