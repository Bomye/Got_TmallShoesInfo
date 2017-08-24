using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;

namespace Anta_Tmall.Data
{
    [Table(TableName = "tmall_detail_anta", DatabaseName = "shoes_tmall", Version = 1)]
    public class Tmall_Detail_Anta : SqlDataBase
    {
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "Id")]
        public UInt64 Id { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "IndexPrice")]
        public double IndexPrice { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "AvePrice")]
        public double AvePrice { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Sales_Mon")]
        public Int32 Sales_Mon { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Sales_Total")]
        public Int32? Sales_Total { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Repertory")]
        public UInt32 Repertory { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Comments_Mon")]
        public UInt32? Comments_Mon { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Comments_Total")]
        public UInt32 Comments_Total { get; set; }
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "LastUpdate")]
        public DateTime LastUpdate { get; set; }
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "State")]
        public byte State { get; set; }
    }

}
