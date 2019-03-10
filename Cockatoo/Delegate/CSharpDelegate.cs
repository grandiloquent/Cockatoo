namespace  Cockatoo
{
	using Microsoft.CodeAnalysis;
	using Microsoft.CodeAnalysis.CSharp;
	using Microsoft.CodeAnalysis.CSharp.Syntax;
	using System;
	using System.Linq;
	using System.Text;
	using Share;
	
	public static class CSharpDelegate
	{
		private static void SplitSourceCodeFileInternal(string fileName)
		{
			var strNameSpace = string.Empty;
			
			var rootNode = CSharpSyntaxTree.ParseText(fileName.ReadAllText()).GetRoot();
			var namespace_ = rootNode.DescendantNodes().OfType<NamespaceDeclarationSyntax>();
			if (namespace_.Any()) {
				var s = new StringBuilder();
				s.Append(namespace_.First().NamespaceKeyword.Text).Append(' ').Append(namespace_.First().Name).Append('{');
				strNameSpace = s.ToString();
			}
			var using_ = rootNode.DescendantNodes().OfType<UsingDirectiveSyntax>();
			var strUsing = string.Empty;
		
			if (using_.Any()) {
				var s = new StringBuilder();
				using_ = using_.OrderBy(i => i.Name.ToString());//.Distinct(i => i.Name.GetText());
				foreach (var item in using_) {
					s.Append(item.ToFullString());
				}
				strUsing = s.ToString();
			}
			var class_ = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>();
			if (class_.Any()) {
				class_ = class_.OrderBy(i => i.Identifier.ValueText);
				foreach (var item in class_) {
					var s = new StringBuilder();
					s.AppendLine(strNameSpace).AppendLine(strUsing)
						.AppendLine(item.ToFullString())
						.AppendLine("}");
					(item.Identifier.ValueText + ".cs").GetDesktopPath().WriteAllText(s.ToString());
				}
				
			}
			
				var interfaces = rootNode.DescendantNodes().OfType<InterfaceDeclarationSyntax>();
			if (interfaces.Any()) {
				interfaces = interfaces.OrderBy(i => i.Identifier.ValueText);
				foreach (var item in interfaces) {
					var s = new StringBuilder();
					s.AppendLine(strNameSpace).AppendLine(strUsing)
						.AppendLine(item.ToFullString())
						.AppendLine("}");
					(item.Identifier.ValueText + ".cs").GetDesktopPath().WriteAllText(s.ToString());
				}
				
			}
				
		}
		[BindMenuItem(Name = "分隔源代码", Toolbar = "javaStrip", SplitButton = "csharpSplitButton")]
		public static void SplitSourceCodeFile()
		{
			WinFormHelper.OnClipboardFile(SplitSourceCodeFileInternal);
			
		}
	}
}