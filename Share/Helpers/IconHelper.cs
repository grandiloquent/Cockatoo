namespace Share
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using System.Xml.Linq;
	public static class IconHelper
	{
		private static Font FindBestFitFont(Graphics g, String text, Font font, Size proposedSize)
		{
			// Compute actual size, shrink if needed
			while (true) {
				SizeF size = g.MeasureString(text, font);
				// It fits, back out
				if (size.Height <= proposedSize.Height &&
				                size.Width <= proposedSize.Width) {
					return font;
				}
				// Try a smaller font (90% of old size)
				Font oldFont = font;
				font = new Font(font.Name, (float)(font.Size * .9), font.Style);
				oldFont.Dispose();
			}
		}


		public static void GenereteIcon(this string letter, int width = 64)
		{
			var bytes = "#e91e63".ParseHexColor();
			Color color = Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
			// Create a PNG inside an ICO.
			using (var bitmap = new Bitmap(width, width, PixelFormat.Format32bppArgb)) {
				using (var g = Graphics.FromImage(bitmap)) {
					//StringFormat format = new StringFormat();
					//format.LineAlignment = StringAlignment.Center;
					//format.Alignment = StringAlignment.Center;
					float emSize = width;
					Font font = new Font(FontFamily.GenericSansSerif, emSize, FontStyle.Regular);
					font = FindBestFitFont(g, letter.ToString(), font, new Size(width, width));
					SizeF size = g.MeasureString(letter.ToString(), font);
					g.Clear(color);
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.PixelOffsetMode = PixelOffsetMode.HighQuality;
					g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
					g.TextContrast = 0;
					g.DrawString(letter, font, new SolidBrush(Color.White), (width - size.Width) / 2, (width - font.Height) / 2);
					g.Flush();
				}
				//bitmap.MakeTransparent(color);
				// RemoveChroma(bitmap, Color.White, color);
				bitmap.Save((letter.GetValidFileName() + ".png").GetDesktopPath(), ImageFormat.Png);
				var output = new FileStream((letter.GetValidFileName() + ".ico").GetDesktopPath(), FileMode.OpenOrCreate);
				Icon.FromHandle(bitmap.GetHicon()).Save(output);
				output.Dispose();
			}
		}
		 
		public static void GenerateTextIcon(this string letter,string fileName, int width = 40)
		{
			var colors = new string[] {"#F44336",
				"#E91E63",
				"#9C27B0",
				"#673AB7",
				"#3F51B5",
				"#2196F3",
				"#03A9F4",
				"#00BCD4",
				"#009688",
				"#4CAF50",
				"#8BC34A",
				"#CDDC39",
				"#FFEB3B",
				"#FFC107",
				"#FF9800",
				"#FF5722",
				"#795548",
				"#9E9E9E",
				"#607D8B",
				"#F44336",
				"#E91E63",
				"#9C27B0",
				"#673AB7",
				"#3F51B5",
				"#2196F3",
				"#03A9F4",
				"#00BCD4",
				"#009688",
				"#4CAF50",
				"#8BC34A",
				"#CDDC39",
				"#FFEB3B",
				"#FFC107",
				"#FF9800",
				"#FF5722",
				"#795548",
				"#9E9E9E",
				"#607D8B"
			};
			var random=new Random(DateTime.Now.Millisecond);
			var index=random.Next(0,colors.Length-1);
			var bytes=colors[index].ParseHexColor();
			Color color = Color.FromArgb(bytes[0], bytes[1], bytes[2], bytes[3]);
			// Create a PNG inside an ICO.
			using (var bitmap = new Bitmap(width, width, PixelFormat.Format32bppArgb)) {
				using (var g = Graphics.FromImage(bitmap)) {
					//StringFormat format = new StringFormat();
					//format.LineAlignment = StringAlignment.Center;
					//format.Alignment = StringAlignment.Center;
					float emSize = width;
					Font font = new Font("微软雅黑", (int)(24.0*72.0/96.0), FontStyle.Regular);
					//font = FindBestFitFont(g, letter.ToString(), font, new Size(width, width));
					SizeF size = g.MeasureString(letter.ToString(), font);
					g.Clear(color);
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.PixelOffsetMode = PixelOffsetMode.HighQuality;
					g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
					g.TextContrast = 0;
					g.DrawString(letter, font, new SolidBrush(Color.White), (width - size.Width) / 2,(width-size.Height)/2);
					g.Flush();
				}
				//bitmap.MakeTransparent(color);
				// RemoveChroma(bitmap, Color.White, color);
				bitmap.Save((fileName + ".png").GetDesktopPath(), ImageFormat.Png);
			
			}
		}
	}
}