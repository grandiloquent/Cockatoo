using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Share;
using HtmlAgilityPack;

namespace  Cockatoo
{
	
	public static class HtmlHelper
	{
		public static string GenerateJavaScript(string value)
		{
			var hd = new HtmlDocument();
			hd.LoadHtml(value);
			
			var ids = hd.DocumentNode.SelectNodes("//@id");
			var list = new List<string>();
			if (ids.Any()) {
				foreach (var element in ids) {
					list.Add(string.Format("var {1}{0} = document.getElementById('{1}');", element.Name.Capitalize(), element.GetAttributeValue("id", "")));
				}
			}
			return list.ConcatenateLines();
		}
		public static String GenerateTemplateInBytesForJava(string v)
		{
			v = MinifyHtml(v);
			var sb = new StringBuilder();
			var sb2 = new StringBuilder();
			sb.AppendLine("private static final byte[][] buffer = new byte[][]{");

			var array = Regex.Split(v, "{{[^}]*?}}");
			var matches = Regex.Matches(v, "{{([^}]*?)}}").Cast<Match>().Select(i => i.Groups[1].Value).ToArray();
			var index = 0;
			foreach (var item in array) {
				var bytes = new UTF8Encoding(false).GetBytes(item).Select(i => {
					if (i > 127) {
						return "(byte)" + i;
					} else {
						return i.ToString();
					}
				}).ToArray();

				sb.AppendFormat("/* {0} {1} */ new byte[]{{{2}}},\n", index + " " + bytes.Length, index < matches.Length ? matches[index] : "", string.Join(",", bytes));
				sb2.AppendFormat("// /*{1}*/os.write(bytes[{0}]);\n", index, index < matches.Length ? matches[index] : "");
				// sb.AppendFormat("/* {0} {1} */new byte[]{{ {2} }},\r\n", index, index < matches.Length ? matches[index] : "", StringHelper.ToUTF8Array(item));
				index++;

			}
			sb.AppendLine("};");
			
			sb.AppendLine(sb2.ToString());
			return sb.ToString();
		}
		public static string MinifyHtml(string value)
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
		public static String GenerateTemplate(string v)
		{
			v = MinifyHtml(v);
			var sb = new StringBuilder();


			var array = Regex.Split(v, "{{[^}]*?}}");
			var matches = Regex.Matches(v, "{{([^}]*?)}}").Cast<Match>().Select(i => i.Groups[1].Value).ToArray();
			var index = 0;
			foreach (var item in array) {

				sb.Append("\"" + item.Trim().Replace("\"", "\\\"") + "\"").Append('+').AppendLine().Append(index < matches.Length ? matches[index] : "").Append('+');
				index++;

			}

			return Regex.Replace( sb.ToString(),"[\\s\\+]+$","")+";";
		}
		public static string GenerateMinifyHtmlBytes(string value)
		{
			var str = StringHelper.ToUTF8Array(HtmlHelper.MinifyHtml(value));

			return str + " // " + (str.Count(i => i == ',') + 1);
		}
	}
}
