using Microsoft.Build.Construction;
using System.Numerics;

namespace RegistrationWeb.Models
{
    /// <summary>
    /// The model contains all the data needed to create a report opject.
    /// </summary>
    public class ReportModel
    {
        public long TraineeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Track { get; set; }
        public int ReportNo { get; set; }
        public double DailyAttendance { get; set; }
        public double LateOrExit { get; set; }
        public double BreakTime { get; set; }
        public double Development { get; set; }
        public double TaskOnTime { get; set; }
        public double Excitement { get; set; }
        public double TaskAchievement { get; set; }
        public double TeamWork { get; set; }
        public double ImprovementSuggestions { get; set; }
        public double Passion { get; set; }
        public double Precision { get; set; }
        public double WorkApproved { get; set; }
        public double TotalScore { get; set; }

    }
}
