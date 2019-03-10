
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using Share;

namespace Cockatoo
{
	
	public static class AspNetHelper
	{
		
		public static string GenerateForm(string value)
		{
			
			var matches = Regex.Matches(value, "[\\w]+(?= \\{)").
				Cast<Match>().
				Select(i => i.Value);
			
			var pattern1 = "<div class=\"form-group\"><label asp-for=\"{0}\"></label> <input asp-for=\"{0}\" class=\"form-control\"/></div>";
			var pattern2 = "<input name=\"original.{0}\" value=\"@Model?.{0}\" type=\"hidden\" />";
			var pattern3 = "formData.Add(new StringContent(\"{0}\"),\"{0}\");";
			
			var pattern5 = "\"{0}\":\"\",";
			
			var list = new List<string>();
			foreach (var element in matches) {
				list.Add(string.Format(pattern1, element));
				list.Add(string.Format(pattern2, element));
				list.Add(string.Format(pattern3, element));
				list.Add(string.Format(pattern5, element));
				
			}
			return list.OrderBy(i => i).ConcatenateLines();
			
		}
		public static string GenerateToString(string value)
		{
			
			var matches = Regex.Matches(value, "[\\w]+(?= \\{)").
				Cast<Match>().
				Select(i => i.Value);
			
			var pattern = "\"\\n{0} = \"+{0}+\n";
			var list = new List<string>();
			foreach (var element in matches) {
				list.Add(string.Format(pattern, element));
			}
			return list.ConcatenateLines();
			
		}
		public static string GenerateToTable(string value)
		{
			
			var matches = Regex.Matches(value, "[\\w]+(?= \\{)").
				Cast<Match>().
				Select(i => i.Value);
			
			var pattern = "<td>@m.{0}</td>";
			var list = new List<string>();
			list.Add("<tr>");
			
			foreach (var element in matches) {
				list.Add(string.Format(pattern, element));
			}
			list.Add("</tr>");
			
			return list.ConcatenateLines();
			
		}
		
	}
}
