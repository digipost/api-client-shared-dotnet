using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Resources.Language;

namespace Digipost.Api.Client.Shared.Extensions
{
    public static class X509Certificate2Extensions
    {
        public static string ToShortString(this X509Certificate2 certificate, string extraInfo = "")
        {
            var shortStringWithPlaceholders = LanguageResource.CertificateShortDescription;
    
            return string.Format(shortStringWithPlaceholders, certificate.Subject, certificate.Thumbprint, extraInfo);
        }
    }
}