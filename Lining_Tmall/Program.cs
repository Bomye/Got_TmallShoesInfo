using System;
using CC.ORM;
using CC.MyTask;
using Lining_Tmall.Task;

namespace Lining_Tmall
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
            TaskView t = new TaskView();
            t.SetDlgTitle("Tmall_Skechers");
            t.AddTask(GetLiningTmallData.Instance);
            //t.AddTask(GetLiningResult.Instance);
            t.ShowDialog();
        }
    }
}
