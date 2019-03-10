namespace  Cockatoo
{
	using System.Text;
	using Share;
	
	public class NginxHelper
	{
		public static string FormatNginxConf(string value)
		{
			var sb = new StringBuilder();
			var count = 0;
			value=System.Text.RegularExpressions.Regex.Replace(value,"\\s{2,}"," " );
			var length=value.Length;
			for (int i = 0; i < length; i++) {
				var item=value[i];
				if (item == '{') {
					count++;
					while(i+1<length && char.IsWhiteSpace(value[i])){
						i++;
					}
					sb.AppendLine( "{").Append("\t".Repeat(count) );
				} else if (item == '}') {
					count--;
					while(i+1<length && char.IsWhiteSpace(value[i])){
						i++;
					}
					sb.AppendLine("\t".Repeat(count) + "}").Append("\t".Repeat(count));

				} else if (item == ';') {
					while(i+1<length && char.IsWhiteSpace(value[i])){
						i++;
					}
					sb.AppendLine(";");
					sb.Append("\t".Repeat(count));
				} else if (item == '\r' || item == '\n' || item == '\t') {

					continue;
				}else if(item=='#'){
					while(i+1<length && value[i]!='\n'){
						i++;
					}
				//sb.Append("\n"+"\t".Repeat(count) + "#");

				} else {
					sb.Append(item);
				}

			}
		 
			return sb.ToString();
		}

	}
}