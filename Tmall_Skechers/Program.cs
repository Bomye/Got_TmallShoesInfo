using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;
using CC.MyTask;
using Tmall_Skechers.TASK;

namespace Tmall_Skechers
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
            MysqlFactory.Instance.DefaultConnStr = MysqlFactory.GetConnStr(ip, user, psw, dataBase);
            ORMHelper.DefaultDataFactory = MysqlFactory.Instance;
            #endregion
            //GetData_CSV.GotResultCsv("Skechers_Tmall_20170328.xlsx", "20170328分析", "Skechers_Tmall_20170328");
            TaskView t = new TaskView();
            t.SetDlgTitle("Tmall_Skechers");
            //t.AddTask(Get_SkechersResult.Instance);
            t.AddTask(Tmall_Skechers_Task.Instance);
            t.ShowDialog();
        }
    }
}
