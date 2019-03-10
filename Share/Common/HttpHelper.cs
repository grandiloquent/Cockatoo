namespace Share
{
	
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Text;
	using System.Linq;
	using System.Collections.Generic;
	
	public static class HttpHelper
	{
		

		public static HttpClient GetHttpClient()
		{
			return  new HttpClient(new HttpClientHandler() {
				UseProxy = false,
				UseDefaultCredentials = true,
				UseCookies = false,
			});
			
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
		
		public async static Task<string> ReadStringWithCookie(HttpClient httpClient, string url, string referrer = null, string cookie=null)
		{
			var httpMessage = GetHttpRequestMessage(HttpMethod.Get, url);
			
			
			if (referrer != null)
				httpMessage.Headers.Add("Referer", referrer);
			 
			httpMessage.Headers.Add("Cookie", cookie);
			var response = await httpClient.SendAsync(httpMessage).ConfigureAwait(false);
			var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			
			
			
			return Encoding.UTF8.GetString(bytes);
		}
		public async static Task<string[]> ReadStringAndCookie(HttpClient httpClient, string url, string referrer = null)
		{
			var httpMessage = GetHttpRequestMessage(HttpMethod.Get, url);
			

			if (referrer != null)
				httpMessage.Headers.Add("Referer", referrer);
			 
			
			var response = await httpClient.SendAsync(httpMessage).ConfigureAwait(false);
			var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			
			string cookieStr = string.Empty;
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