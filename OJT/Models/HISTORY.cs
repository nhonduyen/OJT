using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJT
{
    public class HISTORY
    {
        public int ID { get; set; }
        public int COURSE_ID { get; set; }
        public string EMP_ID { get; set; }
        public int APPROVE { get; set; }
        public int SCORE { get; set; }
        public string RESULT_LEVEL { get; set; }

        public HISTORY(int ID, int COURSE_ID, string EMP_ID, int APPROVE, int SCORE, string RESULT_LEVEL)
        {
            this.ID = ID;
            this.COURSE_ID = COURSE_ID;
            this.EMP_ID = EMP_ID;
            this.APPROVE = APPROVE;
            this.SCORE = SCORE;
            this.RESULT_LEVEL = RESULT_LEVEL;
        }
        public HISTORY() { }

        public virtual List<HISTORY> Select(int ID=0)
        {
            var sql = "SELECT * FROM HISTORY ";
            if (ID == 0) return DBManager<HISTORY>.ExecuteReader(sql);
            sql +=" WHERE ID=@ID";

            return DBManager<HISTORY>.ExecuteReader(sql, new { ID = ID});
        }

        public virtual List<HISTORY> SelectPaging(int start=0, int end=10)
        {
            var sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by id) AS ROWNUM, * FROM HISTORY) as u  WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum;";

            return DBManager<HISTORY>.ExecuteReader(sql, new { start=start, end = end});
        }

        public virtual int GetCount()
        {
            var sql = "SELECT COUNT(1) AS CNT FROM HISTORY;";
            return (int) DBManager<HISTORY>.ExecuteScalar(sql);
        }

        public virtual int Insert(int COURSE_ID,string EMP_ID,int APPROVE,int SCORE,string RESULT_LEVEL)
        {
            var sql = "INSERT INTO HISTORY(COURSE_ID,EMP_ID,APPROVE,SCORE,RESULT_LEVEL) VALUES(@COURSE_ID,@EMP_ID,@APPROVE,@SCORE,@RESULT_LEVEL)";
            return DBManager<HISTORY>.Execute(sql, new { COURSE_ID = COURSE_ID,EMP_ID = EMP_ID,APPROVE = APPROVE,SCORE = SCORE,RESULT_LEVEL = RESULT_LEVEL});
        }

        public virtual int Update(int ID, int COURSE_ID, string EMP_ID, int APPROVE, int SCORE, string RESULT_LEVEL)
        {
            var sql = "UPDATE HISTORY SET COURSE_ID=@COURSE_ID,EMP_ID=@EMP_ID,APPROVE=@APPROVE,SCORE=@SCORE,RESULT_LEVEL=@RESULT_LEVEL WHERE ID=@ID";

            return DBManager<HISTORY>.Execute(sql,  new { ID = ID,COURSE_ID = COURSE_ID,EMP_ID = EMP_ID,APPROVE = APPROVE,SCORE = SCORE,RESULT_LEVEL = RESULT_LEVEL});
        }

        public virtual int Delete(int ID=0)
        {
            var sql = "DELETE FROM HISTORY ";
            if (ID == 0) return DBManager<HISTORY>.Execute(sql);
            sql += " WHERE ID=@ID ";
            return DBManager<HISTORY>.Execute(sql, new { ID = ID });
        }


    }

}
