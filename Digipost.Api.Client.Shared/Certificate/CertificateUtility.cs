using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Enums;

namespace Digipost.Api.Client.Shared.Certificate
{
    public class CertificateUtility
    {
        internal virtual KeyStoreUtility KeyStoreUtility { get; set; } = new KeyStoreUtility();

        internal virtual BomUtility BomUtility { get; set; } = new BomUtility();

        /// <summary>
        ///     Retrieves certificate from personal certificates (StoreName.My) from current user (StoreLocation.CurrentUser) or
        ///     local machine (StoreLocation.LocalMachine) identified by thumbprint.
        ///     A lookup is first performed on CurrentUser and then on LocalMachine.
        /// </summary>
        /// <param name="thumbprint">The thumbprint of the certificate.</param>
        /// <param name="errorMessageLanguage">Specifies the error message language if certificate is not found.</param>
        /// <returns>First certificate found or null</returns>
        public static X509Certificate2 SenderCertificate(string thumbprint, Language errorMessageLanguage)
        {
            return new CertificateUtility().CreateSenderCertificate(thumbprint, errorMessageLanguage);
        }

        /// <summary>
        ///     Retrieves certificate from trusted people (StoreName.TrustedPeople) from current user (StoreLocation.CurrentUser)
        ///     or local machine (StoreLocation.LocalMachine) identified by thumbprint.
        /// </summary>
        /// <param name="thumbprint">The thumbprint of the certificate.</param>
        /// <param name="errorMessageLanguage">Specifies the error message language if certificate is not found.</param>
        /// <returns>The certifikcate</returns>
        public static X509Certificate2 ReceiverCertificate(string thumbprint, Language errorMessageLanguage)
        {
            return new CertificateUtility().CreateReceiverCertificate(thumbprint, errorMessageLanguage);
        }

        internal X509Certificate2 CreateSenderCertificate(string thumbprint, Language errorMessageLanguage)
        {
            return GetFirstCertificateOrThrowException(thumbprint, StoreName.My, errorMessageLanguage);
        }

        internal X509Certificate2 CreateReceiverCertificate(string thumbprint, Language errorMessageLanguage)
        {
            return GetFirstCertificateOrThrowException(thumbprint, StoreName.TrustedPeople, errorMessageLanguage);
        }

        private X509Certificate2 GetFirstCertificateOrThrowException(string thumbprint, StoreName storeName, Language errorMessageLanguage)
        {
            thumbprint = BomUtility.RemoveBom(thumbprint);

            var stores = new List<X509Store>
            {
                new X509Store(storeName, StoreLocation.CurrentUser),
                new X509Store(storeName, StoreLocation.LocalMachine)
            };

            foreach (var store in stores)
            {
                var certificate = KeyStoreUtility.FindCertificate(thumbprint, store);

                if (certificate != null)
                {
                    return certificate;
                }
            }

            throw new InstanceNotFoundException(GetErrorMessage(thumbprint, errorMessageLanguage));
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