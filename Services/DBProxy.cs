using RegistrationWeb.Models;
using RegistrationWeb.Services;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationWeb.Services
{
    /// <summary>
    /// This class is a proxy between the database class and the controller each function calles another function from the UsersDAO service class.
    /// </summary>
    public class DBProxy
    {
        UsersDAO userDAO = new UsersDAO();
        public DBProxy()
        {
        }
        public UserModel Login(UserModel user)
        {
            return userDAO.Login(user);
        }
        public bool TraineeRegFiles(TraineeModel trainee, DocumentModel document)
        {
            return userDAO.TraineeRegFiles(trainee, document);
        }
        public bool TraineeRegInfo(TraineeModel trainee)
        {
            return userDAO.TraineeRegInfo(trainee);
        }
        public bool GetDocument(long id, string filetype)
         {
            //convirting the data from the database to a document form.
            List<DocumentModel> documents = userDAO.GetDocument(id, filetype);
            foreach (var item in documents)
            {
                string newFileName = item.FileType + item.FileName + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + item.Extention;
                File.WriteAllBytes(newFileName, item.Data);
                //opening the file.
                Process.Start(new ProcessStartInfo { FileName = @newFileName, UseShellExecute = true });
            }
            return true;    
        }
        public bool AddReport(ReportModel report)
        {
            return userDAO.AddReport(report);
        }
        public List<ReportModel> GetReports()
        {
            return userDAO.GetReports();    
        }
        public List<ReportModel> GetTrackReports(string Track)
        {
            return userDAO.GetTrackReports(Track);
        }
        public ReportModel GetReport(long traineeID, int reportNum)
        {
            return userDAO.GetReport(traineeID, reportNum); 
        }
        public bool AddUser(UserModel user)
        {
            return userDAO.AddUser(user);
        }
        public List<TraineeModel> SelectTrainees(string track)
        {
            return userDAO.SelectTrainees(track);
        }
        public List<TraineeModel> SelectAllTrainees()
        {
            return userDAO.SelectAllTrainees(); 
        }
        public bool ChangeStatus(long id, string status)
        {
            return userDAO.ChangeStatus(id, status);    
        }
        public TraineeModel GetTrainee(long TraineeID)
        {
            return userDAO.GetTrainee(TraineeID);
        }
        public int GetReportCount(long TraineeID)
        {
            return userDAO.GetReportCount(TraineeID) ;
        }
        public string GetIDs(string track)
        {
            return userDAO.GetIDs(track);
        }
        public string GetNames(string track)
        {
            return userDAO.GetNames(track);
        }
        
        }
}
