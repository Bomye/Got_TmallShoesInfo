using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;
using CC.MyTask;
using Tmall_Skechers.DATA;
using System.IO;

namespace Tmall_Skechers.TASK
{
    class Get_SkechersResult:AutoQueueTask<string>
    {
        static Get_SkechersResult _instance;
        public static Get_SkechersResult Instance
        {
            get
            {
                if (_instance == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _instance, new Get_SkechersResult(), null);
                }
                return _instance;
            }
        }
        Get_SkechersResult() { }
        public override string Name
        {
            get { return "Got_ResultData"; }
        }

        protected override void NoTask()
        {
            var first = ORMHelper.GetModel<Tmall_Skechers_Detail>(" where LastUpdate > '2017-03-19 5:51:33' and LastUpdate < '2017-03-19 23:59:34'");
            Dictionary<Int64, Tmall_Skechers_Detail> dic_First = first.ToDictionary(key => key.Id, Tmall_Skechers_Detail => Tmall_Skechers_Detail);

            var last = ORMHelper.GetModel<Tmall_Skechers_Detail>(" where LastUpdate > '2017-03-27 0:00:00' and LastUpdate < '2017-03-28 23:59:34'");
            Dictionary<Int64, Tmall_Skechers_Detail> dic_Last = last.ToDictionary(key => key.Id, Tmall_Skechers_Detail => Tmall_Skechers_Detail);

            List<Tmall_Skechers_Detail> putAway = new List<Tmall_Skechers_Detail>();
            List<Tmall_Skechers_Detail> saleOut = new List<Tmall_Skechers_Detail>();
            List<Tmall_Skechers_Detail> onSaling = new List<Tmall_Skechers_Detail>();
            #region 上架           
            using (StreamWriter sw = new StreamWriter("新品.csv", false, Encoding.Default))
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}","商品ID","首页价格","本日月销量","总销量","库存","月评价","总评价");
                foreach (var it in last)
                {
                    if (!dic_First.ContainsKey(it.Id))
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "=\"" + it.Id+"\"", it.indexPrice, it.Sales_Mon, it.Sales_Total, it.Repertory, it.Comments_Mon, it.Comments_Total);
                }
                sw.Close();
                ShowMsg("新上架写入完成");
            }
            #endregion

            #region 下架
            using (StreamWriter sw = new StreamWriter("下架.csv", false, Encoding.Default))
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "商品ID", "首页价格", "本日月销量", "总销量", "库存", "月评价", "总评价");
                foreach (var it in first)
                {
                    if (!dic_Last.ContainsKey(it.Id))
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "=\"" + it.Id + "\"", it.indexPrice, it.Sales_Mon, it.Sales_Total, it.Repertory, it.Comments_Mon, it.Comments_Total);
                }
                sw.Close();
                ShowMsg("下架写入完成");
            }
            #endregion

            #region 热卖
            using (StreamWriter sw = new StreamWriter("热卖.csv", false, Encoding.Default))
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", "商品ID", "首页价格(前)", "首页价格(本)", "上期月销量","本日月销量", "上期总销量","总销量", "上期库存","库存", "月评价", "总评价");
                foreach (var it in last)
                {
                    if (dic_First.ContainsKey(it.Id))
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", "=\"" + it.Id + "\"", dic_First[it.Id].indexPrice, it.indexPrice, dic_First[it.Id].Sales_Mon, it.Sales_Mon, dic_First[it.Id].Sales_Total,  it.Sales_Total, dic_First[it.Id].Repertory, it.Repertory, it.Comments_Mon, it.Comments_Total);
                }
                sw.Close();
                ShowMsg("热卖商品写入完成");
            }
            #endregion

        }

        protected override void Fun(string task)
        {
            
        }
    }
}
