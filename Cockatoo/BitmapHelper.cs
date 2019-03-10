namespace Cockatoo
{
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Drawing.Drawing2D;
	using System;
	using System.Linq;
	public class BitmapHelper
	{
		private static void SaveJpegWithCompressionSetting(Image image, string fileName, long compression)
		{
			var eps = new EncoderParameters(1);
			eps.Param[0] = new EncoderParameter(Encoder.Quality, compression);
			var ici = GetEncoderInfo("image/jpeg");
			image.Save(fileName, ici, eps);
		}
		private static ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			var encoders = ImageCodecInfo.GetImageEncoders();
			return encoders.FirstOrDefault(t => t.MimeType == mimeType);
		}
		public static void SaveJpegWithCompression(Image image, string fileName, long compression)
		{
			SaveJpegWithCompressionSetting(image, fileName, compression);
		}
		public static Image ResizeImageKeepAspectRatio(Image source)
		{
			
			var ratio =	16 / 9.0;
			var h = source.Width / ratio;
			Image img;
			if (h <= source.Height) {
				img = ResizeImageKeepAspectRatio(source, source.Width, (int)h);
			} else {
				img =	ResizeImageKeepAspectRatio(source, ((int)(source.Height * ratio)), source.Height);
			}
			return img;
		}
		public static  Image ResizeImageKeepAspectRatio(Image source, int width, int height)
		{
			Image result = null;

			try {
				if (source.Width != width || source.Height != height) {
					// Resize image
					float sourceRatio = (float)source.Width / source.Height;

					using (var target = new Bitmap(width, height)) {
						using (var g = System.Drawing.Graphics.FromImage(target)) {
							g.CompositingQuality = CompositingQuality.HighQuality;
							g.InterpolationMode = InterpolationMode.HighQualityBicubic;
							g.SmoothingMode = SmoothingMode.HighQuality;

							// Scaling
							float scaling;
							float scalingY = (float)source.Height / height;
							float scalingX = (float)source.Width / width;
							if (scalingX < scalingY)
								scaling = scalingX;
							else
								scaling = scalingY;

							int newWidth = (int)(source.Width / scaling);
							int newHeight = (int)(source.Height / scaling);

							// Correct float to int rounding
							if (newWidth < width)
								newWidth = width;
							if (newHeight < height)
								newHeight = height;

							// See if image needs to be cropped
							int shiftX = 0;
							int shiftY = 0;

							if (newWidth > width) {
								shiftX = (newWidth - width) / 2;
							}

							if (newHeight > height) {
								shiftY = (newHeight - height) / 2;
							}

							// Draw image
							g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
						}

						result = (Image)target.Clone();
					}
				} else {
					// Image size matched the given size
					result = (Image)source.Clone();
				}
			} catch (Exception) {
				result = null;
			}

			return result;
		}
	}
}