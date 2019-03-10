namespace Share
{
	
	using System.Text;
	using HtmlAgilityPack;
	using System.IO;
	
	public static class Files{
		public static bool DeleteIfExists(this string fileName){
			if(File.Exists(fileName)){
				 File.Delete(fileName);
				 return File.Exists(fileName);
			}
			return false;
		}
	}
	public static class Strings
	{
		
		public static string RemoveWhiteSpace(this string str)
		{
			var sb = new StringBuilder(str.Length);
			foreach (var element in str) {
				if (char.IsWhiteSpace(element))
					continue;
				sb.Append(element);
			}
			return sb.ToString();
			
		}
		
	
	}
	public static class HtmlAgilityPacks
	{
		public static string GetChildTextByClass(this HtmlNode n,string className)
		{
			return HtmlEntity.DeEntitize(n.SelectSingleNode(".//*[contains(@class,'"+className+"')]").InnerText.Trim());
			
		}
		public static string GetText(this HtmlNode n,string xpath)
		{
			return HtmlEntity.DeEntitize(n.SelectSingleNode(xpath).InnerText.Trim());
			
		}
		public static HtmlNode GetLastClass(this HtmlNode n, string className)
		{
			return n.SelectSingleNode("(//*[contains(@class,'" + className + "')])[last()]");
		}
		public static HtmlNode GetFirstClass(this HtmlNode n, string className)
		{
			return n.SelectSingleNode("//*[contains(@class,'" + className + "')]");
		}
	}
}