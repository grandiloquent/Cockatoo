
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
namespace Share
{
	
	public static class JavaHelper
	{
	
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
		
		
		
		
		
	
	}
}
