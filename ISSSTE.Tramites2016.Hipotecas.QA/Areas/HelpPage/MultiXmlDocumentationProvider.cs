#region

using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

#endregion

namespace ISSSTE.Tramites2016.Hipotecas.Areas.HelpPage
{
    public class MultiXmlDocumentationProvider : XmlDocumentationProvider
    {
        public MultiXmlDocumentationProvider(string xmlDocFilesPath)
        {
            XDocument finalDoc = null;
            foreach (var file in Directory.GetFiles(xmlDocFilesPath, "*.xml"))
            {
                using (var fileStream = File.OpenRead(file))
                {
                    if (finalDoc == null)
                    {
                        finalDoc = XDocument.Load(fileStream);
                    }
                    else
                    {
                        var xdocAdditional = XDocument.Load(fileStream);

                        finalDoc.Root.XPathSelectElement("/doc/members")
                            .Add(xdocAdditional.Root.XPathSelectElement("/doc/members").Elements());
                    }
                }
            }

            // Supply the navigator that rest of the XmlDocumentationProvider code looks for
            _documentNavigator = finalDoc.CreateNavigator();
        }
    }
}