using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;

namespace Adidas_Tmall.DATA
{
    [Table(TableName = "tmall_detail_ad", DatabaseName = "shoes_tmall", Version = 1)]
    public class Tmall_Detail_Ad : SqlDataBase
    {
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "Id")]
        public UInt64 Id { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Price")]
        public double Price { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Sales_Month")]
        public Int32 Sales_Month { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Sales_Total")]
        public Int32 Sales_Total { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Repertory")]
        public UInt32 Repertory { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Comments_Mon")]
        public UInt32 Comments_Mon { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Comments_Total")]
        public UInt32 Comments_Total { get; set; }
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "LastUpdate")]
        public DateTime LastUpdate { get; set; }
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "State")]
        public byte State { get; set; }
    }
    [Table(TableName = "tmall_name_ad", DatabaseName = "shoes_tmall", Version = 1)]
    public class Tmall_Name_Ad : SqlDataBase
    {
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "Id")]
        public UInt64 Id { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Name")]
        public System.String Name { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "LastUpdate")]
        public DateTime LastUpdate { get; set; }
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "State")]
        public byte State { get; set; }
    }

}
