using Newtonsoft.Json.Linq;
using NuGet.Common;
using RegistrationWeb.Models;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RegistrationWeb.Services
{
    /// <summary>
    /// This Class manages the communication with the HAWK Database
    /// </summary>
    public class UsersDAO
    {
        List<DocumentModel> documents;
        List<ReportModel> reports;
        List<TraineeModel> trainees;
        ////////////////////////////////PLEASE CHANGE THE DATABASE CONNECTION STRING BELOW TO THE STRING FROM THE DATABASE USED IN THE HOSTING.////////////////////////////////
        string connectionString = @"Data Source=DESKTOP-6L8H12A\SFEXPRESS;Initial Catalog=InternshipProgram;User ID=smartface;Password=smartface;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        /// <summary>
        /// Login Checks if the user credintials exsists in the system database or not.
        /// </summary>
        /// <param name="user"></param>
        /// <returns> User if found, null if not found</returns>
        public UserModel Login(UserModel user)
        {
            string sqlStatment = "SELECT * FROM [dbo].[Users] WHERE Username = @username AND Password = @password";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, -1).Value = user.Username;
                command.Parameters.Add("@password", System.Data.SqlDbType.VarChar, -1).Value = user.Password;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        user.Role = (string)(reader["Role"]);
                        if (user.Role != "Admin")
                        {
                            user.Track = (string)(reader["Track"]);
                        }
                        return user;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            return null;
        }
        /// <summary>
        /// Registers the trainee information to the database.
        /// </summary>
        /// <param name="trainee"></param>
        /// <returns>true if inserted correctly, false if not inserted</returns>
        public bool TraineeRegInfo(TraineeModel trainee)
        {
            string sqlStatment = "INSERT INTO [dbo].[TraineeInfo] VALUES (@firstname ,@lastname, @email ,@phone ,@nationality,@ID,@DOB, @gender, @region, @city, @degree, @major, @gpascale, @gpa, @expyears, @empstat, @englishlevel, @regreason, @freestatus, @status, @track, @University,@GraduationYear)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@firstname", System.Data.SqlDbType.NVarChar, -1).Value = trainee.FirstName;
                command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar, -1).Value = trainee.LastName;
                command.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, -1).Value = trainee.Email;
                command.Parameters.Add("@phone", System.Data.SqlDbType.NVarChar, -1).Value = trainee.PhoneNumber;
                command.Parameters.Add("@nationality", System.Data.SqlDbType.NVarChar, -1).Value = trainee.Nationality;
                command.Parameters.Add("@ID", System.Data.SqlDbType.BigInt).Value = trainee.NationalID;
                command.Parameters.Add("@DOB", System.Data.SqlDbType.NVarChar, -1).Value = trainee.DateOfBirth;
                command.Parameters.Add("@gender", System.Data.SqlDbType.NVarChar, -1).Value = trainee.Gender;
                command.Parameters.Add("@region", System.Data.SqlDbType.NVarChar, -1).Value = trainee.Region;
                command.Parameters.Add("@city", System.Data.SqlDbType.NVarChar, -1).Value = trainee.City;
                command.Parameters.Add("@degree", System.Data.SqlDbType.NVarChar, -1).Value = trainee.Degree;
                command.Parameters.Add("@major", System.Data.SqlDbType.NVarChar, -1).Value = trainee.Major;
                command.Parameters.Add("@gpascale", System.Data.SqlDbType.NVarChar, -1).Value = trainee.GPAScale;
                command.Parameters.Add("@gpa", System.Data.SqlDbType.NVarChar, -1).Value = trainee.GPA;
                command.Parameters.Add("@expyears", System.Data.SqlDbType.NVarChar, -1).Value = trainee.ExpYears;
                command.Parameters.Add("@empstat", System.Data.SqlDbType.NVarChar, -1).Value = trainee.EmployeeStatus;
                command.Parameters.Add("@englishlevel", System.Data.SqlDbType.NVarChar, -1).Value = trainee.EnglishLevel;
                command.Parameters.Add("@regreason", System.Data.SqlDbType.NVarChar, -1).Value = trainee.RegistrationReason;
                command.Parameters.Add("@freestatus", System.Data.SqlDbType.NVarChar, -1).Value = trainee.FreementStaus;
                command.Parameters.Add("@status", System.Data.SqlDbType.NVarChar, -1).Value = trainee.Status;
                command.Parameters.Add("@track", System.Data.SqlDbType.NVarChar, -1).Value = trainee.Track;
                command.Parameters.Add("@University", System.Data.SqlDbType.NVarChar, -1).Value = trainee.University;
                command.Parameters.Add("@GraduationYear", System.Data.SqlDbType.NVarChar, -1).Value = trainee.GraduationYear;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }

        }
        /// <summary>
        /// Registers the trainee registeration Files to the database.
        /// </summary>
        /// <param name="trainee"></param>
        /// <param name="document"></param>
        /// <returns>true if inserted correctly, false if not inserted</returns>
        public bool TraineeRegFiles(TraineeModel trainee, DocumentModel document)
        {
            string sqlStatment = "INSERT INTO[dbo].[RegistrationFiles] VALUES (@TraineeId, @FileType, @Data, @Extention, @FileName)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@TraineeId", System.Data.SqlDbType.BigInt, -1).Value = trainee.NationalID;
                command.Parameters.Add("@FileType", System.Data.SqlDbType.VarChar, -1).Value = document.FileType;
                command.Parameters.Add("@Data", System.Data.SqlDbType.VarBinary).Value = document.Data;
                command.Parameters.Add("@Extention", System.Data.SqlDbType.Char, 10).Value = document.Extention;
                command.Parameters.Add("@FileName", System.Data.SqlDbType.NVarChar, -1).Value = document.FileName;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;

                }

            }
        }
        /// <summary>
        /// insertes the reports filed by the supervisor to the database.
        /// </summary>
        /// <param name="report"></param>
        /// <returns>true if inserted correctly, false if not inserted</returns>
        public bool AddReport(ReportModel report)
        {
            string sqlStatment = "INSERT INTO[dbo].[Reports] VALUES (@TraineeID, @ReportNo, @DailyAttendance, @LateOrExit, @BreakTime, @Development, @TaskOnTime, @Excitement, @TaskAchievement, @TeamWork, @ImprovementSuggestions, @Passion, @Precision, @WorkApproved, @FirstName, @LastName, @Track, @Total)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@TraineeID", System.Data.SqlDbType.BigInt).Value = report.TraineeID;
                command.Parameters.Add("@ReportNo", System.Data.SqlDbType.Int).Value = report.ReportNo;
                command.Parameters.Add("@DailyAttendance", System.Data.SqlDbType.Float).Value = report.DailyAttendance;
                command.Parameters.Add("@LateOrExit", System.Data.SqlDbType.Float).Value = report.LateOrExit;
                command.Parameters.Add("@BreakTime", System.Data.SqlDbType.Float).Value = report.BreakTime;
                command.Parameters.Add("@Development", System.Data.SqlDbType.Float).Value = report.Development;
                command.Parameters.Add("@TaskOnTime", System.Data.SqlDbType.Float).Value = report.TaskOnTime;
                command.Parameters.Add("@Excitement", System.Data.SqlDbType.Float).Value = report.Excitement;
                command.Parameters.Add("@TaskAchievement", System.Data.SqlDbType.Float).Value = report.TaskAchievement;
                command.Parameters.Add("@TeamWork", System.Data.SqlDbType.Float).Value = report.TeamWork;
                command.Parameters.Add("@ImprovementSuggestions", System.Data.SqlDbType.Float).Value = report.ImprovementSuggestions;
                command.Parameters.Add("@Passion", System.Data.SqlDbType.Float).Value = report.Passion;
                command.Parameters.Add("@Precision", System.Data.SqlDbType.Float).Value = report.Precision;
                command.Parameters.Add("@WorkApproved", System.Data.SqlDbType.Float).Value = report.WorkApproved;
                command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = report.FirstName;
                command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = report.LastName;
                command.Parameters.Add("@Track", System.Data.SqlDbType.NVarChar).Value = report.Track;
                command.Parameters.Add("@Total", System.Data.SqlDbType.Float).Value = report.DailyAttendance+ report.LateOrExit+ report.BreakTime+ report.Development+ report.TaskOnTime+ report.Excitement+ report.TaskAchievement+ report.TeamWork+ report.ImprovementSuggestions + report.Passion+ report.Precision+ report.WorkApproved;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;

                }
                return true;
            }
        }
        /// <summary>
        /// Retrives the Documents uploaded by the trainee in the registeration.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filetype"></param>
        /// <returns>Documents List</returns>
        public List<DocumentModel> GetDocument(long id, string filetype)
        {
            string sqlStatment = "SELECT * FROM [dbo].[RegistrationFiles] Where TraineeID = @TraineeID AND FileType = @FileType";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@TraineeID", System.Data.SqlDbType.VarChar, -1).Value = id;
                command.Parameters.Add("@FileType", System.Data.SqlDbType.VarChar, -1).Value = filetype;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    documents = new List<DocumentModel>();
                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            DocumentModel document = new DocumentModel();
                            document.FileType = (string)(reader["FileType"]);
                            document.Data = (byte[])(reader["Data"]);
                            document.Extention = (string)(reader["Extension"]);
                            document.FileName = (string)(reader["FileName"]);
                            documents.Add(document);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return documents;
            }
        }
        /// <summary>
        /// retrives all the reports in the database, used only by Admin.
        /// </summary>
        /// <returns>Reports list</returns>
        public List<ReportModel> GetReports()
        {
            string sqlStatment = "SELECT * FROM [dbo].[Reports]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reports = new List<ReportModel>();
                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            ReportModel report = new ReportModel();
                            report.TraineeID = (long)(reader["TraineeID"]);
                            report.ReportNo = (int)(reader["ReportNo"]);
                            report.DailyAttendance = (double)(reader["DailyAttendance"]);
                            report.LateOrExit = (double)(reader["LateOrExit"]);
                            report.BreakTime = (double)(reader["BreakTime"]);
                            report.Development = (double)(reader["Development"]);
                            report.TaskOnTime = (double)(reader["TaskOnTime"]);
                            report.Excitement = (double)(reader["Excitement"]);
                            report.TaskAchievement = (double)(reader["TaskAchievement"]);
                            report.TeamWork = (double)(reader["TeamWork"]);
                            report.ImprovementSuggestions = (double)(reader["ImprovementSuggestions"]);
                            report.Passion = (double)(reader["Passion"]);
                            report.Precision = (double)(reader["Precision"]);
                            report.WorkApproved = (double)(reader["WorkApproved"]);
                            report.FirstName = (string)(reader["FirstName"]);
                            report.LastName = (string)(reader["LastName"]);
                            report.Track = (string)(reader["Track"]);
                            report.TotalScore = (double)(reader["Total"]);
                            reports.Add(report);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return reports;
            }
        }
        /// <summary>
        /// retrives all the reports in a certin track, used by the supervisor of the track.
        /// </summary>
        /// <param name="Track"></param>
        /// <returns>Reports list</returns>
        public List<ReportModel> GetTrackReports(string Track)
        {
            string sqlStatment = "SELECT * FROM [dbo].[Reports] Where Track = @track";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@track", System.Data.SqlDbType.NVarChar, -1).Value = Track;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reports = new List<ReportModel>();
                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            ReportModel report = new ReportModel();
                            report.TraineeID = (long)(reader["TraineeID"]);
                            report.ReportNo = (int)(reader["ReportNo"]);
                            report.DailyAttendance = (double)(reader["DailyAttendance"]);
                            report.LateOrExit = (double)(reader["LateOrExit"]);
                            report.BreakTime = (double)(reader["BreakTime"]);
                            report.Development = (double)(reader["Development"]);
                            report.TaskOnTime = (double)(reader["TaskOnTime"]);
                            report.Excitement = (double)(reader["Excitement"]);
                            report.TaskAchievement = (double)(reader["TaskAchievement"]);
                            report.TeamWork = (double)(reader["TeamWork"]);
                            report.ImprovementSuggestions = (double)(reader["ImprovementSuggestions"]);
                            report.Passion = (double)(reader["Passion"]);
                            report.Precision = (double)(reader["Precision"]);
                            report.WorkApproved = (double)(reader["WorkApproved"]);
                            report.FirstName = (string)(reader["FirstName"]);
                            report.LastName = (string)(reader["LastName"]);
                            report.Track = (string)(reader["Track"]);
                            report.TotalScore = (double)(reader["Total"]);
                            reports.Add(report);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return reports;
            }
        }
        /// <summary>
        /// retrives a specific report for full review.
        /// </summary>
        /// <param name="traineeID"></param>
        /// <param name="reportNum"></param>
        /// <returns>Report data</returns>
        public ReportModel GetReport(long traineeID, int reportNum)
        {
            ReportModel report = new ReportModel();
            string sqlStatment = "SELECT * FROM [dbo].[Reports] Where TraineeID = @TraineeID AND ReportNo= @reportno";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@TraineeID", System.Data.SqlDbType.BigInt, -1).Value = traineeID;
                command.Parameters.Add("@reportno", System.Data.SqlDbType.Int, -1).Value = reportNum;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            report.TraineeID = (long)(reader["TraineeID"]);
                            report.ReportNo = (int)(reader["ReportNo"]);
                            report.DailyAttendance = (double)(reader["DailyAttendance"]);
                            report.LateOrExit = (double)(reader["LateOrExit"]);
                            report.BreakTime = (double)(reader["BreakTime"]);
                            report.Development = (double)(reader["Development"]);
                            report.TaskOnTime = (double)(reader["TaskOnTime"]);
                            report.Excitement = (double)(reader["Excitement"]);
                            report.TaskAchievement = (double)(reader["TaskAchievement"]);
                            report.TeamWork = (double)(reader["TeamWork"]);
                            report.ImprovementSuggestions = (double)(reader["ImprovementSuggestions"]);
                            report.Passion = (double)(reader["Passion"]);
                            report.Precision = (double)(reader["Precision"]);
                            report.WorkApproved = (double)(reader["WorkApproved"]);
                            report.FirstName = (string)(reader["FirstName"]);
                            report.LastName = (string)(reader["LastName"]);
                            report.Track = (string)(reader["Track"]);
                            report.TotalScore = (double)(reader["Total"]);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return report;
            }
        }
        /// <summary>
        /// adds a new supervisor to the system, used only by Admin (not implemented yet).
        /// </summary>
        /// <param name="user"></param>
        /// <returns>true if inserted correctly, false if not inserted</returns>
        public bool AddUser(UserModel user)
        {
            string sqlStatment = "INSERT INTO [dbo].[Users] VALUES(@username, @password, @role, @track)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, -1).Value = user.Username;
                command.Parameters.Add("@password", System.Data.SqlDbType.VarChar, -1).Value = user.Password;
                command.Parameters.Add("@role", System.Data.SqlDbType.VarChar, -1).Value = user.Role;
                command.Parameters.Add("@track", System.Data.SqlDbType.NVarChar, -1).Value = user.Track;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// retrives the trainees accepted in a specific track from the database, used by the supervisor if the track.
        /// </summary>
        /// <param name="track"></param>
        /// <returns> Trainee List </returns>
        public List<TraineeModel> SelectTrainees(string track)
        {
            string sqlStatment = "SELECT * FROM [dbo].[TraineeInfo] Where Status = @status AND Track = @track ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@status", System.Data.SqlDbType.NVarChar, -1).Value = "مقبول";
                command.Parameters.Add("@track", System.Data.SqlDbType.NVarChar, -1).Value = track;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    trainees = new List<TraineeModel>();
                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            TraineeModel trainee = new TraineeModel();
                            trainee.FirstName = (string)(reader["FirstName"]);
                            trainee.LastName = (string)(reader["LastName"]);
                            trainee.Email = (string)(reader["Email"]);
                            trainee.PhoneNumber = (string)(reader["PhoneNumber"]);
                            trainee.Nationality = (string)(reader["Nationality"]);
                            trainee.NationalID = (long)(reader["NationalID"]);
                            trainee.DateOfBirth = (string)(reader["DateOfBirth"]);
                            trainee.GraduationYear = (string)(reader["GraduationYear"]);
                            trainee.Gender = (string)(reader["Gender"]);
                            trainee.Region = (string)(reader["Region"]);
                            trainee.City = (string)(reader["City"]);
                            trainee.Degree = (string)(reader["Degree"]);
                            trainee.Major = (string)(reader["Major"]);
                            trainee.GPAScale = (string)(reader["GPAScale"]);
                            trainee.GPA = (string)(reader["GPA"]);
                            trainee.ExpYears = (string)(reader["ExpYears"]);
                            trainee.EmployeeStatus = (string)(reader["EmployeeStatus"]);
                            trainee.EnglishLevel = (string)(reader["EnglishLevel"]);
                            trainee.RegistrationReason = (string)(reader["RegistrationReason"]);
                            trainee.FreementStaus = (string)(reader["FreementStaus"]);
                            trainee.University = (string)(reader["University"]);
                            trainee.Status = (string)(reader["Status"]);
                            trainee.Track = (string)(reader["Track"]);
                            trainees.Add(trainee);
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return trainees;
            }
        }
        /// <summary>
        /// retrives a specific trainee information.
        /// </summary>
        /// <param name="TraineeID"></param>
        /// <returns>Trainee data </returns>
        public TraineeModel GetTrainee(long TraineeID)
        {
            TraineeModel trainee = new TraineeModel();
            string sqlStatment = "SELECT * FROM [dbo].[TraineeInfo] Where NationalID = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@id", System.Data.SqlDbType.BigInt).Value = TraineeID;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            trainee.FirstName = (string)(reader["FirstName"]);
                            trainee.LastName = (string)(reader["LastName"]);
                            trainee.Email = (string)(reader["Email"]);
                            trainee.PhoneNumber = (string)(reader["PhoneNumber"]);
                            trainee.Nationality = (string)(reader["Nationality"]);
                            trainee.NationalID = (long)(reader["NationalID"]);
                            trainee.DateOfBirth = (string)(reader["DateOfBirth"]);
                            trainee.GraduationYear = (string)(reader["GraduationYear"]);
                            trainee.Gender = (string)(reader["Gender"]);
                            trainee.Region = (string)(reader["Region"]);
                            trainee.City = (string)(reader["City"]);
                            trainee.Degree = (string)(reader["Degree"]);
                            trainee.Major = (string)(reader["Major"]);
                            trainee.GPAScale = (string)(reader["GPAScale"]);
                            trainee.GPA = (string)(reader["GPA"]);
                            trainee.ExpYears = (string)(reader["ExpYears"]);
                            trainee.EmployeeStatus = (string)(reader["EmployeeStatus"]);
                            trainee.EnglishLevel = (string)(reader["EnglishLevel"]);
                            trainee.RegistrationReason = (string)(reader["RegistrationReason"]);
                            trainee.FreementStaus = (string)(reader["FreementStaus"]);
                            trainee.University = (string)(reader["University"]);
                            trainee.Status = (string)(reader["Status"]);
                            trainee.Track = (string)(reader["Track"]);
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return trainee;
            }
        }
        /// <summary>
        /// retrives all trainees registered in the database. used only by Admin.
        /// </summary>
        /// <returns>Trainee List</returns>
        public List<TraineeModel> SelectAllTrainees()
        {
            string sqlStatment = "SELECT * FROM [dbo].[TraineeInfo]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    trainees = new List<TraineeModel>();
                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            TraineeModel trainee = new TraineeModel();
                            trainee.FirstName = (string)(reader["FirstName"]);
                            trainee.LastName = (string)(reader["LastName"]);
                            trainee.Email = (string)(reader["Email"]);
                            trainee.PhoneNumber = (string)(reader["PhoneNumber"]);
                            trainee.Nationality = (string)(reader["Nationality"]);
                            trainee.NationalID = (long)(reader["NationalID"]);
                            trainee.DateOfBirth = (string)(reader["DateOfBirth"]);
                            trainee.GraduationYear = (string)(reader["GraduationYear"]);
                            trainee.Gender = (string)(reader["Gender"]);
                            trainee.Region = (string)(reader["Region"]);
                            trainee.City = (string)(reader["City"]);
                            trainee.Degree = (string)(reader["Degree"]);
                            trainee.Major = (string)(reader["Major"]);
                            trainee.GPAScale = (string)(reader["GPAScale"]);
                            trainee.GPA = (string)(reader["GPA"]);
                            trainee.ExpYears = (string)(reader["ExpYears"]);
                            trainee.EmployeeStatus = (string)(reader["EmployeeStatus"]);
                            trainee.EnglishLevel = (string)(reader["EnglishLevel"]);
                            trainee.RegistrationReason = (string)(reader["RegistrationReason"]);
                            trainee.FreementStaus = (string)(reader["FreementStaus"]);
                            trainee.University = (string)(reader["University"]);
                            trainee.Status = (string)(reader["Status"]);
                            trainee.Track = (string)(reader["Track"]);
                            trainees.Add(trainee);
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return trainees;
            }
        }
        /// <summary>
        /// retrives the number of reports for a specefic trainee, to ensure there are no duplication while inserting a new report. 
        /// </summary>
        /// <param name="TraineeID"></param>
        /// <returns> number of reports </returns>
        public int GetReportCount(long TraineeID)
        {
            int count = 0;
            string sqlStatment = "SELECT * FROM [dbo].[Reports] Where TraineeID = @TraineeID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@TraineeID", System.Data.SqlDbType.BigInt, -1).Value = TraineeID;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            count = (int)(reader["ReportNo"]);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return count;
            }
        }
        /// <summary>
        /// changes the satus of the trainee in the database based on the admin request.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns> True if inserted correctly, false if not inserted </returns>
        public bool ChangeStatus(long id, string status)
        {
            string sqlStatment = "UPDATE [dbo].[TraineeInfo] SET Status = @status WHERE NationalID = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@id", System.Data.SqlDbType.BigInt).Value = id;
                command.Parameters.Add("@status", System.Data.SqlDbType.NVarChar, -1).Value = status;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
                return true;
            }
        }

        public string GetIDs(string track)
        {
            string sqlStatment = "SELECT NationalID FROM [dbo].[TraineeInfo] WHERE Track = @track";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@track", System.Data.SqlDbType.NVarChar, -1).Value = track;
                string NationalIDs = "";
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            NationalIDs += (long)(reader["NationalID"]) + ",";
                        }

                    }
                    else
                    {
                        return "none";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return NationalIDs;
            }
        }

        public string GetNames(string track)
        {
            string sqlStatment = "SELECT * FROM [dbo].[TraineeInfo] WHERE Track = @track";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatment, connection);
                command.Parameters.Add("@track", System.Data.SqlDbType.NVarChar, -1).Value = track;
                string Names = "";
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read() != false)
                        {
                            Names += (string)(reader["FirstName"]) + " " + (string)(reader["LastName"]) + ",";
                        }

                    }
                    else
                    {
                        return "none";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return Names;
            }
        }
    }
}