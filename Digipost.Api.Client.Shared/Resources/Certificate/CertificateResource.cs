using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Digipost.Api.Client.Shared.Resources.Certificate
{
    internal static class CertificateResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Resources.Certificate.Data");

        private static X509Certificate2 GetCertificate(params string[] path)
        {
            byte[] cert = ResourceUtility.ReadAllBytes(path);
            return new X509Certificate2(cert, "", X509KeyStorageFlags.Exportable);
        }

        public static class UnitTests
        {
            public static X509Certificate2 GetProduksjonsMottakerSertifikatOppslagstjenesten()
            {
                return GetCertificate("UnitTests", "produksjonsmottakersertifikatFraOppslagstjenesten.pem");
            }

            public static X509Certificate2 GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten()
            {
                return GetCertificate("UnitTests", "testmottakersertifikatFraOppslagstjenesten.pem");
            }

            public static X509Certificate2 NotActivatedSelfSignedTestCertificate()
            {
                return GetCertificate("UnitTests", "NotActivatedSelfSignedBringAs.cer");
            }

            public static X509Certificate2 GetExpiredSelfSignedTestCertificate()
            {
                return GetCertificate("UnitTests", "ExpiredSelfSignedBringAs.cer");
            }

            public static X509Certificate2 GetValidSelfSignedTestCertificate()
            {
                return GetCertificate("UnitTests", "ValidSelfSignedBringAs.cer");
            }

            public static X509Certificate2 TestIntegrasjonssertifikat()
            {
                return GetPostenTestCertificate();
            }

            public static X509Certificate2 GetEnhetstesterSelvsignertSertifikat()
            {
                return GetCertificate("UnitTests", "difi-enhetstester.cer");
            }

            public static X509Certificate2 GetPostenTestCertificate()
            {
                return GetFunksjoneltTestmiljøMottakerSertifikatOppslagstjenesten();
            }

            public static X509Certificate2 Seid2TestSertifikat()
            {
                return GetCertificate("UnitTests", "seid2.cer");
            }
        }

        public static class Chain
        {
            public static List<X509Certificate2> GetDifiTestChain()
            {
                return new List<X509Certificate2>
                {
                    new X509Certificate2(GetCertificate("TestChain", "Buypass_Class_3_Test4_CA_3.pem")),
                    new X509Certificate2(GetCertificate("TestChain", "Buypass_Class_3_Test4_Root_CA.pem")),
                    new X509Certificate2(GetCertificate("TestChain", "intermediate - commfides cpn enterprise-norwegian sha256 ca - test2.pem")),
                    new X509Certificate2(GetCertificate("TestChain", "root - cpn root sha256 ca - test.pem")),
                    new X509Certificate2(GetCertificate("TestChain", "BPCl3CaG2HTBS.cer")),
                    new X509Certificate2(GetCertificate("TestChain", "BPCl3CaG2STBS.cer")),
                    new X509Certificate2(GetCertificate("TestChain", "BPCl3RootCaG2HT.cer")),
                    new X509Certificate2(GetCertificate("TestChain", "BPCl3RootCaG2ST.cer")),
                    new X509Certificate2(GetCertificate("TestChain", "CommfidesLegalPersonCA-G3-TEST.cer")),
                    new X509Certificate2(GetCertificate("TestChain", "CommfidesRootCA-G3-TEST.cer")),
                };
            }

            public static List<X509Certificate2> GetDifiProductionChain()
            {
                return new List<X509Certificate2>
                {
                    new X509Certificate2(GetCertificate("ProdChain", "BPClass3CA3.cer")),
                    new X509Certificate2(GetCertificate("ProdChain", "BPClass3RootCA.cer")),
                    new X509Certificate2(GetCertificate("ProdChain", "cpn enterprise sha256 class 3.crt")),
                    new X509Certificate2(GetCertificate("ProdChain", "cpn rootca sha256 class 3.crt")),
                    new X509Certificate2(GetCertificate("ProdChain", "BPCl3CaG2HTBS.cer")),
                    new X509Certificate2(GetCertificate("ProdChain", "BPCl3CaG2STBS.cer")),
                    new X509Certificate2(GetCertificate("ProdChain", "BPCl3RootCaG2HT.cer")),
                    new X509Certificate2(GetCertificate("ProdChain", "BPCl3RootCaG2ST.cer")),
                    new X509Certificate2(GetCertificate("ProdChain", "CommfidesLegalPersonCA-G3.cer")),
                    new X509Certificate2(GetCertificate("ProdChain", "CommfidesRootCA-G3.cer")),
                };
            }
        }
    }
}
