using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJT
{
    public class EMPLOYEE
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string DEPARTMENT { get; set; }
        public string PICTURE { get; set; }
        public int ROLE { get; set; }
        public string PASSWORD { get; set; }

        public EMPLOYEE(string ID, string NAME, string DEPARTMENT, string PICTURE, int ROLE, string PASSWORD)
        {
            this.ID = ID;
            this.NAME = NAME;
            this.DEPARTMENT = DEPARTMENT;
            this.PICTURE = PICTURE;
            this.ROLE = ROLE;
            this.PASSWORD = PASSWORD;
        }
        public EMPLOYEE() { }

        public virtual List<EMPLOYEE> Select(string ID="")
        {
            var sql = "SELECT * FROM EMPLOYEE ";
            if (string.IsNullOrWhiteSpace(ID)) return DBManager<EMPLOYEE>.ExecuteReader(sql);
            sql +=" WHERE ID=@ID";

            return DBManager<EMPLOYEE>.ExecuteReader(sql, new { ID = ID});
        }

        public virtual List<EMPLOYEE> SelectPaging(int start=0, int end=10)
        {
            var sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by id) AS ROWNUM, * FROM EMPLOYEE) as u  WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum;";

            return DBManager<EMPLOYEE>.ExecuteReader(sql, new { start=start, end = end});
        }

        public virtual int GetCount()
        {
            var sql = "SELECT COUNT(1) AS CNT FROM EMPLOYEE;";
            return (int) DBManager<EMPLOYEE>.ExecuteScalar(sql);
        }

        public virtual int Insert(string ID, string NAME,string DEPARTMENT,string PICTURE,int ROLE,string PASSWORD)
        {
            var sql = "INSERT INTO EMPLOYEE(ID, NAME,DEPARTMENT,PICTURE,ROLE,PASSWORD) VALUES(@ID,@NAME,@DEPARTMENT,@PICTURE,@ROLE,@PASSWORD)";
            return DBManager<EMPLOYEE>.Execute(sql, new { ID=ID, NAME = NAME,DEPARTMENT = DEPARTMENT,PICTURE = PICTURE,ROLE = ROLE,PASSWORD = PASSWORD});
        }

        public virtual int Update(string ID, string NAME, string DEPARTMENT, string PICTURE, int ROLE)
        {
            var sql = "UPDATE EMPLOYEE SET NAME=@NAME,DEPARTMENT=@DEPARTMENT,PICTURE=@PICTURE,ROLE=@ROLE WHERE ID=@ID";

            return DBManager<EMPLOYEE>.Execute(sql,  new { ID = ID,NAME = NAME,DEPARTMENT = DEPARTMENT,PICTURE = PICTURE,ROLE = ROLE});
        }

        public virtual int Update(string ID, string NAME, string DEPARTMENT, int ROLE)
        {
            var sql = "UPDATE EMPLOYEE SET NAME=@NAME,DEPARTMENT=@DEPARTMENT,ROLE=@ROLE WHERE ID=@ID";

            return DBManager<EMPLOYEE>.Execute(sql, new { ID = ID, NAME = NAME, DEPARTMENT = DEPARTMENT, ROLE = ROLE });
        }

        public virtual int Delete(int ID=0)
        {
            var sql = "DELETE FROM EMPLOYEE ";
            if (ID == 0) return DBManager<EMPLOYEE>.Execute(sql);
            sql += " WHERE ID=@ID ";
            return DBManager<EMPLOYEE>.Execute(sql, new { ID = ID });
        }

        public string Encode(string value)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }
        public bool Login(string EMP_ID, string Password)
        {
            var sql = "SELECT ID,NAME,DEPARTMENT,ROLE FROM EMPLOYEE WHERE ID=@EMP_ID AND PASSWORD=@PASSWORD";
            EMPLOYEE employee = DBManager<EMPLOYEE>.ExecuteReader(sql, new { EMP_ID = EMP_ID, PASSWORD = Password }).FirstOrDefault();
            if (employee == null)
                return false;
            HttpContext.Current.Session["Username"] = employee.ID.Trim();
            HttpContext.Current.Session["Name"] = employee.NAME.Trim();
            HttpContext.Current.Session["Dept"] = employee.DEPARTMENT;
            HttpContext.Current.Session["Role"] = employee.ROLE;
            return true;
        }

        public int ChangePassword(string EmpId, string pass, string newpass)
        {
            var sql = "UPDATE EMPLOYEE SET PASSWORD=@NEWPASS WHERE ID=@EMP_ID AND PASSWORD=@PASS";
            return DBManager<EMPLOYEE>.Execute(sql, new { NEWPASS = newpass, EMP_ID = EmpId, PASS = pass });
        }
        public int ResetPassword(string EMP_ID)
        {
            var password = this.Encode("123456");
            var sql = "UPDATE EMPLOYEE SET PASSWORD=@PASSWORD WHERE ID=@EMP_ID";
            return DBManager<EMPLOYEE>.Execute(sql, new { EMP_ID = EMP_ID, PASSWORD = password });
        }

        public virtual List<EMPLOYEE> GetDepartment()
        {
            var sql = "SELECT DISTINCT(DEPARTMENT) FROM EMPLOYEE WHERE DEPARTMENT IS NOT NULL AND DEPARTMENT <> ''";
            return DBManager<EMPLOYEE>.ExecuteReader(sql);
        }

        public int IsMentor(int COURSE_ID, string ID)
        {
            var sql = "SELECT TOP 1 ID FROM HISTORY WHERE COURSE_ID=@COURSE_ID AND MENTOR=@ID";
            HISTORY his = DBManager<HISTORY>.ExecuteReader(sql, new { COURSE_ID = COURSE_ID , ID=ID }).FirstOrDefault();
            return his == null ? 0 : 1;
        }

        public List<EMPLOYEE> Search(string Key, int start = 0, int end = 10)
        {
            var sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by ID) AS ROWNUM, * FROM EMPLOYEE WHERE ID LIKE @KEY +'%' OR NAME LIKE '%' +@KEY+ '%' OR DEPARTMENT LIKE '%' +@KEY+ '%') as u  WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum;";

            return DBManager<EMPLOYEE>.ExecuteReader(sql, new { KEY = Key, start = start, end = end });
        }
        public int GetSearchCount(string Key)
        {
            var sql = "SELECT COUNT(1) AS CNT FROM EMPLOYEE WHERE ID LIKE @KEY +'%' OR NAME LIKE '%' +@KEY+ '%' OR DEPARTMENT LIKE '%' +@KEY+ '%';";
            return (int)DBManager<EMPLOYEE>.ExecuteScalar(sql, new { KEY = Key });
        }
    }

}
