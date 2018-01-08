using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Digipost.Api.Client.Shared.Resources.Language;

namespace Digipost.Api.Client.Shared.Xml
{
    public class XmlValidationRunner //Todo: Var internal, og bør nok være det videre
    {
        internal static readonly List<string> ToleratedErrors = new List<string>
        {
            LanguageResource.ToleratedXsdIdErrorNbNo,
            LanguageResource.ToleratedXsdIdErrorEnUs,
            LanguageResource.ToleratedPrefixListErrorNbNo,
            LanguageResource.ToleratedPrefixListErrorEnUs,
        };

        public XmlValidationRunner(XmlSchemaSet xmlSchemaSet) //Todo: Var internal, og bør nok være det videre
        {
            XmlSchemaSet = xmlSchemaSet;
        }

        public XmlSchemaSet XmlSchemaSet { get; } //Todo: Var internal, og bør nok være det videre

        public ValidationMessages ValidationMessages { get; } = new ValidationMessages(); //Todo: Var internal, og bør nok være det videre

        public bool Validate(string document) ////Todo: Var internal, og bør nok være det videre
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(XmlSchemaSet);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;

            var xmlReader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(document)), settings);

            while (xmlReader.Read())
            {
            }

            return !ValidationMessages.HasErrors && !ValidationMessages.HasWarnings;
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (!IsToleratedError(e))
            {
                ValidationMessages.Add(e.Severity, e.Message);
            }
        }

        private static bool IsToleratedError(ValidationEventArgs e)
        {
            return ToleratedErrors.Contains(e.Message);
        }
    }
}