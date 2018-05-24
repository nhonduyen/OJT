using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJT
{
    public class HIS_DETAIL
    {
        public int ID { get; set; }
        public string EMP_ID { get; set; }
        public int COURSE_ID { get; set; }
        public int SUB_ID { get; set; }
        public string STATUS { get; set; }
        public string SUB_CONTENT { get; set; }
        public string SUB_LEVEL { get; set; }
        public DateTime START_DT { get; set; }
        public DateTime END_DT { get; set; }
        public string APPROVE { get; set; }
        public string OUTCOME_TEMPLATE { get; set; }
        public string OUTCOME_RESULT { get; set; }
        public DateTime REC_START_DT { get; set; }
        public DateTime REC_END_DT { get; set; }
        public DateTime TEST_TIME { get; set; }

        public HIS_DETAIL(int ID, string EMP_ID, int COURSE_ID, int SUB_ID, string STATUS, string SUB_CONTENT, string SUB_LEVEL, DateTime START_DT, DateTime END_DT, string APPROVE, string OUTCOME_TEMPLATE, string OUTCOME_RESULT, DateTime REC_START_DT, DateTime REC_END_DT, DateTime TEST_TIME)
        {
            this.ID = ID;
            this.EMP_ID = EMP_ID;
            this.COURSE_ID = COURSE_ID;
            this.SUB_ID = SUB_ID;
            this.STATUS = STATUS;
            this.SUB_CONTENT = SUB_CONTENT;
            this.SUB_LEVEL = SUB_LEVEL;
            this.START_DT = START_DT;
            this.END_DT = END_DT;
            this.APPROVE = APPROVE;
            this.OUTCOME_TEMPLATE = OUTCOME_TEMPLATE;
            this.OUTCOME_RESULT = OUTCOME_RESULT;
            this.REC_START_DT = REC_START_DT;
            this.REC_END_DT = REC_END_DT;
            this.TEST_TIME = TEST_TIME;
        }
        public HIS_DETAIL() { }

        public virtual List<HIS_DETAIL> Select(int ID=0)
        {
            var sql = "SELECT * FROM HIS_DETAIL ";
            if (ID == 0) return DBManager<HIS_DETAIL>.ExecuteReader(sql);
            sql +=" WHERE ID=@ID";

            return DBManager<HIS_DETAIL>.ExecuteReader(sql, new { ID = ID});
        }

        public virtual List<HIS_DETAIL> SelectPaging(int start=0, int end=10)
        {
            var sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by id) AS ROWNUM, * FROM HIS_DETAIL) as u  WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum;";

            return DBManager<HIS_DETAIL>.ExecuteReader(sql, new { start=start, end = end});
        }

        public virtual int GetCount()
        {
            var sql = "SELECT COUNT(1) AS CNT FROM HIS_DETAIL;";
            return (int) DBManager<HIS_DETAIL>.ExecuteScalar(sql);
        }

        public virtual int Insert(string EMP_ID,int COURSE_ID,int SUB_ID,string STATUS,string SUB_CONTENT,string SUB_LEVEL,DateTime START_DT,DateTime END_DT,string APPROVE,string OUTCOME_TEMPLATE,string OUTCOME_RESULT,DateTime REC_START_DT,DateTime REC_END_DT,DateTime TEST_TIME)
        {
            var sql = "INSERT INTO HIS_DETAIL(EMP_ID,COURSE_ID,SUB_ID,STATUS,SUB_CONTENT,SUB_LEVEL,START_DT,END_DT,APPROVE,OUTCOME_TEMPLATE,OUTCOME_RESULT,REC_START_DT,REC_END_DT,TEST_TIME) VALUES(@EMP_ID,@COURSE_ID,@SUB_ID,@STATUS,@SUB_CONTENT,@SUB_LEVEL,@START_DT,@END_DT,@APPROVE,@OUTCOME_TEMPLATE,@OUTCOME_RESULT,@REC_START_DT,@REC_END_DT,@TEST_TIME)";
            return DBManager<HIS_DETAIL>.Execute(sql, new { EMP_ID = EMP_ID,COURSE_ID = COURSE_ID,SUB_ID = SUB_ID,STATUS = STATUS,SUB_CONTENT = SUB_CONTENT,SUB_LEVEL = SUB_LEVEL,START_DT = START_DT,END_DT = END_DT,APPROVE = APPROVE,OUTCOME_TEMPLATE = OUTCOME_TEMPLATE,OUTCOME_RESULT = OUTCOME_RESULT,REC_START_DT = REC_START_DT,REC_END_DT = REC_END_DT,TEST_TIME = TEST_TIME});
        }

        public virtual int Update(int ID, string EMP_ID, int COURSE_ID, int SUB_ID, string STATUS, string SUB_CONTENT, string SUB_LEVEL, DateTime START_DT, DateTime END_DT, string APPROVE, string OUTCOME_TEMPLATE, string OUTCOME_RESULT, DateTime REC_START_DT, DateTime REC_END_DT, DateTime TEST_TIME)
        {
            var sql = "UPDATE HIS_DETAIL SET EMP_ID=@EMP_ID,COURSE_ID=@COURSE_ID,SUB_ID=@SUB_ID,STATUS=@STATUS,SUB_CONTENT=@SUB_CONTENT,SUB_LEVEL=@SUB_LEVEL,START_DT=@START_DT,END_DT=@END_DT,APPROVE=@APPROVE,OUTCOME_TEMPLATE=@OUTCOME_TEMPLATE,OUTCOME_RESULT=@OUTCOME_RESULT,REC_START_DT=@REC_START_DT,REC_END_DT=@REC_END_DT,TEST_TIME=@TEST_TIME WHERE ID=@ID";

            return DBManager<HIS_DETAIL>.Execute(sql,  new { ID = ID,EMP_ID = EMP_ID,COURSE_ID = COURSE_ID,SUB_ID = SUB_ID,STATUS = STATUS,SUB_CONTENT = SUB_CONTENT,SUB_LEVEL = SUB_LEVEL,START_DT = START_DT,END_DT = END_DT,APPROVE = APPROVE,OUTCOME_TEMPLATE = OUTCOME_TEMPLATE,OUTCOME_RESULT = OUTCOME_RESULT,REC_START_DT = REC_START_DT,REC_END_DT = REC_END_DT,TEST_TIME = TEST_TIME});
        }

        public virtual int Delete(int ID=0)
        {
            var sql = "DELETE FROM HIS_DETAIL ";
            if (ID == 0) return DBManager<HIS_DETAIL>.Execute(sql);
            sql += " WHERE ID=@ID ";
            return DBManager<HIS_DETAIL>.Execute(sql, new { ID = ID });
        }


    }

}
