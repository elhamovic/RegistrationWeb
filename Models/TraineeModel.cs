using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace RegistrationWeb.Models
{
    /// <summary>
    /// The model contains all the data needed to create a Trainee opject.
    /// </summary>
    public class TraineeModel
    {
        [Required(ErrorMessage = "الاسم الأول إلزامي")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "الاسم الثاني إلزامي")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "البريد إلزامي")]
        [EmailAddress(ErrorMessage = "البريد غير صحيح")]
        public string Email { get; set; }
        [Required(ErrorMessage = "رقم الجوال إلزامي")]
        [RegularExpression(@"[0]{1}[5]{1}[0-9]{8}", ErrorMessage = "تحقق من رقم الجوال")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "حقل الجنسية إلزامي")]
        public string Nationality { get; set; }
        [Required(ErrorMessage = "رقم الهوية إلزامي")]
        [RegularExpression(@"[1]{1}[0-9]{9}", ErrorMessage = "تحقق من رقم الهوية")]
        public long NationalID { get; set; }
        [Required(ErrorMessage = "تاريخ الميلاد إلزامي")]
        public string DateOfBirth { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string Region { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string City { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string University { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string GraduationYear { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string Degree { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string Major { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string GPAScale { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string GPA { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string ExpYears { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string EmployeeStatus { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string EnglishLevel { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string RegistrationReason { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string FreementStaus { get; set; }
        [Required(ErrorMessage = "هذا الملف  إلزامي")]
        public IFormFile Certificate { get; set; }
        [Required(ErrorMessage = "هذا الملف  إلزامي")]
        public IFormFile CV { get; set; }
        [Required(ErrorMessage = "هذا الملف  إلزامي")]
        public List<IFormFile> PreviosWork { get; set; }
        [Required(ErrorMessage = "هذا الملف  إلزامي")]
        public IFormFile IntroVideo { get; set; }
        [Required(ErrorMessage = "هذا الملف  إلزامي")]
        public IFormFile Reports { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string Status { get; set; }
        [Required(ErrorMessage = "هذا الحقل  إلزامي")]
        public string Track { get; set; }
    }
}
