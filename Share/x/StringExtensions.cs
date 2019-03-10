namespace Share
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Security.Cryptography;
	using System.Text;
	using System.Text.RegularExpressions;

	public static   class StringExtensions
	{
		// whitespace
		internal const byte E = 1;
		internal const byte Q = 5;
		// quantifier
		internal const byte S = 4;
		// ScanBlank stopper
		internal const byte X = 2;
		// ordinary stoppper
		internal const byte Z = 3;
		// should be escaped
		/*
         * For categorizing ascii characters.
        */
		internal static readonly byte[] _category = new byte[] {
			// 0 1 2 3 4 5 6 7 8 9 A B C D E F 0 1 2 3 4 5 6 7 8 9 A B C D E F 
			0, 0, 0, 0, 0, 0, 0, 0, 0, X, X, 0, X, X, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			//   ! " # $ % & ' ( ) * + , - . / 0 1 2 3 4 5 6 7 8 9 : ; < = > ? 
			X, 0, 0, Z, S, 0, 0, 0, S, S, Q, Q, 0, 0, S, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Q,
			// @ A B C D E F G H I J K L M N O P Q R S T U V W X Y Z [ \ ] ^ _
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, S, S, 0, S, 0,
			// ' a b c d e f g h i j k l m n o p q r s t u v w x y z { | } ~ 
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Q, S, 0, 0, 0
		};
		private static readonly CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
		private const int s_aLower = (int)'a';
		private const int s_aUpper = (int)'A';
		private const int s_zeroChar = (int)'0';
		public static string Capitalize(this string value)
		{
			//  && char.IsLower(value[0])
			if (!string.IsNullOrEmpty(value)) {
				return value.Substring(0, 1).ToUpper() + value.Substring(1);
			}
			return value;
		}
		public static string CapitalizeCamelCaseUnderscore(this string value)
		{
			var buf = new List<char>(value.Length * 2);

			for (int i = 0; i < value.Length; i++) {
				if (i != 0 && char.IsUpper(value[i])) {
					buf.Add('_');
				}
				buf.Add(char.ToUpper(value[i]));
			}
			return new string(buf.ToArray());
		}
	
		public static int ChineseToInt(this string v)
		{
			if (Regex.IsMatch(v, "第[^0-9]+集"))
				return (int)NumberConventer.ChnToArab(Regex.Match(v, "[零一二三四五六七八九十百千万亿]+").Value);
			else
				return	v.ConvertToInt();
		}
       
       
		public static string Concatenate(this IEnumerable<string> strings)
		{
			return strings.Concatenate((builder, nextValue) => builder.Append(nextValue));
		}
		private static string Concatenate(this IEnumerable<string> strings,
			Func<StringBuilder, string, StringBuilder> builderFunc)
		{
			return strings.Aggregate(new StringBuilder(), builderFunc).ToString();
		}
		public static string ConcatenateLines(this IEnumerable<string> strings)
		{
			return strings.Concatenate((builder, nextValue) => builder.AppendLine(nextValue));
		}
		public static int ConvertToInt(this string str)
		{
			var length = str.Length;
			var numbers = new List<char>(length);
			
			for (int i = 0; i < length; i++) {
				if (char.IsDigit(str[i])) {
					numbers.Add(str[i]);
					while (i + 1 < length && char.IsDigit(str[i + 1])) {
						i++;
						numbers.Add(str[i]);
					}
					break;
				} else {
					
				}
			}
			if (numbers.Count == 0)
				return -1;
			return int.Parse(string.Join("", numbers));
		}
		
		public static string Crc32(this string value)
		{
			var buffer = Encoding.UTF8.GetBytes(value);
			var r = Crc32Helper.CalculateHash(Crc32Helper.DefaultSeed, buffer, 0, buffer.Length);
			return r.ToString();
		}
		public static string Decapitalize(this string value)
		{
			//  && char.IsLower(value[0])
			if (!string.IsNullOrEmpty(value)) {
				return value.Substring(0, 1).ToLower() + value.Substring(1);
			}
			return value;
		}

		static string DecodeEncodedNonAsciiCharacters(string value)
		{
			return Regex.Replace(
				value,
				@"\\u(?<Value>[a-zA-Z0-9]{4})",
				m => {
					return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
				});
		}
		public static string EncodeCharacters(this string value)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char c in value) {

				string encodedValue = "\\u" + ((int)c).ToString("x4");
				sb.Append(encodedValue);

			}
			return sb.ToString();

		}
		static string EncodeNonAsciiCharacters(string value)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char c in value) {
				if (c > 127) {
					// This character is too big for ASCII
					string encodedValue = "\\u" + ((int)c).ToString("x4");
					sb.Append(encodedValue);
				} else {
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		public static bool EqualsIgnoreCase(this string s1, string s2)
		{
			if (String.IsNullOrEmpty(s1) && String.IsNullOrEmpty(s2)) {
				return true;
			}
			if (String.IsNullOrEmpty(s1) || String.IsNullOrEmpty(s2)) {
				return false;
			}
			if (s2.Length != s1.Length) {
				return false;
			}
			return 0 == string.Compare(s1, 0, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase);
		}
		public static bool EqualsIgnoreCase(this string s1, int index1, string s2, int index2, int length)
		{
			return String.Compare(s1, index1, s2, index2, length, StringComparison.OrdinalIgnoreCase) == 0;
		}
		public static String EscapeHtml(this String value)
		{
			if (value == null || value.Length == 0)
				return value;
                
			StringBuilder stringBuffer = new StringBuilder();
			int index = value.IndexOf('&');
			if (index > -1) {
				stringBuffer.Append(value);
				stringBuffer.Replace("&", "&#38;", index, stringBuffer.Length - index);
			}
 
			index = value.IndexOf('"');
			if (index > -1) {
				if (stringBuffer.Length == 0)
					stringBuffer.Append(value);
				stringBuffer.Replace("\"", "&#34;", index, stringBuffer.Length - index);
			}
 
			index = value.IndexOf('\'');
			if (index > -1) {
				if (stringBuffer.Length == 0)
					stringBuffer.Append(value);
				stringBuffer.Replace("\'", "&#39;", index, stringBuffer.Length - index);
			}
 
			index = value.IndexOf('<');
			if (index > -1) {
				if (stringBuffer.Length == 0)
					stringBuffer.Append(value);
				stringBuffer.Replace("<", "&#60;", index, stringBuffer.Length - index);
			}
 
			index = value.IndexOf('>');
			if (index > -1) {
				if (stringBuffer.Length == 0)
					stringBuffer.Append(value);
				stringBuffer.Replace(">", "&#62;", index, stringBuffer.Length - index);
			}
 
			index = value.IndexOf(Char.MinValue);
			if (index > -1) {
				if (stringBuffer.Length == 0)
					stringBuffer.Append(value);
				stringBuffer.Replace(Char.MinValue.ToString(), "&#0;", index, stringBuffer.Length - index);
			}
 
			String returnValue = null;
 
			if (stringBuffer.Length > 0)
				returnValue = stringBuffer.ToString();
			else
				returnValue = value;
 
			return returnValue;
		}
		public static String EscapeJSON(this String value)
		{
			if (value == null || value.Length == 0)
				return value;
                
			StringBuilder stringBuffer = new StringBuilder();
			int index = value.IndexOf('\"');
			if (index > -1) {
				stringBuffer.Append(value);
				stringBuffer.Replace("\"", "\\\"", index, stringBuffer.Length - index);
			}
 
			
			String returnValue = null;
 
			if (stringBuffer.Length > 0)
				returnValue = stringBuffer.ToString();
			else
				returnValue = value;
 
			return returnValue;
		}
		public static String EscapeRegex(this String input)
		{
			for (int i = 0; i < input.Length; i++) {
				if (IsMetachar(input[i])) {
					StringBuilder sb = new StringBuilder();
					char ch = input[i];
					int lastpos;
 
					sb.Append(input, 0, i);
					do {
						sb.Append('\\');
						switch (ch) {
							case '\n':
								ch = 'n';
								break;
							case '\r':
								ch = 'r';
								break;
							case '\t':
								ch = 't';
								break;
							case '\f':
								ch = 'f';
								break;
						}
						sb.Append(ch);
						i++;
						lastpos = i;
 
						while (i < input.Length) {
							ch = input[i];
							if (IsMetachar(ch))
								break;
 
							i++;
						}
 
						sb.Append(input, lastpos, i - lastpos);
 
					} while (i < input.Length);
 
					return sb.ToString();
				}
			}
 
			return input;
		}
		public static string Flate(this string value)
		{
			var line = Regex.Replace(value, "\\s{2,}", " ");
			
			return Regex.Replace(line, "[\r\n]+", "");
		}
		public static string GetUniqueKey(this int size)
		{
			char[] chars =
				"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
			byte[] data = new byte[size];
			using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider()) {
				crypto.GetBytes(data);
			}
			StringBuilder result = new StringBuilder(size);
			foreach (byte b in data) {
				result.Append(chars[b % (chars.Length)]);
			}
			return result.ToString();
		}
		public static bool IsDigitString(this string str)
		{
			for (int i = 0; i < str.Length; i++) {
				if (!char.IsDigit(str[i]))
					return false;
			}
			return true;
		}
 
       
		static bool IsMetachar(char ch)
		{
			return(ch <= '|' && _category[ch] >= E);
		}
      
		public static bool IsReadable(this string value)
		{
			return !string.IsNullOrWhiteSpace(value);
		}
		public static bool IsVacuum(this string value)
		{
			return string.IsNullOrWhiteSpace(value);
		}
		public static bool IsWhiteSpace(this string value)
		{
			if (value == null)
				return true;
			for (int i = 0; i < value.Length; i++) {
				if (!char.IsWhiteSpace(value[i]))
					return false;
			}
			return true;
		}
		public static IEnumerable<string> OrderWithPadNumber(this IEnumerable<string> list, int padLength)
		{

			return list.OrderBy(i => {
				return Regex.Replace(i, "[0-9]+", new MatchEvaluator(m => m.Value.PadLeft(padLength)));
			});
		}
		
		public static  string  PadNumber(this  string value, int padLength)
		{

		 
			return Regex.Replace(value, "[0-9]+", new MatchEvaluator(m => m.Value.PadLeft(padLength)));
			 
		}
    
		static private int ParseHexChar(char c)
		{
			int intChar = (int)c;
			if ((intChar >= s_zeroChar) && (intChar <= (s_zeroChar + 9))) {
				return (intChar - s_zeroChar);
			}
			if ((intChar >= s_aLower) && (intChar <= (s_aLower + 5))) {
				return (intChar - s_aLower + 10);
			}
			if ((intChar >= s_aUpper) && (intChar <= (s_aUpper + 5))) {
				return (intChar - s_aUpper + 10);
			}
			throw new FormatException();
		}
		public static byte[] ParseHexColor(this string trimmedColor)
		{
			int a, r, g, b;
			a = 255;
			if (trimmedColor.Length > 7) {
				a = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
				r = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
				g = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
				b = ParseHexChar(trimmedColor[7]) * 16 + ParseHexChar(trimmedColor[8]);
			} else if (trimmedColor.Length > 5) {
				r = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
				g = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
				b = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
			} else if (trimmedColor.Length > 4) {
				a = ParseHexChar(trimmedColor[1]);
				a = a + a * 16;
				r = ParseHexChar(trimmedColor[2]);
				r = r + r * 16;
				g = ParseHexChar(trimmedColor[3]);
				g = g + g * 16;
				b = ParseHexChar(trimmedColor[4]);
				b = b + b * 16;
			} else {
				r = ParseHexChar(trimmedColor[1]);
				r = r + r * 16;
				g = ParseHexChar(trimmedColor[2]);
				g = g + g * 16;
				b = ParseHexChar(trimmedColor[3]);
				b = b + b * 16;
			}
			return new[] { (byte)a, (byte)r, (byte)g, (byte)b };
		}
		public static string RemoveEmptyLines(this string value)
		{
			var lines = value.Split(Environment.NewLine.ToArray(), StringSplitOptions.RemoveEmptyEntries);
			return lines.ConcatenateLines();
		}
		public static string RemoveNewLine(this string value)
		{
			return Regex.Replace(value, "[\r\n]+", "");
		}
		public static string Repeat(this string value, int count)
		{
			if (count <= 0)
				return string.Empty;
			return	string.Concat(Enumerable.Repeat(value, count));
			
		}
		public static string ReplaceFirst(this string line, string str1, string str2)
		{
			int idx = line.IndexOf(str1, StringComparison.Ordinal);
			if (idx >= 0) {
				line = line.Remove(idx, str1.Length);
				line = line.Insert(idx, str2);
			}
			return line;
		}
		public static bool StringEndsWithIgnoreCase(this string s1, string s2)
		{
			int offset = s1.Length - s2.Length;
			if (offset < 0) {
				return false;
			}

			return 0 == string.Compare(s1, offset, s2, 0, s2.Length, StringComparison.OrdinalIgnoreCase);
		}
		public static string SubstringAfter(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringAfter(this string s1, string s2)
		{
			if (s2.Length == 0) {
				return s1;
			}
			//int idx = collation.IndexOf(s1, s2);
			int idx = compareInfo.IndexOf(s1, s2, CompareOptions.Ordinal);
			return (idx < 0) ? string.Empty : s1.Substring(idx + s2.Length);
		}
		public static string SubstringAfterLast(this string value, char delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringAfterLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(index + 1);
		}
		public static string SubstringBefore(this string value, char delimiter)
		{
			var index = value.IndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBefore(this string s1, string s2)
		{
			if (s2.Length == 0) {
				return s2;
			}
			//int idx = collation.IndexOf(s1, s2);
			int idx = compareInfo.IndexOf(s1, s2, CompareOptions.Ordinal);
			return (idx < 1) ? s1 : s1.Substring(0, idx);
		}
		public static string SubstringBeforeLast(this string value, char delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static string SubstringBeforeLast(this string value, string delimiter)
		{
			var index = value.LastIndexOf(delimiter);
			if (index == -1)
				return value;
			else
				return value.Substring(0, index);
		}
		public static IEnumerable<string> ToBlocks(this string value)
		{
			var count = 0;
			var sb = new StringBuilder();
			var ls = new List<string>();
			for (int i = 0; i < value.Length; i++) {
				sb.Append(value[i]);
				if (value[i] == '{') {
					count++;
				} else if (value[i] == '}') {
					count--;
					if (count == 0) {
						ls.Add(sb.ToString());
						sb.Clear();
					}
				}
			}
			return ls;
		}
	}
}