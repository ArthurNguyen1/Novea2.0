using Novea2._0.Model;
using Novea2._0.View.Store_Owner;
using Novea2._0.ViewModel.Store_Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Store_Owner
{
    public class OrderViewModel : BaseViewModel
    {
        private ObservableCollection<HOADON> _listHD;
        public ObservableCollection<HOADON> listHD { get => _listHD; set { _listHD = value; OnPropertyChanged(); } }
        private ObservableCollection<HOADON> _listHD1;
        public ObservableCollection<HOADON> listHD1 { get => _listHD1; set { _listHD1 = value; OnPropertyChanged(); } }

        public ICommand Detail { get; set; }
        public ICommand LoadCsCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public OrderViewModel()
        {
            FilterCommand = new RelayCommand<Order>((p) => true, (p) => Filter(p));
            Detail = new RelayCommand<Order>((p) => p.ListViewHD.SelectedItem != null ? true : false, (p) => _Detail(p));
            LoadCsCommand = new RelayCommand<Order>((p) => true, (p) => _LoadCsCommand(p));
        }
        void _LoadCsCommand(Order parameter)
        {
            DataProvider.Ins.Refresh();
            listHD1 = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs.Where(h => h.MACH == Const.CH.MACH && h.STATU != "Khởi tạo"));
            listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()));
            parameter.cbbFilter.SelectedIndex = 0;
            Filter(parameter);

        }

        void Filter(Order p)
        {
            switch (p.cbbFilter.SelectedIndex.ToString())
            {
                case "0":
                    {
                        listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()));
                        p.ListViewHD.ItemsSource = listHD;
                        break;
                    }
                case "1":
                    {
                        listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()).Where(h => h.STATU == "Đang xử lý"));
                        p.ListViewHD.ItemsSource = listHD;
                        break;
                    }
                case "2":
                    {
                        listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()).Where(h => h.STATU == "Đang giao hàng"));
                        p.ListViewHD.ItemsSource = listHD;
                        break;
                    }
                case "3":
                    {
                        listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()).Where(h => h.STATU == "Bị hủy"));
                        p.ListViewHD.ItemsSource = listHD;
                        break;
                    }
            }
        }

        void _Detail(Order parameter)
        {
            DetailOrder detailOrder = new DetailOrder();
            HOADON temp = (HOADON)parameter.ListViewHD.SelectedItem;
            Const.HD = temp;
            detailOrder.SoHD.Text = temp.SOHD;
            detailOrder.TenKH.Text = temp.KHACH.HOTEN;
            detailOrder.DiaChi.Text = temp.KHACH.DIACHI;
            detailOrder.SDT.Text = temp.KHACH.SDT;
            detailOrder.ShowDialog();
            parameter.ListViewHD.SelectedItem = null;
            listHD1 = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs.Where(h => h.MAND_KHACH == Const.KH.MAND && h.STATU != "Khởi tạo"));
            listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()));
            parameter.ListViewHD.ItemsSource = listHD;
        }
    }
}
