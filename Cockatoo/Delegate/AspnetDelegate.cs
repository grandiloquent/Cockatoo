namespace  Cockatoo
{
	using Share;
	using System.Net.Http;

	using System.Text;
	using System;
	using System.Windows.Forms;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;
	
	public class AspnetDelegate
	{
		
	
		
		private static string GenerateSQLFromFields(string value)
		{
			
			var matches = Regex.Matches(value, "[\\w]+(?= \\{)").
				Cast<Match>().
				Select(i => i.Value);
			
		    //var pattern = "original.{0}=changed.{0};";
			var list1 = new List<string>();
			var list2 = new List<string>();
			var list3= new List<string>();
			var list4= new List<string>();
			var list5= new List<string>();
			
			var p1="if (!string.IsNullOrWhiteSpace(changed.{0}))original.{0}= changed.{0};";
			var p2="if (changed.{0}>0)original.{0}= changed.{0};";
			var p3="\"{0}\" : \"\",";
			var p4="{0} = v.{0},";
			var p5=" <LinearLayout android:layout_width=\"match_parent\" android:layout_height=\"match_parent\" android:orientation=\"horizontal\"> <TextView android:layout_width=\"wrap_content\" android:layout_height=\"wrap_content\" android:text=\"{0}\"/> <EditText android:id=\"@+id/{0}\" android:layout_width=\"0dp\" android:layout_height=\"wrap_content\" android:layout_weight=\"1\"/> </LinearLayout>";
			
			foreach (var element in matches) {
				list1.Add(string.Format(p1, element));
				list2.Add(string.Format(p2, element));
				list3.Add(string.Format(p3, element));
				list4.Add(string.Format(p4, element));
				list5.Add(string.Format(p5, element));
		
			}
			
			return list1.Concat(list2).Concat(list3)
				.Concat(list4).Concat(list5).ConcatenateLines();
			
		}
		
		public AspnetDelegate()
		{
			
			
			
		}
		
		[BindMenuItem(Name = "字段 (文本)", Toolbar = "javaStrip", SplitButton = "aspnetGenerateSplitButton")]
		public static void GenerateField()
		{
			WinFormHelper.OnClipboardString(GenerateSQLFromFields);
		}
		
	
	}
}