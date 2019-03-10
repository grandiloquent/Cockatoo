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
	
	
	public static class JavaDelegate
	{
		public static void FormatSourceFiles(string dir)
		{
			var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories)
                                 .Where(i => Regex.IsMatch(i, "\\.(?:java|kt)$"));
			
		
			if (!files.Any())
				return;
		
			foreach (var file in files) {
				FormatSourceFile(file);
			}
		}
//		public static string JavaGenerateLogForMethodsAndroid(this string value)
//		{
//			return value.ToBlocks().Select(i => i.ReplaceFirst("{", string.Format( "{{\nLog.e(TAG,\"===> [{0}]\");\n"),i.SubstringBefore('(').Trim().SubstringAfterLast(' '));
//
//		}
//		public static string JavaGenerateLogForMethods(this string value)
//        {
//            return value.ToBlocks().Select(i => i.ReplaceFirst("{", $"{{\nSystem.out.println(\"===> [{Regex.Match(i, "(?<= )[^ ]*?\\([^\\)]*?\\)").Value.RemoveNewLine()}]\");\n")).Joining();
//
//        }
		public static string GenerateStaticString(string value)
		{
			var pieces = value.Split(new char[]{ ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
			var sb = new StringBuilder(value.Length);
			for (int i = 0; i < pieces.Length; i++) {
				for (int j = 0; j < pieces[i].Length; j++) {
					if (j != 0 && char.IsUpper(pieces[i][j])) {
						sb.Append('_');
					}
					sb.Append(char.ToUpper(pieces[i][j]));
				}
				sb.Append('_');
			}
			
			return string.Format("public static final String {0} =\"{1}\";\n", sb.ToString().TrimEnd('_'), value);
		}
		private static void FormatSourceFile(string file)
		{
			
			var lines = file.ReadAllLines();
			var list = new List<string>();
			var skip = false;
			var first = true;
			foreach (var line in lines) {
				
				if (first && !skip && line.TrimStart().StartsWith("/*")) {
					skip = true;
					continue;
				}
				if (first && skip && line.TrimEnd().EndsWith("*/")) {
					skip = false;
					first = false;
					continue;
				}
				 
				if (skip || line.IsVacuum())
					continue;
				list.Add(line);
			}
			file.WriteAllLines(list);

		}
		
		private static string FormatGradleImplementation(string value)
		{
			var array = Regex.Matches(value, "'([^']*?)'").Cast<Match>().Select(i => i.Groups[1].Value).ToArray();
			var len = array.Length;
			var list = new List<String>();
			for (int i = 0; i < len; i++) {
				if (i + 2 < len) {
					list.Add(string.Format("implementation \"{0}:{1}:{2}\"",
						array[i],
						array[i + 1],
						array[i + 2]
					));
				}
			}
			return list.OrderBy(i => i).Distinct().Concatenate();
		}
		[BindMenuItem(Name = "标准化 Gradle (文本)", Toolbar = "javaStrip", SplitButton = "javaSplitButton", AddSeparatorBefore = true)]
		
		public static void StandardizationGradle()
		{
			WinFormHelper.OnClipboardString(FormatGradleImplementation);
		}
	}
}