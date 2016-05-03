using System;
using System.Management.Instrumentation;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared.Enums;

namespace ApiClientShared
{
    class CertificateFactory
    {
        internal virtual KeyStoreUtility KeyStoreUtil { get; }
        internal virtual BomUtility BomUtil { get; }

        public CertificateFactory()
        {
            KeyStoreUtil = new KeyStoreUtility();
            BomUtil = new BomUtility();
        }
        public CertificateFactory(KeyStoreUtility keyStoreUtility, BomUtility bomUtility = null)
        {
            KeyStoreUtil = keyStoreUtility;
            BomUtil = bomUtility ?? new BomUtility();
        }

        /// <summary>
        ///     Retrieves certificate from personal certificates (StoreName.My) from current user.
        /// </summary>
        /// <param name="thumbprint">The thumbprint of the certificate.</param>
        /// <param name="errorMessageLanguage">Specifies the error message language if certificate is not found.</param>
        /// <returns>The certifikcate</returns>
        public X509Certificate2 SenderCertificate(string thumbprint, Language errorMessageLanguage)
        {
            var storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            X509Certificate2 senderCertificate;

            thumbprint = BomUtil.RemoveBom(thumbprint);

            try
            {
                senderCertificate = KeyStoreUtil.FindCertificate(thumbprint, storeMy);
            }
            catch (Exception e)
            {
                throw new InstanceNotFoundException(GetErrorMessage(thumbprint, errorMessageLanguage), e);
            }

            return senderCertificate;
        }

        /// <summary>
        ///     Retrieves certificate from trusted People (StoreName.TrustedPeople) from current user.
        /// </summary>
        /// <param name="thumbprint">The thumbprint of the certificate.</param>
        /// <param name="errorMessageLanguage">Specifies the error message language if certificate is not found.</param>
        /// <returns>The certifikcate</returns>
        public X509Certificate2 ReceiverCertificate(string thumbprint, Language errorMessageLanguage)
        {
            var storeTrusted = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
            X509Certificate2 receiverCertificate;

            thumbprint = BomUtil.RemoveBom(thumbprint);
            try
            {
                receiverCertificate = KeyStoreUtil.FindCertificate(thumbprint, storeTrusted);
            }
            catch (Exception e)
            {
                throw new InstanceNotFoundException(GetErrorMessage(thumbprint, errorMessageLanguage), e);
            }
            return receiverCertificate;
        }

        private string GetErrorMessage(string thumbprint, Language language)
        {
            switch (language)
            {
                case Language.English:
                    return $"Could not find certificate with thumbprint: {thumbprint}";
                case Language.Norwegian:
                    return $"Klarte ikke finne sertifikat med thumbprint: {thumbprint}";

                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }
        
    }
}
