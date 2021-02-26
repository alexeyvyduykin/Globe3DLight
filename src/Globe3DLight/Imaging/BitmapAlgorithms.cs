using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight
{
    public enum ImageRowOrder
    {
        BottomToTop,
        TopToBottom
    }

    public static class BitmapAlgorithms
    {
        public static ImageRowOrder RowOrder(Bitmap bitmap)
        {
            BitmapData lockedPixels = bitmap.LockBits(new Rectangle(
                0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            ImageRowOrder rowOrder;
            if (lockedPixels.Stride > 0)
            {
                rowOrder = ImageRowOrder.TopToBottom;
            }
            else
            {
                rowOrder = ImageRowOrder.BottomToTop;
            }

            bitmap.UnlockBits(lockedPixels);

            return rowOrder;
        }

        public static int SizeOfPixelsInBytes(Bitmap bitmap)
        {
            BitmapData lockedPixels = bitmap.LockBits(new Rectangle(
                0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);

            //
            // Includes row padding for Bitmap's 4 byte alignment.
            //
            int sizeInBytes = lockedPixels.Stride * bitmap.Height;
            bitmap.UnlockBits(lockedPixels);

            return sizeInBytes;
        }

    }

}
