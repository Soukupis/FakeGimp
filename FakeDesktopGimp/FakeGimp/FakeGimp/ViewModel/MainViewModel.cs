using FakeGimp.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FakeGimp.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private BitmapImage image;
        public BitmapImage Image
        {
            get => image;
            set
            {
                image = value; NotifyPropertyChanged();
            }
        }

        private Filters filters { get; set; }
        public Command LoadImage { get; set; }
        public Command GrayScale { get; set; }
        public Command OdstinyCervene { get; set; }
        public Command OdstinyZelene { get; set; }
        public Command OdstinyModre { get; set; }
        
        
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
            OdstinyCervene = new Command(
                () => { Image = filters.Shades(0, Image); },
                () => { return true; }
            );
            OdstinyZelene = new Command(
                () => { Image = filters.Shades(1, Image); },
                () => { return true; }
            );
            OdstinyModre = new Command(
                () => { Image = filters.Shades(2, Image); },
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
