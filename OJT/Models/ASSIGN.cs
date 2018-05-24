using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJT
{
    public class ASSIGN
    {
        public int ID { get; set; }
        public string EMP_ID { get; set; }
        public string MENTOR { get; set; }
        public int COURSE_ID { get; set; }

        public ASSIGN(int ID, string EMP_ID, string MENTOR, int COURSE_ID)
        {
            this.ID = ID;
            this.EMP_ID = EMP_ID;
            this.MENTOR = MENTOR;
            this.COURSE_ID = COURSE_ID;
        }
        public ASSIGN() { }

        public virtual List<ASSIGN> Select(int ID=0)
        {
            var sql = "SELECT * FROM ASSIGN ";
            if (ID == 0) return DBManager<ASSIGN>.ExecuteReader(sql);
            sql +=" WHERE ID=@ID";

            return DBManager<ASSIGN>.ExecuteReader(sql, new { ID = ID});
        }

        public virtual List<ASSIGN> SelectPaging(int start=0, int end=10)
        {
            var sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by id) AS ROWNUM, * FROM ASSIGN) as u  WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum;";

            return DBManager<ASSIGN>.ExecuteReader(sql, new { start=start, end = end});
        }

        public virtual int GetCount()
        {
            var sql = "SELECT COUNT(1) AS CNT FROM ASSIGN;";
            return (int) DBManager<ASSIGN>.ExecuteScalar(sql);
        }

        public virtual int Insert(string EMP_ID,string MENTOR,int COURSE_ID)
        {
            var sql = "INSERT INTO ASSIGN(EMP_ID,MENTOR,COURSE_ID) VALUES(@EMP_ID,@MENTOR,@COURSE_ID)";
            return DBManager<ASSIGN>.Execute(sql, new { EMP_ID = EMP_ID,MENTOR = MENTOR,COURSE_ID = COURSE_ID});
        }

        public virtual int Update(int ID, string EMP_ID, string MENTOR, int COURSE_ID)
        {
            var sql = "UPDATE ASSIGN SET EMP_ID=@EMP_ID,MENTOR=@MENTOR,COURSE_ID=@COURSE_ID WHERE ID=@ID";

            return DBManager<ASSIGN>.Execute(sql,  new { ID = ID,EMP_ID = EMP_ID,MENTOR = MENTOR,COURSE_ID = COURSE_ID});
        }

        public virtual int Delete(int ID=0)
        {
            var sql = "DELETE FROM ASSIGN ";
            if (ID == 0) return DBManager<ASSIGN>.Execute(sql);
            sql += " WHERE ID=@ID ";
            return DBManager<ASSIGN>.Execute(sql, new { ID = ID });
        }


    }

}
