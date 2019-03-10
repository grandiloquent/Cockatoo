namespace  Cockatoo
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using Renci.SshNet;
	using Share;
	using System.IO;
	using System.Threading;
	using Renci.SshNet.Sftp;
	using HtmlAgilityPack;
	
	public static class SafariDelegate
	{
		 private const string BaseTocFileName = "目录.html";

		private static void GenerateTocFromEpubFile(string dir)
        {
            var file = Directory.GetFiles(dir, "*.ncx")[0];
            var hd = new HtmlDocument();
            hd.LoadHtml(file.ReadAllText());
            var nodes = hd.DocumentNode.SelectNodes("//navpoint");
            var list = new List<string>();

            list.Add("<ol>");
            foreach (var item in nodes)
            {
 
            	
                list.Add(string.Format("<li><a href=\"{0}\">{1}</a></li>",
            	                       item.SelectSingleNode(".//content").GetAttributeValue("src", ""),
            	                       item.SelectSingleNode(".//text").InnerText));
            }
            list.Add("</ol>");

            Path.Combine(dir, BaseTocFileName).WriteAllLines(list);

        }
		
			[BindMenuItem(Name = "整理 EPUB 文件 (目录)", Toolbar = "otherStrip", SplitButton = "safariSplitButton", AddSeparatorBefore = true)]
		
		public static void OrganizeEPUB()
		{
			WinFormHelper.OnClipboardDirectory(Share.EpubHelper.PrettyName);
		}
		[BindMenuItem(Name = "移除 HTML 文件中的冗余标记 (目录)", Toolbar = "otherStrip", SplitButton = "safariSplitButton", AddSeparatorBefore = true)]
		
		public static void RemoveRedundancyTags()
		{
			WinFormHelper.OnClipboardDirectory(SafariHelper.PrettyFormat);
		}
		[BindMenuItem(Name = "从 EPUB 解压文件生成 TOC (目录)", Toolbar = "otherStrip", SplitButton = "safariSplitButton", AddSeparatorBefore = true)]
		
		public static void ZipNetCoreProject()
		{
			WinFormHelper.OnClipboardDirectory(GenerateTocFromEpubFile);
		}
	}
}