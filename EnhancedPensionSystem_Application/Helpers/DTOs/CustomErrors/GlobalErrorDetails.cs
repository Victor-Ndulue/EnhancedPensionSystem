namespace EnhancedPensionSystem_Application.Helpers.DTOs.CustomErrors;


public class GlobalErrorDetails
{
    public bool Status { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
    public DateTime? RequestDate { get; set; } = DateTime.UtcNow;
}
