using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MongoDBDemo.Model
{
    public class User : MongoBaseEntity
    {
        public ObjectId Id { get ; set ; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Age { get; set; }
        public string Creator { get ; set ; }
        public string CreateDate { get ; set ; }
        public string LastEditer { get ; set ; }
        public string LastEditDate { get ; set ; }

        public string SystemName
        {
            get
            {
                return "测试";
            }
        }

        public string EX { get ; set ; }


        public string IP {
            get
            {
                return GetIp();
            }
        }

        private string GetIp()
        {
            //if (HttpContext.Current.Request.ServerVariables.Get("Remote_Addr") == null)
            //    return "";
            //var IP = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString();
            return "192.168.2.126";
        }
    }
}

