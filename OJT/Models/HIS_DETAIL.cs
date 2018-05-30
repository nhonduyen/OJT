using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJT
{
    public class HIS_DETAIL
    {
        public int ID { get; set; }
        public int HIS_ID { get; set; }
        public int COURSE_ID { get; set; }
        public string EMP_ID { get; set; }
        public string MENTOR { get; set; }
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
        public string MANAGER_CMT { get; set; }
        public string HR_CMT { get; set; }

        public HIS_DETAIL(int ID, string EMP_ID, int COURSE_ID, int SUB_ID, string STATUS, string SUB_CONTENT, string SUB_LEVEL, DateTime START_DT, DateTime END_DT, string APPROVE, string OUTCOME_TEMPLATE, string OUTCOME_RESULT, DateTime REC_START_DT, DateTime REC_END_DT, DateTime TEST_TIME)
        {
            this.ID = ID;

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

        public virtual List<HIS_DETAIL> Select(int ID = 0)
        {
            var sql = "SELECT * FROM HIS_DETAIL ";
            if (ID == 0) return DBManager<HIS_DETAIL>.ExecuteReader(sql);
            sql += " WHERE ID=@ID";

            return DBManager<HIS_DETAIL>.ExecuteReader(sql, new { ID = ID });
        }

        public virtual List<HIS_DETAIL> SelectPaging(int start = 0, int end = 10)
        {
            var sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by id) AS ROWNUM, * FROM HIS_DETAIL) as u  WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum;";

            return DBManager<HIS_DETAIL>.ExecuteReader(sql, new { start = start, end = end });
        }

        public virtual int GetCount()
        {
            var sql = "SELECT COUNT(1) AS CNT FROM HIS_DETAIL;";
            return (int)DBManager<HIS_DETAIL>.ExecuteScalar(sql);
        }

        public virtual int Insert(string EMP_ID, int COURSE_ID, int SUB_ID, string STATUS, string SUB_CONTENT, string SUB_LEVEL, DateTime START_DT, DateTime END_DT, string APPROVE, string OUTCOME_TEMPLATE, string OUTCOME_RESULT, DateTime REC_START_DT, DateTime REC_END_DT, DateTime TEST_TIME)
        {
            var sql = "INSERT INTO HIS_DETAIL(EMP_ID,COURSE_ID,SUB_ID,STATUS,SUB_CONTENT,SUB_LEVEL,START_DT,END_DT,APPROVE,OUTCOME_TEMPLATE,OUTCOME_RESULT,REC_START_DT,REC_END_DT,TEST_TIME) VALUES(@EMP_ID,@COURSE_ID,@SUB_ID,@STATUS,@SUB_CONTENT,@SUB_LEVEL,@START_DT,@END_DT,@APPROVE,@OUTCOME_TEMPLATE,@OUTCOME_RESULT,@REC_START_DT,@REC_END_DT,@TEST_TIME)";
            return DBManager<HIS_DETAIL>.Execute(sql, new { EMP_ID = EMP_ID, COURSE_ID = COURSE_ID, SUB_ID = SUB_ID, STATUS = STATUS, SUB_CONTENT = SUB_CONTENT, SUB_LEVEL = SUB_LEVEL, START_DT = START_DT, END_DT = END_DT, APPROVE = APPROVE, OUTCOME_TEMPLATE = OUTCOME_TEMPLATE, OUTCOME_RESULT = OUTCOME_RESULT, REC_START_DT = REC_START_DT, REC_END_DT = REC_END_DT, TEST_TIME = TEST_TIME });
        }

        public virtual int Insert(int HIS_ID, int COURSE_ID, string EMP_ID, string MENTOR, string APPROVE = "NO")
        {
            var sql = "INSERT INTO HIS_DETAIL(HIS_ID,APPROVE,COURSE_ID,EMP_ID,MENTOR) VALUES(@HIS_ID,@APPROVE,@COURSE_ID,@EMP_ID,@MENTOR)";
            return DBManager<HIS_DETAIL>.Execute(sql, new
            {
                HIS_ID = HIS_ID,
                APPROVE = APPROVE,
                COURSE_ID = COURSE_ID,
                EMP_ID = EMP_ID,
                MENTOR = MENTOR
            });
        }

        public virtual int Update(int ID, string COLUMN, string VALUE)
        {
            var sql =string.Format(@"UPDATE HIS_DETAIL SET {0}=@VALUE WHERE ID=@ID", COLUMN);

            return DBManager<HIS_DETAIL>.Execute(sql, new { ID = ID, VALUE = VALUE});
        }

        public virtual int Update(int ID, int SUB_ID, string STATUS, string SUB_CONTENT, string SUB_LEVEL, DateTime START_DT, DateTime END_DT, string APPROVE, string OUTCOME_TEMPLATE, string OUTCOME_RESULT, DateTime REC_START_DT, DateTime REC_END_DT, DateTime TEST_TIME)
        {
            var sql = "UPDATE HIS_DETAIL SET SUB_ID=@SUB_ID,STATUS=@STATUS,SUB_CONTENT=@SUB_CONTENT,SUB_LEVEL=@SUB_LEVEL,START_DT=@START_DT,END_DT=@END_DT,APPROVE=@APPROVE,OUTCOME_TEMPLATE=@OUTCOME_TEMPLATE,OUTCOME_RESULT=@OUTCOME_RESULT,REC_START_DT=@REC_START_DT,REC_END_DT=@REC_END_DT,TEST_TIME=@TEST_TIME WHERE ID=@ID";

            return DBManager<HIS_DETAIL>.Execute(sql, new { ID = ID, SUB_ID = SUB_ID, STATUS = STATUS, SUB_CONTENT = SUB_CONTENT, SUB_LEVEL = SUB_LEVEL, START_DT = START_DT, END_DT = END_DT, APPROVE = APPROVE, OUTCOME_TEMPLATE = OUTCOME_TEMPLATE, OUTCOME_RESULT = OUTCOME_RESULT, REC_START_DT = REC_START_DT, REC_END_DT = REC_END_DT, TEST_TIME = TEST_TIME });
        }

        public virtual int UpdateByMentor(int ID, string STATUS, string SUB_CONTENT, string SUB_LEVEL, DateTime START_DT, DateTime END_DT)
        {
            var sql = "UPDATE HIS_DETAIL SET STATUS=@STATUS,SUB_CONTENT=@SUB_CONTENT,SUB_LEVEL=@SUB_LEVEL,START_DT=@START_DT,END_DT=@END_DT WHERE ID=@ID";

            return DBManager<HIS_DETAIL>.Execute(sql, new { ID = ID, STATUS = STATUS, SUB_CONTENT = SUB_CONTENT, SUB_LEVEL = SUB_LEVEL, START_DT = START_DT, END_DT = END_DT });
        }

        public virtual int UpdateByMentee(int ID, string OUTCOME_TEMPLATE, string OUTCOME_RESULT, DateTime REC_START_DT, DateTime REC_END_DT)
        {
            var sql = "UPDATE HIS_DETAIL SET OUTCOME_TEMPLATE=@OUTCOME_TEMPLATE,OUTCOME_RESULT=@OUTCOME_RESULT,REC_START_DT=@REC_START_DT,REC_END_DT=@REC_END_DT WHERE ID=@ID";

            return DBManager<HIS_DETAIL>.Execute(sql, new { ID = ID, OUTCOME_TEMPLATE = OUTCOME_TEMPLATE, OUTCOME_RESULT = OUTCOME_RESULT, REC_START_DT = REC_START_DT, REC_END_DT = REC_END_DT });
        }

        public virtual int Approve(int ID, string APPROVE)
        {
            var sql = "UPDATE HIS_DETAIL SET APPROVE=@APPROVE WHERE ID=@ID";

            return DBManager<HIS_DETAIL>.Execute(sql, new { ID = ID, APPROVE = APPROVE });
        }

        public virtual int HRComment(int ID, string HR_CMT)
        {
            var sql = "UPDATE HIS_DETAIL SET HR_CMT=@HR_CMT WHERE ID=@ID";

            return DBManager<HIS_DETAIL>.Execute(sql, new { ID = ID, HR_CMT = HR_CMT });
        }

        public virtual int MNGComment(int ID, string MANAGER_CMT)
        {
            var sql = "UPDATE HIS_DETAIL SET MANAGER_CMT=@MANAGER_CMT WHERE ID=@ID";

            return DBManager<HIS_DETAIL>.Execute(sql, new { ID = ID, MANAGER_CMT = MANAGER_CMT });
        }

        public virtual int UpdateTestTime(int ID, DateTime TEST_TIME)
        {
            var sql = "UPDATE HIS_DETAIL SET TEST_TIME=@TEST_TIME WHERE ID=@ID";

            return DBManager<HIS_DETAIL>.Execute(sql, new { ID = ID, TEST_TIME = TEST_TIME });
        }

        public virtual int Delete(int ID = 0)
        {
            var sql = "DELETE FROM HIS_DETAIL ";
            if (ID == 0) return DBManager<HIS_DETAIL>.Execute(sql);
            sql += " WHERE ID=@ID ";
            return DBManager<HIS_DETAIL>.Execute(sql, new { ID = ID });
        }


    }

}
