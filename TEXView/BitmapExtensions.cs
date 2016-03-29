using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace TEXView
{
    public static class BitmapExtensions
    {
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);

        public static System.Drawing.Bitmap BitmapSourceFromArray(byte[] pixels, int width, int height, int bitDept, BitmapPalette pal = null)
        {
            Graphics g = Graphics.FromHwnd(Process.GetCurrentProcess().MainWindowHandle);
            if (bitDept == 8)
            {
                return BitmapSourceFromArray(pixels, width, height, System.Windows.Media.PixelFormats.Indexed8, BitmapPalettes.Halftone256Transparent);
            }
            else if (bitDept == 16)
            {
                return BitmapSourceFromArray(pixels, width, height, System.Windows.Media.PixelFormats.Bgr565, pal);
            }
            else if (bitDept == 24)
            {
                return BitmapSourceFromArray(pixels, width, height, System.Windows.Media.PixelFormats.Bgr24, pal);
            }
            else if (bitDept == 32)
            {
                return BitmapSourceFromArray(pixels, width, height, System.Windows.Media.PixelFormats.Bgr32, pal);
            }
            return null;
        }

        public static System.Drawing.Bitmap BitmapSourceFromArrayIndex(byte[] pixels, int width, int height, int bitDept, BitmapPalette pal = null)
        {
            Graphics g = Graphics.FromHwnd(Process.GetCurrentProcess().MainWindowHandle);
            return BitmapSourceFromArray(pixels, width, height, System.Windows.Media.PixelFormats.Indexed8, pal);
        }

        public static System.Drawing.Bitmap BitmapSourceFromArray(byte[] pixels, int width, int height, System.Windows.Media.PixelFormat pxF, BitmapPalette pal = null)
        {
            Graphics g = Graphics.FromHwnd(Process.GetCurrentProcess().MainWindowHandle);
            WriteableBitmap bitmap = new WriteableBitmap(width, height, g.DpiX, g.DpiY, pxF, pal);
            bitmap.WritePixels(new System.Windows.Int32Rect(0, 0, width, height), pixels, width * (bitmap.Format.BitsPerPixel / 8), 0);
            return BitmapFromSource(bitmap);
        }

        public static System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            MemoryStream outStream = new MemoryStream();
            BitmapEncoder enc = new BmpBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bitmapsource));
            enc.Save(outStream);
            bitmap = new System.Drawing.Bitmap(outStream);
            return bitmap;
        }

    }
}
