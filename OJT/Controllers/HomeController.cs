using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OJT.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int COURSE_ID = -1, string MENTOR = "", string EMP_ID = "")
        {
            HISTORY his = new HISTORY();
            COURSE course = new COURSE();
            EMPLOYEE em = new EMPLOYEE();
            List<COURSE> courses = course.Select();
            if (COURSE_ID == -1)
            {
                if (course != null && courses.Count > 0)
                {
                    COURSE_ID = courses[0].ID;
                }
                else
                {
                    COURSE_ID = 0;
                }
            }
            var lstHis = his.GetHistory(MENTOR, EMP_ID, COURSE_ID);
            //var cntHis = his.CountHistory(MENTOR, EMP_ID, COURSE_ID);
            ViewBag.COURSE = courses;
            ViewBag.HIS = lstHis;
            //ViewBag.CNT = cntHis;
            ViewBag.MENTORS = em.GetListMentor(COURSE_ID);
            ViewBag.MENTEES = em.GetListMentee(COURSE_ID, MENTOR);
            ViewBag.SELECT_COURSE = COURSE_ID;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Manage(int COURSE_ID = 0, string MENTOR = "", string EMP_ID = "")
        {
            EMPLOYEE em = new EMPLOYEE();
            COURSE course = new COURSE();
            List<COURSE> courses = course.Select();
            ViewBag.COURSE = courses;
            ViewBag.MENTORS = em.GetListMentor(COURSE_ID);
            ViewBag.MENTEES = em.GetListMentee(COURSE_ID, MENTOR);
            return View();
        }

        public ActionResult ChangePassword()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string EMP_ID, string Password, string NewPassword, string PwConfirm)
        {
            if (!NewPassword.Equals(PwConfirm))
            {
                return RedirectToAction("ChangePassword", new { success = -1, message = "Password does not match" });
            }
            EMPLOYEE em = new EMPLOYEE();
            Password = em.Encode(Password);
            NewPassword = em.Encode(NewPassword);
            var result = em.ChangePassword(EMP_ID, Password, NewPassword);
            var message = result > 0 ? "Success" : "Fail";
            return RedirectToAction("ChangePassword", new { success = result, message = message });
        }

        [HttpPost]
        public ActionResult Signin(string username, string password)
        {
            EMPLOYEE em = new EMPLOYEE();
            password = em.Encode(password);
            bool login = em.Login(username, password);
            if (login)
                return RedirectToAction("Index");
            TempData["alert"] = "Login fail. Please check you ID and Password.";
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase OUTCOME_RESULT, int ID)
        {
            var allowedExtensions = new[] { ".ppt", ".pptx" };
            var ext = Path.GetExtension(OUTCOME_RESULT.FileName).ToLower();
            if (!allowedExtensions.Contains(ext))
            {
                TempData["message"] = "Upload fail. Please select ppt/pptx file";
                return Redirect(Request.UrlReferrer.ToString());
            }
            HIS_DETAIL detail = new HIS_DETAIL();
            var mypath = "~/Upload/File/" + ID.ToString();
            var path = Server.MapPath(mypath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            string savedFileName = Path.Combine(path, Path.GetFileName(OUTCOME_RESULT.FileName));
            OUTCOME_RESULT.SaveAs(savedFileName);
            string fileName = Path.GetFileName(OUTCOME_RESULT.FileName);

            var result = detail.Update(ID, "OUTCOME_RESULT", mypath+"/"+fileName);
            if (result > 0)
                TempData["message"] = "Upload success";
            else
                TempData["message"] = "Upload fail";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult UploadImg()
        {
            ACTIVITY_IMG act = new ACTIVITY_IMG();
            var result = 0;
            int ID = Convert.ToInt32(Request["ID"]);
            var mypath = "~/Upload/IMG/" + ID.ToString();
            var path = Server.MapPath(mypath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            for (int i = 0; i < Request.Files.Count; i++ )
            {
                HttpPostedFileBase file = Request.Files[i];
                if (file.ContentLength == 0)
                {
                    //Repeated upload file be skipped .
                    continue;
                }
                string savedFileName = Path.Combine(path, Path.GetFileName(file.FileName));
                file.SaveAs(savedFileName);
                string fileName = Path.GetFileName(file.FileName);
                result = act.Insert(ID, fileName);
            }
            if (result > 0)
                TempData["message"] = "Upload success";
            else
                TempData["message"] = "Upload fail";
            return Redirect(Request.UrlReferrer.ToString());
        }
        public FileResult Download(string url)
        {
            var path = Server.MapPath(url);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            string fileName = Path.GetFileName(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
