using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;
using CC.MyTask;
using Tmall_Skechers.DATA;
using OpenQA.Selenium;

namespace Tmall_Skechers.TASK
{
    class Tmall_Skechers_Task : AutoQueueTask<List<urlInfo>>
    {
        public static IWebDriver driver;
        static Tmall_Skechers_Task _instance;
        public static Tmall_Skechers_Task Instance
        {
            get
            {
                if (_instance == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _instance, new Tmall_Skechers_Task(), null);
                }
                return _instance;
            }
        }
        Tmall_Skechers_Task() 
        { 
            NoTaskSleepControl.Interval_ms = 24 * 3600 * 1000;
            driver = new OpenQA.Selenium.Chrome.ChromeDriver(@"C:\Program Files (x86)\Google\Chrome\Application");
        }
        public override string Name
        {
            get { return "Tmall_Skechers"; }
        }

        protected override void NoTask()
        {
            List<urlInfo> indexList = new List<urlInfo>();
            string[] usls = new string[]{
                "https://skechers.tmall.com/i/asynSearch.htm?_ksTS=1489735339386_178&callback=jsonp179&mid=w-15004456963-0&wid=15004456963&path=/category-526898310.htm&&spm=a1z10.5-b-s.w4011-15004456963.392.QmjRFT&scene=taobao_shop&catId=526898310&scid=526898310&pageNo=",//女鞋
                "https://skechersnx.tmall.com/i/asynSearch.htm?_ksTS=1489735707723_125&callback=jsonp126&mid=w-15038655369-0&wid=15038655369&path=/search.htm&&search=y&spm=a1z10.3-b-s.w4011-15038655369.380.DnM87k&scene=taobao_shop&orderType=defaultSort&keyword=%D0%AC&tsearch=y&pageNo=",//男鞋
                "https://skechersyd.tmall.com/i/asynSearch.htm?_ksTS=1497596125893_364&callback=jsonp365&mid=w-15008901561-0&wid=15008901561&path=/search.htm&&search=y&spm=a1z10.3-b-s.w4011-15008901561.83.RqWGVQ&viewType=grid&keyword=%D0%AC&tsearch=y&pageNo=",//童鞋
                "https://skechersyd.tmall.com/i/asynSearch.htm?_ksTS=1489736155257_57&callback=jsonp58&mid=w-15008901561-0&wid=15008901561&path=/search.htm&&search=y&spm=a1z10.3-b-s.w4011-15008901561.83.lPurZK&scene=taobao_shop&orderType=defaultSort&keyword=%D0%AC&tsearch=y&pageNo="//运动鞋
            };
            ShowMsg("<获取商品列表>");
            foreach (var url in usls)
            {
                PageDataHelper ph = new PageDataHelper();
                var result = ph.GetIndexList(url, 1, new List<urlInfo>());
                indexList.AddRange(result);
            }
            ShowMsg("<获取商品列表成功>");
            AddTask(indexList);
        }

        protected override void Fun(List<urlInfo> task)
        {
            System.Threading.Thread.Sleep(10 * 1000);
            string date = DateTime.Now.Date.AddDays(-1).ToShortDateString() + " 0:00:00";
            var gotList = ORMHelper.GetModel<Tmall_Skechers_Name>(" where State =" + Program.UpdateTimes);
            Dictionary<Int64, Tmall_Skechers_Name> dic_Got = new Dictionary<long, Tmall_Skechers_Name>();
            foreach (var dg in gotList)
            {
                if (!dic_Got.ContainsKey(dg.Id)) dic_Got.Add(dg.Id, dg);
            }

            List<Tmall_Skechers_Name> nsList = new List<Tmall_Skechers_Name>();
            List<Tmall_Skechers_Detail> dsList = new List<Tmall_Skechers_Detail>();
            int a = 0;
            foreach (var t in task)
            {
                ShowMsg(t.dataId.ToString());
                if (dic_Got.ContainsKey((long)t.dataId))
                {
                    continue;
                }
                //if (++a == 20)
                //{
                //    System.Threading.Thread.Sleep(60 * 1000);
                //    a = 0;
                //}
                var result = PageDataHelper.GotDetailData(t);
                Tmall_Skechers_Detail td = new Tmall_Skechers_Detail();
                Tmall_Skechers_Name tn = new Tmall_Skechers_Name();
                td.Id = tn.Id = (Int64)result.Id;
                tn.Name = result.Name;
                td.LastUpdate = tn.LastUpdate = result.LastUpdate;
                td.indexPrice = result.IndexPrice;
                td.AvePrice = result.AvePrice;
                td.Comments_Mon = result.IndexComment;
                td.Comments_Total = result.TotalComment;
                td.Repertory = (int)result.Repertory;
                td.Sales_Mon = result.MonSales;
                tn.State = td.State = sbyte.Parse(Program.UpdateTimes);
                td.Sales_Total = result.TotalSales;
                ShowMsg(t.dataId + "  " + t.name + " " + td.AvePrice + " " + td.Sales_Mon + " " + td.Comments_Mon + td.LastUpdate);
                nsList.Add(tn);
                dsList.Add(td);
                Random random = new Random();
                int interval = random.Next(35, 80);
                ShowMsg(interval.ToString());
                System.Threading.Thread.Sleep(interval * 100);
            }
            DataToBase.SaveData(nsList);
            DataToBase.SaveData(dsList);
            //更新配置文件
            CC.Utility.iniHelper ini = new CC.Utility.iniHelper(Program.FilePath);
            ini.Write("state", "times", (sbyte.Parse(Program.UpdateTimes) + 1).ToString());
        }
    }
}
