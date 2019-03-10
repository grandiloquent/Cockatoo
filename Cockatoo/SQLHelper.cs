
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using Share;

namespace Cockatoo
{
	
	public static class SQLHelper
	{
		public static string GenerateForCSharp(string value)
		{
			var body = value.SubstringAfter('(');
			var tableName = value.SubstringBefore('(').Trim().SubstringAfterLast(' ').Trim().Trim('"');
			var matches = Regex.Matches(body, "(?<=\")[\\w]+(?=\"\\s)").
				Cast<Match>().
				Select(i => i.Value);
			
			//var pattern = "original.{0}=changed.{0};";
			var list1 = new List<string>();
			var list2 = new List<string>();
			var list3 = new List<string>();
			
			var p1 = " public string {0} {{ get; set; }}";
			var p2 = "{0}";
			var p3 = "{0}";
			
			
			foreach (var element in matches) {
				list1.Add(string.Format(p1, element.Capitalize()));
				//list2.Add(string.Format(p2, element));
				//list3.Add(string.Format(p3, element));
		
			}
			
			return list1.Concat(list2).Concat(list3).ConcatenateLines();
			
		}
		public static string GenerateForJavaScript(string value)
		{
			var body = value.SubstringAfter('(');
			var tableName = value.SubstringBefore('(').Trim().SubstringAfterLast(' ').Trim().Trim('"');
			var matches = Regex.Matches(body, "(?<=\")[\\w]+(?=\"\\s)").
				Cast<Match>().
				Select(i => i.Value);
			
			//var pattern = "original.{0}=changed.{0};";
			var list1 = new List<string>();
			var list2 = new List<string>();
			var list3 = new List<string>();
			
			var p1 = "obj['{0}'],";
			var p2 = "{0}";
			var p3 = "{0}";
			
			list1.Add(string.Format("INSERT INTO {0} ({1}) VALUES ({2});", tableName,
				string.Join(",", matches),
				"?,".Repeat(matches.Count()).TrimEnd(',')));
			list1.Add(string.Format("SELECT {1} FROM {0};", tableName,
				string.Join(",", matches)));
				list1.Add(string.Format("SELECT {1} FROM {0};", tableName,
			                        string.Join(",", matches.Select(i=>tableName+"."+i))));
			foreach (var element in matches) {
				list1.Add(string.Format(p1, element));
				list2.Add(string.Format(p2, element));
				list3.Add(string.Format(p3, element));
		
			}
			
			return list1.Concat(list2).Concat(list3).ConcatenateLines();
			
		}
	}
}
