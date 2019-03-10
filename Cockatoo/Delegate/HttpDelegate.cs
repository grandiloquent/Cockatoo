namespace  Cockatoo
{
	using Share;
	using System.Net.Http;
	using System.Collections.Generic;
	using System;
	using System.Windows.Forms;
	using System.Linq;
	using System.Text.RegularExpressions;
	
	public class HttpDelegate
	{
		private static string ConvertRequestHeaders(string value)
		{
			
			var lines = value.Split(new char[]{ '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(i => i.Split(new char[]{':'},2));
			
			var list1 = new List<string>();
			var list2 = new List<string>();
			
			
			var p1 = "httpMessage.Headers.Add(\"{0}\",\"{1}\");";
			var p2 = "w.Header().Set(\"{0}\",\"{1}\");";
			
			foreach (var element in lines) {
				list1.Add(string.Format(p1, element.First().Trim(),element.Last().Trim()));
				list2.Add(string.Format(p2, element.First().Trim(),element.Last().Trim()));
				
			}
			return  list1.Concat(list2).ConcatenateLines();
			
		}
		
		[BindMenuItem(Name = "格式化 Chrome 请求头 (文本)", Toolbar = "toolStrip", SplitButton = "httpButton")]
		
		public static void ConvertRequestHeaders()
		{
		
			WinFormHelper.OnClipboardString(ConvertRequestHeaders);
			
		}
		
	}
}