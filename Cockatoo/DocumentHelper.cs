using HtmlAgilityPack;
using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Share;

namespace Cockatoo
{
	public class DocumentHelper
	{
		
		public static string MDN(string str)
		{
			var patttern = @"
element.addEventListener('{0}', function () {{
                console.log('{0}',arguments);
            }});
";
			var hd = new HtmlDocument();
			hd.LoadHtml(str);
			var list1 = new List<string>();
			var list2 = new List<string>();
			
			var sb = new StringBuilder();
			var tables = hd.DocumentNode.SelectNodes("//table[@class='standard-table']");
			tables.ForEach(table => {
				var trNodes = table.SelectNodes(".//tbody/tr");
				trNodes.ForEach(tr => {
					var tdList = tr.SelectNodes(".//td");
					//list.Add(tdList.First().InnerText);
				                	
					list1.Add("\"" + tdList.First().InnerText + "\"");
					list2.Add(string.Format(patttern, tdList.First().InnerText));
					sb.AppendLine("- `" + tdList.First().InnerText + "`");
					sb.AppendLine();
					sb.AppendLine("\t" + tdList.Last().InnerText)
				                		.AppendLine();
				});
			               
			});
			return "\n\n\n```\n\n\n" + string.Join(",\n", list1) + "\n\n\n" + string.Join("\n", list2) + "\n\n\n```\n\n\n" +	sb.ToString();
			
			
		}
		public static string AndroidDevelopers(string str)
		{
			var hd = new HtmlDocument();
			hd.LoadHtml(str);
			var list1 = new List<string>();
			
			 
			var tables = hd.DocumentNode.SelectNodes("//table[contains(@class,'responsive')]").ToArray();
			tables.ForEach(table => {
				var trNodes = table.SelectNodes(".//tbody/tr/td[last()]");
				trNodes.ForEach(td => {
					var children = td.ChildNodes.Where(i => i.NodeType == HtmlNodeType.Element).ToArray();
					 
					if (children != null) {
						var value = string.Format("- `{0}`\r\n\r\n\t{1}\r\n\r\n",
							                          children[0].InnerText.Trim(),
							                          children.Length>1? children[1].InnerText.Trim().Flate():"");
						list1.Add(value);
					}
				});
			               
			});
			 
			
			return  string.Join(Environment.NewLine, list1.OrderBy(i=>i.SubstringBefore('('))
			                    .DistinctBy(i=>i.SubstringBefore('(')));
			
		}
		public static string MSDN(string str)
		{
			var hd = new HtmlDocument();
			hd.LoadHtml(str);
			var list1 = new List<string>();
			var list2 = new List<string>();
			 
			var tables = hd.DocumentNode.SelectNodes("//table[@class='nameValue']").ToArray();
			tables.ForEach(table => {
				var trNodes = table.SelectNodes(".//tbody/tr");
				trNodes.ForEach(tr => {
					var tdList = tr.SelectNodes(".//td");
					//list.Add(tdList.First().InnerText);
					var name = tdList.First().SelectSingleNode(".//*[contains(@class,'lang-csharp')]").InnerText;
					name = HtmlEntity.DeEntitize(name);
					list1.Add("\"" + name + "\"");
					list2.Add("- `" + name + "`\n\n" + "\t" + tdList.Last().InnerText.Trim() + "\n");
				                		
				});
			               
			});
			 
			return "\n\n\n```\n\n\n" + string.Join(",\n", list1.OrderBy(i => i.SubstringBefore("<")
			                                                            .SubstringBefore('('))
			                                                            .DistinctBy(i => i.SubstringBefore("<")
			                                                                        .SubstringBefore('('))) + "\n\n\n```\n\n\n" + "\n\n\n" + string.Join("\n", list2.OrderBy(i => i.SubstringBefore("<")
			                                                            .SubstringBefore('('))
			                                                            .DistinctBy(i => i.SubstringBefore("<")
			                                                                        .SubstringBefore('(')));
			
			
		}
	}
}