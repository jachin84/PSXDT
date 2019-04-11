using System;
using System.IO;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace PsXdt
{
    internal class XmlTransformer
    {
        private readonly IXmlTransformationLogger _logger = null;

        public XmlTransformer(IXmlTransformationLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool Transform(string sourcePath, string transformPath, string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new ArgumentNullException(nameof(sourcePath));
            }

            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new ArgumentNullException(nameof(transformPath));
            }

            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new ArgumentNullException(nameof(destinationPath));
            }

            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException("File to transform not found.", sourcePath);
            }

            if (!File.Exists(transformPath))
            {
                throw new FileNotFoundException("Transform file not found.", transformPath);
            }

            using (var document = new XmlTransformableDocument())
            using (var transformation = new XmlTransformation(transformPath, _logger))
            {
                using (XmlTextReader reader = new XmlTextReader(sourcePath))
                {
                    reader.DtdProcessing = DtdProcessing.Ignore;

                    document.PreserveWhitespace = true;
                    document.Load(reader);
                }

                var success = transformation.Apply(document);

                if (success)
                {
                    document.Save(destinationPath);
                }

                return success;
            }
        }
    }
}
