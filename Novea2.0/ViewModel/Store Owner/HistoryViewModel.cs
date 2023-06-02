using Novea2._0.Model;
using Novea2._0.View.Store_Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Novea2._0.ViewModel.Store_Owner
{
    public class HistoryViewModel : BaseViewModel
    {
        private ObservableCollection<HOADON> _listHD;
        public ObservableCollection<HOADON> listHD { get => _listHD; set { _listHD = value; OnPropertyChanged(); } }

        private ObservableCollection<HOADON> _listHD1;
        public ObservableCollection<HOADON> listHD1 { get => _listHD1; set { _listHD1 = value; OnPropertyChanged(); } }
        public ICommand SearchCommand { get; set; }
        public ICommand DetailPdCommand { get; set; }
        public ICommand LoadCsCommand { get; set; }
        public ICommand SortDayCommand { get; set; }
        public ICommand SortMoneyCommand { get; set; }


        public HistoryViewModel()
        {
            SearchCommand = new RelayCommand<History>((p) => { return p == null ? false : true; }, (p) => _SearchCommand(p));
            LoadCsCommand = new RelayCommand<History>((p) => true, (p) => _LoadCsCommand(p));
            SortDayCommand = new RelayCommand<History>((p) => { return p == null ? false : true; }, (p) => _SortDayCommand(p));
            SortMoneyCommand = new RelayCommand<History>((p) => { return p == null ? false : true; }, (p) => _SortMoneyCommand(p));
            DetailPdCommand = new RelayCommand<History>((p) => { return p.ListViewHistory.SelectedItem == null ? false : true; }, (p) => _DetailPd(p));
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
        }
        void _LoadCsCommand(History parameter)
        {
            listHD1 = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs.Where(p => p.MACH == Const.MACH && (p.STATU == "Đã nhận" || p.STATU == "Bị hủy")));
            listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(p => p.SOHD).Select(grp => grp.FirstOrDefault()));
            parameter.cbbSortDay.SelectedIndex = 0;
            parameter.cbbSortMoney.SelectedIndex = 0;
            _SearchCommand(parameter);
            _SortDayCommand(parameter);
            _SortMoneyCommand(parameter);
        }

        void _SortDayCommand(History p)
        {
            switch (p.cbbSortDay.SelectedIndex.ToString())
            {
                case "0":
                    {
                        listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()));
                        p.ListViewHistory.ItemsSource = listHD;
                        break;
                    }
                case "1":
                    {
                        listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()).OrderBy(h => h.NGMH));
                        p.ListViewHistory.ItemsSource = listHD;
                        break;
                    }
                case "2":
                    {
                        listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()).OrderByDescending(h => h.NGMH));
                        p.ListViewHistory.ItemsSource = listHD;
                        break;
                    }
            }
        }

        void _SortMoneyCommand(History p)
        {
            switch (p.cbbSortMoney.SelectedIndex.ToString())
            {
                case "0":
                    {
                        listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()));
                        p.ListViewHistory.ItemsSource = listHD;
                        break;
                    }
                case "1":
                    {
                        listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()).OrderBy(h => h.TONGTIEN));
                        p.ListViewHistory.ItemsSource = listHD;
                        break;
                    }
                case "2":
                    {
                        listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(h => h.SOHD).Select(grp => grp.FirstOrDefault()).OrderByDescending(h => h.TONGTIEN));
                        p.ListViewHistory.ItemsSource = listHD;
                        break;
                    }
            }
        }

        void _SearchCommand(History paramater)
        {
            ObservableCollection<HOADON> temp = new ObservableCollection<HOADON>();
            if (paramater.tbSearch.Text == "")
            {
                paramater.ListViewHistory.ItemsSource = listHD;
            }
            else
            {
                foreach (HOADON s in listHD)
                {
                    if (s.SOHD.ToLower().Contains(paramater.tbSearch.Text.ToLower()))
                    {
                        temp.Add(s);
                    }
                }
                if (temp != null)
                {
                    paramater.ListViewHistory.ItemsSource = temp;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy số hóa đơn");
                }
            }
        }

        void _DetailPd(History paramater)
        {
            DetailOrder detailOrder = new DetailOrder();
            HOADON temp = (HOADON)paramater.ListViewHistory.SelectedItem;
            Const.HD = temp;
            detailOrder.SoHD.Text = temp.SOHD;
            detailOrder.TenKH.Text = temp.KHACH.HOTEN;
            detailOrder.DiaChi.Text = temp.KHACH.DIACHI;
            detailOrder.SDT.Text = temp.KHACH.SDT;
            detailOrder.ShowDialog();
            paramater.ListViewHistory.SelectedItem = null;
            listHD1 = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs.Where(p => p.MACH == Const.MACH && (p.STATU == "Đã nhận" || p.STATU == "Bị hủy")));
            listHD = new ObservableCollection<HOADON>(listHD1.GroupBy(p => p.SOHD).Select(grp => grp.FirstOrDefault()));
            paramater.ListViewHistory.ItemsSource = listHD;
        }
    }
}
