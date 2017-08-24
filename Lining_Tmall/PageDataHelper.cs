using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CC.Json;
using CC.Web.Http;
using HtmlAgilityPack;
using Lining_Tmall.Data;
using Lining_Tmall.Task;
using System.Threading;

namespace Lining_Tmall
{
    class PageDataHelper
    {
        bool first = true;
        public List<urlInfo> GetIndexList(string url, int pg, List<urlInfo> list)
        {
            HttpHelper http = new HttpHelper();
            http.DefaultHeadInfo.timeout = 100000;
            http.DefaultHeadInfo.cookie_collection = new System.Net.CookieContainer();
            Uri uri = new Uri(url + pg);
            http.DefaultHeadInfo.cookie_collection.Add(uri, new System.Net.Cookie("t", "1b02b877ceaafe8fab8c5b4541177358"));
            http.DefaultHeadInfo.cookie_collection.Add(uri, new System.Net.Cookie("_tb_token_", "NFAkGXsNAv9K"));
            http.DefaultHeadInfo.cookie_collection.Add(uri, new System.Net.Cookie("cookie2", "7c4655a6a3d1d07a26aeddccf8cb5cdb"));
            http.DefaultHeadInfo.header.Add("Accept-Encoding", " gzip, deflate, sdch, br");
            var page = http.Get(url + pg);
            int a = 0;
            if (page.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (a > 5) return null;
                System.Threading.Thread.Sleep(2000);
                page = http.Get(url + pg);
            }
            HtmlDocument hd = new HtmlDocument();
            hd.LoadHtml(page.Html.Replace("\\", ""));
            var nodes = hd.DocumentNode.SelectNodes("//div[@class='J_TItems']//div[@class='item4line1']");
            if (nodes == null)
            {
                nodes = hd.DocumentNode.SelectNodes("//div[@class='J_TItems']//div[@class='item5line1']");
                if (nodes == null)
                    return null;
            }
            for (int i = 0; i < nodes.Count - 2; i++)
            {
                var node_line = nodes[i].SelectNodes("./dl");
                for (int j = 0; j < node_line.Count; j++)
                {
                    var uf = new urlInfo();
                    uf.dataId = UInt64.Parse(node_line[j].Attributes["data-id"].Value);
                    uf.name = node_line[j].SelectSingleNode("./dd[@class='detail']/a").InnerText;
                    uf.detailUrl = "http:" + node_line[j].SelectSingleNode("./dd[@class='detail']/a").Attributes["href"].Value;
                    uf.indexPrice = double.Parse(node_line[j].SelectSingleNode("./dd[@class='detail']/div[@class='attribute']/div[@class='cprice-area']//span[2]").InnerText);
                    try
                    {
                        uf.totalSales = int.Parse(node_line[j].SelectSingleNode("./dd[@class='detail']/div[@class='attribute']/div[@class='sale-area']//span[1]").InnerText);
                        uf.indexComment = uint.Parse(Regex.Match(node_line[j].SelectSingleNode("./dd[@class='rates']/div[@class='title']").InnerText, @"\d+").Value);
                    }
                    catch { }
                    list.Add(uf);
                }
            }
            if (first)
            {
                var pgStr = hd.DocumentNode.SelectSingleNode("//div[@class='filter clearfix J_TFilter']/p/b[@class='ui-page-s-len']").InnerText.Split('/')[1];
                int pgNodes = int.Parse(pgStr);
                if (pgStr != null)
                {
                    Console.WriteLine("共有 {0} 页", pgNodes);
                    first = false;
                    for (int un = 2; un <= pgNodes; un++)
                    {
                        System.Threading.Thread.Sleep(1000);
                        GetIndexList(url, un, list);
                        Console.WriteLine("第{0}页", un);
                    }
                }
            }
            return list;
        }
        static void LoginHint(string url, string pageStr)
        {
            if (pageStr.Contains("<title>上天猫，就够了</title>"))
            {
                Console.WriteLine("要求登录,登录后按任意键继续");
                Console.ReadKey();
                GetLiningTmallData.driver.Navigate().GoToUrl(url);
                Thread.Sleep(3000);
            }
            else if (pageStr.Contains("验证码:"))
            {
                Console.WriteLine("要求登录,登录后按任意键继续");
                Console.ReadKey();
                GetLiningTmallData.driver.Navigate().GoToUrl(url);
                Thread.Sleep(3000);
            }
        }

        public static GoodsInfo GotDetailData(urlInfo ui)
        {

            GoodsInfo gif = new GoodsInfo();
            string detailUrl = "https://detail.tmall.com/item.htm?spm=a220m.1000858.1000725.5.rWDxsT&id=" + ui.dataId;
            GetLiningTmallData.driver.Navigate().GoToUrl(detailUrl);
            LoginHint(detailUrl, GetLiningTmallData.driver.PageSource);

            HtmlAgilityPack.HtmlDocument hd = new HtmlDocument();
            hd.LoadHtml(GetLiningTmallData.driver.PageSource);
            try
            {
                gif.TotalComment = uint.Parse(hd.DocumentNode.SelectSingleNode("//body/div[@id='page']/div[@id='content']/div[@id='detail']/div[@id='J_DetailMeta']/div[@class='tm-clear']/div[@class='tb-property']/div[@class='tb-wrap']/ul/li[2]/div/span[@class='tm-count']").InnerText);
                gif.MonSales = int.Parse(hd.DocumentNode.SelectSingleNode("//body/div[@id='page']/div[@id='content']/div[@id='detail']/div[@id='J_DetailMeta']/div[@class='tm-clear']/div[@class='tb-property']/div[@class='tb-wrap']/ul/li[1]/div/span[@class='tm-count']").InnerText);
                string re = hd.DocumentNode.SelectSingleNode("//*[@id='J_EmStock']").InnerText;
                string kucun = Regex.Match(re, @"\d+").Value;
                gif.Repertory = uint.Parse(kucun);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            gif.Id = ui.dataId;
            gif.IndexPrice = ui.indexPrice;
            gif.TotalSales = ui.totalSales;
            gif.Name = ui.name;
            gif.IndexComment = ui.indexComment;
            gif.LastUpdate = DateTime.Now;
            return gif;

            //string jsonUrl = "https://dsr-rate.tmall.com/list_dsr_info.htm?itemId=" + ui.dataId + "&callback=jsonp220";
            //var comCount = GetCommInfo(jsonUrl);

            //string jsUrl = "https://mdskip.taobao.com/core/initItemDetail.htm?itemId=" + ui.dataId;
            //var saleInfo = GetSaledData(jsUrl);
            //GoodsInfo gif = new GoodsInfo();
            //gif.Id = ui.dataId;
            //gif.IndexPrice = ui.indexPrice;
            //double price = 0.0;
            //foreach (var kv in saleInfo.defaultModel.itemPriceResultDO.priceInfo)
            //{
            //    price += kv.Value.price;
            //}
            //gif.AvePrice = price / saleInfo.defaultModel.itemPriceResultDO.priceInfo.Count;
            //gif.TotalSales = ui.totalSales;
            //gif.MonSales = (int)saleInfo.defaultModel.sellCountDO.sellCount;
            //gif.Repertory = saleInfo.defaultModel.inventoryDO.TotalQuantity;
            //gif.Name = ui.name;
            //gif.IndexComment = ui.indexComment;
            //gif.TotalComment = comCount;
            //gif.LastUpdate = DateTime.Now;
            //return gif;
        }
        /// <summary>
        /// 销售信息
        /// </summary>
        /// <param name="url"></param>
        static JsonInfo GetSaledData(string url)
        {
            HttpHelper http = new HttpHelper();
            Uri uri = new Uri(url);
            http.DefaultHeadInfo.timeout = 10000;
            http.DefaultHeadInfo.cookie_collection = new CookieContainer();
            http.DefaultHeadInfo.cookie_collection.Add(uri, new Cookie("cna", "uumzEMPq/n4CAQHApdL5b3E8"));
            http.DefaultHeadInfo.cookie_collection.Add(uri, new Cookie("cookie2", "7c4655a6a3d1d07a26aeddccf8cb5cdb"));
            http.DefaultHeadInfo.cookie_collection.Add(uri, new Cookie("t", "1b02b877ceaafe8fab8c5b4541177358"));

            http.DefaultHeadInfo.user_agent = "  Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            HttpItem item = new HttpItem()
            {
                url = url,
                referer = "https://detail.tmall.com/item.htm?"
            };

            var pageRespone = http.GetHtml(item);
            var reww = pageRespone.Html.Trim();
            while (!reww.StartsWith("{\"defaultModel\""))
            {
                System.Threading.Thread.Sleep(16 * 60 * 1000);
                pageRespone = http.GetHtml(item);
                reww = pageRespone.Html.Trim();
            }
            var re = JsonHelper.Deserializ<JsonInfo>(reww.Replace("\\", ""), false);
            return re;
        }

        /// <summary>
        /// 获取评论数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns>评论条数</returns>
        static uint GetCommInfo(string url)
        {
            var page = HttpHelper.DefaultHttp.Get(url);
            while (page.StatusCode != System.Net.HttpStatusCode.OK)
            {
                page = HttpHelper.DefaultHttp.Get(url);
            }
            string jsonStr = Regex.Match(page.Html, @"(?<=\()[^)]+").Groups[0].Value;
            var re = JsonHelper.Deserializ<Comment>(jsonStr, false);
            return re.dsr.rateTotal;
        }
    }
    class urlInfo
    {
        public ulong dataId;
        public string name;
        public string detailUrl;
        public double indexPrice;
        public int totalSales;
        public uint indexComment;
    }
    class GoodsInfo
    {
        public UInt64 Id { get; set; }
        public string Name { get; set; }
        public double IndexPrice { get; set; }
        public double AvePrice { get; set; }
        public int TotalSales { get; set; }
        public int MonSales { get; set; }
        public uint Repertory { get; set; }
        public uint IndexComment { get; set; }
        public uint TotalComment { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
