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
        public string MENTOR { get; set; }
        public int SCORE { get; set; }
        public int ARCHIEVEMENT { get; set; }
        public string RESULT_LEVEL { get; set; }

        public HISTORY(int ID, int COURSE_ID, string EMP_ID, int APPROVE, int SCORE, string RESULT_LEVEL)
        {
            this.ID = ID;
            this.COURSE_ID = COURSE_ID;
            this.EMP_ID = EMP_ID;
            this.SCORE = SCORE;
            this.RESULT_LEVEL = RESULT_LEVEL;
        }
        public HISTORY() { }

        public virtual List<HISTORY> Select(int ID = 0)
        {
            var sql = "SELECT * FROM HISTORY ";
            if (ID == 0) return DBManager<HISTORY>.ExecuteReader(sql);
            sql += " WHERE ID=@ID";

            return DBManager<HISTORY>.ExecuteReader(sql, new { ID = ID });
        }

        public virtual HISTORY GetLastId(int COURSE_ID, string EMP_ID, string MENTOR)
        {
            var sql = "SELECT ID FROM HISTORY WHERE COURSE_ID=@COURSE_ID AND EMP_ID=@EMP_ID AND MENTOR=@MENTOR";

            return DBManager<HISTORY>.ExecuteReader(sql, new { MENTOR = MENTOR, COURSE_ID = COURSE_ID, EMP_ID = EMP_ID }).FirstOrDefault();
        }

        public virtual List<HISTORY> SelectPaging(int start = 0, int end = 10)
        {
            var sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by id) AS ROWNUM, * FROM HISTORY) as u  WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum;";

            return DBManager<HISTORY>.ExecuteReader(sql, new { start = start, end = end });
        }

        public virtual int GetCount()
        {
            var sql = "SELECT COUNT(1) AS CNT FROM HISTORY;";
            return (int)DBManager<HISTORY>.ExecuteScalar(sql);
        }

        public virtual int Insert(int COURSE_ID, string EMP_ID, string MENTOR)
        {
            var sql = "INSERT INTO HISTORY(COURSE_ID,EMP_ID,MENTOR) VALUES(@COURSE_ID,@EMP_ID,@MENTOR)";
            return DBManager<HISTORY>.Execute(sql, new { COURSE_ID = COURSE_ID, EMP_ID = EMP_ID, MENTOR = MENTOR });
        }

        public virtual int Update(int ID, int COURSE_ID, string EMP_ID, int MENTOR, int SCORE, string RESULT_LEVEL)
        {
            var sql = "UPDATE HISTORY SET COURSE_ID=@COURSE_ID,EMP_ID=@EMP_ID,MENTOR=@MENTOR,SCORE=@SCORE,RESULT_LEVEL=@RESULT_LEVEL WHERE ID=@ID";

            return DBManager<HISTORY>.Execute(sql, new { ID = ID, COURSE_ID = COURSE_ID, EMP_ID = EMP_ID, MENTOR = MENTOR, SCORE = SCORE, RESULT_LEVEL = RESULT_LEVEL });
        }

        public virtual int Update(int ID, int SCORE, string RESULT_LEVEL)
        {
            var sql = "UPDATE HISTORY SET SCORE=@SCORE,RESULT_LEVEL=@RESULT_LEVEL WHERE ID=@ID";

            return DBManager<HISTORY>.Execute(sql, new { ID = ID, SCORE = SCORE, RESULT_LEVEL = RESULT_LEVEL });
        }

        public virtual int Update(int ID, string COLUMN, string VALUE)
        {
            var sql = string.Format(@"UPDATE HISTORY SET {0}=@VALUE WHERE ID=@ID", COLUMN);

            return DBManager<HISTORY>.Execute(sql, new { ID = ID, VALUE = VALUE });
        }

        public virtual int Delete(int ID = 0)
        {
            var sql = "DELETE FROM HISTORY ";
            if (ID == 0) return DBManager<HISTORY>.Execute(sql);
            sql += " WHERE ID=@ID ";
            return DBManager<HISTORY>.Execute(sql, new { ID = ID });
        }

        public List<dynamic> GetHistory(string mentor = "", string mentee = "", int course_id = 0, string dept = "", int page = 1)
        {
            var start = (page - 1) * 10 + 1;
            var end = start + 10 - 1;
            var sql = string.Format(@"
SELECT * FROM (
SELECT ROW_NUMBER() OVER (ORDER BY H.EMP_ID) AS ROWNUM,
E.NAME AS EMP_NAME, PICTURE,DEPARTMENT,ARCHIEVEMENT,H.MENTOR AS MENTOR1,S.NAME AS SUBJECT,
C.NAME AS PERIOD, SCORE,RESULT_LEVEL,D.*,
(SELECT COUNT(1) FROM ACTIVITY_IMG AS IMG WHERE D.ID=IMG.DETAIL_ID) AS ACTIVITY
FROM COURSE AS C
INNER JOIN HISTORY AS H ON C.ID=H.COURSE_ID
INNER JOIN HIS_DETAIL AS D ON H.ID=D.HIS_ID
INNER JOIN EMPLOYEE AS E ON E.ID=H.EMP_ID
LEFT JOIN SUBJECT AS S ON S.ID=D.SUB_ID
WHERE (@MENTOR='' OR H.MENTOR=@MENTOR) AND (@EMP_ID='' OR H.EMP_ID=@EMP_ID) AND (@COURSE_ID=0 OR H.COURSE_ID=@COURSE_ID) 
AND (@DEPT='' OR DEPARTMENT LIKE '%'+@DEPT+'%')) AS U
 WHERE ROWNUM BETWEEN @start AND @end ORDER BY ROWNUM;
");
            return DBManager<HISTORY>.ExecuteDynamic(sql, new
            {
                MENTOR = mentor,
                COURSE_ID = course_id,
                EMP_ID = mentee,
                DEPT = dept,
                start = start,
                end = end
            });
        }
        public List<dynamic> GetHistoryExport(string mentor = "", string mentee = "", int course_id = 0, string dept = "")
        {

            var sql = string.Format(@"
SELECT 
E.NAME AS EMP_NAME, PICTURE,DEPARTMENT,H.MENTOR AS MENTOR1,S.NAME AS SUBJECT,ARCHIEVEMENT,
C.NAME AS PERIOD, SCORE,RESULT_LEVEL,D.*
FROM COURSE AS C
INNER JOIN HISTORY AS H ON C.ID=H.COURSE_ID
INNER JOIN HIS_DETAIL AS D ON H.ID=D.HIS_ID
INNER JOIN EMPLOYEE AS E ON E.ID=H.EMP_ID
LEFT JOIN SUBJECT AS S ON S.ID=D.SUB_ID
WHERE (@MENTOR='' OR H.MENTOR=@MENTOR) AND (@EMP_ID='' OR H.EMP_ID=@EMP_ID) AND (@COURSE_ID=0 OR H.COURSE_ID=@COURSE_ID) 
AND (@DEPT='' OR DEPARTMENT LIKE '%'+@DEPT+'%')
");
            return DBManager<HISTORY>.ExecuteDynamic(sql, new
            {
                MENTOR = mentor,
                COURSE_ID = course_id,
                EMP_ID = mentee,
                DEPT = dept
            });
        }
        public List<dynamic> GetHistorySimple(string mentor = "", string mentee = "", int course_id = 0, string dept = "", int page = 1)
        {
            var start = (page - 1) * 10 + 1;
            var end = start + 10 - 1;
            var sql = string.Format(@"
SELECT * FROM (
SELECT ROW_NUMBER() OVER (order by COURSE_ID desc) AS ROWNUM,C.NAME AS COURSE,H.COURSE_ID,E.ID,E.DEPARTMENT,E.NAME,H.RESULT_LEVEL,SCORE
FROM COURSE AS C
INNER JOIN HISTORY AS H ON C.ID=H.COURSE_ID
INNER JOIN EMPLOYEE AS E ON E.ID=H.EMP_ID
WHERE (@MENTOR='' OR H.MENTOR=@MENTOR) AND (@EMP_ID='' OR H.EMP_ID=@EMP_ID) AND (@COURSE_ID=0 OR H.COURSE_ID=@COURSE_ID)
AND (@DEPARTMENT='' OR DEPARTMENT LIKE '%'+@DEPARTMENT+'%')) AS U
WHERE ROWNUM BETWEEN @start AND @end ORDER BY ROWNUM");
            return DBManager<HISTORY>.ExecuteDynamic(sql, new
            {
                MENTOR = mentor,
                COURSE_ID = course_id,
                EMP_ID = mentee,
                DEPARTMENT = dept,
                start = start,
                end = end
            });
        }
        public List<dynamic> GetHistorySimpleExport(string mentor = "", string mentee = "", int course_id = 0, string dept = "")
        {

            var sql = string.Format(@"
SELECT ROW_NUMBER() OVER (order by COURSE_ID desc) AS ROWNUM,C.NAME AS COURSE,H.COURSE_ID,E.ID,E.DEPARTMENT,E.NAME,H.RESULT_LEVEL,SCORE
FROM COURSE AS C
INNER JOIN HISTORY AS H ON C.ID=H.COURSE_ID
INNER JOIN EMPLOYEE AS E ON E.ID=H.EMP_ID
WHERE (@MENTOR='' OR H.MENTOR=@MENTOR) AND (@EMP_ID='' OR H.EMP_ID=@EMP_ID) AND (@COURSE_ID=0 OR H.COURSE_ID=@COURSE_ID)
AND (@DEPARTMENT='' OR DEPARTMENT LIKE '%'+@DEPARTMENT+'%')
");
            return DBManager<HISTORY>.ExecuteDynamic(sql, new
            {
                MENTOR = mentor,
                COURSE_ID = course_id,
                EMP_ID = mentee,
                DEPARTMENT = dept
            });
        }

        public int CountHistorySimpleTotal(string mentor = "", string mentee = "", string dept = "", int course_id = 0)
        {

            var sql = string.Format(@"
SELECT COUNT(1) AS CNT
FROM HISTORY AS H 
INNER JOIN EMPLOYEE AS  E ON E.ID=H.EMP_ID
WHERE (@COURSE_ID='' OR H.COURSE_ID=@COURSE_ID) AND (@MENTOR='' OR H.MENTOR=@MENTOR) AND (@EMP_ID='' OR H.EMP_ID=@EMP_ID) AND (@DEPARTMENT='' OR DEPARTMENT LIKE '%'+@DEPARTMENT+'%')

");
            return (int)DBManager<HISTORY>.ExecuteScalar(sql, new
            {
                MENTOR = mentor,
                COURSE_ID = course_id,
                EMP_ID = mentee,
                DEPARTMENT = dept
            });
        }

        public int CountHistory(string mentor = "", string mentee = "", int course_id = 0, string dept = "")
        {
            var sql = string.Format(@"
SELECT COUNT(1)
FROM HISTORY AS H
INNER JOIN HIS_DETAIL AS D ON H.ID=D.HIS_ID
INNER JOIN EMPLOYEE AS E ON E.ID=D.EMP_ID
WHERE (@MENTOR='' OR H.MENTOR=@MENTOR) AND (@EMP_ID='' OR H.EMP_ID=@EMP_ID) AND (@COURSE_ID=0 OR H.COURSE_ID=@COURSE_ID) 
AND (@DEPT='' OR DEPARTMENT LIKE '%'+DEPT+'%')
");
            return (int)DBManager<HISTORY>.ExecuteScalar(sql, new
            {
                MENTOR = mentor,
                COURSE_ID = course_id,
                EMP_ID = mentee,
                DEPT = dept
            });
        }
    }

}
