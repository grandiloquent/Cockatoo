
using System;
// https://github.com/JimBobSquarePants/ImageProcessor

using ImageProcessor.Plugins.WebP.Imaging.Formats;
using System.Drawing;
using System.Drawing.Imaging;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace Share
{
	
	public  static class ImageProcessorHelper
	{
		public static void ToWebp(string fileName,string outFileName)
		{
			var factory =	new ImageFactory();
			var dstFileName=string.Empty;
			if(!string.IsNullOrWhiteSpace(outFileName)){
				dstFileName=fileName.GetDirectoryName().Combine(outFileName);
			}else{
				dstFileName=fileName.ChangeExtension(".webp");
			}
			factory.Load(fileName).Format(new WebPFormat()).Save(dstFileName);
		
			
		}
		private static ImageFactory ResizeAndCropCenter(string fileName, int width, int height)
		{
			var factory = new ImageFactory();
			factory.Load(fileName);
			var w = factory.Image.Width;
			var h = factory.Image.Height;
			if (w == width && h == height)
				return factory;
			var targetRatio = width * 1.0f / height * 1.0f;
			
			int size =	Math.Max(w, h);
			
			int targetWidth = w;
			int targetHeight =h;
			if(size==w){
				targetWidth = (int)(h * targetRatio);
			}else{
				targetHeight=(int)(w/targetRatio);
			}
			
			var rectangle = new Rectangle((w - targetWidth) / 2, (h - targetHeight) / 2, targetWidth, targetHeight);
			
			factory.Crop(rectangle).Resize(new Size(width, height));
			return factory;
		}
		public static void ResizeByPercentage(string fileName, double percentage)
		{
			var factory = new ImageFactory();
			factory.Load(fileName);
			var width=factory.Image.Width;
			var height=factory.Image.Height;
			factory.Resize(new Size((int)(width*percentage),(int)(height*percentage)))
					.Format(new WebPFormat())
			.Save(fileName.ChangeFileNameAndExtension(f => string.Format("{0}-{1}x{2}", f, width, height), ext => ".webp"));
		
		}
	
		public static void ResizeCropAsJpeg(string fileName, int width, int height)
		{
			
			ResizeAndCropCenter(fileName, width, height).Resize(new Size(width, height))
				.Format(new JpegFormat())
				.Save(fileName.ChangeFileNameAndExtension(f => string.Format("{0}-{1}x{2}", f, width, height), ext => ".jpg"));
			
		}
	}
}
