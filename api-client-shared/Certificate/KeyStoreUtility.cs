using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Digipost.Api.Client.Shared.Certificate
{
    public class KeyStoreUtility //Todo: Var internal, og bør nok være det videre
    {
        public virtual X509Certificate2 FindCertificate(string thumbprint, X509Store store) 
        {
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var x509Certificate2Collection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true);
                var certificates = x509Certificate2Collection.Cast<X509Certificate2>().ToList();

                return certificates.FirstOrDefault();
            }
            finally
            {
                store.Close();
            }
        }
    }
}