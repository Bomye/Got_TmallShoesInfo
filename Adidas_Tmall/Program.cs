using System;
using Adidas_Tmall.TASK;
using CC.MyTask;
using CC.ORM;

namespace Adidas_Tmall
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
            MysqlFactory.Instance.DefaultConnStr = MysqlFactory.GetConnStr(ip,user,psw,dataBase);
            ORMHelper.DefaultDataFactory = MysqlFactory.Instance;
            #endregion
            //GetData_CSV.GotResultCsv("Adidas_Tmall_20170328.xlsx", "20170328分析", "Adidas_Tmall_20170328");


            TaskView t = new TaskView();
            t.SetDlgTitle("Adidas_Skechers");
            t.AddTask(GetAdidasTmall.Instance);
            //t.AddTask(GetAdidasResult.Instance);
            t.ShowDialog();
        }
    }
}
