using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJT
{
    public class ACTIVITY_IMG
    {
        public int DETAIL_ID { get; set; }
        public string IMG_URL { get; set; }

        public ACTIVITY_IMG(int DETAIL_ID, string IMG_URL)
        {
            this.DETAIL_ID = DETAIL_ID;
            this.IMG_URL = IMG_URL;
        }
        public ACTIVITY_IMG() { }

        public virtual List<ACTIVITY_IMG> Select(int ID=0)
        {
            var sql = "SELECT * FROM ACTIVITY_IMG ";
            if (ID == 0) return DBManager<ACTIVITY_IMG>.ExecuteReader(sql);
            sql += " WHERE DETAIL_ID=@ID";

            return DBManager<ACTIVITY_IMG>.ExecuteReader(sql, new { ID = ID});
        }

        public virtual List<ACTIVITY_IMG> SelectPaging(int start=0, int end=10)
        {
            var sql = "SELECT * FROM(SELECT ROW_NUMBER() OVER (order by id) AS ROWNUM, * FROM ACTIVITY_IMG) as u  WHERE   RowNum >= @start   AND RowNum < @end ORDER BY RowNum;";

            return DBManager<ACTIVITY_IMG>.ExecuteReader(sql, new { start=start, end = end});
        }

        public virtual int GetCount()
        {
            var sql = "SELECT COUNT(1) AS CNT FROM ACTIVITY_IMG;";
            return (int) DBManager<ACTIVITY_IMG>.ExecuteScalar(sql);
        }

        public virtual int Insert(int DETAIL_ID,string IMG_URL)
        {
            var sql = "INSERT INTO ACTIVITY_IMG(DETAIL_ID,IMG_URL) VALUES(@DETAIL_ID,@IMG_URL)";
            return DBManager<ACTIVITY_IMG>.Execute(sql, new { DETAIL_ID = DETAIL_ID,IMG_URL = IMG_URL});
        }

        public virtual int Update(int DETAIL_ID, string IMG_URL)
        {
            var sql = "UPDATE ACTIVITY_IMG SET DETAIL_ID=@DETAIL_ID,IMG_URL=@IMG_URL WHERE ID=@ID";

            return DBManager<ACTIVITY_IMG>.Execute(sql,  new { DETAIL_ID = DETAIL_ID,IMG_URL = IMG_URL});
        }

        public virtual int Delete(int ID=0)
        {
            var sql = "DELETE FROM ACTIVITY_IMG ";
            if (ID == 0) return DBManager<ACTIVITY_IMG>.Execute(sql);
            sql += " WHERE ID=@ID ";
            return DBManager<ACTIVITY_IMG>.Execute(sql, new { ID = ID });
        }


    }

}
