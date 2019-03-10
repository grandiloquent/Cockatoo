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
	
	public static class SSHDelegate
	{
		const string host = "149.129.88.215";
		const string username = "root";
		const string password = "Td5STIBy";
		public static string ExecuteCommand(string commandText)
		{ 
			
			using (var client = new SshClient(host, username, password)) {
				client.Connect();

				var command = client.RunCommand(commandText);
				var result = command.Result;
				client.Disconnect();
				return result;

			}
		}
		public static void ExecuteCommands(string[] commandTexts)
		{ 
			
			using (var client = new SshClient(host, username, password)) {
				client.Connect();

				commandTexts.ForEach(i => client.RunCommand(i));
				
				client.Disconnect();

			}
		}
		[BindMenuItem(Name = "上传源代码 (文件)", Toolbar = "otherStrip", SplitButton = "sshButton", AddSeparatorBefore = true)]
		
		public static void UploadProjectFile()
		{
			DeleteNetCore();
			
			WinFormHelper.OnClipboardFile(fileName => {
			                              
				var target = new SftpClient(host, username, password);
				target.Connect();
			
				
				using (var file = File.OpenRead(fileName)) {
			
				
					var path = "/root/public/NetCore.zip";
					target.UploadFile(file, path);
				}
				target.Dispose();
				
				using (var client = new SshClient(host, username, password)) {
					client.Connect();

					client.RunCommand("sudo unzip /root/public/NetCore.zip -d /root/public");
					client.RunCommand("cd /root/public && dotnet publish -c release && sudo systemctl start kestrel.service");
					client.Disconnect();

				}
			});
			
		
		}
		
		[BindMenuItem(Name = "下载文件 (文本)", Toolbar = "otherStrip", SplitButton = "sshButton", AddSeparatorBefore = true)]
		public static void DownloadFile()
		{
			WinFormHelper.OnClipboardString(filePath => {
				var target = new SftpClient(host, username, password);
				target.Connect();
				var dir = ".cache".GetDesktopPath();
				dir.CreateDirectoryIfNotExists();
				using (var file = new FileStream(Path.Combine(dir, Path.GetFileName(filePath)), FileMode.OpenOrCreate)) {
					target.DownloadFile(filePath, file);
				}
				target.Dispose();
				return null;
			                                
			});
		
			
		}
		[BindMenuItem(Name = "上传资源文件 (文件)", Toolbar = "otherStrip", SplitButton = "sshButton")]
		
		public static void UploadStaticFile()
		{
			WinFormHelper.OnClipboardFile(fileName => {
			                              
				var target = new SftpClient(host, username, password);
				target.Connect();
			
				const string parent = "C:\\NetCore";
			
				if (!fileName.StartsWith(parent, StringComparison.Ordinal))
					return;
				using (var file = File.OpenRead(fileName)) {
			
				
					var path = "/root/public/bin/release/netcoreapp2.2/publish" + fileName.Replace('\\', '/').Substring(parent.Length);
					target.UploadFile(file, path);
				}
				target.Dispose();
			});
			
		
		}
		[BindMenuItem(Name = "执行命令 (文本)", Toolbar = "otherStrip", SplitButton = "sshButton", AddSeparatorBefore = true)]
		public static void ExecuteCommand()
		{
			WinFormHelper.OnClipboardString(v => {
				ExecuteCommand(v);
				return null;
			});
			
		}
		[BindMenuItem(Name = "更新 Nginx 配置文件 (文件)", Toolbar = "otherStrip", SplitButton = "sshButton", AddSeparatorBefore = true)]
		public static void ReloadNginx()
		{
			
			WinFormHelper.OnClipboardFile(fileName => {
				var target = new SftpClient(host, username, password);
				target.Connect();
			
				using (var file = File.OpenRead(fileName)) {
					target.UploadFile(file, "/usr/local/webserver/nginx/conf/nginx.conf");
					// /usr/local/webserver/nginx/conf/nginx.conf
				}
				target.Dispose();
				using (var client = new SshClient(host, username, password)) {
					client.Connect();
					client.RunCommand("/usr/local/nginx/sbin/nginx -s reload");
				}
			});
		}
		
			
		[BindMenuItem(Name = "启动 Net Core 项目", Toolbar = "otherStrip", SplitButton = "sshButton", AddSeparatorBefore = true)]
		public static void StartNetCore()
		{
			ExecuteCommand("sudo systemctl start kestrel.service");
			
		}
		[BindMenuItem(Name = "停止 Net Core 项目", Toolbar = "otherStrip", SplitButton = "sshButton")]
		public static void StopNetCore()
		{
			var commands = new string[] {"kill $(ps aux | grep 'NetCore.dll' | awk '{print $2}')",
				"sudo systemctl stop kestrel.service"
			};
			ExecuteCommands(commands);
		}
		[BindMenuItem(Name = "重启 Net Core 项目", Toolbar = "otherStrip", SplitButton = "sshButton")]
		public static void RestartNetCore()
		{
			ExecuteCommand("sudo systemctl restart kestrel.service");
			
		}
		[BindMenuItem(Name = "清空 Net Core 项目", Toolbar = "otherStrip", SplitButton = "sshButton")]
		public static void DeleteNetCore()
		{
				
			
			
			var target = new SftpClient(host, username, password);
			
			target.Connect();
				
				
			IEnumerable<SftpFile> files = null;
			IAsyncResult asyncResult = target.BeginListDirectory("/root/public", (IAsyncResult ar) => files = target.EndListDirectory(ar), null);
				
			while (!asyncResult.IsCompleted) {
				
				Thread.Sleep(500);
			}
			target.Dispose();
		
            	
			var client = new SshClient(host, username, password);
			client.Connect();
				
			foreach (var element in files) {
						
				if (element.Name != "appsettings.json" && element.Name != "Migrations") {
					try {
						client.RunCommand("sudo rm -rf " + element.FullName);
						
						
					} catch (Exception e) {
						Console.WriteLine(e.Message);
					}
				}
			}
			client.Dispose();
		
		}
		
	}
}