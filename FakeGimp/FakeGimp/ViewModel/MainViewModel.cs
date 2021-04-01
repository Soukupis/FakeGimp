using FakeGimp.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FakeGimp.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private BitmapImage image;
        private BitmapImage original_image;
        private Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;
        public BitmapImage Image
        {
            get => image;
            set
            {
                image = value; NotifyPropertyChanged();
            }
        }
        public BitmapImage OriginImage
        {
            get => original_image;
            set
            {
                original_image = value;
                NotifyPropertyChanged();
            }
        }

        private Filters filters { get; set; }
        public Command LoadImage { get; set; }
        public Command GrayScale { get; set; }
        public Command RedScale { get; set; }
        public Command GreenScale { get; set; }
        public Command BlueScale { get; set; }
        public Command PinkScale { get; set; }
        public Command BlurSerial { get; set; }
        public Command BlurParallel { get; set; }
        public Command Reset { get; set; }
        

        public MainViewModel()
        {
            filters = new Filters();
            LoadImage = new Command(
                () => { LoadImageExecute(); },
                () => { return true; }
            );
            GrayScale = new Command(
                () => { Image = filters.GrayScale(Image); },
                () => { return true; }
            );
            RedScale = new Command(
                () => { Image = filters.Shades(0, Image); },
                () => { return true; }
            );
            GreenScale = new Command(
                () => { Image = filters.Shades(1, Image); },
                () => { return true; }
            );
            BlueScale = new Command(
                () => { Image = filters.Shades(2, Image); },
                () => { return true; }
            );
            PinkScale = new Command(
                () => { Image = filters.Shades(3, Image); },
                () => { return true; }
            );
            BlurSerial = new Command(
               () => { Image = filters.BlurSerial(Image); },
               () => { return true; }
               );
             BlurParallel = new Command(
               () => { Image = filters.BlurParallel(Image); },
            
               () => { return true; }
           );
        }
        public async void LoadImageExecute()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Obrázky (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            var result = dialog.ShowDialog();
            BitmapImage imgSource = new BitmapImage(new Uri(dialog.FileName));
            Image = imgSource;
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
