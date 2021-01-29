using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBDemo
{
    /// <summary>
    /// 数据库配置参数
    /// </summary>
    public class  DbConfigParams
    {
        private static string _conntionString = ConfigurationManager.AppSettings["MongodbURL"];

        /// <summary>
        /// 获取数据库连接串
        /// </summary>
        public static string ConntionString
        {
            get { return _conntionString; }
        }

        private static string _dbName = ConfigurationManager.AppSettings["MongodbName"];

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        public static string DbName
        {
            get { return _dbName; }
        }
    }
}
