using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace OJT.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int COURSE_ID = -1, string MENTOR = "", string EMP_ID = "", int PAGE = 1)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Home");
            var dept = "";
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
            if (Convert.ToInt32(Session["Role"].ToString()) == 0)
            {
                var isMentee = em.IsMentee(COURSE_ID, Session["Username"].ToString());
                var isMentor = em.IsMentor(COURSE_ID, Session["Username"].ToString());
                if (isMentee == 1 && string.IsNullOrWhiteSpace(EMP_ID))
                {
                    EMP_ID = Session["Username"].ToString();
                    MENTOR = em.GetListMentor(COURSE_ID).FirstOrDefault().ID;
                }
                else if (isMentor == 1 && string.IsNullOrWhiteSpace(MENTOR))
                {
                    MENTOR = Session["Username"].ToString();
                }
                else
                {
                    EMP_ID = "0";
                    MENTOR = "0";
                }
            }
            if (Convert.ToInt32(Session["Role"].ToString()) == 2)
            {
                dept = Session["Dept"].ToString();
            }

            var lstHis = his.GetHistory(MENTOR, EMP_ID, COURSE_ID, dept, PAGE);
            var lstCntEm = new DataTable();
            var listCntCourse = new DataTable();
            lstCntEm.Columns.Add("EMP_ID");
            lstCntEm.Columns.Add("CNT_EMP");
            listCntCourse.Columns.Add("EMP_ID");
            listCntCourse.Columns.Add("COURSE_ID");
            listCntCourse.Columns.Add("CNT_COURSE");
            foreach (var item in lstHis)
            {
                var count1 = 0;
                var count2 = 0;
                foreach (var item1 in lstHis)
                {
                    if (item.EMP_ID.Equals(item1.EMP_ID))
                    {
                        count1++;
                        if (item.COURSE_ID.Equals(item1.COURSE_ID))
                        {
                            count2++;
                        }

                    }
                    else
                    {
                        continue;
                    }
                }
                bool exists1 = lstCntEm.Select().ToList().Exists(row => row["EMP_ID"].ToString().ToUpper() == item.EMP_ID);
                bool exists2 = listCntCourse.Select().ToList().Exists(row => row["EMP_ID"].ToString().ToUpper() == item.EMP_ID
                    && Convert.ToInt32(row["COURSE_ID"].ToString()) == item.COURSE_ID);
                if (exists1)
                {
                    var rowsToUpdate = lstCntEm.AsEnumerable().Where(r => r.Field<string>("EMP_ID") == item.EMP_ID);

                    foreach (var row in rowsToUpdate)
                    {
                        row.SetField("CNT_EMP", count1);
                    }
                }
                else
                {
                    DataRow r = lstCntEm.NewRow();
                    r[0] = item.EMP_ID;
                    r[1] = count1;
                    lstCntEm.Rows.Add(r);
                }
                if (exists2)
                {
                    var rowsToUpdate = listCntCourse.AsEnumerable().Where(r => r.Field<string>("EMP_ID") == item.EMP_ID
                        && r.Field<string>("COURSE_ID") == item.COURSE_ID.ToString());

                    foreach (var row in rowsToUpdate)
                    {
                        row.SetField("CNT_COURSE", count2);
                    }
                }
                else
                {
                    DataRow r = listCntCourse.NewRow();
                    r[0] = item.EMP_ID;
                    r[1] = item.COURSE_ID;
                    r[2] = count2;
                    listCntCourse.Rows.Add(r);
                }
            }
            // var countEmp = his.GetCountByEmp(MENTOR, EMP_ID, COURSE_ID, dept);
            //var countCourse = his.GetCountByCourse(MENTOR, EMP_ID, COURSE_ID, dept);

            ViewBag.COURSE = courses;
            ViewBag.HIS = lstHis;
            ViewBag.CNT_EMP = lstCntEm;
            ViewBag.CNT_COURSE = listCntCourse;
            ViewBag.TOTAL = his.CountHistory(MENTOR, EMP_ID, COURSE_ID, dept);
            ViewBag.MENTORS = em.GetListMentor(COURSE_ID);
            ViewBag.MENTEES = em.GetListMentee(COURSE_ID, MENTOR);
            ViewBag.SELECT_COURSE = COURSE_ID;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Manage(int COURSE_ID = 0, string MENTOR = "", string EMP_ID = "", int PAGE = 1)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Home");
            HISTORY his = new HISTORY();
            EMPLOYEE em = new EMPLOYEE();
            COURSE course = new COURSE();
            List<COURSE> courses = course.Select();
            ViewBag.COURSE = courses;
            var DEPARTMENT = "";
            
            if (Convert.ToInt32(Session["Role"].ToString()) == 0)
            {
                var isMentee = em.IsMentee(COURSE_ID, Session["Username"].ToString());
                var isMentor = em.IsMentor(COURSE_ID, Session["Username"].ToString());
                if (isMentee == 1 && string.IsNullOrWhiteSpace(EMP_ID))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (isMentor == 1 && string.IsNullOrWhiteSpace(MENTOR))
                {
                    MENTOR = Session["Username"].ToString();
                }
                else
                {
                    EMP_ID = "0";
                    MENTOR = "0";
                }
            }
            if (Convert.ToInt32(Session["Role"].ToString()) == 2)
            {
                DEPARTMENT = Session["Dept"].ToString();
            }
            var lstHis = his.GetHistorySimple(MENTOR, EMP_ID, COURSE_ID, DEPARTMENT, PAGE);
            var cnt = his.CountHistorySimpleTotal(MENTOR, EMP_ID, DEPARTMENT, COURSE_ID);
            var listCntCourse = new DataTable();
            listCntCourse.Columns.Add("COURSE_ID");
            listCntCourse.Columns.Add("CNT_COURSE");
            foreach (var item in lstHis)
            {
                var count1 = 0;
                foreach (var item1 in lstHis)
                {
                    if (item.COURSE_ID.Equals(item1.COURSE_ID))
                    {
                        count1++;
                    }
                    else
                    {
                        continue;
                    }
                }
                bool exists2 = listCntCourse.Select().ToList().Exists(row => Convert.ToInt32(row["COURSE_ID"].ToString()) == item.COURSE_ID);
               
                if (exists2)
                {
                    var rowsToUpdate = listCntCourse.AsEnumerable().Where(r => r.Field<string>("COURSE_ID") == item.COURSE_ID.ToString());

                    foreach (var row in rowsToUpdate)
                    {
                        row.SetField("CNT_COURSE", count1);
                    }
                }
                else
                {
                    DataRow r = listCntCourse.NewRow();
                    r[0] = item.COURSE_ID;
                    r[1] = count1;
                    listCntCourse.Rows.Add(r);
                }
            }
            ViewBag.HIS = lstHis;
            ViewBag.Total = cnt;
            ViewBag.CNT = listCntCourse;
            ViewBag.MENTORS = em.GetListMentor(COURSE_ID);
            ViewBag.MENTEES = em.GetListMentee(COURSE_ID, MENTOR);
            return View();
        }
        public ActionResult ExportCourse(int COURSE_ID = -1, string MENTOR = "", string EMP_ID = "")
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Home");
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var template = Server.MapPath("~/Upload/Template/Export/OJT_EXP2.xlsx");
            HISTORY his = new HISTORY();
            EMPLOYEE em = new EMPLOYEE();
            COURSE course = new COURSE();
            List<COURSE> courses = course.Select();
            var DEPARTMENT = "";
           
            if (Convert.ToInt32(Session["Role"].ToString()) == 0)
            {
                var isMentee = em.IsMentee(COURSE_ID, Session["Username"].ToString());
                var isMentor = em.IsMentor(COURSE_ID, Session["Username"].ToString());
                if (isMentee == 1 && string.IsNullOrWhiteSpace(EMP_ID))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (isMentor == 1 && string.IsNullOrWhiteSpace(MENTOR))
                {
                    MENTOR = Session["Username"].ToString();
                }
                else
                {
                    EMP_ID = "0";
                    MENTOR = "0";
                }
            }
            if (Convert.ToInt32(Session["Role"].ToString()) == 2)
            {
                DEPARTMENT = Session["Dept"].ToString();
            }
            var lstHis = his.GetHistorySimpleExport(MENTOR, EMP_ID, COURSE_ID, DEPARTMENT);
            var cnt = his.CountHistorySimple(MENTOR, EMP_ID, DEPARTMENT, COURSE_ID);
            using (ExcelPackage package = new ExcelPackage(new FileInfo(template)))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();
                for (int i = 0; i < lstHis.Count; i++)
                {
                    var history = lstHis[i];
                    var startIndex = i + 3;
                    ws.Cells["B" + startIndex].Value = history.ROWNUM;
                    ws.Cells["C" + startIndex].Value = history.COURSE;
                    ws.Cells["D" + startIndex].Value = history.ID;
                    ws.Cells["E" + startIndex].Value = history.DEPARTMENT;
                    ws.Cells["F" + startIndex].Value = history.NAME;
                    ws.Cells["G" + startIndex].Value = history.RESULT_LEVEL;

                }
                for (int i = 0; i < lstHis.Count; i++)
                {
                    int count = 0;

                    var history = lstHis[i];
                    var startIndex = i + 3;
                    foreach (var item in cnt)
                    {
                        if (history.COURSE_ID == item.COURSE_ID)
                        {
                            count = item.CNT;
                            break;
                        }
                    }
                    ws.Cells["C" + startIndex + ":C" + (startIndex + count - 1).ToString()].Merge = true;
                    ws.Cells["C" + startIndex].Value = history.COURSE;

                    i += count - 1;
                }
                Byte[] fileBytes = package.GetAsByteArray();
                Response.ClearContent();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                Response.BinaryWrite(fileBytes);
                Response.Flush();
                Response.End();
            }
            return RedirectToAction("Manage", "Home");
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
            Session.Remove("Username");
            Session.Remove("Name");
            Session.Remove("Dept");
            Session.Remove("Role");
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

            var result = detail.Update(ID, "OUTCOME_RESULT", mypath + "/" + fileName);
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
            for (int i = 0; i < Request.Files.Count; i++)
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

        public ActionResult ChangeLanguage(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }

        public ActionResult Export(int COURSE_ID = 0, string MENTOR = "", string EMP_ID = "")
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Home");
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var template = Server.MapPath("~/Upload/Template/Export/OJT_EXP1.xlsx");
            var dept = "";
            HISTORY his = new HISTORY();
            COURSE course = new COURSE();
            EMPLOYEE em = new EMPLOYEE();

            List<COURSE> courses = course.Select();
           
            if (Convert.ToInt32(Session["Role"].ToString()) == 0)
            {
                var isMentee = em.IsMentee(COURSE_ID, Session["Username"].ToString());
                var isMentor = em.IsMentor(COURSE_ID, Session["Username"].ToString());
                if (isMentee == 1 && string.IsNullOrWhiteSpace(EMP_ID))
                {
                    EMP_ID = Session["Username"].ToString();
                    MENTOR = em.GetListMentor(COURSE_ID).FirstOrDefault().ID;
                }
                else if (isMentor == 1 && string.IsNullOrWhiteSpace(MENTOR))
                {
                    MENTOR = Session["Username"].ToString();
                }
                else
                {
                    EMP_ID = "0";
                    MENTOR = "0";
                }
            }
            if (Convert.ToInt32(Session["Role"].ToString()) == 2)
            {
                dept = Session["Dept"].ToString();
            }
            var countEmp = his.GetCountByEmp(MENTOR, EMP_ID, COURSE_ID, dept);
            var countCourse = his.GetCountByCourse(MENTOR, EMP_ID, COURSE_ID, dept);
            var lstHis = his.GetHistoryExport(MENTOR, EMP_ID, COURSE_ID, dept);
            using (ExcelPackage package = new ExcelPackage(new FileInfo(template)))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();
                for (int i = 0; i < lstHis.Count; i++)
                {
                    var step = i;

                    var history = lstHis[i];
                    var startIndex = i + 4;
                    ws.Cells["B" + startIndex].Value = history.EMP_ID;
                    ws.Cells["C" + startIndex].Value = history.EMP_NAME;
                    ws.Cells["D" + startIndex].Value = history.DEPARTMENT;
                    ws.Cells["E" + startIndex].Value = history.PERIOD;
                    ws.Cells["F" + startIndex].Value = history.STATUS;
                    ws.Cells["G" + startIndex].Value = history.SUBJECT;
                    ws.Cells["H" + startIndex].Value = history.SUB_LEVEL;
                    ws.Cells["I" + startIndex].Value = history.START_DT == null ? "" : history.START_DT.ToString("yyyy-MM-dd");
                    ws.Cells["J" + startIndex].Value = history.END_DT == null ? "" : history.END_DT.ToString("yyyy-MM-dd");
                    ws.Cells["K" + startIndex].Value = history.APPROVE;
                    ws.Cells["L" + startIndex].Value = history.REC_START_DT == null ? "" : history.REC_START_DT.ToString("yyyy-MM-dd");
                    ws.Cells["M" + startIndex].Value = history.REC_END_DT == null ? "" : history.REC_END_DT.ToString("yyyy-MM-dd");
                    ws.Cells["N" + startIndex].Value = history.MANAGER_CMT;
                    ws.Cells["O" + startIndex].Value = history.HR_CMT;
                    ws.Cells["P" + startIndex].Value = history.TEST_TIME == null ? "" : history.TEST_TIME.ToString("yyyy-MM-dd");
                    ws.Cells["Q" + startIndex].Value = history.SCORE;
                    ws.Cells["R" + startIndex].Value = history.RESULT_LEVEL;

                }
                for (int i = 0; i < lstHis.Count; i++)
                {
                    var step = i;
                    int count = 0;
                    var history = lstHis[i];
                    foreach (var item in countEmp)
                    {
                        if (history.EMP_ID.Equals(item.EMP_ID))
                        {
                            count = item.CNT_EMP;
                            break;
                        }
                    }

                    var startIndex = i + 4;
                    ws.Cells["B" + startIndex + ":B" + (startIndex + count - 1).ToString()].Merge = true;
                    ws.Cells["B" + startIndex].Value = history.EMP_ID;
                    ws.Cells["C" + startIndex + ":C" + (startIndex + count - 1).ToString()].Merge = true;
                    ws.Cells["C" + startIndex].Value = history.EMP_NAME;
                    ws.Cells["D" + startIndex + ":D" + (startIndex + count - 1).ToString()].Merge = true;
                    ws.Cells["D" + startIndex].Value = history.DEPARTMENT;
                    i += count - 1;
                }
                for (int i = 0; i < lstHis.Count; i++)
                {
                    var step = i;
                    var cntCourse = 0;
                    var history = lstHis[i];
                    var startIndex = i + 4;
                    foreach (var item in countCourse)
                    {
                        if (history.COURSE_ID.Equals(item.COURSE_ID))
                        {
                            cntCourse = item.CNT_COURSE;
                            break;
                        }
                    }
                    ws.Cells["E" + startIndex + ":E" + (startIndex + cntCourse - 1).ToString()].Merge = true;
                    ws.Cells["E" + startIndex].Value = history.PERIOD;
                    ws.Cells["Q" + startIndex + ":Q" + (startIndex + cntCourse - 1).ToString()].Merge = true;
                    ws.Cells["Q" + startIndex].Value = history.SCORE;
                    ws.Cells["R" + startIndex + ":R" + (startIndex + cntCourse - 1).ToString()].Merge = true;
                    ws.Cells["R" + startIndex].Value = history.RESULT_LEVEL;
                    i += cntCourse - 1;
                }
                //buffer = package.Stream as MemoryStream;
                Byte[] fileBytes = package.GetAsByteArray();
                Response.ClearContent();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + fileName);
                Response.BinaryWrite(fileBytes);
                Response.Flush();
                Response.End();

            }

            return RedirectToAction("Index");
        }
    }
}
