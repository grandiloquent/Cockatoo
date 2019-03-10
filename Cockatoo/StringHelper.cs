using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Share;

namespace  Cockatoo
{
	
	public static class StringHelper
	{
		public static string ToUTF8Array(string value)
		{
			return string.Join(",", new UTF8Encoding(false).GetBytes(value).Select(i => i.ToString()));
		}
		
	
		public static string ToLine(string value)
		{
			var line = Regex.Replace(value, "\\s{2,}", " ");
			
			return Regex.Replace(line, "[\r\n]+", "");
		}
		public static string ReverseBlocks(string value)
		{
			
			return string.Join(Environment.NewLine, value.ToBlocks().Reverse());
		}
		public static string ToggleHead(string value,string v)
		{
			return	string.Join("\r\n", value.Split('\n').Select(i => {
			                                          
				if (i.StartsWith(v)) {
					return i.SubstringAfter(v);
				}
				return v + i.TrimEnd();
			}));
		}
		public static string BreakString(string value)
		{
			int limit = 80;
			var str = Regex.Replace(value, "[\r\n]+", " ");
			str = Regex.Replace(value, "\\s{2,}", " ");
			var pieces = str.Split(' ');
			var sb = new StringBuilder(value.Length);
			var s = string.Empty;
			foreach (var element in pieces) {
				if (sb.Length > limit) {
					s += sb + Environment.NewLine;
					sb.Clear();
				}
				sb.Append(element).Append(' ');
			}
			return s + sb;
		}
	}
}
