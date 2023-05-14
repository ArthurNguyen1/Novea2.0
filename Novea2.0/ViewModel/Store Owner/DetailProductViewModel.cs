using LiveCharts.Wpf;
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
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Novea2._0.ViewModel.Store_Owner
{
    public class DetailProductViewModel : BaseViewModel
    {
        private string MaSP_Now;
        private byte[] imageData;
        public ICommand GetMaSPCommand { get; set; }
        public ICommand LoadWdCommand { get; set; }
        public ICommand MoveWdCommand { get; set; }
        public ICommand CloseWdCommand { get; set; }
        public ICommand UpdateImageCommand { get; set; }
        public ICommand SetAvailProduct { get; set; }
        public ICommand SetUnAvailProduct { get; set; }
        public ICommand UpdateProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public DetailProductViewModel()
        {
            GetMaSPCommand = new RelayCommand<DetailProduct>((p) => true, (p) => GetMaSP(p));
            LoadWdCommand = new RelayCommand<DetailProduct>((p) => true, (p) => LoadWindow(p));
            MoveWdCommand = new RelayCommand<DetailProduct>((p) => true, (p) => MoveWindow(p));
            CloseWdCommand = new RelayCommand<DetailProduct>((p) => true, (p) => CloseWindow(p));
            UpdateImageCommand = new RelayCommand<DetailProduct>((p) => true, (p) => UpdateImage(p));
            SetAvailProduct = new RelayCommand<DetailProduct>((p) => true, (p) => SetAvailable(p));
            SetUnAvailProduct = new RelayCommand<DetailProduct>((p) => true, (p) => SetUnavailable(p));
            UpdateProductCommand = new RelayCommand<DetailProduct>((p) => true, (p) => UpdateProduct(p));
            DeleteProductCommand = new RelayCommand<DetailProduct>((p) => true, (p) => DeleteProduct(p));
        }
        void DeleteProduct(DetailProduct p)
        {
            MessageBoxResult h = MessageBox.Show("Bạn muốn xóa sản phẩm ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                var itemToRemove = DataProvider.Ins.DB.SANPHAMs.SingleOrDefault(pa => (pa.MASP == MaSP_Now));
                if (itemToRemove != null)
                {
                    DataProvider.Ins.DB.SANPHAMs.Remove(itemToRemove);
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Xóa sản phẩm thành công !", "THÔNG BÁO");
                }
            }
            p.Close();
        }
        private void UpdateProduct(DetailProduct p)
        {
            MessageBoxResult h = MessageBox.Show("Bạn muốn cập nhật sản phẩm ?", "THÔNG BÁO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (h == MessageBoxResult.Yes)
            {
                if (string.IsNullOrEmpty(p.TenSP.Text))
                {
                    MessageBox.Show("Thông tin chưa đầy đủ !", "THÔNG BÁO");
                }
                else
                {
                    foreach (SANPHAM a in DataProvider.Ins.DB.SANPHAMs.Where(pa => (pa.MASP == MaSP_Now)))
                    {
                        a.TENSP = p.TenSP.Text;
                        a.MOTA = p.Mota.Text;
                        a.HINHSP = imageData;
                    }
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Cập nhật sản phẩm thành công !", "THÔNG BÁO");
                }
            }
            p.Close();
        }
        private void SetAvailable(DetailProduct p)
        {
            var uRow = DataProvider.Ins.DB.SANPHAMs.Where(s => s.MASP == MaSP_Now).FirstOrDefault();
            uRow.AVAILABLE = true;
            DataProvider.Ins.DB.SaveChanges();
            p.txbAvail.Text = "AVAILABLE";
        }
        private void SetUnavailable(DetailProduct parameter)
        {
            var uRow = DataProvider.Ins.DB.SANPHAMs.Where(p => p.MASP == MaSP_Now).FirstOrDefault();
            uRow.AVAILABLE = false;
            DataProvider.Ins.DB.SaveChanges();
            parameter.txbAvail.Text = "UNAVAILABLE";
        }
        private void UpdateImage(DetailProduct p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                //SelectedImage = new BitmapImage(new Uri(openFileDialog.FileName));
                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(memoryStream);
                }
                imageData = memoryStream.ToArray();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(imageData);
                bitmapImage.EndInit();
                p.HinhAnh.ImageSource = bitmapImage;
            }
        }
        private void CloseWindow(DetailProduct p)
        {
            p.Close();
        }
        private void MoveWindow(DetailProduct p)
        {
            p.DragMove();
        }
        private void LoadWindow(DetailProduct p)
        {
            SANPHAM temp = DataProvider.Ins.DB.SANPHAMs.Where(s => s.MASP == MaSP_Now).FirstOrDefault();
            imageData = temp.HINHSP;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(imageData);
            bitmapImage.EndInit();
            p.HinhAnh.ImageSource = bitmapImage;
            p.TenSP.IsEnabled = true;
            p.Mota.IsEnabled = true;
            p.GiaSP.IsEnabled = false;
            p.LoaiSP.IsEnabled = false;
            p.DVT.IsEnabled = false;
            p.Size.IsEnabled = false;
            if (temp.AVAILABLE == false)
            {
                p.txbAvail.Text = "UNAVAILABLE";
            }
            else
            {
                p.txbAvail.Text = "AVAILABLE";
            }
        }
        private void GetMaSP(DetailProduct p)
        {
            MaSP_Now = p.MaSP.Text;
        }
    }
}
