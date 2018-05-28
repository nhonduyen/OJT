using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OJT.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Manage/

        public ActionResult Index()
        {
            EMPLOYEE EM = new EMPLOYEE();
            var dept =EM.GetDepartment();
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
    }
}
