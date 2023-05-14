using Novea2._0.Model;
using Novea2._0.View.Store_Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Novea2._0.ViewModel.Store_Owner
{
    public class KetQuaHienThiList
    {
        private int _Hour;
        public int Hour { get => _Hour; set { _Hour = value; } }
        private int _SP;
        public int SP { get => _SP; set { _SP = value; } }
        public KetQuaHienThiList(int h = 0, int sp = 0)
        {
            Hour = h; SP = sp;
        }
    }
    public class HomeViewModel : BaseViewModel
    {
        private string _DoanhThu;
        public string DoanhThu { get => _DoanhThu; set { _DoanhThu = value; OnPropertyChanged(); } }
        public string SanPham { get; set; }
        public int SL { get; set; }
        private ObservableCollection<KHACH> _listKH;
        public ObservableCollection<KHACH> listKH { get => _listKH; set { _listKH = value; OnPropertyChanged(); } }
        private ObservableCollection<SANPHAM> _listSP;
        public ObservableCollection<SANPHAM> listSP { get => _listSP; set { _listSP = value; OnPropertyChanged(); } }
        private ObservableCollection<HOADON> _listHD;
        public ObservableCollection<HOADON> listHD { get => _listHD; set { _listHD = value; OnPropertyChanged(); } }
        private ObservableCollection<CTHD> _CTHD;
        public ObservableCollection<CTHD> CTHD { get => _CTHD; set { _CTHD = value; OnPropertyChanged(); } }
        public List<KetQuaHienThiList> Data { get; set; }
        public DateTime Ngay { get; set; }
        public ICommand LoadWdCommand { get; set; }
        public ICommand LoadSP { get; set; }
        public ICommand LoadDon { get; set; }
        public ICommand LoadDoanhThu { get; set; }
        public ICommand LoadChart { get; set; }
        public HomeViewModel()
        {
            CTHD = new ObservableCollection<CTHD>(DataProvider.Ins.DB.CTHDs);
            LoadWdCommand = new RelayCommand<Home>((p) => true, (p) => LoadWindow(p));
            LoadSP = new RelayCommand<Home>((p) => true, (p) => _LoadSP(p));
            LoadDon = new RelayCommand<Home>((p) => true, (p) => SoDon(p));
            LoadDoanhThu = new RelayCommand<Home>((p) => true, (p) => LoadDT(p));
            LoadChart = new RelayCommand<Home>((p) => true, (p) => LineChart(p));
        }
        private void LineChart(Home p)
        {
            var query = from a in DataProvider.Ins.DB.CTHDs
                        join b in DataProvider.Ins.DB.HOADONs
                        on a.SOHD equals b.SOHD
                        where b.MACH == Const.MACH
                        select new HomeViewModel()
                        {
                            Ngay = (System.DateTime)b.NGMH,
                            SL = (int)a.SOLUONG,
                            SanPham = a.MASP
                        };
            Data = new List<KetQuaHienThiList>();
            for (int h = 0; h < 24; h++)
            {
                int value = 0;
                if (query.Where(x => x.Ngay.Hour == h && x.Ngay.Day == DateTime.Now.Day && x.Ngay.Month == DateTime.Now.Month && x.Ngay.Year == DateTime.Now.Year).Select(x => x.SL).Count() > 0)
                {
                    value = query.Where(x => x.Ngay.Hour == h && x.Ngay.Day == DateTime.Now.Day && x.Ngay.Month == DateTime.Now.Month && x.Ngay.Year == DateTime.Now.Year).Select(x => x.SL).Sum();
                }
                KetQuaHienThiList KetQuaHienThiList = new KetQuaHienThiList(h, value);
                Data.Add(KetQuaHienThiList);
            }
            p.Chart.ItemsSource = Data;
        }
        private void LoadDT(Home p)
        {
            long total = 0;
            if (listHD.Select(x => x.TONGTIEN).Count() != 0)
            {
                total = (long)listHD.Select(x => x.TONGTIEN).Sum();
                DoanhThu = total.ToString("#,###") + " VNĐ";
            }
            else DoanhThu = "0 VNĐ";
            p.totalrevenue.Text = DoanhThu;
        }
        private void SoDon(Home p)
        {
            int count = (int)listHD.Count();
            p.totalorders.Text = count.ToString();
        }
        private void _LoadSP(Home p)
        {
            int count = (int)listSP.Count();
            p.totalproducts.Text = count.ToString();
        }
        private void LoadWindow(Home p)
        {
            //Set hd.STATU
            listKH = new ObservableCollection<KHACH>(DataProvider.Ins.DB.KHACHes.Where(kh => kh.HOADONs.Any(hd => hd.MACH == Const.MACH && hd.STATU == "")));
            listSP = new ObservableCollection<SANPHAM>(DataProvider.Ins.DB.SANPHAMs.Where(sp => sp.MACH == Const.CH.MACH));
            listHD = new ObservableCollection<HOADON>(DataProvider.Ins.DB.HOADONs.Where(hd => hd.MACH == Const.CH.MACH && hd.STATU == ""));
        }
    }
}
