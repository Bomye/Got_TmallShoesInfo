using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;
using CC.MyTask;
using Adidas_Tmall.DATA;
using OpenQA.Selenium;

namespace Adidas_Tmall.TASK
{
    class GetAdidasTmall:AutoQueueTask<List<urlInfo>>
    {
        public static IWebDriver driver;
        static GetAdidasTmall _instance;
        public static GetAdidasTmall Instance
        {
            get
            {
                if (_instance == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _instance, new GetAdidasTmall(), null);
                }
                return _instance;
            }
        }
        GetAdidasTmall() 
        { 
            NoTaskSleepControl.Interval_ms = 24 * 3600 * 1000; 
            driver = new OpenQA.Selenium.Chrome.ChromeDriver(@"C:\Program Files (x86)\Google\Chrome\Application");
        }
        public override string Name
        {
            get { return "Adidas_Tmall"; }
        }
        protected override void NoTask()
        {
            string nikeUrl = "https://adidas.tmall.com/view_shop.htm?spm=a1z10.3-b-s.w4011-14687612648.108.ijX6sv&type=p&newHeader_b=s_from&searcy_type=item&from=.shop.pc_2_searchbutton&search=y&orderType=hotsell_desc&keyword=%D0%AC&tsearch=y&pageNo=";
            ShowMsg("<获取商品列表>");
            PageDataHelper ph = new PageDataHelper();
            var resultList = ph.GetIndexList(nikeUrl, 1, new List<urlInfo>());
            ShowMsg("<获取商品列表成功>");
            AddTask(resultList);
        }
        protected override void Fun(List<urlInfo> task)
        {
            string date = DateTime.Now.Date.ToShortDateString() + " 0:00:00";
            var gotList = ORMHelper.GetModel<Tmall_Name_Ad>(" where LastUpdate >'" + date + "' ");
            Dictionary<UInt64, Tmall_Name_Ad> dic_Got = new Dictionary<UInt64, Tmall_Name_Ad>();
            foreach (var dg in gotList)
            {
                if (!dic_Got.ContainsKey(dg.Id)) dic_Got.Add(dg.Id, dg);
            }
            int a = 0;
            List<Tmall_Detail_Ad> dsList = new List<Tmall_Detail_Ad>();
            List<Tmall_Name_Ad> nsList = new List<Tmall_Name_Ad>();
            foreach (var t in task)
            {
                a++;
                if (dic_Got.ContainsKey(t.dataId))
                    continue;
                ShowMsg(t.dataId.ToString());
                var result = PageDataHelper.GotDetailData(t);
                Tmall_Detail_Ad td = new Tmall_Detail_Ad();
                Tmall_Name_Ad tn = new Tmall_Name_Ad();
                td.Id = tn.Id = (UInt64)result.Id;
                tn.Name = result.Name;
                td.LastUpdate = tn.LastUpdate = DateTime.Now;
                td.Price = result.IndexPrice;
                td.Comments_Mon = result.IndexComment;
                td.Comments_Total = result.TotalComment;
                td.Repertory = (uint)result.Repertory;
                td.Sales_Month = result.MonSales;
                td.Sales_Total = result.TotalSales;
                td.State = tn.State = byte.Parse(Program.UpdateTimes);
                ShowMsg(t.dataId + "  " + t.name + " " + td.Price + " " + td.Sales_Month + " " + td.Comments_Mon + td.LastUpdate);
                nsList.Add(tn);
                dsList.Add(td);
                ShowMsg("<加入一条数据>");
                Random random = new Random();
                int interval = random.Next(11, 60);
                ShowMsg(interval.ToString());
                System.Threading.Thread.Sleep(interval * 100);
                //if (a == 20)
                {
                    DataToBase.SaveData(nsList);
                    DataToBase.SaveData(dsList);
                    a = 0;
                }              
            }
            driver.Close();
            //更新配置文件
            CC.Utility.iniHelper ini = new CC.Utility.iniHelper(Program.FilePath);
            ini.Write("state", "times", (sbyte.Parse(Program.UpdateTimes) + 1).ToString());
        }
    }
    
}
