using Novea2._0.Model;
using Novea2._0.View.Customer;
using Novea2._0.View.Shipper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Shipper
{
    public class OrderViewModel : BaseViewModel
    {
        private ObservableCollection<HOADON> listHD;
        public ObservableCollection<HOADON> ListHD { get => listHD; set { listHD = value; OnPropertyChanged(); } }
        private ObservableCollection<HOADON> listHD1;
        public ObservableCollection<HOADON> ListHD1 { get => listHD1; set { listHD1 = value; OnPropertyChanged(); } }
        public ICommand LoadCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand DetailOrderCommand { get; set; }
        public OrderViewModel()
        {
            LoadCommand = new RelayCommand<Order>((p) => true, (p) => Load(p));
            FilterCommand = new RelayCommand<Order>((p) => true, (p) => Filter(p));
            SearchCommand = new RelayCommand<Order>((p) => true, (p) => Search(p));
            DetailOrderCommand = new RelayCommand<Order>((p) => p.ListViewHD.SelectedItem != null, (p) => Detail(p));
        }
        void Load(Order parameter)
        {
            DataProvider.Ins.Refresh();
            ListHD1 = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs.Where(h => h.MAND_SHIPPER == Const.SHP.MAND));
            ListHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()));
            parameter.cbbFilter.SelectedIndex = 0;
            Filter(parameter);
        }
        void Filter(Order p)
        {
            switch (p.cbbFilter.SelectedIndex.ToString())
            {
                case "0":
                    ListHD = new ObservableCollection<HOADON>(ListHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()));
                    p.ListViewHD.ItemsSource = ListHD;
                    break;
                case "1":
                    ListHD = new ObservableCollection<HOADON>(ListHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()).Where(h => h.STATU == "Đang giao hàng"));
                    p.ListViewHD.ItemsSource = ListHD;
                    break;
                case "2":
                    ListHD = new ObservableCollection<HOADON>(ListHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()).Where(h => h.STATU == "Đã nhận"));
                    p.ListViewHD.ItemsSource = ListHD;
                    break;
            }
        }
        void Search(Order p)
        {
            ObservableCollection<HOADON> temp = new ObservableCollection<HOADON>();
            if (p.txbSearch.Text == "")
            {
                p.ListViewHD.ItemsSource = ListHD;
            }
            else
            {
                foreach (HOADON h in ListHD)
                {
                    if (h.SOHD.ToLower().Contains(p.txbSearch.Text.ToLower()))
                    {
                        temp.Add(h);
                    }
                }
                if (temp != null)
                {
                    p.ListViewHD.ItemsSource = temp;
                }
            }
        }
        void Detail(Order parameter)
        {
            DetailOrder detailOrder = new DetailOrder();
            HOADON temp = (HOADON)parameter.ListViewHD.SelectedItem;
            Const.HD = temp;
            detailOrder.SoHD.Text = temp.SOHD;
            detailOrder.TenKH.Text = temp.KHACH.HOTEN;
            detailOrder.DiaChi.Text = temp.KHACH.DIACHI;
            detailOrder.SDT.Text = temp.KHACH.SDT;
            if (temp.STATU == "Đang giao hàng")
            {
                detailOrder.btConfirm.Visibility = System.Windows.Visibility.Visible;
            }
            detailOrder.ShowDialog();
            parameter.ListViewHD.SelectedItem = null;
            ListHD1 = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs.Where(h => h.MAND_SHIPPER == Const.SHP.MAND));
            ListHD = new ObservableCollection<HOADON>(ListHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()));
            parameter.ListViewHD.ItemsSource = ListHD;
            parameter.cbbFilter.SelectedIndex = 0;
        }
    }
}
