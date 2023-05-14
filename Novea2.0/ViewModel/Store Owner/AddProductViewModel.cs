using Microsoft.Win32;
using Novea2._0.Model;
using Novea2._0.View.Store_Owner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Novea2._0.ViewModel.Store_Owner
{
    public class AddProductViewModel : BaseViewModel
    {
        public ICommand MinimizeWdCommand { get; set; }
        public ICommand CloseWdCommand { get; set; }
        public ICommand MoveWdCommand { get; set; } 
        public ICommand AddImageCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        private byte[] imageData;
        private BitmapImage selectedImage;
        public BitmapImage SelectedImage
        {
            get { return selectedImage; }
            set { selectedImage = value; OnPropertyChanged(); }
        }
        public AddProductViewModel() 
        {
            MinimizeWdCommand = new RelayCommand<AddProduct>((p) => true, (p) => MinimizeWd(p));
            CloseWdCommand = new RelayCommand<AddProduct>((p) => true, (p) => CloseWd(p));
            AddImageCommand = new RelayCommand<Image>((p) => true, (p) => AddImage());
            MoveWdCommand = new RelayCommand<AddProduct>((p) => true, (p) => MoveWindow(p));
            AddProductCommand = new RelayCommand<AddProduct>((p) => true, (p) => AddProduct(p));
        }
        private void AddProduct(AddProduct p)
        {
            if (string.IsNullOrEmpty(p.MaSp.Text) || string.IsNullOrEmpty(p.TenSp.Text) || string.IsNullOrEmpty(p.LoaiSp.Text) || string.IsNullOrEmpty(p.GiaSp.Text) || string.IsNullOrEmpty(p.SizeSp.Text) || string.IsNullOrEmpty(p.DvSp.Text))
            {
                MessageBox.Show("Bạn chưa nhập đủ thông tin.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult h = MessageBox.Show("Bạn muốn thêm sản phẩm mới ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (h == MessageBoxResult.Yes)
                {
                    if (DataProvider.Ins.DB.SANPHAMs.Where(s => s.MASP == p.MaSp.Text).Count() > 0)
                    {
                        MessageBox.Show("Mã sản phẩm đã tồn tại.", "Thông Báo");
                    }
                    else
                    {
                        SANPHAM sanpham = new SANPHAM();
                        sanpham.MASP = p.MaSp.Text;
                        sanpham.TENSP = p.TenSp.Text;
                        try
                        {
                            sanpham.DONGIA = int.Parse(p.GiaSp.Text);
                        }
                        catch
                        {
                            MessageBox.Show("Giá sản phẩm không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (int.Parse(p.GiaSp.Text) < 0)
                        {
                            MessageBox.Show("Giá sản phẩm không hợp lệ !", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        sanpham.LOAISP = p.LoaiSp.Text;
                        sanpham.DONVI = p.DvSp.Text;
                        sanpham.SIZE = p.SizeSp.Text;
                        sanpham.MOTA = p.MotaSp.Text;
                        sanpham.MACH = Const.CH.MACH;
                        sanpham.AVAILABLE = true;
                        sanpham.HINHSP = imageData;
                        DataProvider.Ins.DB.SANPHAMs.Add(sanpham);
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Thêm sản phẩm mới thành công !", "THÔNG BÁO");
                        p.TenSp.Clear();
                        p.LoaiSp.SelectedItem = null;
                        p.GiaSp.Clear();
                        p.DvSp.Clear();
                        p.SizeSp.SelectedItem = null;
                        p.MotaSp.Clear();
                        SelectedImage = null;
                    }
                }
            }
            p.Close();
        }
        private void MoveWindow(AddProduct p)
        {
            p.DragMove();
        }
        private void AddImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);
                }
                imageData = memoryStream.ToArray();
            }
        }
        private void MinimizeWd(AddProduct p)
        {
            p.WindowState = WindowState.Minimized;
        }
        private void CloseWd(AddProduct p)
        {
            p.Close();
        }
    }
}
