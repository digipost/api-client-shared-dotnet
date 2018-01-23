using System.Security.Cryptography.X509Certificates;
using static Digipost.Api.Client.Shared.Resources.Language.LanguageResource;

namespace Digipost.Api.Client.Shared.Extensions
{
    public static class X509Certificate2Extensions
    {
        public static string ToShortString(this X509Certificate2 certificate, string extraInfo = "")
        {
            return string.Format(CertificateShortDescription, certificate.Subject, certificate.Thumbprint, extraInfo);
        }
    }
}