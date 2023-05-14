using Microsoft.Win32;
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
        public ICommand AddImageCommand { get; set; }
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
