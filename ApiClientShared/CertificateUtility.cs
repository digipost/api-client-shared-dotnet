using System.Security.Cryptography.X509Certificates;
using ApiClientShared.Enums;

namespace ApiClientShared
{
    public class CertificateUtility
    {
        public static string EnglishErrorDescription = "Could not find certificate";
        public static string NorwegianErrorDescription = "Klarte ikke finne sertifikat";

        public static X509Certificate2 SenderCertificate(string thumbprint, Language errorMessageLanguage)
        {
            return new CertificateFactory().SenderCertificate(thumbprint, errorMessageLanguage);
        }

        public static X509Certificate2 ReceiverCertificate(string thumbprint, Language errorMessageLanguage)
        {
            return new CertificateFactory().ReceiverCertificate(thumbprint, errorMessageLanguage);
        }
    }
}