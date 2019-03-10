using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO.Compression;
using System.IO;

namespace Share
{
	public static class HttpClientExtensions
	{
		public static HttpClient GetHttpClient()
		{
			return new HttpClient(new HttpClientHandler() {
				UseProxy = false,
				UseDefaultCredentials = true,
				UseCookies = false,
			});
		}
		public async static Task<string> Authenticate(this HttpClient httpClient,
			string url, 
			string userName,
			string password)
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
		public async static Task<string> Post(this HttpClient httpClient,string url, string accessToken = null)
		{
			
			
			var httpMessage = GetHttpRequestMessage(HttpMethod.Post, url);
        	
			if (accessToken != null)
				httpMessage.Headers.Add("Authorization", "Bearer " + accessToken);
			var response = await httpClient.SendAsync(httpMessage);
		
			var message = await response.Content.ReadAsStringAsync();
		
			if (string.IsNullOrWhiteSpace(message)) {
				message = response.StatusCode.ToString();
			}
			return message;
		}
		public async static Task<string> PostJson(this HttpClient httpClient, string content, string url, string accessToken = null)
		{
			
			//System.Windows.Forms.Clipboard.SetText(content);
			var httpMessage = GetHttpRequestMessage(HttpMethod.Post, url);
        	
			if (accessToken != null)
				httpMessage.Headers.Add("Authorization", "Bearer " + accessToken);

			
			var stream=content.ToStream();
			var sc=new StreamContent(stream);
			sc.Headers.ContentLength=stream.Length;
			sc.Headers.Add("Content-Encoding", "gzip");
			sc.Headers.ContentType=new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			
			httpMessage.Content = sc;// new StringContent(content, Encoding.UTF8, "application/json");
			var response = await httpClient.SendAsync(httpMessage);
			return response.StatusCode.ToString();
//			var message = await response.Content.ReadAsStringAsync();
//		
//			if (string.IsNullOrWhiteSpace(message)) {
//				message = response.StatusCode.ToString();
//			}
//			return message;
		}
		private static HttpRequestMessage GetHttpRequestMessage(HttpMethod method, string url)
		{
			var httpMessage = new HttpRequestMessage(method, url);
			httpMessage.Headers.Add("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");
			httpMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
			httpMessage.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
			httpMessage.Headers.Add("Connection", "keep-alive");
			httpMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.92 Safari/537.36");

			return httpMessage;
		}
        	
		public async static Task<string> ReadStringWithCookie(this HttpClient httpClient, string url, string referrer = null, string cookie = null)
		{
			var httpMessage = GetHttpRequestMessage(HttpMethod.Get, url);
			
			
			if (referrer != null)
				httpMessage.Headers.Add("Referer", referrer);
			 
			httpMessage.Headers.Add("Cookie", cookie);
			var response = await httpClient.SendAsync(httpMessage).ConfigureAwait(false);
			var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			
			
			
			return Encoding.UTF8.GetString(bytes);
		}
		public async static Task<string[]> ReadStringAndCookie(this HttpClient httpClient, string url, string referrer = null)
		{
			var httpMessage = GetHttpRequestMessage(HttpMethod.Get, url);
			

			if (referrer != null)
				httpMessage.Headers.Add("Referer", referrer);
			 
			
			var response = await httpClient.SendAsync(httpMessage).ConfigureAwait(false);
			var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			
			var cookieStr = string.Empty;
			IEnumerable<string> values;
			if (response.Headers.TryGetValues("Set-Cookie", out values)) {
				cookieStr = values.First();
			}
			
			return new [] {
				cookieStr,
				Encoding.UTF8.GetString(bytes)
			};
		}
	}
}