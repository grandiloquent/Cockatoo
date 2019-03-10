namespace Cockatoo
{
	using Share;

	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Windows.Forms;
	using System.Net.Http;
	
	using System.IO;
	
	public partial   class MainForm: Form
	{
		private	string _accessToken = null;
		private int _hotKeyALTC = -1;
		private int _hotKeyF6 = -1;
		private int _hotKeyF7 = -1;
		private int _hotKeyF8 = -1;
		//string _url = "http://localhost:5000";
		string _url = "http://localhost:5000";
		
		public MainForm()
		{
			
			InitializeComponent();
			
		}
		void AppStripButtonClick(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("".GetExePath());
		}
		void Crc32ButtonClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(v => {
				return v.Crc32();
			});
		}
		void CToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(SQLHelper.GenerateForCSharp);
		}
		async	void DispatchCtrlF()
		{
			GoDelegate.GenerateHTMLArrayTemplate();
			
//			try {
//				var uri="/api/insert";
//				//var uri = "/api/import";
//				
//				//var f=@"C:\Web\.cache\CCTV\武林外传\create.json";
//				var f = @"C:\NetCore\.cache\CCTV\武林外传\create.json";
//				
//				var r = await HttpHelper.PostJson(f, _url + uri, _accessToken);
//				this.Text = r;
//			} catch (Exception ex) {
//				this.Text = ex.Message;
//			}
		}
		private void DispatchF6()
		{
			JavaScriptHelper.CombineCssFiles();
		}
		private void DispatchF7()
		{
			//JavaScriptDelegate.CompileTypeScript(@"C:\NetCore\wwwroot\typescripts");
			//WinFormHelper.OnClipboardDirectory(JavaScriptDelegate.CompileTypeScript);
			JavaScriptDelegate.CombineJavaScriptFiles(@"C:\NetCore\wwwroot\javascripts");
			
		}
		private void DispatchF8()
		{
			GoDelegate.GenerateHTMLArrayTemplate();
			//WinFormHelper.OnClipboardString(StringHelper.ReverseBlocks);
		}
		void EscapeHTMLButtonClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(HtmlHelper.MinifyHtml);
		}
		void GenerateRandomStringClick(object sender, EventArgs e)
		{
			Clipboard.SetText(8.GetUniqueKey());
		}
		void HTML数组文本ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(HtmlHelper.GenerateTemplateInBytesForJava);
		}
		void HTML数组文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(HtmlHelper.GenerateMinifyHtmlBytes);
		}
		void HTML文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(HtmlHelper.GenerateTemplate);
			
		}
		void JavaScriptToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(SQLHelper.GenerateForJavaScript);
		}
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			var handle = NativeMethods.HWND.Cast(this.Handle);
			if (_hotKeyF7 != -1) {
				WinFormHelper.UnregisterHotKey(handle, _hotKeyF7);
			}
			if (_hotKeyF6 != -1) {
				WinFormHelper.UnregisterHotKey(handle, _hotKeyF6);
			}
			if (_hotKeyF8 != -1) {
				WinFormHelper.UnregisterHotKey(handle, _hotKeyF8);
			}
			if (_hotKeyALTC != -1) {
				WinFormHelper.UnregisterHotKey(handle, _hotKeyALTC);
			}
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{ 
			
			Inject(typeof(AspnetDelegate));
			Inject(typeof(ZipDelegate));
			Inject(typeof(FileDelegate));
			Inject(typeof(HttpDelegate));
			Inject(typeof(SafariDelegate));
			Inject(typeof(JavaDelegate));
			Inject(typeof(GoDelegate));
			Inject(typeof(CSSDelegate));
			Inject(typeof(CSharpDelegate));
			messageLabel.Text = _url;
			_hotKeyF8 = 8;
			_hotKeyF6 = 6;
			_hotKeyF7 = 7;
			_hotKeyALTC = 11;
			
			var handle = NativeMethods.HWND.Cast(this.Handle);
			WinFormHelper.RegisterHotKey(handle, _hotKeyF8, 0, (int)Keys.F8);
			WinFormHelper.RegisterHotKey(handle, _hotKeyF6, 0, (int)Keys.F6);
			WinFormHelper.RegisterHotKey(handle, _hotKeyF7, 0, (int)Keys.F7);
			WinFormHelper.RegisterHotKey(handle, _hotKeyALTC, 0x0001, (int)Keys.C);
			
		}
		
		void Inject(Type type)
		{
			foreach (var method in type.GetMethods()) {
				var attributes = method.GetCustomAttributes(
					                 typeof(BindMenuItemAttribute), false);
				
				if (!attributes.Any())
					continue;
				
				
				var attribute = (BindMenuItemAttribute)attributes.First();
					
				var splitButton = (ToolStripSplitButton)((ToolStrip)this.Controls[attribute.Toolbar]).Items[attribute.SplitButton];
					
				if (attribute.AddSeparatorBefore) {
					splitButton.DropDownItems.Add(new ToolStripSeparator());
				}
				var item = new ToolStripMenuItem(attribute.Name);
			 
				item.Click += (a, b) => method.Invoke(null, null);
				splitButton.DropDownItems.Add(item);
				
			
			}
		}
		
		void NetSplitButtonButtonClick(object sender, EventArgs e)
		{
			HttpHelper.TouchServer();
		}
		void ReverseBlocksToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(StringHelper.ReverseBlocks);
		}
		 
		void StringBreakStripButtonClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(StringHelper.BreakString);
		}
		void StringRemoveEmptyLinesButtonClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(v => v.RemoveEmptyLines());
		}
		void StringToLineStripButtonClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(StringHelper.ToLine);
			
		}
		 
		void ToolStripMenuItem2Click(object sender, EventArgs e)
		{
			Clipboard.SetText(24.GetUniqueKey());
			
		}
		async	void ToolStripSplitButton1ButtonClick(object sender, EventArgs e)
		{
			var r =	await HttpHelper.PostXML("http://ykugc.cp31.ott.cibntv.net/6578274B7324A71DFE6844D46/03000C0A075B5250362C3281468DEFE062F059-2E90-4192-A946-8D86F8CD1255.mp4.ts?sid=055093337894930cb8dde_07_Ac6fd8129bf75e7a046c7029dad4dbda2&sign=9d16c6b87659ad5a76acf699bd5d2563&ctype=30&s=cc001f06962411de83b1&ts_start=117.94&ts_end=129.34&ts_seg_no=100&ts_keyframe=1",
			                                 
				        "http://pl.cp31.ott.cibntv.net/");
			this.Text = r;
		}
		void TouchServer(string uri)
		{
			
			this.Text = "";
			WinFormHelper.OnClipboardFile(async (f) => {
				try {
					var r = await HttpHelper.PostJson(f, _url + uri, _accessToken);
					this.Text = r;
				} catch (Exception ex) {
					this.Text = ex.Message;
				}
			});
			
		}
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x0312) {
				var k = ((int)m.LParam >> 16) & 0xFFFF;
				if (k == 67) {
					DispatchCtrlF();
				} else if (k == 0x75) {

					DispatchF6();
				} else if (k == 0x76) {

					DispatchF7();
				} else if (k == 0x77) {

					DispatchF8();
				}
				//                else if (k == 0x78)
				//                {
				//                    DispatchF9();
//
				//                }
				//                else if (k == 0x57)
				//                {
				//                    DispatchCtrlW();
				//                }
			}
			base.WndProc(ref m);
		}
		void 创建JSON文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			TouchServer("/api/update");
			
		}
		void 从Id生成JavaScript文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(HtmlHelper.GenerateJavaScript);
		}
		void 从属性生成Td文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(AspNetHelper.GenerateToTable);
			
		}
		void 从属性生成ToString文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(AspNetHelper.GenerateToString);
		}
		void 从属性生成表格文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(AspNetHelper.GenerateForm);
		}
		async	void 导出ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var message = await HttpHelper.Post(_url + "/api/export", _accessToken);
			this.Text = message;
		}
		 
		void 格式化目录ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardDirectory(CSharpHelper.FormatCodeInDirectory);
		}
		void 格式化文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(CSharpHelper.FormatCode);
		}
		void 格式化文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardFile(CSharpHelper.FormatCodeInFile);
		}
		 
		void 行首ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(v => StringHelper.ToggleHead(v, "- "));
			
		}
		void 排序Chrome书签文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			
			WinFormHelper.OnClipboardFile(f => {
				var hd = new HtmlAgilityPack.HtmlDocument();
				hd.LoadHtml(f.ReadAllText());
			                              	
				var nodes = hd.DocumentNode.SelectNodes("//dl/dt");
			                              	
			                              	
				nodes.OrderBy(i => i.SelectSingleNode(".//a").GetAttributeValue("href", ""));
			                              	
				"1.htm".WriteAllText(hd.DocumentNode.OuterHtml);
			                              	
			});
		}
		void 排序C目录ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardDirectory(CSharpHelper.FormatCodeInDirectory);
		}
		void 排序C文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardFile(CSharpHelper.FormatCodeInFile);
			
		}
		void 排序Function文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(JavaScriptHelper.SortFunctions);
		}
		void 排序Nginx文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardFile(f => {
			                              	
				f.WriteAllText(NginxHelper.FormatNginxConf(f.ReadAllText()));
			});
		}
		void 排序文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(CssHelper.ForamtSort);
		}
		 
		
		async	void 授权ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var message = await HttpHelper.Authenticate(_url + "/api/authenticate"
			                                            , "0dWOg5AD"
			                                            , "BsgJmcFn");
			var obj =	Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string,string>>(message);
			
			obj.TryGetValue("token", out _accessToken);
			this.Text = message;
		}
		 
		
		void 下载目录文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(v => {
				GitHubHelper.DownloadFolder(v);
				return "";
			});
		}
		void 下载文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(v => {
				SafariHelper.DownloadBook(v);
				return null;
			});
		}
		void 压缩C目录ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardDirectory(dir => dir.ZipCSharp());
		}
		void 压缩NodeJS目录ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardDirectory(v => {
			                                   	
				var r = v.ZipNode("data");
				Clipboard.SetText(r);
			});
		}
		
		void 压缩目录目录ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardDirectory(v => v.ZipDirectory());
		}
		void 压缩文件ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardFile(JavaScriptHelper.MinifyJavaScriptInFile);
			
		}
		void 压缩文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardFile(f => ImageProcessorHelper.ResizeCropAsJpeg(f, 265, 199));
		}
		void 压缩子目录目录ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardDirectory(v => v.ZipDirectories());
		}
		void 压缩子目录目录ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardDirectory(dir => dir.ZipAndroidProjects());
		}
		
		
		 
	
		void MDNToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(DocumentHelper.MDN);
		}
		void 压缩文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(JavaScriptHelper.MinifyJavaScript);
	
		}
		void MSDNToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(DocumentHelper.MSDN);
	
		}
		
	
		 
	
	
		void 裁剪169文件ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardFile(f => {
			                              
				using (var img = Bitmap.FromFile(f)) {
					BitmapHelper.SaveJpegWithCompression(BitmapHelper.ResizeImageKeepAspectRatio(img),
						f.ChangeFileName(fileName => {
			                              		                                                      
							return fileName + "16x9";
						}), 100
					);
				}
			});
		}
	
		void AndroidDeveloper文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(DocumentHelper.AndroidDevelopers);
		}
		void JSON文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardString(v => JavaScriptHelper.MinifyJavaScript(v).EscapeJSON());
	
		}
		void 重启服务器ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
		}
		void 下载文件文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
	
		}
		async	void 测试ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var nvc = new List<KeyValuePair<string, string>>();
			nvc.Add(new KeyValuePair<string, string>("titurl", "http://v.youku.com/v_show/id_XMTQwNzMzMTc1Mg==.html"));
			var client = new HttpClient();
			var req = new HttpRequestMessage(HttpMethod.Post, "https://cuijiahua.com/video/data/title.php") { Content = new FormUrlEncodedContent(nvc) };
			var res = await client.SendAsync(req);
			var xx = await res.Content.ReadAsStringAsync();
			var xy = xx;
		
		}
	 
		void 生成图标文本ToolStripMenuItemClick(object sender, EventArgs e)
		{
			WinFormHelper.OnClipboardText(v => IconHelper.GenereteIcon(v.Trim()[0].ToString()));
		}
	
	}
}