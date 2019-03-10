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

namespace  Cockatoo
{
	// https://github.com/sshnet/SSH.NET/blob/7691cb0b55f5e0de8dc2ad48dd824419471ab710/src/Renci.SshNet.Tests/Classes/SshCommandTest.cs
	
	
	public static class SSHHelper
	{

		public static string UpdateNodeProject(string uploadedFileName)
		{
			const string host = "149.129.88.215";
			const string username = "root";
			const string password = "Td5STIBy";
			var client = new SshClient(host, username, password);
			client.Connect();
		 
			client.RunCommand("pm2 stop /root/www/bin/www");
			
			var target = new SftpClient(host, username, password);
			target.Connect();
			
			SSHHelper.DeleteNodeProject(target, client, "/root/www/");
			using (var file = File.OpenRead(uploadedFileName)) {
				target.UploadFile(file, "/root/www/web.zip");
			}
			client.RunCommand("unzip /root/www/web.zip -d /root/www");
			client.RunCommand("export NODE_ENV=production");
			var cmd =	client.RunCommand("pm2 start /root/www/bin/www -i 0");
			var r = cmd.Result;
			target.Dispose();
			client.Dispose();
			return Regex.Replace(r, "[\r\n]+", " ").Trim();
		}
		public static void UploadProjectFile(string uploadedFileName)
		{
			const string host = "149.129.88.215";
			const string username = "root";
			const string password = "Td5STIBy";
			
			var target = new SftpClient(host, username, password);
			target.Connect();
			
			var parent="C:\\NetCore";
			
			
			using (var file = File.OpenRead(uploadedFileName)) {
			
				if(!uploadedFileName.StartsWith(parent))return;
				var path="/root/public"+uploadedFileName.Replace('\\','/').Substring(parent.Length);
				target.UploadFile(file, path);
			}
			target.Dispose();
		}
			
		public static void UploadFile(string uploadedFileName, string path)
		{
			const string host = "149.129.88.215";
			const string username = "root";
			const string password = "Td5STIBy";
			
			var target = new SftpClient(host, username, password);
			target.Connect();
			
			using (var file = File.OpenRead(uploadedFileName)) {
				var pieces = path.Split('/').Skip(1).Take(path.Split('/').Length - 2);
				var dst = "";
				foreach (var element in pieces) {
					dst += "/" + element;
					if (!target.Exists(dst))
						target.CreateDirectory(dst);
				}
				target.UploadFile(file, path);
			}
			target.Dispose();
		}
		 
		public static void ReloadNginx(string uploadedFileName)
		{
			const string host = "149.129.88.215";
			const string username = "root";
			const string password = "Td5STIBy";
			
			var target = new SftpClient(host, username, password);
			target.Connect();
			
			using (var file = File.OpenRead(uploadedFileName)) {
				target.UploadFile(file, "/usr/local/webserver/nginx/conf/nginx.conf");
				// /usr/local/webserver/nginx/conf/nginx.conf
			}
			target.Dispose();
			using (var client = new SshClient(host, username, password)) {
				client.Connect();
				client.RunCommand("/usr/local/nginx/sbin/nginx -s reload");
			}
		}
		
		public static void DeleteNetCore()
		{
			const string host = "149.129.88.215";
			const string username = "root";
			const string password = "Td5STIBy";
			
				var target = new SftpClient(host, username, password);
			
				target.Connect();
				
				
			IEnumerable<SftpFile> files = null;
			IAsyncResult asyncResult = target.BeginListDirectory("/root/public", delegate(IAsyncResult ar) {
				
				files =	target.EndListDirectory(ar);
				
			}, null);
				
			while (!asyncResult.IsCompleted) {
				
				Thread.Sleep(500);
			}
			target.Dispose();
		
            	
				var client = new SshClient(host, username, password);
				client.Connect();
				
			foreach (var element in files) {
						
				if (element.Name != "appsettings.json" ) {
					try {
//						if (element.IsDirectory) {
//							target.DeleteDirectory(element.FullName);
//						} else {
//							element.Delete();
//						}
						client.RunCommand("sudo rm -rf " + element.FullName);
						
						
					} catch (Exception e) {
						Console.WriteLine(e.Message);
					}
				}
			}
					client.Dispose();
				
		}
		private static void DeleteNodeProject(SftpClient target, SshClient client, string path)
		{
			
			IEnumerable<SftpFile> files = null;
			IAsyncResult asyncResult = target.BeginListDirectory(path, delegate(IAsyncResult ar) {
				
				files =	target.EndListDirectory(ar);
				
			}, null);
				
			while (!asyncResult.IsCompleted) {
				
				Thread.Sleep(500);
			}
            	
			foreach (var element in files) {
						
				if (element.Name != "node_modules" && element.Name != "data") {
					try {
//						if (element.IsDirectory) {
//							target.DeleteDirectory(element.FullName);
//						} else {
//							element.Delete();
//						}
						client.RunCommand("rm -rf " + element.FullName);
						
						
					} catch (Exception e) {
						Console.WriteLine(e.Message);
					}
				}
			}
		}
		private static string ListDirectory(string host, string username, string password)
		{
			
			return ExecuteCommand("cd /root/www && ls");
		}
		
		public static string ExecuteCommand(string commandText)
		{ 
			const string host = "149.129.88.215";
			const string username = "root";
			const string password = "Td5STIBy";

			using (var client = new SshClient(host, username, password)) {
				client.Connect();

				var command = client.RunCommand(commandText);
				var result = command.Result;
				//result = result.Substring(0, result.Length - 1);    //  Remove \n character returned by command

				client.Disconnect();
                
				return result;

			}
		}

	}
}
