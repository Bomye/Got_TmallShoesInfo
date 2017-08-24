using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;
using CC.MyTask;
using Nike_Tmall.TASK;

namespace Nike_Tmall
{
    class Program
    {
        public static string FilePath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
        internal static string UpdateTimes = CC.Utility.iniHelper.ReadValue(FilePath, "state", "times");

        static void Main(string[] args)
        {
            #region Mysql
            string ip = CC.Utility.iniHelper.ReadValue(FilePath, "Mysql", "ip");
            string user = CC.Utility.iniHelper.ReadValue(FilePath, "Mysql", "user");
            string psw = CC.Utility.iniHelper.ReadValue(FilePath, "Mysql", "psw");
            string dataBase = CC.Utility.iniHelper.ReadValue(FilePath, "Mysql", "dataBase");
            MysqlFactory.Instance.DefaultConnStr = MysqlFactory.GetConnStr(ip, user, "MySqlPsw", dataBase);
            ORMHelper.DefaultDataFactory = MysqlFactory.Instance;
            #endregion

            //GetData_CSV.GotResultCsv("Nike_Tmall_20170328.xlsx", "2017328销售数据", "Nike_Tmall_20170328");
            TaskView t = new TaskView();
            t.SetDlgTitle("Tmall_Nike");
            //t.AddTask(Get_NikeResult.Instance);
            t.AddTask(GetNikeTmallData.Instance);
            t.ShowDialog();
        }
        
    }
}
