using System;
using System.Reflection;
using System.Resources;

namespace Digipost.Api.Client.Shared.Resources.Language
{
    public static class LanguageResource
    {
        public static string CertificateCouldNotFind = "Could not find certificate with thumbprint {0:certificateThumbrint}";
        public static string CertificateExpiredResult = "expired on {0:expirationDateString}.";
        public static string CertificateInvalidChainResult = "has the following certificate chain errors: {0:prettyChainErrorStatuses}";
        public static string CertificateIsNull = "The certificate is null! Please check that the certificate is loaded correctly.";
        public static string CertificateNotActivatedResult = "is not active until {0:effectiveDateString}";
        public static string CertificateNotIssuedToOrganization = "is not issued to organization number '{0:certificateOrganizationNumber}'. This occurs if the certificate is issued to a different organization or if it isn't an organizational certificate. An organizational certificate can be aquired from Buypass or Commfides.";
        public static string CertificateSelfSignedErrorResult = "is invalid because the chain length is 1. This means that the certificate is self signed. An organizational certificate issued by a valid certificate issuer is required.  An organizational certificate can be aquired from Buypass or Commfides";
        public static string CertificateShortDescription = "Certificate with Subject '{0:certificateSubject}' and Thumbprint '{1:certificateThumbprint}' {2:certificateExtraInfo}";
        public static string CertificateUsedExternalResult = "Validation of {0:certificateShortDescription} failed." +
                                               "This happened because the chain was built with the following certificates: " +
                                               "{1:chainAsString}, but only the following are allowed to build the chain: " +
                                               "2:validatorCertificatesAsString}. This usually happens if the certificate is retrieved from Windows Certificate Store and this is not allowed during validation. The chain can only be built using the validator certificates.";
        public static string CertificateValidResult = "is a valid certificate.";
        public static string ToleratedPrefixListErrorEnUs = "The 'PrefixList' attribute is invalid - The value '' is invalid according to its datatype 'http://www.w3.org/2001/XMLSchema:NMTOKENS' - The attribute value cannot be empty.";
        public static string ToleratedXsdIdErrorEnUs = "It is an error if there is a member of the attribute uses of a type definition with type xs:ID or derived from xs:ID and another attribute with type xs:ID matches an attribute wildcard.";
        public static string ToleratedPrefixListErrorNbNo = "Attributtet PrefixList er ugyldig - Verdien  er ugyldig i henhold til datatypen http://www.w3.org/2001/XMLSchema:NMTOKENS - Attributtverdien kan ikke være tom.";
        public static string ToleratedXsdIdErrorNbNo = "Det er en feil hvis det finnes et medlem av attributtet som bruker en typedefinisjon med typen xs:ID eller avledet fra xs:ID og et annet attributt med typen xs:ID tilsvarer et attributtjokertegn.";
        
    }
}