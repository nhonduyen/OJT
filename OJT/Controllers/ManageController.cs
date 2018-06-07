using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace OJT.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Manage/

        public ActionResult Index()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Login", "Home");
            EMPLOYEE EM = new EMPLOYEE();
            var dept = EM.GetDepartment();
            ViewBag.DEPT = dept;
            COURSE course = new COURSE();
            List<COURSE> courses = course.Select();
            ViewBag.COURSE = courses;
            return View();
        }

        [HttpPost]
        public ActionResult InsertCourse(string Name, string From, string To)
        {
            COURSE course = new COURSE();
            var result = course.Insert(From, To, Name);
            if (result > 0)
            {
                TempData["return"] = "Insert Course successfully";
            }
            else
            {
                TempData["return"] = "Insert Course Fail";
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public ActionResult InsertSubject(List<string> Subjects)
        {
            Subjects = Subjects.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            SUBJECT sbj = new SUBJECT();
            var result = 0;
            foreach (var subject in Subjects)
            {
                if (!string.IsNullOrWhiteSpace(subject))
                {
                    result = sbj.Insert(subject);
                }
            }
            if (result > 0)
            {
                TempData["return"] = "Insert subject successfully";
            }
            else
            {
                TempData["return"] = "Insert subject fail";
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public ActionResult Assign()
        {
            COURSE course = new COURSE();
            List<COURSE> courses = course.Select();
            ViewBag.COURSE = courses;
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public ActionResult InsertUpdateEmployee(EMPLOYEE EMP, string Mode)
        {
            EMPLOYEE employeeManager = new EMPLOYEE();
            var result = 0;
            var path = Server.MapPath("~/Images/");
            foreach (string item in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[item];
                if (file.ContentLength == 0)
                {
                    //Repeated upload file be skipped .
                    continue;
                }
                string savedFileName = Path.Combine(path, Path.GetFileName(file.FileName));
                file.SaveAs(savedFileName);
                string fileName = Path.GetFileName(file.FileName);
                EMP.PICTURE = "Images\\" + fileName;
            }

            if (Mode == "0")
            {

                var em = employeeManager.Select(EMP.ID);
                if (em.Count > 0)
                    return Json(-1);
                result = employeeManager.Insert(EMP.ID, EMP.NAME, EMP.DEPARTMENT, EMP.PICTURE, EMP.ROLE, EMP.Encode("123456"));
            }
            else
            {
                if (EMP.PICTURE == null)
                    result = employeeManager.Update(EMP.ID, EMP.NAME, EMP.DEPARTMENT, EMP.ROLE);
                else
                    result = employeeManager.Update(EMP.ID, EMP.NAME, EMP.DEPARTMENT, EMP.PICTURE, EMP.ROLE);
            }
            if (result > 0)
            {
                TempData["return"] = "Add/Update new employee successfully";
            }
            else
            {
                TempData["return"] = "Add/Update new employee fail";
            }
            return RedirectToAction("Index", "Manage");
        }
    }
}
