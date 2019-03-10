using System;
using System.Windows.Forms;
namespace  Cockatoo
{

	[AttributeUsage(AttributeTargets.All)]
	public class BindMenuItemAttribute:Attribute
	{
		public String Name{get;set;}
		public String SplitButton{get;set;}
		public String Toolbar{get;set;}
		public bool AddSeparatorBefore{get;set;}
	}
}