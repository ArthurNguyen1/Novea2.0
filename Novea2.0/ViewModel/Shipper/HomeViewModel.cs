using Novea2._0.Model;
using Novea2._0.View.Shipper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Shipper
{
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<HOADON> listHD;
        public ObservableCollection<HOADON> ListHD { get => listHD; set { listHD = value; OnPropertyChanged(); } }
        private int totalsalary;
        public int TotalSalary { get => totalsalary; set { totalsalary = value; OnPropertyChanged(); } }
        public ICommand LoadWdCommand { get; set; }
        public ICommand Load1Command { get; set; }
        public ICommand Load2Command { get; set; }
        public HomeViewModel()
        {
            LoadWdCommand = new RelayCommand<Home>((p) => true, (p) => LoadWindow());
            Load1Command = new RelayCommand<Home>((p) => true, (p) => LoadTotalDeliveredOrders(p));
            Load2Command = new RelayCommand<Home>((p) => true, (p) => LoadTotalOrdersInDelivery(p));
        }
        private void LoadTotalOrdersInDelivery(Home p)
        {
            int count = (int)ListHD.Select(hd => hd.STATU == "Đang giao hàng").Count();
            p.tb3.Text = count.ToString();
        }
        private void LoadTotalDeliveredOrders(Home p)
        {
            int count = (int)ListHD.Select(hd => hd.STATU == "Đã nhận").Count();
            p.tb1.Text = count.ToString();
            TotalSalary = count * 30000;
        }
        private void LoadWindow()
        {
            ListHD = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs.Where(hd => hd.MAND_SHIPPER == Const.SHP.MAND));
        }
    }
}
