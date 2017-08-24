using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.ORM;
using CC.MyTask;
using Anta_Tmall.Task;

namespace Anta_Tmall
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

            //GetData_CSV.GotResultCsv("Anta_Tmall_20170328.xlsx", "20170328分析", "Anta_Tmall_20170328");
            TaskView t = new TaskView();
            t.SetDlgTitle("Tmall_Anta");
            t.AddTask(GetAntaTmalData.Instance);
            //t.AddTask(GetAntaResult.Instance);
            t.ShowDialog();
        }
    }
}
