using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJT
{
    public class COURSE
    {
        public int ID { get; set; }
        public string FROM_MONTH { get; set; }
        public string TO_MONTH { get; set; }
        public string NAME { get; set; }

        public COURSE(int ID, string FROM_MONTH, string TO_MONTH)
        {
            this.ID = ID;
            this.FROM_MONTH = FROM_MONTH;
            this.TO_MONTH = TO_MONTH;
        }
        public COURSE() { }

        public virtual List<COURSE> Select(int ID = 0)
        {
            var sql = "SELECT * FROM COURSE ";
            if (ID == 0) return DBManager<COURSE>.ExecuteReader(sql);
            sql += " WHERE ID=@ID";

            return DBManager<COURSE>.ExecuteReader(sql, new { ID = ID });
        }

        public virtual List<COURSE> SelectPaging(int start = 0, int end = 10)
        {
            var sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by id) AS ROWNUM, * FROM COURSE) as u  WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum;";

            return DBManager<COURSE>.ExecuteReader(sql, new { start = start, end = end });
        }

        public virtual int GetCount()
        {
            var sql = "SELECT COUNT(1) AS CNT FROM COURSE;";
            return (int)DBManager<COURSE>.ExecuteScalar(sql);
        }

        public virtual int Insert(string FROM_MONTH, string TO_MONTH, string NAME)
        {
            var sql = "INSERT INTO COURSE(FROM_MONTH,TO_MONTH,NAME) VALUES(@FROM_MONTH,@TO_MONTH,@NAME)";
            return DBManager<COURSE>.Execute(sql, new
            {
                FROM_MONTH = FROM_MONTH,
                TO_MONTH = TO_MONTH,
                NAME = NAME
            });
        }

        public virtual int Update(int ID, string FROM_MONTH, string TO_MONTH, string NAME)
        {
            var sql = "UPDATE COURSE SET FROM_MONTH=@FROM_MONTH,TO_MONTH=@TO_MONTH, NAME=@NAME WHERE ID=@ID";

            return DBManager<COURSE>.Execute(sql, new
            {
                ID = ID,
                FROM_MONTH = FROM_MONTH,
                TO_MONTH = TO_MONTH,
                NAME = NAME
            });
        }

        public virtual int Delete(int ID = 0)
        {
            var sql = "DELETE FROM COURSE ";
            if (ID == 0) return DBManager<COURSE>.Execute(sql);
            sql += " WHERE ID=@ID ";
            return DBManager<COURSE>.Execute(sql, new { ID = ID });
        }


    }

}
