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
            listKHtemp = new ObservableCollection<KHACH>(DataProvider.Ins.DB.KHACHes);
            //Set hd.STATU
            listKH = new ObservableCollection<KHACH>(listKHtemp.GroupBy(k => k.HOTEN).Select(grp => grp.FirstOrDefault()).Where(kh => kh.HOADONs.Any(hd => hd.MACH == Const.MACH && hd.STATU == "")));
            p.cbbSort.SelectedIndex = 0;
            p.cbbSort.Items.SortDescriptions.Add(new SortDescription("HOTEN", ListSortDirection.Ascending));
        }
        private void Sort(View.Store_Owner.Customer p)
        {
            var SortDirection = p.cbbSort.SelectedIndex.ToString() == "0" ? ListSortDirection.Ascending : ListSortDirection.Descending;
            p.ListViewKH.Items.SortDescriptions[0] = new SortDescription("HOTEN", SortDirection);
        }
        private void Search(View.Store_Owner.Customer p)
        {
            ObservableCollection<KHACH> temp = new ObservableCollection<KHACH>();
            if (p.tbSearch.Text == "")
            {
                p.ListViewKH.ItemsSource = listKH;
            } 
            else
            {
                foreach (KHACH k in listKH)
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
