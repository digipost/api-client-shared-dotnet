using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Extensions;
using Digipost.Api.Client.Shared.Resources.Language;
using static Digipost.Api.Client.Shared.Resources.Language.LanguageResource;

namespace Digipost.Api.Client.Shared.Certificate
{
    public class CertificateChainValidator
    {
        public CertificateChainValidator(X509Certificate2Collection certificateStore)
        {
            CertificateStore = certificateStore;
        }

        public X509Certificate2Collection CertificateStore { get; set; }

        /// <summary>
        ///    Validates the certificate chain of the certificate, using the <see cref="CertificateStore" />.
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public bool IsValidChain(X509Certificate2 certificate)
        {
            return Validate(certificate).Type == CertificateValidationType.Valid;
        }

        /// <summary>
        ///    Validates the certificate chain of the certificate, using the <see cref="CertificateStore" />.
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="detailedErrorInformation">Status of chain validation if failed.</param>
        /// <returns></returns>
        public bool IsValidChain(X509Certificate2 certificate, out string detailedErrorInformation)
        {
            var result = Validate(certificate);
            detailedErrorInformation = result.Message;

            return result.Type == CertificateValidationType.Valid;
        }

        public CertificateValidationResult Validate(X509Certificate2 certificate)
        {
            var chain = BuildCertificateChain(certificate);

            var onlyUsingValidatorCertificatesResult = ValidateThatUsingOnlyValidatorCertificates(chain, certificate);

            return onlyUsingValidatorCertificatesResult.Type != CertificateValidationType.Valid
                ? onlyUsingValidatorCertificatesResult
                : Validate(certificate, chain);
        }

        private X509Chain BuildCertificateChain(X509Certificate2 sertifikat)
        {
            var chain = new X509Chain
            {
                ChainPolicy = ChainPolicy()
            };
            chain.Build(sertifikat);
            return chain;
        }

        private CertificateValidationResult ValidateThatUsingOnlyValidatorCertificates(X509Chain chain, X509Certificate2 certificate)
        {
            foreach (var chainElement in chain.ChainElements)
            {
                var isCertificateToValidate = IsSameCertificate(chainElement.Certificate, certificate);
                if (isCertificateToValidate)
                {
                    continue;
                }

                var isValidatorCertificate = CertificateStore.Cast<X509Certificate2>().Any(lagerSertifikat => IsSameCertificate(chainElement.Certificate, lagerSertifikat));
                if (isValidatorCertificate)
                {
                    continue;
                }

                var chainAsString = chain.ChainElements
                    .Cast<X509ChainElement>()
                    .Where(c => c.Certificate.Thumbprint != certificate.Thumbprint)
                    .Aggregate("", (result, curr) => GetCertificateInfo(result, curr.Certificate));

                var validatorCertificatesAsString = CertificateStore
                    .Cast<X509Certificate2>()
                    .Aggregate("", GetCertificateInfo);

                return UsedExternalCertificatesResult(certificate, chainAsString, validatorCertificatesAsString);
            }

            return ValidResult(certificate);
        }

        private static CertificateValidationResult UsedExternalCertificatesResult(X509Certificate2 certificate, string chainAsString, string validatorCertificatesAsString)
        {
            var externalCertificatesUsedMessage =
                string.Format(
                    GetResource(LanguageResourceKey.CertificateUsedExternalResult),
                    certificate.ToShortString(), chainAsString, validatorCertificatesAsString);

            return new CertificateValidationResult(CertificateValidationType.InvalidChain, externalCertificatesUsedMessage);
        }

        private static bool IsSameCertificate(X509Certificate2 certificate1, X509Certificate2 certificate2)
        {
            return certificate2.Thumbprint == certificate1.Thumbprint;
        }

        private static string GetCertificateInfo(string current, X509Certificate2 certificate)
        {
            return current + $"'{certificate.Subject}' {Environment.NewLine}";
        }

        public X509ChainPolicy ChainPolicy()
        {
            var policy = new X509ChainPolicy
            {
                RevocationMode = X509RevocationMode.NoCheck
            };

            policy.ExtraStore.AddRange(CertificateStore);

            return policy;
        }

        private static CertificateValidationResult Validate(X509Certificate2 certificate, X509Chain chain)
        {
            if (IsSelfSignedCertificate(chain))
            {
                return SelfSignedErrorResult(certificate);
            }

            var detailedErrorInformation = chain.ChainStatus;
            switch (detailedErrorInformation.Length)
            {
                case 0:
                    return ValidResult(certificate);
                case 1:
                    var chainError = detailedErrorInformation.ElementAt(0).Status;
                    return chainError == X509ChainStatusFlags.UntrustedRoot
                        ? ValidResult(certificate)
                        : InvalidChainResult(certificate, detailedErrorInformation); //We tolerate this 'UntrustedRoot' because it occurs when loading a root certificate from file, which is always done here. We trust the certificates as they are preloaded in library.
                default:
                    return InvalidChainResult(certificate, detailedErrorInformation);
            }
        }

        private static CertificateValidationResult InvalidChainResult(X509Certificate2 certificate, params X509ChainStatus[] x509ChainStatuses)
        {
            var invalidChainResult = string.Format(GetResource(LanguageResourceKey.CertificateInvalidChainResult), GetPrettyChainErrorStatuses(x509ChainStatuses));
            return new CertificateValidationResult(CertificateValidationType.InvalidChain, certificate.ToShortString(invalidChainResult));
        }

        private static CertificateValidationResult ValidResult(X509Certificate2 certificate)
        {
            var validChainResult = GetResource(LanguageResourceKey.CertificateValidResult);
            return new CertificateValidationResult(CertificateValidationType.Valid, certificate.ToShortString(validChainResult));
        }

        private static CertificateValidationResult SelfSignedErrorResult(X509Certificate2 certificate)
        {
            var selfSignedErrorResult = GetResource(LanguageResourceKey.CertificateSelfSignedErrorResult);
            return new CertificateValidationResult(CertificateValidationType.InvalidChain, certificate.ToShortString(selfSignedErrorResult));
        }

        private static string GetPrettyChainErrorStatuses(X509ChainStatus[] chainStatuses)
        {
            return chainStatuses.Aggregate("", (result, curr) => $"{curr.Status}: {curr.StatusInformation}");
        }

        private static bool IsSelfSignedCertificate(X509Chain chain)
        {
            const int selfSignedChainLength = 1;
            return chain.ChainElements.Count == selfSignedChainLength;
        }
    }
}