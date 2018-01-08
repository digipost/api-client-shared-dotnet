﻿using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Resources.Certificate;

namespace Digipost.Api.Client.Shared.Certificate
{
    public static class CertificateChainUtility
    {
        public static X509Certificate2Collection FunksjoneltTestmiljøSertifikater()
        {
            return new X509Certificate2Collection(CertificateResource.Chain.GetDifiTestChain().ToArray());
        }

        public static X509Certificate2Collection ProduksjonsSertifikater()
        {
            return new X509Certificate2Collection(CertificateResource.Chain.GetDifiProductionChain().ToArray());
        }
    }
}