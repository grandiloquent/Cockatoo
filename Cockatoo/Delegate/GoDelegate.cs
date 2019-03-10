namespace  Cockatoo
{
	using Share;
	using System.Net.Http;
	using System.Collections.Generic;
	using System;
	using System.Windows.Forms;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Text;
	
	public static class GoDelegate
	{
		private static string ConvertRequestHeaders(string value)
		{
			
			var lines = value.Split(new char[]{ '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(i => i.Split(new char[]{':'},2));
			
			var list1 = new List<string>();
			var list2 = new List<string>();
			
			
			var p2 = "w.Header().Set(\"{0}\",\"{1}\");";
			
			foreach (var element in lines) {
				list2.Add(string.Format(p2, element.First().Trim(),element.Last().Trim()));
				
			}
			return  list2.ConcatenateLines();
			
		}
		private static string MinifyHtml(string value)
		{
			// https://github.com/JadeX/JadeX.HtmlMinifier/blob/c6617d602eedc31d90a8637cbb7d32006f48605c/Filters/MinifyHtmlFilter.cs
			var m = new WebMarkupMin.Core.HtmlMinifier(
				        new WebMarkupMin.Core.HtmlMinificationSettings {
					AttributeQuotesRemovalMode = WebMarkupMin.Core.HtmlAttributeQuotesRemovalMode.KeepQuotes,
					RemoveTagsWithoutContent = false,
					EmptyTagRenderMode = WebMarkupMin.Core.HtmlEmptyTagRenderMode.Slash,
				}
			        );
			return m.Minify(value).MinifiedContent;

		}
		private static String GenerateTemplateInBytes(string v)
		{
			v = MinifyHtml(v);
			
			var list1=new List<string>();
			
			var array = Regex.Split(v, "{{[^}]*?}}");
			var matches = Regex.Matches(v, "{{([^}]*?)}}").Cast<Match>().Select(i => i.Groups[1].Value).ToArray();
			var index = 0;
			foreach (var item in array) {
				var bytes = new UTF8Encoding(false).GetBytes(item);
//				
//				var bytes = new UTF8Encoding(false).GetBytes(item).Select(i => {
//					if (i > 127) {
//						return "(byte)" + i;
//					} else {
//						return i.ToString();
//					}
//				}).ToArray();

				list1.Add(string.Format("var {0} = []byte{{{1}}}",index < matches.Length ? matches[index] : "tail",string.Join(",", bytes)));
				index++;

			}
			
			return list1.ConcatenateLines();
		}
		[BindMenuItem(Name = "HTML 数组模板", Toolbar = "javaStrip", SplitButton = "goButton")]
		
		public static void GenerateHTMLArrayTemplate()
		{
		
			WinFormHelper.OnClipboardString(GenerateTemplateInBytes);
			
		}
		[BindMenuItem(Name = "格式化 Chrome 请求头 (文本)", Toolbar = "javaStrip", SplitButton = "goButton")]
		
		public static void ConvertRequestHeaders()
		{
		
			WinFormHelper.OnClipboardString(ConvertRequestHeaders);
			
		}
		
	}
}