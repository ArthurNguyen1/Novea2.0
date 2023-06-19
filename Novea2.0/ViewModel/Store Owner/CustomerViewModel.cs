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

namespace Novea2._0.ViewModel.Store_Owner
{
    public class CustomerViewModel : BaseViewModel
    {
        private ObservableCollection<KHACH> listKH;
        public ObservableCollection<KHACH> ListKH { get => listKH; set { listKH = value; OnPropertyChanged(); } }

        private ObservableCollection<KHACH> listKHtemp;
        public ObservableCollection<KHACH> ListKHtemp { get => listKHtemp; set { listKHtemp = value; OnPropertyChanged(); } }

        public ICommand LoadCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public CustomerViewModel()
        {
            LoadCommand = new RelayCommand<View.Store_Owner.Customer>((p) => true, (p) => Load(p));
            SortCommand = new RelayCommand<View.Store_Owner.Customer>((p) => { return p != null; }, (p) => Sort(p));
            SearchCommand = new RelayCommand<View.Store_Owner.Customer>((p) => { return p != null; }, (p) => Search(p));
        }
        private void Load(View.Store_Owner.Customer p)
        {
            ListKHtemp = new ObservableCollection<KHACH>(DataProvider.Ins.DB.KHACHes);
            //Set hd.STATU
            //ListKH = new ObservableCollection<KHACH>(ListKHtemp.GroupBy(k => k.HOTEN).Select(grp => grp.FirstOrDefault()).Where(kh => kh.HOADONs.Any(hd => hd.MACH == Const.MACH && hd.STATU == "Đã nhận")));
            p.cbbSort.SelectedIndex = 0;
            Sort(p); 
            Search(p);
        }
        private void Sort(View.Store_Owner.Customer p)
        {
            switch (p.cbbSort.SelectedIndex.ToString())
            {
                case "0":
                    {
                        ListKH = new ObservableCollection<KHACH>(ListKHtemp.GroupBy(k => k.HOTEN).Select(grp => grp.FirstOrDefault()).Where(kh => kh.HOADONs.Any(hd => hd.MACH == Const.MACH && hd.STATU == "Đã nhận")).OrderBy(m => m.HOTEN));
                        p.ListViewKH.ItemsSource = ListKH;
                        break;
                    }
                case "1":
                    {
                        ListKH = new ObservableCollection<KHACH>(ListKHtemp.GroupBy(k => k.MAND).Select(grp => grp.FirstOrDefault()).Where(kh => kh.HOADONs.Any(hd => hd.MACH == Const.MACH && hd.STATU == "Đã nhận")).OrderByDescending(m => m.HOTEN));
                        p.ListViewKH.ItemsSource = ListKH;
                        break;
                    }
            }
        }
        private void Search(View.Store_Owner.Customer p)
        {
            ObservableCollection<KHACH> temp = new ObservableCollection<KHACH>();
            if (p.tbSearch.Text == "")
            {
                p.ListViewKH.ItemsSource = ListKH;
            } 
            else
            {
                foreach (KHACH k in ListKH)
                {
                    if (k.HOTEN.ToLower().Contains(p.tbSearch.Text.ToLower()))
                    {
                        temp.Add(k);
                    }
                }
                if (temp != null)
                {
                    p.ListViewKH.ItemsSource = temp;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy họ tên");
                }
            }
        }
    }
}
