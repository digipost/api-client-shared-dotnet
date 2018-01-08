namespace Digipost.Api.Client.Shared.Certificate
{
    public class CertificateValidationResult
    {
        public CertificateValidationResult(CertificateValidationType type, string message)
        {
            Type = type;
            Message = message;
        }

        public CertificateValidationType Type { get; set; }

        public string Message { get; set; }
    }
}