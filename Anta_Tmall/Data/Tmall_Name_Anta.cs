using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;

namespace Anta_Tmall.Data
{
    [Table(TableName = "tmall_name_anta", DatabaseName = "shoes_tmall", Version = 1)]
    public class Tmall_Name_Anta : SqlDataBase
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
