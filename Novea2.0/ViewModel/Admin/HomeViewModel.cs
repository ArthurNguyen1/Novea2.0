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
    public class HomeViewModel : BaseViewModel
    {
        private ObservableCollection<CUAHANG> _listCH;
        public ObservableCollection<CUAHANG> listCH { get => _listCH; set { _listCH = value; OnPropertyChanged(); } }
        private ObservableCollection<KHACH> _listKH;
        public ObservableCollection<KHACH> listKH { get => _listKH; set { _listKH = value; OnPropertyChanged(); } }
        private ObservableCollection<SHIPPER> _listSHP;
        public ObservableCollection<SHIPPER> listSHP { get => _listSHP; set { _listSHP = value; OnPropertyChanged(); } }

        public DateTime Ngay { get; set; }
        public ICommand LoadWdCommand { get; set; }
        public ICommand LoadCH { get; set; }
        public ICommand LoadKH { get; set; }
        public ICommand LoadSHP { get; set; }
        public HomeViewModel()
        {
            LoadWdCommand = new RelayCommand<Home>((p) => true, (p) => LoadWindow(p));
            LoadCH = new RelayCommand<Home>((p) => true, (p) => _LoadCH(p));
            LoadKH = new RelayCommand<Home>((p) => true, (p) => _LoadKH(p));
            LoadSHP = new RelayCommand<Home>((p) => true, (p) => _LoadSHP(p));
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
