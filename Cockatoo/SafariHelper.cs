
using System;
using System.IO;
using System.Text.RegularExpressions;
using Share;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Cockatoo
{
	
	public static class SafariHelper
	{
		private const string BasePath = "https://www.oreilly.com/";
		private const string BaseDirectory = "Books";
		private const string BaseLinkFileName = "links.txt";
		private const string BaseTocFileName = "目录.html";

		public async static void DownloadBook(string value)
		{
			var dir = BaseDirectory.GetDesktopPath();
			dir.CreateDirectoryIfNotExists();


			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

			var client = new HttpClient();
			var result = await client.GetStringAsync(value);
			var hd = new HtmlDocument();
			hd.LoadHtml(result);

			var title = hd.DocumentNode.SelectSingleNode("//title").InnerText.SubstringBeforeLast('[').Trim().GetValidFileName();
			var targetDirectory = Path.Combine(dir, HtmlEntity.DeEntitize(title));
			targetDirectory.CreateDirectoryIfNotExists();
			var targetLinkFile = Path.Combine(targetDirectory, BaseLinkFileName);
			var targetTocFile = Path.Combine(targetDirectory, BaseTocFileName);
			var toc = hd.DocumentNode.SelectSingleNode("//*[@class='detail-toc']");
			var linkNodes = toc.SelectNodes(".//a");
			var links = new List<string>();

			foreach (var item in linkNodes) {
				var href = item.GetAttributeValue("href", "");
				var link = href.SubstringAfter("com/").SubstringBeforeLast('#');
				var l = BasePath + link;
				if (!links.Contains(l))
					links.Add(l);

				var src = Path.GetFileName(href);

				item.SetAttributeValue("href", src.ChangeExtension(".html"));
			}

			var str = toc.InnerHtml;

			targetLinkFile.WriteAllLines(links);
			targetTocFile.WriteAllText(str);

			Process.Start(new ProcessStartInfo {
				FileName = "aria2c",
				Arguments = "--input-file=\"links.txt\" --load-cookies=\"C:\\Users\\psycho\\Desktop\\Books\\\\cookie.txt\"",
				WorkingDirectory = targetDirectory
			});
		}

		public static void PrettyFormat(string dir)
		{
			var diretories = Directory.GetDirectories(dir);
			foreach (var r in diretories) {
				const string str = "<div><div><img src=\"./images/\"><div><div><div><button><svg><g><g><g><rect></rect><title>Playlists</title><path></path><circle></circle><circle></circle><rect></rect><rect></rect><rect></rect></g></g></g></svg><div>Add&nbsp;To</div></button></div></div></div></div></div>";
				const string str1 = "<div><div><img><div><div><div><button><svg><g><g><g><rect></rect><title>Playlists</title><path></path><circle></circle><circle></circle><rect></rect><rect></rect><rect></rect></g></g></g></svg><div>Add&nbsp;To</div></button></div></div></div></div></div>";


				foreach (var element in Directory.GetFiles(r, "*.html", SearchOption.TopDirectoryOnly)) {
					var sv = Regex.Replace(element.ReadAllText().Replace(str, "").Replace(str1, ""), "(style|width|height)=\"[^\"]*?\"", "");


					element.WriteAllText(sv);
				}
			}

			var files = Directory.GetFiles(dir, "*.html", SearchOption.AllDirectories);
			foreach (var element in files) {
				// <a href="06_Chapter01.xhtml#c

				var valuue = Regex.Replace(element.ReadAllText(), "(?<=\\<a href\\=\")[\\w\\d\\-\\.]+", new MatchEvaluator((m) => {
					if (m.Value == "https" || m.Value == "http")
						return m.Value;
					return m.Value.SubstringBeforeLast(".") + ".html";
				}));
				element.WriteAllText(valuue);
			}
			files = Directory.GetFiles(dir, "*.ncx", SearchOption.AllDirectories);
			foreach (var element in files) {
				// <a href="06_Chapter01.xhtml#c

				var value = Regex.Replace(element.ReadAllText(), "(?<=\\<content src\\=\")[\\:\\w\\d\\-\\./#]+\"", new MatchEvaluator((m) => {

					return m.Value.SubstringAfterLast("/");
				}));
				element.WriteAllText(value);
			}

		}
	}
}
