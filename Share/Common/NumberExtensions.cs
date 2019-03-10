namespace Share
{
	using System.Collections.Generic;
	using System.Linq;
	
	public static class NumberExtensions
	{
		/*
		 var ls = new List<int>();
			ls.Add(1);
			ls.Add(2);
			ls.Add(4);
			ls.Add(5);
			ls.Add(6);
			ls.Add(7);
		
			var list =	ls.FindMissing(1, 7);
			
			3
		 */
		public static IEnumerable<int> FindMissing(this IEnumerable<int> values,int start,int end)
		{
			HashSet<int> myRange = new HashSet<int>(Enumerable.Range(start, end));
			myRange.ExceptWith(values);
			return myRange;
		}
	}
}