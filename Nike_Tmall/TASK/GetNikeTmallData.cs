using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;
using CC.MyTask;
using Nike_Tmall.DATA;
using OpenQA.Selenium;


namespace Nike_Tmall.TASK
{
    class GetNikeTmallData : AutoQueueTask<List<urlInfo>>
    {
        public static IWebDriver driver;
        static GetNikeTmallData _instance;
        public static GetNikeTmallData Instance
        {
            get
            {
                if (_instance == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _instance, new GetNikeTmallData(), null);
                }
                return _instance;
            }
        }
        GetNikeTmallData()
        {
            NoTaskSleepControl.Interval_ms = 24 * 3600 * 1000;
            driver = new OpenQA.Selenium.Chrome.ChromeDriver(@"C:\Program Files (x86)\Google\Chrome\Application");
        }
        public override string Name
        {
            get { return "NIKE_Tmall"; }
        }
        protected override void NoTask()
        {
            string nikeUrl = "https://nike.tmall.com/view_shop.htm?spm=a1z10.3-b-s.w4011-14234872766.169.XC2eb5&type=p&newHeader_b=s_from&searcy_type=item&from=.shop.pc_2_searchbutton&search=y&keyword=%D0%AC&tsearch=y&pageNo=";
            ShowMsg("<获取商品列表>");
            PageDataHelper ph = new PageDataHelper();
            var resultList = ph.GetIndexList(nikeUrl, 1, new List<urlInfo>());
            Console.WriteLine();
            ShowMsg("<获取商品列表成功>");
            AddTask(resultList);
        }
        protected override void Fun(List<urlInfo> task)
        {
            int a = 0;
            List<Tmall_Detail_Nike> dsList = new List<Tmall_Detail_Nike>();
            List<Tmall_Name_Nike> nsList = new List<Tmall_Name_Nike>();
            foreach (var t in task)
            {
                //if (++a == 20)
                //{
                //    System.Threading.Thread.Sleep(60 * 1000);
                //    a = 0;
                //}
                ShowMsg(t.dataId.ToString());
                var result = PageDataHelper.GotDetailData(t);
                Tmall_Detail_Nike td = new Tmall_Detail_Nike();
                Tmall_Name_Nike tn = new Tmall_Name_Nike();
                td.Id = tn.Id = (UInt64)result.Id;
                tn.Name = result.Name;
                td.LastUpdate = tn.LastUpdate = result.LastUpdate;
                td.IndexPrice = result.IndexPrice;
                td.AvePrice = result.AvePrice;
                td.Comments_Mon = result.IndexComment;
                td.Comments_Total = result.TotalComment;
                td.Repertory = (uint)result.Repertory;
                td.Sales_Mon = result.MonSales;
                tn.State = td.State = sbyte.Parse(Program.UpdateTimes);
                td.Sales_Total = result.TotalSales;
                ShowMsg(t.dataId + "  " + t.name + " " + td.AvePrice + " " + td.Sales_Mon + " " + td.Comments_Mon + td.LastUpdate);
                nsList.Add(tn);
                dsList.Add(td);
                ShowMsg("<加入一条数据>");
                DataToBase.SaveData(nsList);
                DataToBase.SaveData(dsList);
                Random random = new Random();
                int interval = random.Next(56, 65);
                ShowMsg(interval.ToString());
                System.Threading.Thread.Sleep(interval * 100);
            }
            //更新配置文件
            CC.Utility.iniHelper ini = new CC.Utility.iniHelper(Program.FilePath);
            ini.Write("state", "times", (sbyte.Parse(Program.UpdateTimes) + 1).ToString());
        }
    }
}
