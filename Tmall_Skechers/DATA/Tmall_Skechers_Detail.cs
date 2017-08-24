using CC.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmall_Skechers.DATA
{
    [Table(TableName = "tmall_detail_skechers", DatabaseName = "shoes_tmall", Version = 1)]
    public class Tmall_Skechers_Detail : SqlDataBase
    {
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "Id")]
        public Int64 Id { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "indexPrice")]
        public double indexPrice { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "AvePrice")]
        public double AvePrice { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Sales_Mon")]
        public Int32 Sales_Mon { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Sales_Total")]
        public Int32? Sales_Total { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Repertory")]
        public Int32 Repertory { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Comments_Mon")]
        public UInt32? Comments_Mon { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Comments_Total")]
        public UInt32 Comments_Total { get; set; }
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "LastUpdate")]
        public DateTime LastUpdate { get; set; }
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "State")]
        public sbyte State { get; set; }
    }

}
