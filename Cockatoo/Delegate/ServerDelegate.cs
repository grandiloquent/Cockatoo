namespace  Cockatoo
{
	using Share;
	using System.Net.Http;
	using System.Collections.Generic;
	using System;
	using System.Windows.Forms;
	using System.Linq;
	
	public class ServerDelegate
	{
		private	string _accessToken = null;
		private	readonly MainForm _mainForm;
		private HttpClient _client;
		private string _baseUrl = "http://localhost:5000";

		private void AddItem(string name, ToolStripSplitButton button, Action action)
		{
			var fieldItem = new ToolStripMenuItem(name);
			button.DropDownItems.Add(fieldItem);
			fieldItem.Click += (a, b) => action();
		}
		
		public ServerDelegate(MainForm mainForm)
		{
			_mainForm = mainForm;
			_mainForm.Text = _baseUrl;
			
		}
		
		private void Check()
		{
			if (_client == null) {
				_client = new HttpClient();
			}
		}
		private	void TouchServer(string uri)
		{
			
			_mainForm.messageLabel.Text = "服务器";
			WinFormHelper.OnClipboardFile(async (f) => {
				try {
					var r = await HttpHelper.PostJson(f, _baseUrl + uri, _accessToken);
					_mainForm.messageLabel.Text = r;
				} catch (Exception ex) {
					_mainForm.messageLabel.Text = ex.Message;
				}
			});
			
		}
		[BindMenuItem(Name = "授权", Toolbar = "otherStrip", SplitButton = "serverButton")]
		public async void Authenticate()
		{
		
			try {
				var message = await HttpHelper.Authenticate(_baseUrl + "/api/authenticate"
			                                            , "uhOYyqgq"
			                                            , "GteMO7VK");
			var obj =	Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,string>>(message);
			
			obj.TryGetValue("token", out _accessToken);
			_mainForm.messageLabel.Text=_accessToken.Substring(0,10);
			} catch (Exception ex) {
				

				//var v=ex.ToString();
				_mainForm.messageLabel.Text=ex.Message;
			}
		}
		[BindMenuItem(Name = "创建视频 (文件)", Toolbar = "otherStrip", SplitButton = "serverButton", AddSeparatorBefore = true)]
		public void CreateVideo()
		{
			TouchServer("/api/insert");
		}
		[BindMenuItem(Name = "修改视频 (文件)", Toolbar = "otherStrip", SplitButton = "serverButton")]
		public void UpdateVideo()
		{
			TouchServer("/api/update");
		}
		
		[BindMenuItem(Name = "批量插入视频 (文件)", Toolbar = "otherStrip", SplitButton = "serverButton")]
	
		public void ImportVideos()
		{
			TouchServer("/api/import");
		}
		
		[BindMenuItem(Name = "删除视频 (文本)", Toolbar = "otherStrip", SplitButton = "serverButton")]
		public void DeleteVideo()
		{
			WinFormHelper.OnClipboardString((str) => {
				if (!str.IsDigitString())
					return null;
				_mainForm.messageLabel.Text = "删除 " + str;
				Check();
				
				var response = _client.PostAsync(_baseUrl + "/api/delete?videoid=" + str, null).GetAwaiter().GetResult();
				var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
				_mainForm.messageLabel.Text = message;
				return null;
			});
		}
		
		[BindMenuItem(Name = "导出", Toolbar = "otherStrip", SplitButton = "serverButton")]
		public async void Export()
		{
			var client = new HttpClient();
			
			if (_accessToken != null)
				client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
		
			var res = await client.GetAsync(_baseUrl + "/api/export");
			var str = await res.Content.ReadAsStringAsync();
			_mainForm.messageLabel.Text = str;
		}
		
		[BindMenuItem(Name = "切换服务器", Toolbar = "otherStrip", SplitButton = "serverButton", AddSeparatorBefore = true)]
		public void SwitchServer()
		{
			if (_baseUrl != "http://localhost:5000")
				_baseUrl = "http://localhost:5000";
			else {
				_baseUrl = "https://149.129.88.215";
			}
			_mainForm.Text = _baseUrl;
		
		}
		 
	}
}