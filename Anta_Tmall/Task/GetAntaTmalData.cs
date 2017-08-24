using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;
using CC.MyTask;
using Anta_Tmall.Data;
using OpenQA.Selenium;

namespace Anta_Tmall.Task
{
    class GetAntaTmalData : AutoQueueTask<List<urlInfo>>
    {
        public static IWebDriver driver;
        static GetAntaTmalData _instance;
        public static GetAntaTmalData Instance
        {
            get
            {
                if (_instance == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _instance, new GetAntaTmalData(), null);
                }
                return _instance;
            }
        }
        GetAntaTmalData() { 
            NoTaskSleepControl.Interval_ms = 24 * 3600 * 1000;
            driver = new OpenQA.Selenium.Chrome.ChromeDriver(@"C:\Program Files (x86)\Google\Chrome\Application");
        }
        public override string Name
        {
            get { return "Anta_Tmall"; }
        }

        protected override void NoTask()
        {
            string[] antaUrl = new string[]
            {
                "https://anta.tmall.com/view_shop.htm?spm=a1z10.3-b-s.w4011-14727303322.493.1LywM1&type=p&search=y&newHeader_b=s_from&searcy_type=item&from=.shop.pc_2_searchbutton&keyword=%D0%AC&tsearch=y&pageNo=",
                "https://antakids.tmall.com/view_shop.htm?spm=a1z10.3-b-s.w4011-14681036885.92.kaIptO&shopId=106096685&search=y&orderType=hotsell_desc&keyword=Ь&pageNo="
            };
            
            ShowMsg("<获取商品列表>");
            var resultList = new List<urlInfo>();
            foreach (var item in antaUrl)
            {
                PageDataHelper ph = new PageDataHelper();                
                var aaa = ph.GetIndexList(item, 1, new List<urlInfo>());
                resultList.AddRange(aaa);
                Console.WriteLine("共有" + resultList.Count + "个");
            }
            ShowMsg("<获取商品列表成功>");
            AddTask(resultList);
        }

        protected override void Fun(List<urlInfo> task)
        {
            string date = DateTime.Now.Date.ToShortDateString() + " 0:00:00";
            var gotList = ORMHelper.GetModel<Tmall_Name_Anta>(" where State = '" + (Program.UpdateTimes + 1) + "' ");
            Dictionary<UInt64, Tmall_Name_Anta> dic_Got = new Dictionary<ulong, Tmall_Name_Anta>();
            foreach (var dg in gotList)
            {
                if (!dic_Got.ContainsKey(dg.Id)) dic_Got.Add(dg.Id, dg);
            }
            int a = 0;
            List<Tmall_Detail_Anta> dsList = new List<Tmall_Detail_Anta>();
            List<Tmall_Name_Anta> nsList = new List<Tmall_Name_Anta>();
            foreach (var t in task)
            {
                a++;
                ShowMsg(t.dataId.ToString());
                if (dic_Got.ContainsKey(t.dataId))
                {
                    continue;
                }
                
                var result = PageDataHelper.GotDetailData(t);
                Tmall_Detail_Anta td = new Tmall_Detail_Anta();
                Tmall_Name_Anta tn = new Tmall_Name_Anta();
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
                tn.State = td.State = byte.Parse(Program.UpdateTimes);
                ShowMsg(t.dataId + "  " + t.name + " " + td.AvePrice + " " + td.Sales_Mon + " " + td.Comments_Mon + td.LastUpdate);
                nsList.Add(tn);
                dsList.Add(td);                
                Random random = new Random();
                int interval = random.Next(25, 76);
                ShowMsg(interval.ToString());
                System.Threading.Thread.Sleep(interval * 100);
                if (a==50)
                {
                    ShowMsg("<加入50条数据>");
                    DataToBase.SaveData(nsList);
                    DataToBase.SaveData(dsList);
                }
            }
            //更新配置文件
            CC.Utility.iniHelper ini = new CC.Utility.iniHelper(Program.FilePath);
            ini.Write("state", "times", (byte.Parse(Program.UpdateTimes) + 1).ToString());
            driver.Close();
        }

    }
}
