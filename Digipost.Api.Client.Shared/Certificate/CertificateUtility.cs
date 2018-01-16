using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static System.Security.Cryptography.X509Certificates.StoreLocation;
using static Digipost.Api.Client.Shared.Resources.Language.LanguageResource;

[assembly:InternalsVisibleTo("Digipost.Api.Client.Shared.Tests")]

namespace Digipost.Api.Client.Shared.Certificate
{
    public class CertificateUtility
    {
        internal KeyStoreUtility KeyStoreUtility { get; set; } = new KeyStoreUtility();

        internal BomUtility BomUtility { get; set; } = new BomUtility();

        /// <summary>
        ///     Retrieves certificate from personal certificates (StoreName.My) from current user (StoreLocation.CurrentUser) or
        ///     local machine (StoreLocation.LocalMachine) identified by thumbprint.
        ///     A lookup is first performed on CurrentUser and then on LocalMachine.
        /// </summary>
        /// <param name="thumbprint">The thumbprint of the certificate.</param>
        /// <returns>First certificate found or null</returns>
        public static X509Certificate2 SenderCertificate(string thumbprint)
        {
            return new CertificateUtility().CreateSenderCertificate(thumbprint);
        }

        /// <summary>
        ///     Retrieves certificate from trusted people (StoreName.TrustedPeople) from current user (StoreLocation.CurrentUser)
        ///     or local machine (StoreLocation.LocalMachine) identified by thumbprint.
        /// </summary>
        /// <param name="thumbprint">The thumbprint of the certificate.</param>
        /// <returns>The certifikcate</returns>
        public static X509Certificate2 ReceiverCertificate(string thumbprint)
        {
            return new CertificateUtility().CreateReceiverCertificate(thumbprint);
        }

        internal X509Certificate2 CreateSenderCertificate(string thumbprint)
        {
            return GetFirstCertificateOrThrowException(thumbprint, StoreName.My);
        }

        internal X509Certificate2 CreateReceiverCertificate(string thumbprint)
        {
            return GetFirstCertificateOrThrowException(thumbprint, StoreName.TrustedPeople);
        }

        private X509Certificate2 GetFirstCertificateOrThrowException(string thumbprint, StoreName storeName)
        {
            thumbprint = BomUtility.RemoveBom(thumbprint);

            var stores = new List<X509Store>
            {
                new X509Store(storeName, CurrentUser),
                new X509Store(storeName, LocalMachine)
            };

            foreach (var store in stores)
            {
                var certificate = KeyStoreUtility.FindCertificate(thumbprint, store);

                if (certificate != null)
                {
                    return certificate;
                }
            }

            throw new FileNotFoundException(GetErrorMessage(thumbprint));
        }

        private static string GetErrorMessage(string thumbprint)
        {
            return string.Format(CertificateCouldNotFind, thumbprint);
        }
    }
}