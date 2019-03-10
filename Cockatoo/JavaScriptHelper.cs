using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Share;
using HtmlAgilityPack;
using Microsoft.Ajax.Utilities;
using System.IO;


namespace  Cockatoo
{
	
	public static class JavaScriptHelper
	{
		public static string MinifyJavaScript(string str)
		{
			var js =	new  Minifier();
			
			return	js.MinifyJavaScript(str);
		
		}
		
		public static void CombineCssFiles()
		{
			 // @"C:\Users\psycho\Desktop\Scripts\Go\Halalla\public\css";
			
			var dir = @"C:\NetCore\wwwroot\stylesheets";
			
//			string[] files = new string[] {
//				"_reset.css",
//				"album-list.css",
//				"player.css",
//				"layout.css",
//				"button.css",
//				"toolbar.css",
//				"owner.css",
//				"list.css",
//				"badge.css",
//				"actions.css",
//				"spinner.css",
//				"modal.css",
//				"unmute.css",
//				"loading-more.css",
//				"search-sub-menu-renderer.css",
//				"toast.css"
//			};
			
			var files=Directory.GetFiles(dir,"*.css").Where(i=>!i.GetFileName().StartsWith(".")).OrderBy(i=>i.GetFileName());
			var sb = new StringBuilder();
			foreach (var element in files) {
				sb.AppendLine((Path.Combine(dir, element).ReadAllText()));
			}
			var min = new Minifier();
			var str = min.MinifyStyleSheet(sb.ToString());
			
			Path.Combine(Path.GetDirectoryName(dir), "app.min.css").WriteAllText(str);
		}
		public static void MinifyJavaScriptInFile(string fileName)
		{
			fileName.ChangeFileName(f => f + ".min").WriteAllText(MinifyJavaScript(fileName.ReadAllText()));
		
		}
		public static string SortFunctions(string str)
		{
			
			var list = str.ToBlocks();
			
			var names = list.Select(i => i.SubstringBefore('(').TrimEnd().SubstringAfterLast(' '));
			
			
			return "/*\n " + string.Join("\n", names.OrderBy(i => i).Select(i => {
				if (i.StartsWith("jqLite")) {
					return	i.Substring(6).Decapitalize() + ":" + i + ",";
				} else {
					return	i + ":" + i + ",";
				}
			})) +
			"\n*/\n" +
			"\n\n\n" +
			string.Join("\n", list.OrderBy(v => v.SubstringBefore('(').TrimEnd().SubstringAfterLast(' ')));
		}
	}
}
