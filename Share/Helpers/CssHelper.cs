
using System;
using System.Linq;

namespace Share
{

	public static class CssHelper
	{
		public static string ForamtSort(string value)
		{
			var blocks = value.ToBlocks();
			var sorted = blocks
				.OrderBy(i => {
				var sort = i.SubstringBefore('{')
			                            .TrimEnd()
			                            .SubstringAfterLast('.')
			                            .SubstringBeforeLast(' ')
				        	.SubstringBeforeLast('#');
				return sort;
			}).ThenBy(i => i.SubstringBefore('{').Trim().Length)
				;
			
			return sorted.ConcatenateLines().RemoveEmptyLines();
			
		}
	}
}
