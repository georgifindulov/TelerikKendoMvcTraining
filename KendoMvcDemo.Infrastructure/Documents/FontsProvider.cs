using Telerik.Documents.Core.Fonts;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Extensibility;

namespace KendoMvcDemo.Infrastructure.Documents
{
    public class FontsProvider : FontsProviderBase
    {
        public override byte[] GetFontData(FontProperties fontProperties)
        {
            string fontFileName = fontProperties.FontFamilyName + ".ttf";
            string fontFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

            //The fonts can differ depending on the file
            if (fontProperties.FontFamilyName == "Arial")
            {
                if (fontProperties.FontStyle == FontStyles.Italic && fontProperties.FontWeight == FontWeights.Bold)
                {
                    fontFileName = $"arialbi.ttf";
                }
                else if (fontProperties.FontStyle == FontStyles.Italic)
                {
                    fontFileName = $"ariali.ttf";
                }
                else if (fontProperties.FontWeight == FontWeights.Normal)
                {
                    fontFileName = "arial.ttf";
                }
                else if (fontProperties.FontWeight == FontWeights.Bold)
                {
                    fontFileName = $"arialbd.ttf";
                }
            }

            //...add more fonts if needed...

            DirectoryInfo directory = new DirectoryInfo(fontFolder);
            FileInfo[] fontFiles = directory.GetFiles();

            var fontFile = fontFiles.FirstOrDefault(f => f.Name.Equals(fontFileName, StringComparison.InvariantCultureIgnoreCase));
            if (fontFile != null)
            {
                var targetPath = fontFile.FullName;
                using (FileStream fileStream = File.OpenRead(targetPath))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }

            return null;
        }
    }
}
