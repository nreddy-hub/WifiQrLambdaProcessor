/// <summary>
/// Model matching the message sent from .NET API
/// This should match your FastAPIHomeWifiQR.Models.WifiQrCreatedMessage
/// </summary>
public class WifiQrCreatedMessage
{
    public Guid WifiId { get; set; }
    public string Ssid { get; set; } = string.Empty;
    public string Encryption { get; set; } = string.Empty;
    public bool Hidden { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public Dictionary<string, string>? Metadata { get; set; }
}