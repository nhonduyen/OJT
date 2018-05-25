using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJT
{
    public class SUBJECT
    {
        public int ID { get; set; }
        public string NAME { get; set; }

        public SUBJECT(int ID, string NAME)
        {
            this.ID = ID;
            this.NAME = NAME;
        }
        public SUBJECT() { }

        public virtual List<SUBJECT> Select(int ID=0)
        {
            var sql = "SELECT * FROM SUBJECT ";
            if (ID == 0) return DBManager<SUBJECT>.ExecuteReader(sql);
            sql +=" WHERE ID=@ID";

            return DBManager<SUBJECT>.ExecuteReader(sql, new { ID = ID});
        }

        public virtual List<SUBJECT> SelectPaging(int start=0, int end=10)
        {
            var sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by id) AS ROWNUM, * FROM SUBJECT) as u  WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum;";

            return DBManager<SUBJECT>.ExecuteReader(sql, new { start=start, end = end});
        }

        public virtual int GetCount()
        {
            var sql = "SELECT COUNT(1) AS CNT FROM SUBJECT;";
            return (int) DBManager<SUBJECT>.ExecuteScalar(sql);
        }

        public virtual int Insert(string NAME)
        {
            var sql = "INSERT INTO SUBJECT(NAME) VALUES(@NAME)";
            return DBManager<SUBJECT>.Execute(sql, new { NAME = NAME});
        }

      
        public virtual int Update(int ID, string NAME)
        {
            var sql = "UPDATE SUBJECT SET NAME=@NAME WHERE ID=@ID";

            return DBManager<SUBJECT>.Execute(sql,  new { ID = ID,NAME = NAME});
        }

        public virtual int Delete(int ID=0)
        {
            var sql = "DELETE FROM SUBJECT ";
            if (ID == 0) return DBManager<SUBJECT>.Execute(sql);
            sql += " WHERE ID=@ID ";
            return DBManager<SUBJECT>.Execute(sql, new { ID = ID });
        }


    }

}
