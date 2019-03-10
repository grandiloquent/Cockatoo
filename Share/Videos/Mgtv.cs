namespace Share
{
	using System.Text;
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Net.Http;
	using System.Text.RegularExpressions;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using System.Threading.Tasks;
	
	public static class Mgtv
	{
	

	
		public async static Task<string> FetchVideoAddress(string url)
		{
			string vid = null;
			var match = Regex.Match(url, "https?://www.mgtv.com/b/\\d+/(\\d+).html");
			if (match.Success) {
				vid	= match.Groups[1].Value;
			}
		
			if (vid == null)
				return null;
			var did = Guid.NewGuid().ToString();
			var tk2 = MakeTk2(did);
			
			var client = HttpHelper.GetHttpClient();
			var fisrtArray = await HttpHelper.ReadStringAndCookie(client, MakeUrl(tk2, vid), url);

			string pm2;
			
		
			var obj =	(JObject)JsonConvert.DeserializeObject(fisrtArray[1]);
			
			pm2 = obj.GetStringFromChain(new []{ "data", "atc", "pm2" });
			if (pm2 == null)
				return null;
			
			var next = string.Format("https://pcweb.api.mgtv.com/player/getSource?pm2={0}&tk2={1}&video_id={2}&type=pch5",
				           pm2, tk2, vid);
			
			var second = await HttpHelper.ReadStringWithCookie(client, next, url, fisrtArray[0]);
		

			obj = second.ToObject<JObject>();
			
			var domain = obj.GetArrayFromChain<string>(new string[] {
				"data",
				"stream_domain"
			}, null).First();
			var streams = obj.GetArrayFromChain<JObject>(new string[]{ "data", "stream" }, null);
			
			foreach (var element in streams) {
				var type = element.GetString("name");
			
				if (type == "高清") {
					var m3u8 =	string.Format("{0}{1}&did={2}", domain, element.GetString("url"), did);
					var videoObj=await HttpHelper.ReadStringWithCookie(client,m3u8,url,fisrtArray[0]);
					
					return videoObj.ToObject<JObject>().GetString("info");
				}
			}
			 
			return null;
		
		}
		
		private static string  MakeTk2(string did)
		{
			var s = new char[]{ '+', '/', '=' };
			var t = new char[]{ '_', '~', '-' };
			 
			var timestamp = DateTimeHelper.ToUnixTimeSeconds();
			var tk2 = string.Format("did={0}|pno=1030|ver=0.3.0301|clit={1}", did, timestamp);
			string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(tk2));
			base64 =	base64.ReverseString();
			
			for (int i = 0, j = s.Length; i < j; i++) {
				base64 = base64.Replace(s[i], t[i]);
			}
		
			return base64;
		}
		private static string MakeUrl(string tk2, string vid)
		{
			
			string url = string.Format("https://pcweb.api.mgtv.com/player/video?tk2={0}&video_id={1}&type=pch5", tk2, vid);
			
				
			return url;
		}
	}
}