using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using OpenXmlPowerTools;
using Segurplan.Core.Actions.AllDocuments.Models;
using Segurplan.Core.Actions.AllDocuments.Models.DocumentDtos;

namespace Segurplan.Core.Domain.Documents {
    public class DocumentProcessor {
        private const string WordDocumentMediaType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        private const string HtmlPropertyNameLastChars = "Html";
        private const string RootPropertyName = "Root";

        private readonly IHtmlStringCleaner htmlStringCleaner;

        public DocumentProcessor(IHtmlStringCleaner htmlStringCleaner) {
            this.htmlStringCleaner = htmlStringCleaner;
        }

        public async Task<ProcesedDocument> ProcessDocument(DocumentContent templateContent, byte[] template, string outputFileName) {
            using (var xmlStream = new MemoryStream()) {
                using (var fileStream = new MemoryStream()) {
                    var serializer = new XmlSerializer(typeof(DocumentContent));

                    using (var writer = XmlWriter.Create(xmlStream, new XmlWriterSettings() { Indent = false, NewLineHandling = NewLineHandling.None, CloseOutput = false })) {
                        serializer.Serialize(writer, templateContent);
                    }

                    xmlStream.Position = 0;

                    await fileStream.WriteAsync(template, 0, template.Length);

                    var wmlDocument = new WmlDocument(outputFileName, fileStream.ToArray());
                    var element = XElement.Load(xmlStream);

                    wmlDocument = HtmlElementToXElementRecursive(element, GetHtmlPropetiesDictionaryRecursive(templateContent, nameof(DocumentContent), RootPropertyName), wmlDocument);

                    var outputDocument = DocumentAssembler.AssembleDocument(wmlDocument, element, out var templateError);

                    var responseStream = new MemoryStream();

                    outputDocument.WriteByteArray(responseStream);
                    responseStream.Position = 0;

                    return new ProcesedDocument(responseStream, outputFileName, WordDocumentMediaType);
                }
            }
        }

        private HtmlToWmlConverterSettings GetCustomSettings() {
            var defaultSettings = HtmlToWmlConverter.GetDefaultSettings();
            defaultSettings.SectPr.RemoveAll();
            return defaultSettings;
        }

        /// <summary>
        /// Map html string properties from ReflectedListHtmlDto to WmlDocument, for the lists it calls automatically for each reflectedEntity.Sons to this method
        /// </summary>
        /// <param name="element">XElement of the object from which it was obtained by reflection reflectedEntity.</param>
        /// <param name="reflectedEntity">Html properties obtained by reflection.</param>
        /// <param name="wmlDocument">Word file document, only on first level call.</param>
        /// <returns></returns>
        private WmlDocument HtmlElementToXElementRecursive(XElement element, ReflectedListHtmlDto reflectedEntity, WmlDocument wmlDocument = null) {
            foreach (var propertyValue in reflectedEntity.PropertyValues) {
                var htmlDocument = HtmlToWmlConverter.ConvertHtmlToWml(
                            "",
                            "",
                            "",
                            XElement.Parse("<html><body>" + htmlStringCleaner.Sanitize(propertyValue.Value, propertyValue.Key) + "</body></html>"),
                            GetCustomSettings());

                var wmlFilteredData = htmlDocument.MainDocumentPart.GetXmlNode().FirstChild.FirstChild.GetXElement();
                if (reflectedEntity.PropertyName == RootPropertyName) {
                    element.Descendants(propertyValue.Key).Single().Add(wmlFilteredData.Elements().Where(x => x.Name.LocalName != "sectPr"));
                    // wmlDocument = WriteImageParts(wmlDocument, htmlDocument, propertyValue.Key == "AnagramaHtml");

                } else {
                    element.Descendants(reflectedEntity.TypeName)
                        .Elements(reflectedEntity.PropertyName)
                        .First(x => x.Element(propertyValue.Key).Value == propertyValue.Value)
                        .Element(propertyValue.Key)
                        .ReplaceNodes(wmlFilteredData);
                }
                wmlDocument = WriteImageParts(wmlDocument, htmlDocument, propertyValue.Key == "AnagramaHtml");
            }

            foreach (var son in reflectedEntity.Sons)
                wmlDocument = HtmlElementToXElementRecursive(element, son, wmlDocument);

            //   if (reflectedEntity.PropertyName == RootPropertyName)
            return wmlDocument;
            // solo se necesita el return en el nivel del root
            // return null;

        }

        /// <summary>
        /// Create a ReflectedListHtmlDto from generic object parameter (templateContent), use reflection to go through the properties.
        /// Gets the properties that HtmlPropertyNameLastChars has as the end of the name.
        /// If it finds a property with a value of string type it puts it in the ReflectedListHtmlDto.PropertyValues.
        /// If it finds a property with a value of type list it calls to this same method once per element in the list and in the return it adds it in ReflectedListHtmlDto.Sons.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="templateContent">Generic object to map</param>
        /// <param name="typeName">Name of the type as a string to facilitate the filters, you only have to pass it in the first call, for the collection property types it is entered automatically</param>
        /// <param name="propertyName">Name of the property as a string to facilitate the filters, you only have to pass it in the first call, for the collection properties it is entered automatically</param>
        /// <returns>A ReflectedListHtmlDto representation of templateContent</returns>
        private ReflectedListHtmlDto GetHtmlPropetiesDictionaryRecursive<T>(T templateContent, string typeName, string propertyName) {
            var reflectedRootValues = new ReflectedListHtmlDto(typeName, propertyName);
            var htmlProps = templateContent.GetType()
                .GetProperties()
                .Where(p => p.Name.EndsWith(HtmlPropertyNameLastChars))
                .ToList();

            foreach (var property in htmlProps) {
                if (property.PropertyType.Namespace == "System.Collections.Generic") {
                    var collection = (IEnumerable)property.GetValue(templateContent);

                    foreach (var item in collection)
                        reflectedRootValues.Sons.Add(GetHtmlPropetiesDictionaryRecursive(item, property.Name, property.PropertyType.GenericTypeArguments[0].Name));

                } else {
                    if (string.IsNullOrEmpty((string)property.GetValue(templateContent)))
                        continue;

                    reflectedRootValues.PropertyValues.Add(property.Name, (string)property.GetValue(templateContent) ?? string.Empty);
                }
            }
            return reflectedRootValues;
        }

        private WmlDocument WriteImageParts(WmlDocument wmlDocument, WmlDocument htmlDocument, bool isHeader) {
            using (var memoryHtmlDocument = new OpenXmlMemoryStreamDocument(htmlDocument)) {
                using (var wordHtmlDocument = memoryHtmlDocument.GetWordprocessingDocument()) {
                    using (var memoryOutputDocument = new OpenXmlMemoryStreamDocument(wmlDocument)) {
                        using (var wordOutputDocument = memoryOutputDocument.GetWordprocessingDocument()) {
                            foreach (var imagePart in wordHtmlDocument.MainDocumentPart.ImageParts) {
                                var id = wordHtmlDocument.MainDocumentPart.GetIdOfPart(imagePart);
                                if (isHeader) {
                                    foreach (var item in wordOutputDocument.MainDocumentPart.HeaderParts) {
                                        item.AddPart(imagePart, id);
                                    }
                                } else {
                                    wordOutputDocument.MainDocumentPart.AddPart(imagePart, id);
                                }
                            }
                        }
                        wmlDocument = memoryOutputDocument.GetModifiedWmlDocument();
                    }
                }
            }
            return wmlDocument;
        }
    }
}
