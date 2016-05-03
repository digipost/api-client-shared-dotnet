using System.Security.Cryptography.X509Certificates;
using ApiClientShared.Enums;

namespace ApiClientShared
{
    public class CertificateUtility
    {

        /// <summary>
        ///     Retrieves certificate from personal certificates (StoreName.My) from current user.
        /// </summary>
        /// <param name="thumbprint">The thumbprint of the certificate.</param>
        /// <param name="errorMessageLanguage">Specifies the error message language if certificate is not found.</param>
        /// <returns>The certifikcate</returns>
        public static X509Certificate2 SenderCertificate(string thumbprint, Language errorMessageLanguage)
        {
            return new CertificateFactory().SenderCertificate(thumbprint, errorMessageLanguage);
        }
        /// <summary>
        ///     Retrieves certificate from trusted People (StoreName.TrustedPeople) from current user.
        /// </summary>
        /// <param name="thumbprint">The thumbprint of the certificate.</param>
        /// <param name="errorMessageLanguage">Specifies the error message language if certificate is not found.</param>
        /// <returns>The certifikcate</returns>
        public static X509Certificate2 ReceiverCertificate(string thumbprint, Language errorMessageLanguage)
        {
            return new CertificateFactory().ReceiverCertificate(thumbprint, errorMessageLanguage);
        }
    }
}