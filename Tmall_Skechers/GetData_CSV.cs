using Tmall_Skechers.DATA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CC.ORM;

namespace Tmall_Skechers
{
    class GetData_CSV
    {
        public static void GotResultCsv(string excelPath, string tableName, string excelName)
        {
            var dic_Excel = ExcelToDs(excelPath,tableName,excelName);
            List<Tmall_Skechers_Detail> orm_List = ORMHelper.GetModel<Tmall_Skechers_Detail>("where  LastUpdate > '2017-04-10 12:00:00'");
            Dictionary<ulong, Tmall_Skechers_Detail> orm_Dic = new Dictionary<ulong, Tmall_Skechers_Detail>();
            foreach (var ol in orm_List)
            {
                if (!orm_Dic.ContainsKey((ulong)ol.Id)) orm_Dic.Add((ulong)ol.Id, ol);
            }
            var dic_Xiajia = new Dictionary<ulong, string[]>();
            var dic_Shangjia = new Dictionary<ulong, Tmall_Skechers_Detail>();
            var dic_SjNo = new Dictionary<ulong, Tmall_Skechers_Detail>();//上期没有数据但是以前有的

            #region 本次没有 上次-本次
            foreach (var del in dic_Excel)
            {
                if (!orm_Dic.ContainsKey((UInt64)del.Key))
                    dic_Xiajia.Add((UInt64)del.Key, del.Value);
            }
            #endregion

            #region 本次新上
            foreach (var od in orm_Dic)
            {
                if (!dic_Excel.ContainsKey((Int64)od.Key))
                    dic_Shangjia.Add(od.Key, od.Value);
            }
            #endregion
            string date = DateTime.Now.Date.ToShortDateString();
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("result_Skechers.csv", false, Encoding.UTF8))
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}", "商品ID", "首页价格（周）", "", "", "首页价格（月）", "期末月销量", "", "", "期末总销量", "", "", "库存", "", "", "月评论", "总评论", "期末月销售额", "期间销售量", "期间销售额");
                sw.WriteLine("{0}", "新品");
                List<long> have = new List<long>();
                foreach (var dsj in dic_Shangjia)
                {
                    if (IsNew((long)dsj.Key, dsj.Value.LastUpdate, "tmall_skechers_detail"))
                    {
                        have.Add((long)dsj.Key);
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", "=" + "\"" + dsj.Key + "\"", 0, 0, dsj.Value.indexPrice, 0, 0, 0, dsj.Value.Sales_Mon, 0, 0, dsj.Value.Sales_Total, 0, 0, dsj.Value.Repertory, dsj.Value.Comments_Mon, dsj.Value.Comments_Total);
                    }
                    else
                    {
                        dic_SjNo.Add(dsj.Key, dsj.Value);
                    }
                }
                sw.WriteLine("{0}", "旧款");
                foreach (var dx in dic_Xiajia)
                {
                    have.Add((long)dx.Key);
                    sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", "=" + "\"" + dx.Key + "\"", dx.Value[1], dx.Value[2], 0, dx.Value[3], dx.Value[4], dx.Value[5], 0, dx.Value[6], dx.Value[7], 0, dx.Value[8], dx.Value[9], 0, 0, 0, 0, 0);
                }
                sw.WriteLine("{0}", "热卖");
                foreach (var die in dic_Excel)
                {
                    if (!have.Contains((long)die.Key))
                    {
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", "=" + "\"" + die.Key + "\"", die.Value[1], die.Value[2], orm_Dic[(ulong)die.Key].indexPrice, die.Value[3], die.Value[4], die.Value[5], orm_Dic[(ulong)die.Key].Sales_Mon, die.Value[6], die.Value[7], orm_Dic[(ulong)die.Key].Sales_Total, die.Value[8], die.Value[9], orm_Dic[(ulong)die.Key].Repertory, orm_Dic[(ulong)die.Key].Comments_Mon, orm_Dic[(ulong)die.Key].Comments_Total, 0, 0);
                    }
                }
                sw.Close();
            }
            Console.WriteLine("---------输出Csv----------");
        }
        /// <summary>
        /// 判断是否为新商品
        /// </summary>
        /// <param name="Id">商品Id</param>
        /// <param name="lastUpdate">本次更新时间</param>
        /// <param name="T">库名</param>
        /// <returns></returns>
        static private bool IsNew(long Id, DateTime lastUpdate, string T)
        {
            var dc = ORMHelper.DefaultDataFactory.Create();
            using (var dr = dc.ExecuteReader("select * from " + T + " where Id = " + Id + " and LastUpdate < '" + lastUpdate + "'"))
            {
                if (dr.Read())
                {
                    return true;
                }
            }
            return false;
        }
        private static Dictionary<Int64, string[]> ExcelToDs(string path,string tableName,string excelName)
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + path + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "select * from ["+tableName+"$]";
            OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel,strConn );
            DataSet ds = new DataSet();
            myCommand.Fill(ds, excelName);
            int nullCell = 0, issueCo = 0;
            bool rowEnd = false;
            Dictionary<Int64, string[]> dic_Excel = new Dictionary<Int64, string[]>();
            foreach (DataRow col in ds.Tables[0].Rows)
            {
                string[] data = new string[30];
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    data[i] = col.ItemArray[i].ToString();
                }
                if (!rowEnd)
                {
                    foreach (DataColumn row in ds.Tables[0].Columns)
                    {
                        if (col[row].ToString() == "" && !rowEnd) nullCell++;
                        if (col[row].ToString() == "期间销售额") rowEnd = true;
                        Console.WriteLine(col[row].ToString());
                        issueCo = (nullCell + 4) / 4;//计算总期数
                    }
                }

                string fi = col.ItemArray[0].ToString();
                if (Regex.IsMatch(fi, "[0-9]+"))
                {
                    Int64 id = Convert.ToInt64(fi);
                    if (!dic_Excel.ContainsKey(id)) dic_Excel.Add(id, new string[] { });
                    dic_Excel[id] = data;
                }
            }
            Console.WriteLine("成功读取Excel");
            return dic_Excel;
        }
    }
}
