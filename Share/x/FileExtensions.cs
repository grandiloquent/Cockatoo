namespace Share
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Security.Cryptography;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	
	public static  class FileExtensions
	{
internal const char AltDirectorySeparatorChar = '/';
		internal const char DirectorySeparatorChar = '\\';
		private static readonly char[] InvalidFileNameChars = {
			'\"',
			'<',
			'>',
			'|',
			'\0',
			':',
			'*',
			'?',
			'\\',
			'/'
		};
		private static volatile Encoding _UTF8NoBOM;
		static Encoding UTF8NoBOM {
			get { 
				if (_UTF8NoBOM == null) {
					// No need for double lock - we just want to avoid extra
					// allocations in the common case.
					UTF8Encoding noBOM = new UTF8Encoding(false, true);
					Thread.MemoryBarrier();
					_UTF8NoBOM = noBOM;
				}
				return _UTF8NoBOM;
			}
		}
		public static string ChangeExtension(this string path, string extension)
		{
			if (path != null) {
				string s = path;
				for (int i = path.Length - 1; i >= 0; i--) {
					char ch = path[i];
					if (ch == '.') {
						s = path.Substring(0, i);
						break;
					}
					if (IsDirectorySeparator(ch))
						break;
				}

				if (extension != null && path.Length != 0) {
					s = (extension.Length == 0 || extension[0] != '.') ?
                        s + "." + extension :
                        s + extension;
				}

				return s;
			}
			return null;
		}
		public static string ChangeFileName(this string fileName, Func<string,string> processor)
		{
			return Path.Combine(Path.GetDirectoryName(fileName), processor(Path.GetFileNameWithoutExtension(fileName)) + Path.GetExtension(fileName));
		}
		public static string ChangeFileNameAndExtension(this string fileName, Func<string,string> processor, Func<string,string> extensionProcessor)
		{
			return Path.Combine(Path.GetDirectoryName(fileName), processor(Path.GetFileNameWithoutExtension(fileName)) + extensionProcessor(Path.GetExtension(fileName)));
		}
		public static string Combine(this string dir, string name)
		{
			return Path.Combine(dir, name);
		}
		public static void CreateDirectoryIfNotExists(this string path)
		{
			if (Directory.Exists(path))
				return;
			Directory.CreateDirectory(path);
		}

		
		public static bool FileExists(this string path)
		{
			return File.Exists(path);
		}
		public static string GetDesktopPath(this string f)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), f);
		}
		public static string GetDirectoryFileName(this string v)
		{
			return Path.GetFileName(Path.GetDirectoryName(v));
		}
		public static string GetDirectoryName(this string v)
		{
			return Path.GetDirectoryName(v);
		}

		public static string GetExePath(this string f)
		{
			return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), f);
		}

		public static string GetExtension(this string v)
		{
			return Path.GetExtension(v);
		}
		public static string GetFileName(this string v)
		{
			return Path.GetFileName(v);
		}
       
		public static string GetMD5(this string filename)
		{
			using (var md5 = MD5.Create()) {
				using (var stream = File.OpenRead(filename)) {
					var hash = md5.ComputeHash(stream);
					return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
				}
			}
		}
		public static string GetUniqueFileName(this string v)
		{
			int i = 1;
			Regex regex = new Regex(" \\- [0-9]+");
			string t = Path.Combine(Path.GetDirectoryName(v),
				           regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
				           Path.GetExtension(v));
			while (File.Exists(t)) {
				i++;
				t = Path.Combine(Path.GetDirectoryName(v),
					regex.Split(Path.GetFileNameWithoutExtension(v), 2).First() + " - " + i.ToString().PadLeft(3, '0') +
					Path.GetExtension(v));
			}
			return t;
		}


		public static string GetValidFileName(this string v)
		{
			if (v == null)
				return null;
			// (Char -> Int) 1-31 Invalid;
			List<char> chars = new List<char>(v.Length);
			for (int i = 0; i < v.Length; i++) {
				if (InvalidFileNameChars.Contains(v[i])) {
					chars.Add(' ');
				} else {
					chars.Add(v[i]);
				}
			}
			return new string(chars.ToArray());
		}
		public static string GetValidFileName(this string value, char c)
		{
			var chars = Path.GetInvalidFileNameChars();
			return new string(value.Select<char, char>((i) => {
				if (chars.Contains(i))
					return c;
				return i;
			}).Take(125).ToArray());
		}
		public static bool IsDirectoryEmpty(this DirectoryInfo possiblyEmptyDirectory)
		{
			using (var enumerator = Directory.EnumerateFileSystemEntries(possiblyEmptyDirectory.FullName).GetEnumerator()) {
				return !enumerator.MoveNext();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsDirectorySeparator(char c)
		{
			return c == DirectorySeparatorChar || c == AltDirectorySeparatorChar;
		}
		internal static string NormalizeDirectorySeparators(string path)
		{
			if (string.IsNullOrEmpty(path))
				return path;

			char current;

			// Make a pass to see if we need to normalize so we can potentially skip allocating
			bool normalized = true;

			for (int i = 0; i < path.Length; i++) {
				current = path[i];
				if (IsDirectorySeparator(current)
				    && (current != DirectorySeparatorChar
                        // Check for sequential separators past the first position (we need to keep initial two for UNC/extended)
				    || (i > 0 && i + 1 < path.Length && IsDirectorySeparator(path[i + 1])))) {
					normalized = false;
					break;
				}
			}

			if (normalized)
				return path;

			StringBuilder builder = new StringBuilder(path.Length);

			int start = 0;
			if (IsDirectorySeparator(path[start])) {
				start++;
				builder.Append(DirectorySeparatorChar);
			}

			for (int i = start; i < path.Length; i++) {
				current = path[i];

				// If we have a separator
				if (IsDirectorySeparator(current)) {
					// If the next is a separator, skip adding this
					if (i + 1 < path.Length && IsDirectorySeparator(path[i + 1])) {
						continue;
					}

					// Ensure it is the primary separator
					current = DirectorySeparatorChar;
				}

				builder.Append(current);
			}

			return builder.ToString();
		}
		public static string[] ReadAllLines(this string path)
		{
			string line;
			List<string> lines = new List<string>();
			using (StreamReader sr = new StreamReader(path, UTF8NoBOM))
				while ((line = sr.ReadLine()) != null)
					lines.Add(line);
			return lines.ToArray();
		}
		public static string ReadAllText(this string path)
		{
			 
			using (StreamReader sr = new StreamReader(path, UTF8NoBOM, true))
				return sr.ReadToEnd();
		}
		public static void WriteAllLines(this string path, IEnumerable<String> contents)
		{
			 
			var writer =	new StreamWriter(path, false, UTF8NoBOM);
			using (writer) {
				foreach (String line in contents) {
					writer.WriteLine(line);
				}
			}
		}
		public static void WriteAllText(this String path, String contents)
		{
			using (StreamWriter sw = new StreamWriter(path, false, UTF8NoBOM, 1024))
				sw.Write(contents);
		}
	}
}