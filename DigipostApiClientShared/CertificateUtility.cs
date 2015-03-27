using System;
using System.Management.Instrumentation;
using System.Security.Cryptography.X509Certificates;
using DigipostApiClientShared.Enums;

namespace DigipostApiClientShared
{
    public class CertificateUtility
    {
        public static string EnglishErrorDescription = "Could not find certificate";
        public static string NorwegianErrorDescription = "Klarte ikke finne sertifikat";

        private static string GetErrorMessage(string thumbprint, Language language)
        {
            switch (language)
            {
                case Language.English:
                    return String.Format("Could not find certificate with thumbprint: {0}",thumbprint);
                case Language.Norwegian:
                    return String.Format("Klarte ikke finne sertifikat med thumbprint: {0}", thumbprint);

                default:
                    throw new ArgumentOutOfRangeException("language");
            }
        }
        
        /// <summary>
        /// Retrieves certificate from personal certificates (StoreName.My) from current user.
        /// </summary>
        /// <param name="thumbprint">The thumbprint of the certificate.</param>
        /// <param name="errorMessageLanguage">Specifies the error message language if certificate is not found.</param>
        /// <returns>The certifikcate</returns>
        public static X509Certificate2 SenderCertificate(string thumbprint, Language errorMessageLanguage)
        {
            X509Store storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            X509Certificate2 tekniskAvsenderSertifikat;
            try
            {
                storeMy.Open(OpenFlags.ReadOnly);
                tekniskAvsenderSertifikat = storeMy.Certificates.Find(
                    X509FindType.FindByThumbprint, thumbprint, true)[0];
            }
            catch (Exception e)
            {
                throw new InstanceNotFoundException(GetErrorMessage(thumbprint,errorMessageLanguage), e);
            }
            storeMy.Close();
            return tekniskAvsenderSertifikat;
        }

        /// <summary>
        /// Retrieves certificate from trusted People (StoreName.TrustedPeople) from current user.
        /// </summary>
        /// <param name="thumbprint">The thumbprint of the certificate.</param>
        /// <param name="errorMessageLanguage">Specifies the error message language if certificate is not found.</param>
        /// <returns>The certifikcate</returns>
        public static X509Certificate2 ReceiverCertificate(string thumbprint, Language errorMessageLanguage)
        {
            var storeTrusted = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
            X509Certificate2 mottakerSertifikat;
            try
            {
                storeTrusted.Open(OpenFlags.ReadOnly);
                mottakerSertifikat =
                    storeTrusted.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true)[0];
            }
            catch (Exception e)
            {
                throw new InstanceNotFoundException(GetErrorMessage(thumbprint,errorMessageLanguage), e);
            }
            storeTrusted.Close();
            return mottakerSertifikat;
        }


    }
}
