using System.Security.Cryptography.X509Certificates;

namespace ApiClientShared
{
    internal class KeyStoreUtility
    {
        internal virtual X509Certificate2 FindCertificate(string thumbprint, X509Store storeMy)
        {
            try
            {
                storeMy.Open(OpenFlags.ReadOnly);
                return storeMy.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true)[0];
            }
            finally
            {
                storeMy.Close();
            }
        }
    }
}