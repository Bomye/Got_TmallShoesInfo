using CC.ORM;
using System;
using System.Collections.Generic;

namespace Anta_Tmall
{
    class DataToBase
    {
        /// <summary>
        /// 数据库通用方法
        /// </summary>
        /// <typeparam name="T">数据集合类型</typeparam>
        /// <param name="datas">数据集</param>
        public static void SaveData<T>(IEnumerable<T> datas)
            where T : CC.ORM.SqlDataBase, new()
        {
            Console.WriteLine("开始数据库事务...");
            var dc = ORMHelper.DefaultDataFactory.Create();
            dc.BeginTransaction();
            foreach (var item in datas)
            {
                ORMHelper.InsertOrUpdate(item, dc);
            }
            dc.Commit();
            dc.Close();
        }
    }
}
