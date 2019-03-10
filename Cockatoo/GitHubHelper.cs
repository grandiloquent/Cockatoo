using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Share;
using HtmlAgilityPack;
using System.Diagnostics;
namespace  Cockatoo
{

	public static class GitHubHelper
	{
		public static void DownloadFolder(string url)
		{
			
			var bin = @"node\node_modules\.bin\fetcher.cmd".GetExePath();
			var config = @"node\config.json".GetExePath();
			Process.Start(new ProcessStartInfo {
			              	WorkingDirectory="c:/",
				FileName = bin,
				Arguments = string.Format("--file=\"{0}\" --url=\"{1}\"", config, url)
			});
		}
	}
}
