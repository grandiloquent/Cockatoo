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
	
	public static class CSSDelegate
	{
		[BindMenuItem(Name = "压缩 (文本)", Toolbar = "javaStrip", SplitButton = "cssSplitButton", AddSeparatorBefore = true)]
		
		public static void StandardizationGradle()
		{
			WinFormHelper.OnClipboardString(MinifyCss);
		}
		private static string MinifyCss(string str){
			var minfiy=new Minifier();
			
			return minfiy.MinifyStyleSheet(str);
		}
		
	}
}
