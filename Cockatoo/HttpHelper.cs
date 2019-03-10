using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Share;
using HtmlAgilityPack;
using Microsoft.Ajax.Utilities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Net;
using System.Security.Cryptography;

namespace  Cockatoo
{
	
	public static class HttpHelper
	{
		
		
			
		public async static Task<string> Authenticate(string url, string userName, string password)
		{
			var client = new HttpClient();
		
			var msg = new HttpRequestMessage(HttpMethod.Post, url);
		
			var dic = new Dictionary<string,string>();
			dic.Add("username", userName);
			dic.Add("password", password);
			ServicePointManager.ServerCertificateValidationCallback =
    		delegate {
				return true;
			};
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			msg.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dic), Encoding.UTF8, "application/json");
	 
			var res = await client.SendAsync(msg);
	
			return  await res.Content.ReadAsStringAsync();
		}
			
		public async static Task<string> Post(string url, string accessToken = null)
		{
			var client = new HttpClient();
		
			var msg = new HttpRequestMessage(HttpMethod.Post, url);
			if (accessToken != null)
				msg.Headers.Add("Authorization", "Bearer " + accessToken);
			
			var res = await client.SendAsync(msg);
	
			return  await res.Content.ReadAsStringAsync();
		}
		
		public async static Task<string> PostXML(string url, string referer)
		{
			var client = new HttpClient();
		
			// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers
			
			var msg = new HttpRequestMessage(HttpMethod.Get, url);
		
			if (referer.IsReadable()) {
				msg.Headers.Add("Referer", referer);
			}
			var res = await client.SendAsync(msg);
	
			return res.StatusCode.ToString();//+" "+await res.Content.ReadAsStringAsync();
		}
		public async static Task<string> PostJson(string fileName, string url, string accessToken = null)
		{
			
			var client = new HttpClient();
			
			if (accessToken != null)
				client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
			var content = new StringContent(fileName.ReadAllText(), Encoding.UTF8, "application/json");
			var response = await client.PostAsync(url, content);
		
			var message = await response.Content.ReadAsStringAsync();
		
			if (message.IsNullOrWhiteSpace()) {
				message = response.StatusCode.ToString();
			}
			return message;
		}
		public async static void TouchServer(Action<string> act = null)
		{
			
			try {
				var message = await TouchServer("http://localhost:5000/Home/Create");
				if (act != null)
					act(message);
			} catch (Exception e) {
				if (act != null)
					act(e.Message);
			}
		}
	 
		public async static Task<string> TouchServer(string url)
		{
			
			var formData = new MultipartFormDataContent();
			
			formData.Add(new StringContent("Album"), "Album.Title");
//			formData.Add(new StringContent("AlbumId"), "AlbumId");
			formData.Add(new StringContent("Cover"), "Cover");
//			formData.Add(new StringContent("CreatedAt"), "CreatedAt");
			formData.Add(new StringContent("11:00"), "Duration");
			formData.Add(new StringContent("720"), "Height");
//			formData.Add(new StringContent("Id"), "Id");
			formData.Add(new StringContent("Tags"), "Tags");
			formData.Add(new StringContent("Thumbnail"), "Thumbnail");
			formData.Add(new StringContent("Title"), "Title");
//			formData.Add(new StringContent("UpdatedAt"), "UpdatedAt");
			formData.Add(new StringContent("Url"), "Url");
//			formData.Add(new StringContent("VideoTags"), "VideoTags");
//			formData.Add(new StringContent("VoteDown"), "VoteDown");
//			formData.Add(new StringContent("VoteUp"), "VoteUp");
//			formData.Add(new StringContent("WatchedCount"), "WatchedCount");
			formData.Add(new StringContent("1280"), "Width");
			var client = new HttpClient();
			var response = await client.PostAsync(url, formData);
			return await response.Content.ReadAsStringAsync();
		}
	}
}
