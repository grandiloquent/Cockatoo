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
	
	public static class ZipDelegate
	{
		[BindMenuItem(Name = "压缩 Net Core 项目 (目录)", Toolbar = "otherStrip", SplitButton = "zipStripSplitButton", AddSeparatorBefore = true)]
		
		public static void ZipNetCoreProject(){
			WinFormHelper.OnClipboardDirectory(v => v.ZipNetCore());
		}
	}
}