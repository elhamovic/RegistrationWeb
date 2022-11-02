using RegistrationWeb.Services;
using Microsoft.AspNetCore.Mvc;
using RegistrationWeb.Models;
using System.Diagnostics;
using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace RegistrationWeb.Controllers
{
    /// <summary>
    /// The controller connect the views to the functions nedded to operate the website.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        /// <summary>
        /// defiining thr objects of the db proxy and the email message.
        /// </summary>
        public DBProxy dbProxy = new DBProxy();
        MimeMessage message = new MimeMessage();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Login function opens the login page. 
        /// </summary>
        /// <returns>Login View</returns>
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// LoginDB takes the user login data, search for the user in the database, then redirect the user to the appropriate page.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>InfoDashboard Action Controller or Login View</returns>
        public IActionResult LoginDB(UserModel user)
        {
            user = dbProxy.Login(user);
            if (user != null)
            {
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                return RedirectToAction("InfoDashboard");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        /// <summary>
        /// InfoDashboard function used whenever the InfoDashboard is opend, it requests the data displayed in the page from the database based on the user role. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>InfoDashboard View with data based on role</returns>
        public IActionResult InfoDashboard(UserModel user)
        {
            if (user.Role == null)
            {
                var sessionOb = HttpContext.Session.GetString("user");
                if (sessionOb != null)
                {
                    user = JsonConvert.DeserializeObject<UserModel>(sessionOb);

                }
            }
            ViewData["Role"] = user.Role;
            if (user.Role == "Admin")
            {
                return View("InfoDashboard", dbProxy.SelectAllTrainees());

            }
            else
            {
                return View("InfoDashboard", dbProxy.SelectTrainees(user.Track));
            }
        }
        /// <summary>
        /// ReportsDashboard function used whenever the ReportsDashboard is opend, it requests the data displayed in the page from the database based on the user role. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>ReportsDashboard View with data based on role</returns>
        public IActionResult ReportsDashboard(UserModel user)
        {
            if (user.Role == null)
            {
                var sessionOb = HttpContext.Session.GetString("user");
                if (sessionOb != null)
                {
                    user = JsonConvert.DeserializeObject<UserModel>(sessionOb);

                }
            }
            ViewBag.role = user.Role;
            if (user.Role == "Admin")
            {
                return View(dbProxy.GetReports());
            }
            else
            {
                return View(dbProxy.GetTrackReports(user.Track));
            }
        }
        /// <summary>
        /// Index function opens the website main page.
        /// </summary>
        /// <returns>Index View</returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Registration mathod displays the Registration form.
        /// </summary>
        /// <returns>Registration form View</returns>
        public IActionResult Registration()
        {
            return View();
        }
        /// <summary>
        /// File Task is called by the Registration form to save the files submitted by the user to the database.
        /// </summary>
        /// <param name="trainee"></param>
        /// <returns>Registration View</returns>
        [HttpPost]
        public async Task<ActionResult> File(TraineeModel trainee)
        {
            bool done = false;
            DocumentModel document = new DocumentModel();
            List<IFormFile> files = new List<IFormFile>();
            files.Add(trainee.CV);
            files.Add(trainee.Certificate);
            files.Add(trainee.IntroVideo);
            files.AddRange(trainee.PreviosWork);
            int count = files.Count;
            done = dbProxy.TraineeRegInfo(trainee);
            if (count > 0 && done)
            {
                foreach (var file in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        document.Data = memoryStream.ToArray();
                        document.Extention = file.ContentType.Split("/")[1];
                        document.FileName = file.FileName;
                        document.FileType = file.Name;
                    }
                    done = dbProxy.TraineeRegFiles(trainee, document);
                }
            }
            if (done)
            {
                return View("success");
            }
            else
            {
                return View("Registration");
                ////////////////////////////////PLEASE CHANGE THE SENTENCE TO WHAT THE CUSTOMER WANTS.////////////////////////////////
                ViewBag.data = "الرجاء مراجعة البيانات والتسجيل مرة أخرى";
            }

        }
        /// <summary>
        /// Infoform method opens the information form that contains all the trainee data (information and files).
        /// </summary>
        /// <param name="trainee"></param>
        /// <returns>The form page</returns>
        public IActionResult Infoform(long trainee)
        {
            TraineeModel tr = dbProxy.GetTrainee(trainee);
            return View(tr);
        }
        /// <summary>
        /// OpenFiles retrives the Registration files and open them based on the admin/supervisor request.
        /// </summary>
        /// <param name="TraineeID"></param>
        /// <returns>InfoDashboard View or Index View in case of error</returns>
        public IActionResult OpenFile(long TraineeID, string filetype)
        {
            dbProxy.GetDocument(TraineeID, filetype);
            var sessionOb = HttpContext.Session.GetString("user");
            if (sessionOb != null)
            {
                UserModel user = JsonConvert.DeserializeObject<UserModel>(sessionOb);

                TraineeModel tr = dbProxy.GetTrainee(TraineeID);
                return View("Infoform", tr);
            }
            else
            {
                return View("Index");
            }
        }
        /// <summary>
        /// changeStatus allows the admin to change the status of the trainee to new/Bassed first round/accepted, and sends an email to the trainee to inform them of the new status.
        /// </summary>
        /// <param name="traineeID"></param>
        /// <param name="status"></param>
        /// <returns>InfoDashboard View or Index View in case of error</returns>

        public IActionResult changeStatus(long traineeID, string status)
        {
            TraineeModel trainee = dbProxy.GetTrainee(traineeID);
            dbProxy.ChangeStatus(trainee.NationalID, status);
            //-------------------------------------------------------
            ////////////////////////////////PLEASE CHANGE THE EMAIL NAME AND EMAIL ADDRESS BELOW TO WHAT THE CUSTOMER WANTS.////////////////////////////////
            message.From.Add(new MailboxAddress("Ministry of Media", "ersthethreemusketeers@gmail.com"));
            message.To.Add(MailboxAddress.Parse(trainee.Email));
            ////////////////////////////////PLEASE CHANGE THE EMAIL SUBJECT BELOW TO WHAT THE CUSTOMER WANTS.////////////////////////////////
            message.Subject = "تم تغيير حالة التسجيل";
            message.Body = new TextPart("plain")
            {
            ////////////////////////////////PLEASE CHANGE THE EMAIL BODY BELOW TO WHAT THE CUSTOMER WANTS.////////////////////////////////
                Text = @"تم تغيير الحاله الخاصه بك إلى " + status
            };
            SmtpClient client = new SmtpClient();
            try
            {
            ////////////////////////////////PLEASE CHANGE THE EMAIL HOST BELOW TO WHAT THE CUSTOMER WANTS.////////////////////////////////
                client.Connect("smtp.gmail.com", 465, true);
            ////////////////////////////////PLEASE CHANGE THE EMAIL ADDRESS AND EMAIL PASSWORD (DEPENDING ON THE EMAIL HOST, SOME HAVE APP SPECIFIC PASSWORD) BELOW TO WHAT THE CUSTOMER WANTS.////////////////////////////////
                client.Authenticate("ersthethreemusketeers@gmail.com", "exirtvgidrucwpsd");
                client.Send(message);
                Console.WriteLine("Email sent to trainee successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
            return RedirectToAction("InfoDashboard");
        }
        /// <summary>
        /// NewReport method opens the review templete to be filled by the supervisor
        /// </summary>
        /// <returns>the report form</returns>
        public IActionResult NewReport()
        {
            UserModel user = new UserModel();
            ViewData["note"] = "x";
            var sessionOb = HttpContext.Session.GetString("user");
            if (sessionOb != null)
            {
                user = JsonConvert.DeserializeObject<UserModel>(sessionOb);
                if(user.Track != null)
                {
                    string t = dbProxy.GetIDs(user.Track);
                    ViewData["students"] = dbProxy.GetIDs(user.Track);
                    ViewData["names"] = dbProxy.GetNames(user.Track);
                    string[] counts = t.Split(',');
                    string s= "";
                    for (int i = 0; i < counts.Length-1; i++)
                    {
                        s += dbProxy.GetReportCount(long.Parse(counts[i])) + ",";

                    }
                    ViewData["reportcounts"] = s;
                }

            }
                else
                {
                ViewData["students"] = "none";
                ViewData["reportcounts"] = "none";
                }

            return View("ReportForm");
        }
        /// <summary>
        /// AddReport adds the filled report to the database using the database proxy.
        /// </summary>
        /// <param name="report"></param>
        /// <returns>the reports dashboard</returns>
        public IActionResult AddReport(ReportModel report)
        {
            if (dbProxy.GetTrainee(report.TraineeID) is not null) {
                ViewData["note"] = "x";
                TraineeModel tr = dbProxy.GetTrainee(report.TraineeID);
                report.FirstName = tr.FirstName;
                report.LastName = tr.LastName;
                report.Track = tr.Track;
                if (report.ReportNo <= dbProxy.GetReportCount(report.TraineeID))
                {
                    ViewData["note"] = "y";
                    return View("ReportForm");
                }
                dbProxy.AddReport(report);
                return RedirectToAction("ReportsDashboard");
            }
            else
            {
                return RedirectToAction("ReportsDashboard");
            }
        }
        /// <summary>
        /// ViewReport opens the report form filled with the report criteria based on the trainee selected.
        /// </summary>
        /// <param name="traineeID"></param>
        /// <param name="reportNum"></param>
        /// <returns>the filled report form</returns>
        public IActionResult ViewReport(long traineeID, int reportNum)
        {
            return View(dbProxy.GetReport(traineeID, reportNum));
        }
        /// <summary>
        /// Error handles the errors in the website
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}