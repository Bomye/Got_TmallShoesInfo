using CC.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tmall_Skechers.DATA
{
    [Table(TableName = "tmall_name_skechers", DatabaseName = "shoes_tmall", Version = 1)]
    public class Tmall_Skechers_Name : SqlDataBase
    {
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "Id")]
        public Int64 Id { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "Name")]
        public System.String Name { get; set; }
        [Column(IsPrimary = false, IsIdentity = false, Fieldname = "LastUpdate")]
        public DateTime LastUpdate { get; set; }
        [Column(IsPrimary = true, IsIdentity = false, Fieldname = "State")]
        public sbyte State { get; set; }
    }

}
