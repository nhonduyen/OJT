using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OJT.Controllers
{
    public class AjaxController : Controller
    {
        //
        // GET: /Ajax/

        [HttpPost]
        public JsonResult GetEmployee(DataTableParameters dataTableParameters)
        {
            EMPLOYEE em = new EMPLOYEE();

            var resultSet = new DataTableResultSet();
            resultSet.draw = dataTableParameters.Draw;
            var lst = em.SelectPaging( dataTableParameters.Start + 1,
                dataTableParameters.Start + dataTableParameters.Length + 1);
            resultSet.recordsTotal = resultSet.recordsFiltered = em.GetCount();

            foreach (var i in lst)
            {
                string role = "";
                if (i.ROLE == 2)
                    role = "Manager";
                if (i.ROLE == 3)
                    role = "HR";
                var columns = new List<string>();
                columns.Add("<input type='checkbox' class='ckb' id='" + i.ID + "' />");
                columns.Add("<img src='" + i.PICTURE + "' class='img-rounded' height='100' width='100' alt='X' />");
                columns.Add(i.ID.ToString().Trim());
                columns.Add(i.NAME == null ? "" : i.NAME.Trim());
                columns.Add(i.DEPARTMENT == null ? "" : i.DEPARTMENT.Trim());
                columns.Add(role);
                resultSet.data.Add(columns);

            }
            return Json(resultSet);

        }

        [HttpPost]
        public JsonResult GetCourseById(DataTableParameters dataTableParameters, int ID)
        {
            COURSE course = new COURSE();

            var resultSet = new DataTableResultSet();
            resultSet.draw = dataTableParameters.Draw;
            var lst = course.GetCourseByID(ID,dataTableParameters.Start + 1,
                dataTableParameters.Start + dataTableParameters.Length + 1);
            resultSet.recordsTotal = resultSet.recordsFiltered = course.GetCount(ID);

            foreach (var i in lst)
            {
                var columns = new List<string>();

                columns.Add("<img src='" + i.PICTURE + "' class='img-rounded' height='100' width='100' alt='X' />");
                columns.Add(i.EMP_ID == null ? "" : i.EMP_ID.Trim());
                columns.Add(i.NAME == null ? "" : i.NAME.Trim());
                columns.Add(i.DEPARTMENT == null ? "" : i.DEPARTMENT.Trim());
                columns.Add(i.COURSE.Trim());
                columns.Add(i.TEACHER == null ? "" : i.TEACHER.Trim());
                resultSet.data.Add(columns);

            }
            return Json(resultSet);

        }

        [HttpPost]
        public JsonResult Assign(string MENTOR, int COURSE_ID, List<string> IDS)
        {
            var result = 0;
            HISTORY his = new HISTORY();
            HIS_DETAIL detail = new HIS_DETAIL();
            foreach (var id in IDS)
            {
                HISTORY last = his.GetLastId(COURSE_ID, id, MENTOR);
                if (last == null)
                {
                    result = his.Insert(COURSE_ID, id, MENTOR);
                    if (result > 0)
                    {
                        last = his.GetLastId(COURSE_ID, id, MENTOR);
                        if (last != null)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                result = detail.Insert(last.ID);
                            }
                        }
                    }
                }
            }
            return Json(result);

        }
    }
}
