using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;
using CC.MyTask;
using Lining_Tmall.Data;
using OpenQA.Selenium;

namespace Lining_Tmall.Task
{
    class GetLiningTmallData : AutoQueueTask<List<urlInfo>>
    {
        public static IWebDriver driver;
        static GetLiningTmallData _instance;
        public static GetLiningTmallData Instance
        {
            get
            {
                if (_instance == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _instance, new GetLiningTmallData(), null);
                }
                return _instance;
            }
        }
        GetLiningTmallData()
        {
            NoTaskSleepControl.Interval_ms = 24 * 3600 * 1000;
            driver = new OpenQA.Selenium.Chrome.ChromeDriver(@"C:\Program Files (x86)\Google\Chrome\Application");
        }
        public override string Name
        {
            get { return "Lining_Tmall"; }
        }

        protected override void NoTask()
        {
            string nikeUrl = "https://lining.tmall.com/i/asynSearch.htm?_ksTS=1489859374230_126&callback=jsonp127&mid=w-14681080882-0&wid=14681080882&path=/search.htm&&search=y&spm=a1z10.3-b-s.w4011-14681080882.520.uk4GXH&scene=taobao_shop&orderType=hotsell_desc&keyword=%D0%AC&tsearch=y&pageNo=";
            ShowMsg("<获取商品列表>");
            PageDataHelper ph = new PageDataHelper();
            var resultList = ph.GetIndexList(nikeUrl, 1, new List<urlInfo>());
            ShowMsg("<获取商品列表成功>");
            AddTask(resultList);
        }
        protected override void Fun(List<urlInfo> task)
        {
            System.Threading.Thread.Sleep(10 * 1000);
            string date = DateTime.Now.Date.ToShortDateString() + " 0:00:00";
            var gotList = ORMHelper.GetModel<Tmall_Name_Lining>(" where State = '" + Program.UpdateTimes + "' ");
            Dictionary<Int64, Tmall_Name_Lining> dic_Got = new Dictionary<long, Tmall_Name_Lining>();
            foreach (var dg in gotList)
            {
                if (!dic_Got.ContainsKey((Int64)dg.Id)) dic_Got.Add((Int64)dg.Id, dg);
            }
            int a = 0;
            List<Tmall_Detail_Lining> dsList = new List<Tmall_Detail_Lining>();
            List<Tmall_Name_Lining> nsList = new List<Tmall_Name_Lining>();
            foreach (var t in task)
            {
                if (dic_Got.ContainsKey((long)t.dataId))
                {
                    continue;
                }
                ShowMsg(t.dataId.ToString());
                var result = PageDataHelper.GotDetailData(t);
                Tmall_Detail_Lining td = new Tmall_Detail_Lining();
                Tmall_Name_Lining tn = new Tmall_Name_Lining();
                td.Id = tn.Id = (UInt64)result.Id;
                tn.Name = result.Name;
                td.LastUpdate = tn.LastUpdate = result.LastUpdate;
                td.IndexPrice = result.IndexPrice;
                td.AvePrice = result.AvePrice;
                td.Comments_Mon = result.IndexComment;
                td.Comments_Total = result.TotalComment;
                td.Repertory = (uint)result.Repertory;
                td.Sales_Mon = result.MonSales;
                td.Sales_Total = result.TotalSales;
                tn.State = td.State = sbyte.Parse(Program.UpdateTimes);
                ShowMsg(t.dataId + "  " + t.name + " " + td.AvePrice + " " + td.Sales_Mon + " " + td.Comments_Mon+td.LastUpdate);
                nsList.Add(tn);
                dsList.Add(td);
                ShowMsg("<加入一条数据>");
                DataToBase.SaveData(nsList);
                DataToBase.SaveData(dsList);
                Random random = new Random();
                int interval = random.Next(16, 55);
                ShowMsg(interval.ToString());
                System.Threading.Thread.Sleep(interval * 100);
            }
            //更新配置文件
            CC.Utility.iniHelper ini = new CC.Utility.iniHelper(Program.FilePath);
            ini.Write("state", "times", (sbyte.Parse(Program.UpdateTimes) + 1).ToString());
        }
    }
}
