using MongoDB.Bson;
using MongoDBDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBDemo
{
    class Program
    {
        public static void Main(string[] args)
        {
            //添加数据
            //var model = new User()
            //{
            //    Id = ObjectId.GenerateNewId(),
            //    Name= "测试信息name",
            //    EX= "错误信息",
            //    Sex= "男",
            //    Address= "北京市",
            //    Age="24",
            //    Creator="test",
            //    CreateDate=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //    LastEditDate= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            //};
            //var connStr = "mongodb://127.0.0.1:27017/?safe=true";
            //var client = new MongoDB.Driver.MongoClient(connStr);
            //创建或打开已有数据库test
            //var database = client.GetDatabase("mongodb_name");
            //collection类似与数据库中的table，这里创建了名字为person的collection，存放Person对象
            //var collection = database.GetCollection<User>("user");
            //collection.InsertOne(model);
            //var res = new MongoConfig();
            //res.InsertOne(model);
            Console.Title = "mongodb test";

            //MongoDBTest.InsertTest();

            MongoDBTest.QueryTest();

            //MongoDBTest.UpdateTest();
            //MongoDBTest.GetByIdTest();
            //MongoDBTest.GetOneByConditionTest();
            //MongoDBTest.GetManyByConditionTest();
            //MongoDBTest.FindListByPageTest();
            //MongoDBTest.DeleteTest();
            Console.WriteLine("ok");
            Console.ReadKey();
        }
    }
}
