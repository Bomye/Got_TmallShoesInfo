using Adidas_Tmall.DATA;
using CC.MyTask;
using CC.ORM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adidas_Tmall.TASK
{
    class GetAdidasResult:AutoQueueTask<string>
    {
        static GetAdidasResult _instance;
        public static GetAdidasResult Instance
        {
            get
            {
                if (_instance == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _instance, new GetAdidasResult(), null);
                }
                return _instance;
            }
        }
        GetAdidasResult() { }
        public override string Name
        {
            get { return "Got_ResultData"; }
        }

        protected override void NoTask()
        {
            var first = ORMHelper.GetModel<Tmall_Detail_Ad>(" where LastUpdate > '2017-03-18 5:51:33' and LastUpdate < '2017-03-19 0:59:34'");
            Dictionary<UInt64, Tmall_Detail_Ad> dic_First = first.ToDictionary(key => key.Id, Tmall_Detail_Ad => Tmall_Detail_Ad);

            var last = ORMHelper.GetModel<Tmall_Detail_Ad>(" where LastUpdate > '2017-03-27 0:00:00' and LastUpdate < '2017-03-28 23:59:34'");
            Dictionary<UInt64, Tmall_Detail_Ad> dic_Last = last.ToDictionary(key => key.Id, Tmall_Detail_Ad => Tmall_Detail_Ad);

            List<Tmall_Detail_Ad> putAway = new List<Tmall_Detail_Ad>();
            List<Tmall_Detail_Ad> saleOut = new List<Tmall_Detail_Ad>();
            List<Tmall_Detail_Ad> onSaling = new List<Tmall_Detail_Ad>();
            #region 上架
            using (StreamWriter sw = new StreamWriter("新品.csv", false, Encoding.Default))
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "商品ID", "首页价格", "本日月销量", "本日总销量", "本日库存", "本日月评价", "本日总评价");
                foreach (var it in last)
                {
                    if (!dic_First.ContainsKey(it.Id))
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "=\"" + it.Id + "\"", it.Price, it.Sales_Month, it.Sales_Total, it.Repertory, it.Comments_Mon, it.Comments_Total);
                }
                sw.Close();
                ShowMsg("新上架写入完成");
            }
            #endregion

            #region 下架
            using (StreamWriter sw = new StreamWriter("下架.csv", false, Encoding.Default))
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "商品ID", "首页价格", "上期日月销量", "上期总销量", "上期库存", "上期月评价", "上期总评价");
                foreach (var it in first)
                {
                    if (!dic_Last.ContainsKey(it.Id))
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", "=\"" + it.Id + "\"", it.Price, it.Sales_Month, it.Sales_Total, it.Repertory, it.Comments_Mon, it.Comments_Total);
                }
                sw.Close();
                ShowMsg("下架写入完成");
            }
            #endregion

            #region 热卖
            using (StreamWriter sw = new StreamWriter("热卖.csv", false, Encoding.Default))
            {
                sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", "商品ID", "首页价格(前)", "首页价格(本)", "上期月销量", "本日月销量", "上期总销量", "总销量", "上期库存", "库存", "月评价", "总评价");
                foreach (var it in last)
                {
                    if (dic_First.ContainsKey(it.Id))
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", "=\"" + it.Id + "\"", dic_First[it.Id].Price, it.Price, dic_First[it.Id].Sales_Month, it.Sales_Month, dic_First[it.Id].Sales_Total, it.Sales_Total, dic_First[it.Id].Repertory, it.Repertory, it.Comments_Mon, it.Comments_Total);
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
