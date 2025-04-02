using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;

namespace ConsoleApp1
{
    public class Program
    {
        public static string xmlURL = "https://brendant-t.github.io/Assignment4CSE445/Hotels.xml"; // Replace with your actual URL
        public static string xmlErrorURL = "https://brendant-t.github.io/Assignment4CSE445/HotelsErrors.xml"; // Replace with your actual URL
        public static string xsdURL = "https://brendant-t.github.io/Assignment4CSE445/Hotels.xsd"; // Replace with your actual URL

        public static void Main(string[] args)
        {
            // Test verification with valid XML
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            // Test verification with error XML
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            // Test XML to JSON conversion
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1 - XML Validation against XSD
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                // Set the validation settings
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, XmlReader.Create(xsdUrl));
                settings.ValidationEventHandler += (sender, e) =>
                {
                    throw e.Exception; // Throw exception when validation fails
                };

                // Validate the XML
                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read()) { } // Read through the document to trigger validation
                }

                return "No Error";
            }
            catch (XmlSchemaValidationException ex)
            {
                return $"Validation Error: {ex.Message} (Line: {ex.LineNumber}, Position: {ex.LinePosition})";
            }
            catch (XmlException ex)
            {
                return $"XML Error: {ex.Message} (Line: {ex.LineNumber}, Position: {ex.LinePosition})";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // Q2.2 - Convert XML to JSON
        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                // Load the XML document
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlUrl);

                // Convert to JSON
                string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);

                return jsonText;
            }
            catch (Exception ex)
            {
                return $"Error converting XML to JSON: {ex.Message}";
            }
        }
    }
}