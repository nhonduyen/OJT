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

        public virtual List<EMPLOYEE> Select(int ID=0)
        {
            var sql = "SELECT * FROM EMPLOYEE ";
            if (ID == 0) return DBManager<EMPLOYEE>.ExecuteReader(sql);
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

        public virtual int Insert(string NAME,string DEPARTMENT,string PICTURE,int ROLE,string PASSWORD)
        {
            var sql = "INSERT INTO EMPLOYEE(NAME,DEPARTMENT,PICTURE,ROLE,PASSWORD) VALUES(@NAME,@DEPARTMENT,@PICTURE,@ROLE,@PASSWORD)";
            return DBManager<EMPLOYEE>.Execute(sql, new { NAME = NAME,DEPARTMENT = DEPARTMENT,PICTURE = PICTURE,ROLE = ROLE,PASSWORD = PASSWORD});
        }

        public virtual int Update(string ID, string NAME, string DEPARTMENT, string PICTURE, int ROLE, string PASSWORD)
        {
            var sql = "UPDATE EMPLOYEE SET NAME=@NAME,DEPARTMENT=@DEPARTMENT,PICTURE=@PICTURE,ROLE=@ROLE,PASSWORD=@PASSWORD WHERE ID=@ID";

            return DBManager<EMPLOYEE>.Execute(sql,  new { ID = ID,NAME = NAME,DEPARTMENT = DEPARTMENT,PICTURE = PICTURE,ROLE = ROLE,PASSWORD = PASSWORD});
        }

        public virtual int Delete(int ID=0)
        {
            var sql = "DELETE FROM EMPLOYEE ";
            if (ID == 0) return DBManager<EMPLOYEE>.Execute(sql);
            sql += " WHERE ID=@ID ";
            return DBManager<EMPLOYEE>.Execute(sql, new { ID = ID });
        }


    }

}
