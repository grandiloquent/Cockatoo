namespace  Cockatoo
{
	using Share;
	using System.Net.Http;
	using System.Collections.Generic;
	using System;
	using System.Windows.Forms;
	using System.Linq;
	using System.Diagnostics;
	using System.IO;
	using System.Text;
	using Microsoft.Ajax.Utilities;
	
	public static class JavaScriptDelegate
	{
		public static void CombineJavaScriptFiles(String dir)
		{
		
			var files=Directory.GetFiles(dir,"*.js").OrderBy(i=>i.GetFileName()).ToArray();
			var sb = new StringBuilder();
			foreach (var element in files) {
				sb.AppendLine(element.ReadAllText());
				
				//sb.AppendLine((Path.Combine(dir, element).ReadAllText()));
			}
			var min = new Minifier();
			var str = min.MinifyJavaScript(sb.ToString());
			
			Path.Combine(Path.GetDirectoryName(dir), "app.min.js").WriteAllText(str);
		}
		public static void CompileTypeScript(string sourceDir)
		{
			string outFile=Path.GetDirectoryName(sourceDir).Combine("bundle.min.js");
			var files=Directory.GetFiles(sourceDir,"*.ts");
			
			var sb=new StringBuilder();
			
			
			sb.Append("--alwaysStrict ")
				.Append("--removeComments ")
				.Append("--outFile ")
				.Append('"')
				.Append(outFile)
				.Append('"')
				.Append(' ');
			
			foreach (var element in files) {
				sb.Append('"')
				.Append(element)
				.Append('"')
					.Append(' ');
			}
			
			Clipboard.SetText(sb.ToString());
			
			Process.Start(new ProcessStartInfo(){
			              
			              	FileName="tsc",
			              	WindowStyle=ProcessWindowStyle.Hidden,
			              	Arguments=sb.ToString()
			              });
		}
	}
}
