using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FakeGimp.Model
{
    class Filters
    {
        public BitmapImage image { get; set; }
        private BitmapImage original_image;
        private int[,] array;

        public BitmapImage GrayScale(BitmapImage image)
        {
            if (image != null)
            {
                array = BitmapImageToArray2D(image);
                for (int x = 0; x < image.PixelHeight; x++)
                {
                    for (int y = 0; y < image.PixelWidth; y++)
                    {
                        int red = ((array[x, y] & 0x00FF0000) >> 16);
                        int green = ((array[x, y] & 0x0000FF00) >> 8);
                        int blue = (array[x, y] & 0x000000FF);
                        int prumer = (int)(red * 0.3 + green * 0.59 + blue * 0.11) / 3;
                        array[x, y] = (int)(array[x, y] & 0xFF000000) + prumer + (prumer << 8) + (prumer << 16);

                    }
                }
                return ConvertWriteableBitmapToBitmapImage(Array2DToWriteableBitmap(array, image));
            }
            return null;
        }
        public BitmapImage BlurSerial(BitmapImage image)
        {
            int blurAmmount = 3;
            if (image != null)
            {
                array = BitmapImageToArray2D(image);
                for (int x = blurAmmount; x < image.PixelHeight-1- blurAmmount; x++)
                {
                    for (int y = blurAmmount; y < image.PixelWidth-1- blurAmmount; y++)
                    {
                        int prevX = array[x - blurAmmount, y];
                        int nextX = array[x + blurAmmount, y];
                        int nextY = array[x, y+ blurAmmount];
                        int prevY = array[x, y - blurAmmount];
                        
                        int avgRed = ((((prevX & 0x00FF0000) >> 16) + ((nextX & 0x00FF0000) >> 16) + ((prevY & 0x00FF0000) >> 16) + ((nextY & 0x00FF0000) >> 16))/4);
                        int avgGreen = ((((prevX & 0x0000FF00) >> 8) + ((nextX & 0x0000FF00) >> 8) + ((prevY & 0x0000FF00) >> 8) + ((nextY & 0x0000FF00) >> 8))/4);
                        int avgBlue = ((prevX & 0x000000FF) + (nextX & 0x000000FF) + (prevY & 0x000000FF) + (nextY & 0x000000FF))/4;
                        array[x, y] = (int)((array[x, y] & 0xFF000000) + (avgRed << 16) + (avgGreen << 8) + (avgBlue));
                        

                    }
                }
                return ConvertWriteableBitmapToBitmapImage(Array2DToWriteableBitmap(array, image));
            }
            return null;
        }
        public BitmapImage Shades(int channel, BitmapImage image)
        {
            if (image != null)
            {
                Stopwatch timer = new Stopwatch();
                uint maska = 0;
                array = BitmapImageToArray2D(image);
                timer.Start();
                switch (channel)
                {
                    case 0: maska = 0xFFFF0000; break;
                    case 1: maska = 0xFF00FF00; break;
                    case 2: maska = 0xFF0000FF; break;
                    case 3: maska = 0xFFFF00FF; break;
                }
                for (int x = 0; x < image.PixelHeight; x++)
                {
                    for (int y = 0; y < image.PixelWidth; y++)
                    {
                        array[x, y] &= (int)maska;
                    }

                }
                timer.Stop();
                Console.WriteLine("čas na jedno vlákno: " + timer.ElapsedMilliseconds);
                return ConvertWriteableBitmapToBitmapImage(Array2DToWriteableBitmap(array, image));
            }
            return null;
        }
        public static int[,] BitmapImageToArray2D(BitmapImage src)
        {
            int[,] array2D = new int[src.PixelHeight, src.PixelWidth];

            WriteableBitmap wb = new WriteableBitmap(src);
            int width = wb.PixelWidth;
            int height = wb.PixelHeight;
            int bytesPerPixel = (wb.Format.BitsPerPixel + 7) / 8;
            int stride = wb.BackBufferStride;
            wb.Lock();
            unsafe
            {
                byte* pImgData = (byte*)wb.BackBuffer;
                int cRowStart = 0;
                int cColStart = 0;
                for (int row = 0; row < height; row++)
                {
                    cColStart = cRowStart;
                    for (int col = 0; col < width; col++)
                    {
                        byte* bPixel = pImgData + cColStart;
                        //bPixel[0] // Blue
                        //bPixel[1] // Green
                        //bPixel[2] // Red
                        int pixel = bPixel[2]; //Red
                        pixel = (pixel << 8) + bPixel[1]; //Green
                        pixel = (pixel << 8) + bPixel[0]; //Blue
                        array2D[row, col] = pixel;

                        cColStart += bytesPerPixel;
                    }
                    cRowStart += stride;
                }
            }
            wb.Unlock();
            wb.Freeze();

            return array2D;
        }

        public static WriteableBitmap Array2DToWriteableBitmap(int[,] array2D, BitmapImage src)
        {
            WriteableBitmap wb = new WriteableBitmap(src);
            int width = wb.PixelWidth;
            int height = wb.PixelHeight;
            int bytesPerPixel = (wb.Format.BitsPerPixel + 7) / 8;
            int stride = wb.BackBufferStride;
            wb.Lock();
            unsafe
            {
                byte* pImgData = (byte*)wb.BackBuffer;
                int cRowStart = 0;
                int cColStart = 0;
                for (int row = 0; row < height; row++)
                {
                    cColStart = cRowStart;
                    for (int col = 0; col < width; col++)
                    {
                        byte* bPixel = pImgData + cColStart;

                        bPixel[0] = (byte)((array2D[row, col] & 0xFF));// Blue
                        bPixel[1] = (byte)((array2D[row, col] & 0xFF00) >> 8);// Green
                        bPixel[2] = (byte)((array2D[row, col] & 0xFF0000) >> 16);// Red

                        cColStart += bytesPerPixel;
                    }
                    cRowStart += stride;
                }
            }
            wb.Unlock();
            wb.Freeze();

            return wb;
        }

        public static BitmapImage ConvertWriteableBitmapToBitmapImage(WriteableBitmap wbm)
        {
            BitmapImage bmImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wbm));
                encoder.Save(stream);
                bmImage.BeginInit();
                bmImage.CacheOption = BitmapCacheOption.OnLoad;
                bmImage.StreamSource = stream;
                bmImage.EndInit();
                bmImage.Freeze();
            }
            return bmImage;
        }


    }
}
