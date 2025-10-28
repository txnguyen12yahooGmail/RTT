using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace CS_CommonBusinessLayer
{

    public class ImageTools : IDisposable
    {
        // Flag: Has Dispose already been called?
        // bool disposed = false;

        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        //Replacement color for green screen filter.
        public enum replacementColor
        {
            Transparent, White, Magenta
        }

        /// <summary>
        /// Resize a picture from a source file and save to a new destination file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <param name="preserveAspectRatio"></param>
        public void ResizeImageFile(string sourceFile, string destinationFile, int newWidth, int newHeight, bool preserveAspectRatio)
        {

            Size newImageSize = new Size(newWidth, newHeight);  //New size object for the destination file.
            Image orginialImage = Image.FromFile(sourceFile);   //New image object for the destination file.

            //Resize the image.
            Image reSizedImage = ResizeImage(orginialImage, newImageSize, preserveAspectRatio);

            //Save the resized image in png format.
            reSizedImage.Save(destinationFile, ImageFormat.Png);

            orginialImage.Dispose();
            reSizedImage.Dispose();

        }

        /// <summary>
        /// Resize a picture in memory, returning a new image object.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="size"></param>
        /// <param name="preserveAspectRatio"></param>
        /// <returns></returns>
        private static Image ResizeImage(Image image, Size size, bool preserveAspectRatio = true)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float)size.Width / (float)originalWidth;
                float percentHeight = (float)size.Height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            }
            else
            {
                newWidth = size.Width;
                newHeight = size.Height;
            }
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage))
            {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        /// <summary>
        /// Copy a picture source file to a new destination file and filter out the color green for a green screen affect.
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        /// <param name="newColor"></param>
        public void GreenScreenFilter(string sourceFilePath, string destinationFilePath, replacementColor newColor)
        {

            Bitmap input = new Bitmap(sourceFilePath);
            Bitmap output = new Bitmap(input.Width, input.Height);

            //Loop through all pixels of the image from top to bottom
            for (int y = 0; y < output.Height; y++)
            {
                // Loop through all pixels left to right
                for (int x = 0; x < output.Width; x++)
                {
                    // Get the pixel color
                    Color pixelColor = input.GetPixel(x, y);

                    // Every component (red, green, and blue) can have a value from 0 to 255, so determine the extremes
                    byte max = Math.Max(Math.Max(pixelColor.R, pixelColor.G), pixelColor.B);
                    byte min = Math.Min(Math.Min(pixelColor.R, pixelColor.G), pixelColor.B);

                    // Replace the pixel if green is the dominate color
                    bool replace =
                        pixelColor.G != min // green is not the smallest value
                        && (pixelColor.G == max // green is the biggest value
                        || max - pixelColor.G < 8) // or at least almost the biggest value
                        && (max - min) > 35; // 96; // minimum difference between smallest/biggest value (avoid grays)
                    if (replace)

                        switch (newColor)
                        {
                            case replacementColor.Transparent:
                                pixelColor = Color.Transparent;
                                break;
                            case replacementColor.White:
                                pixelColor = Color.White;
                                break;
                            case replacementColor.Magenta:
                                pixelColor = Color.Magenta;
                                break;
                            default:
                                pixelColor = Color.Transparent;
                                break;
                        }
                    // Set the output pixel
                    output.SetPixel(x, y, pixelColor);
                }
            }
            output.Save(destinationFilePath);
            input.Dispose();
            output.Dispose();

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    // TODO: dispose managed state (managed objects).

                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                handle.Dispose();
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ImageTools() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion




    }

}
