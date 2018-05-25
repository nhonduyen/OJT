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
            return View();
        }

        [HttpPost]
        public ActionResult InsertCourse(string Name, string From, string To)
        {
            COURSE course = new COURSE();
            TempData["return"] = course.Insert(From, To, Name);
            return RedirectToAction("Index", "Manage");
        }

        [HttpPost]
        public ActionResult InsertSubject(List<string> Subjects)
        {
            Subjects = Subjects.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            SUBJECT sbj = new SUBJECT();

            foreach (var subject in Subjects)
            {
                if (!string.IsNullOrWhiteSpace(subject))
                {
                    TempData["return"] = sbj.Insert(subject);
                }
            }
           
            return RedirectToAction("Index", "Manage");
        }
    }
}
