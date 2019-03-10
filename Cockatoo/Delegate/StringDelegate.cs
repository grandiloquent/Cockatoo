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
	
	public static class StringDelegate
	{
		
        private static string EscapeForCSharp(string value)
        {
            value = Regex.Replace(value, "[\t ]{2,}", " ");
           return Regex.Replace(value, "[{}\r\n\"]", new MatchEvaluator(m =>
            {
                switch (m.Value)
                {
                    case "{": return "{{";
                    case "}": return "}}";
                    case "\r": return "";
                    case "\"":return "\\\"";
                    case "\n": return "\\n";

                }
                return m.Value;
            }));
        }
	}
}