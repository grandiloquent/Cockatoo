using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.IO.Compression;
namespace Share
{
    public static class EpubHelper
    {
        private static void ProcessBook(string fileName, string directory)
        {
            XNamespace dc = "http://purl.org/dc/elements/1.1/";
            var val = ZipFile.Open(fileName, ZipArchiveMode.Read);
            var val2 = val.Entries.First(i => i.Name.EndsWith(".opf"));
            var memoryStream=val2.Open();
           // memoryStream.Position = 0L;
            StreamReader streamReader = new StreamReader(memoryStream);
            XDocument xDocument = XDocument.Load((Stream)memoryStream);
            XElement[] source = xDocument.Descendants().ToArray();
            XElement xElement = (from i in source
                                 where i.Name == dc + "title"
                                 select i).First();
            XElement xElement2 = null;
            try
            {
                xElement2 = (from i in source
                             where i.Name == dc + "creator"
                             select i).First();
            }
            catch
            {
                xElement2 = (from i in source
                             where i.Name == dc + "description"
                             select i).First();
            }
            if (xElement != null)
            {
                string value = xElement.Value;
                value = value.Split("/;:".ToArray(), 2).First();
                value = value.GetValidFileName(' ').Trim();
                string arg = "Anonymous";
                if (xElement2 != null)
                {
                    arg = xElement2.Value.Split('/').Last().Trim().GetValidFileName(' ');
                }
                //$"{value}-{arg}.epub"
                string text = directory.Combine(string.Format("{0} - {1}.epub", value, arg));
                val.Dispose();
                memoryStream.Dispose();
                streamReader.Dispose();
                if (fileName != text && !text.FileExists())
                {
                    File.Move(fileName, text);
                }
            }
        }
        public static void PrettyName(string directory)
        {
            const string outputDirectory = ".EPUB";
            var targetDirectory = Path.Combine(directory, outputDirectory);
            targetDirectory.CreateDirectoryIfNotExists();
            Directory.GetFiles(directory, "*.epub", SearchOption.AllDirectories)
                .ForEach(file =>
              {
                  if (!file.Contains(outputDirectory))
                      ProcessBook(file, targetDirectory);
              });
        }
    }
}
