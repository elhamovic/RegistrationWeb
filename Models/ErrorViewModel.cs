namespace RegistrationWeb.Models
{
    /// <summary>
    /// The model contains all the data needed to responed to an error.
    /// </summary>
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        
    }
}