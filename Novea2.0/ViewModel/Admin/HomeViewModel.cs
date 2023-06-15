using Novea2._0.Model;
using Novea2._0.View.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Admin
{
    public class KetQuaHienThiList
    {
        private int _Hour;
        public int Hour { get => _Hour; set { _Hour = value; } }
        private int _SLND;
        public int SLND { get => _SLND; set { _SLND = value; } }
        public KetQuaHienThiList(int h = 0, int slnd = 0)
        {
            Hour = h; SLND = slnd;
        }
    }

    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<CUAHANG> _listCH;
        public ObservableCollection<CUAHANG> listCH { get => _listCH; set { _listCH = value; OnPropertyChanged(); } }
        private ObservableCollection<KHACH> _listKH;
        public ObservableCollection<KHACH> listKH { get => _listKH; set { _listKH = value; OnPropertyChanged(); } }
        private ObservableCollection<SHIPPER> _listSHP;
        public ObservableCollection<SHIPPER> listSHP { get => _listSHP; set { _listSHP = value; OnPropertyChanged(); } }

        public List<KetQuaHienThiList> Data { get; set; }
        public DateTime Ngay { get; set; }
        public ICommand LoadWdCommand { get; set; }
        public ICommand LoadCH { get; set; }
        public ICommand LoadKH { get; set; }
        public ICommand LoadSHP { get; set; }
        public ICommand LoadChart { get; set; }
        public HomeViewModel()
        {
            LoadWdCommand = new RelayCommand<Home>((p) => true, (p) => LoadWindow(p));
            LoadCH = new RelayCommand<Home>((p) => true, (p) => _LoadCH(p));
            LoadKH = new RelayCommand<Home>((p) => true, (p) => _LoadKH(p));
            LoadSHP = new RelayCommand<Home>((p) => true, (p) => _LoadSHP(p));
            LoadChart = new RelayCommand<Home>((p) => true, (p) => LineChart(p));
        }
        private void LineChart(Home p)
        {
            var _listCH_temp = from ch in DataProvider.Ins.DB.CUAHANGs
                               where ch.STATU == true
                               select new HomeViewModel()
                               {
                                   Ngay = (System.DateTime)ch.NGDK,
                               };
            var _listKH_temp = from kh in DataProvider.Ins.DB.KHACHes
                               where kh.STATU == true
                               select new HomeViewModel()
                               {
                                   Ngay = (System.DateTime)kh.NGDK,
                               };
            var _listSHP_temp = from shp in DataProvider.Ins.DB.SHIPPERs
                                where shp.STATU == true
                                select new HomeViewModel()
                                {
                                    Ngay = (System.DateTime)shp.NGDK,
                                };
            var query = _listCH_temp.Union(_listKH_temp).Union(_listSHP_temp).ToList();         

            Data = new List<KetQuaHienThiList>();
            for (int h = 0; h < 24; h++)
            {
                int value = 0;
                if (query.Where(x => x.Ngay.Hour == h && x.Ngay.Day == DateTime.Now.Day && x.Ngay.Month == DateTime.Now.Month && x.Ngay.Year == DateTime.Now.Year).Count() > 0)
                {
                    value++;
                }
                KetQuaHienThiList KetQuaHienThiList = new KetQuaHienThiList(h, value);
                Data.Add(KetQuaHienThiList);
            }
            p.Chart.ItemsSource = Data;
        }
        private void _LoadSHP(Home p)
        {
            int count = (int)listSHP.Count();
            p.totalshippers.Text = count.ToString();
        }
        private void _LoadKH(Home p)
        {
            int count = (int)listKH.Count();
            p.totalcustomers.Text = count.ToString();
        }
        private void _LoadCH(Home p)
        {
            int count = (int)listCH.Count();
            p.totalstores.Text = count.ToString();
        }
        private void LoadWindow(Home p)
        {
            //Get user throught user.STATU
            listCH = new ObservableCollection<CUAHANG>(DataProvider.Ins.DB.CUAHANGs.Where(ch => ch.STATU == true));
            listKH = new ObservableCollection<KHACH>(DataProvider.Ins.DB.KHACHes.Where(kh => kh.STATU == true));
            listSHP = new ObservableCollection<SHIPPER>(DataProvider.Ins.DB.SHIPPERs.Where(shp => shp.STATU == true));
        }
    }
}
