namespace  Cockatoo
{
	using Share;
	using System.Net.Http;
	using System.Collections.Generic;
	using System;
	using System.Windows.Forms;
	using System.Linq;
	using System.IO;
	
	public static class FileDelegate
	{
		
		[BindMenuItem(Name = "删除 (目录+文件)", Toolbar = "otherStrip", SplitButton = "fileButton", AddSeparatorBefore = true)]
		public static void DeleteFileList()
		{
			WinFormHelper.OnClipboardFileDropList(array => {
				foreach (var dir in array) {
					if (Directory.Exists(dir)) {
					Directory.Delete(dir, true);
				} else if (File.Exists(dir)) {
					File.Delete(dir);
				}
				}
			});
		
		}
		[BindMenuItem(Name = "按扩展名整理文件 (目录)", Toolbar = "otherStrip", SplitButton = "fileButton", AddSeparatorBefore = true)]
		public static void FormatImportJson()
		{
			WinFormHelper.OnClipboardDirectory(dir => {
				var files = Directory.GetFiles(dir);
				var targetDirectory = Path.Combine(dir, ".cache");
				targetDirectory.CreateDirectoryIfNotExists();
				
				foreach (var element in files) {
					var extension = Path.GetExtension(element);
					var dstDir = Path.Combine(targetDirectory, extension);
					dstDir.CreateDirectoryIfNotExists();
					
					var dstFile = Path.Combine(dstDir, Path.GetFileName(element));
					if (!File.Exists(dstFile)) {
						try{
							if(element.EndsWith("bootmgr"))continue;
								File.Move(element, dstFile);
						}catch{
						
						}
					}
				}
			});
		
		}
	}
}