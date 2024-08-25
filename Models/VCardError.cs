namespace VCardQRGenerator.Models
{
    public class VCardError
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
