using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmall_Skechers.DATA
{

    #region==评论信息==
    class Comment
    {
        public Dsr dsr;
    }
    /// <summary>
    /// 评论数量
    /// </summary>
    class Dsr
    {
        public uint rateTotal { get; set; }
        public double gradeAvg { get; set; }
        public Int64 itemId { get; set; }
        public string peopleNumber { get; set; }
        public Int64 spuId { get; set; }
        public int totalSoldQuantity { get; set; }
    }
    #endregion

    #region==销售信息==
    class JsonInfo
    {
        public DefaultModel defaultModel;
    }
    /// <summary>
    /// 商品价格与销量
    /// </summary>
    class DefaultModel
    {
        public SellCountDO sellCountDO;
        public InventoryDo inventoryDO;
        public ItemPriceResultDO itemPriceResultDO;
    }
    class SellCountDO
    {
        public uint sellCount { get; set; }
    }
    class InventoryDo
    {
        public uint IcTotalQuantity { get; set; }
        public uint TotalQuantity { get; set; }
    }
    class ItemPriceResultDO
    {
        public Dictionary<string, GoodsPriceInfo> priceInfo { get; set; }//键值对解析不确定节点json
    }
    class GoodsPriceInfo
    {
        public bool areaSold { get; set; }
        public double price { get; set; }
        public bool onlyShowOnePrice { get; set; }
        public string sortOrder { get; set; }
        public PromotionList[] promotionList { get; set; }
    }
    class PromotionList
    {

    }
    #endregion
}
