using Novea2._0.Model;
using Novea2._0.View.Customer;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;

namespace Novea2._0.ViewModel.Customer
{
    public class CartViewModel : BaseViewModel
    {
        private ObservableCollection<CTHD> _listCTHD;
        public ObservableCollection<CTHD> listCTHD { get => _listCTHD; set { _listCTHD = value; OnPropertyChanged(); } }
        private int _TongTien;
        public int TongTien { get => _TongTien; set { _TongTien = value; OnPropertyChanged(); } }
        public ICommand LoadCartCommand { get; set; }
        public ICommand DetailPdCommand { get; set; }
        public ICommand DeleteCartCommand { get; set; }
        public ICommand AcceptCartCommand { get; set; }
        public CartViewModel()
        {
            LoadCartCommand = new RelayCommand<Cart>((p) => true, (p) => _LoadCartCommand(p));
            DeleteCartCommand = new RelayCommand<Cart>((p) => true, (p) => _DeleteCartCommand(p));
            AcceptCartCommand = new RelayCommand<Cart>((p) => true, (p) => _AcceptCartCommand(p));
            DetailPdCommand = new RelayCommand<Cart>((p) => { return p.ListViewCTHD.SelectedItem != null; }, (p) => DisplayDetailProduct(p));
        }
        public void DisplayDetailProduct(Cart paramater)
        {
            CartDetail cartDetail = new CartDetail();
            CTHD temp = (CTHD)paramater.ListViewCTHD.SelectedItem;
            cartDetail.txbTENSP.Text = temp.SANPHAM.TENSP;
            cartDetail.txbDONGIA.Text = string.Format("{0:0,0}", temp.SANPHAM.DONGIA) + " VNĐ";
            cartDetail.txbMOTA.Text = temp.SANPHAM.MOTA;
            cartDetail.txbSIZE.Text = "Size: " + temp.SANPHAM.SIZE;
            cartDetail.txbSL.Text = temp.SOLUONG.ToString();
            Const.CTHD_temp = temp;

            cartDetail.ShowDialog();
            paramater.ListViewCTHD.SelectedItem = null;
        }
        void _LoadCartCommand(Cart parameter)
        {
            DataProvider.Ins.Refresh();
            var hoadon = DataProvider.Ins.DB.HOADONs.Where(h => h.MAND_KHACH == Const.KH.MAND && h.STATU == "Khởi tạo").FirstOrDefault();
            if (hoadon != null)
            {
                listCTHD = new ObservableCollection<CTHD>(DataProvider.Ins.DB.CTHDs.Where(p => p.SOHD == hoadon.SOHD));
                TongTien = (int)hoadon.TONGTIEN;
                parameter.acceptbtn.IsEnabled = true;
                parameter.deletebtn.IsEnabled = true;
            }
        }
        void _DeleteCartCommand(Cart parameter)
        {
            MessageBoxResult h = MessageBox.Show("Bạn có muốn hủy giỏ hàng hiện tại ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                var itemToRemove = DataProvider.Ins.DB.HOADONs.Where(pa => pa.MAND_KHACH == Const.KH.MAND && pa.STATU == "Khởi tạo").SingleOrDefault();

                ObservableCollection<CTHD> ListCTHD = new ObservableCollection<CTHD>(DataProvider.Ins.DB.CTHDs.Where(p => p.SOHD == itemToRemove.SOHD));

                if (itemToRemove != null)
                {
                    if (ListCTHD != null)
                    {
                        for (int i = 0; i < ListCTHD.Count; i++)
                        {
                            DataProvider.Ins.DB.CTHDs.Remove(ListCTHD[i]);
                        }
                    }
                    DataProvider.Ins.DB.HOADONs.Remove(itemToRemove);
                    DataProvider.Ins.DB.SaveChanges();
                }
                listCTHD = null;
                Const.HD = null;
                TongTien = 0;
                parameter.acceptbtn.IsEnabled = false;
                parameter.deletebtn.IsEnabled = false;
                _LoadCartCommand(parameter);
            }
        }
        void _AcceptCartCommand(Cart parameter)
        {
            MessageBoxResult h = MessageBox.Show("Bạn xác nhận mua hàng ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                var hoadon = DataProvider.Ins.DB.HOADONs.Where(hd => hd.MAND_KHACH == Const.KH.MAND && hd.STATU == "Khởi tạo").FirstOrDefault();
                hoadon.STATU = "Đang xử lý";
                DataProvider.Ins.DB.SaveChanges();

                listCTHD = null;
                Const.HD = null;
                TongTien = 0;
                parameter.acceptbtn.IsEnabled = false;
                parameter.deletebtn.IsEnabled = false;
                _LoadCartCommand(parameter);
            }
        }
    }
}
