namespace RegistrationWeb.Models
{
    /// <summary>
    /// The model contains all the data needed to create a document opject.
    /// </summary>
    public class DocumentModel
    {
        public string FileType { get; set; }
        public byte[] Data { get; set; }
        public string Extention { get; set; }
        public string FileName { get; set; }
    }
}
