using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBDemo
{
    /// <summary>
    /// 测试mongodb增删改查
    /// </summary>
    public class MongoDBTest
    {
        /// <summary>
        ///  插入数据测试
        /// </summary>
        public static void InsertTest()
        {
            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var item = new Role()
                {
                    UserName="我的名字"+i,
                    Age=random.Next(25,30),
                    State=i%2==0?State.Normal:State.Unused
                };
                MongoDbHelper.Insert(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.Role, item);
            }
            
        }

        /// <summary>
        /// 查询测试
        /// </summary>
        public static void QueryTest()
        {
            var list = MongoDbHelper.GetAll<Role>(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.Role);
            if (list!=null&&list.Count>0)
            {
                foreach (var item in list)
                {
                    Console.WriteLine("姓名{0},年龄{1},状态{2}",item.UserName,item.Age, GetStateDesc(item.State));
                }
            }
        }
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        public static void GetByIdTest()
        {
            var ID = "601383d7d25a3a01e9714f93";
            ObjectId id;
            if (!ObjectId.TryParse(ID, out id))
            {
                throw new ArgumentOutOfRangeException("转换类型错误");
            }

            var result = MongoDbHelper.GetById<Role>(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.Role,
                id);
            if (result!=null)
            {
                Console.WriteLine("姓名{0},年龄{1},状态{2}", result.UserName, result.Age, GetStateDesc(result.State));
            }
        }
        /// <summary>
        /// 根据查询条件获取一条数据
        /// </summary>
        public static void GetOneByConditionTest()
        {
            Expression<Func<Role, bool>> filterWhere = x => x.UserName.Contains("我的名字");
            var result = MongoDbHelper.GetOneByCondition<Role>(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.Role,
                filterWhere);
            if (result != null)
            {
                Console.WriteLine("姓名{0},年龄{1},状态{2}", result.UserName, result.Age, GetStateDesc(result.State));
            }
        }

        /// <summary>
        /// 根据查询条件获取多条数据
        /// </summary>
        public static void GetManyByConditionTest()
        {
            Expression<Func<Role, bool>> filterWhere = x => x.UserName.Contains("我的名字");
            var list = MongoDbHelper.GetManyByCondition<Role>(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.Role,
                filterWhere);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    Console.WriteLine("姓名{0},年龄{1},状态{2}", item.UserName, item.Age, GetStateDesc(item.State));
                }
            }
        }
        /// <summary>
        /// 分页查询数据
        /// </summary>
        public static void FindListByPageTest()
        {
            //var filter = Builders<Role>.Filter.Eq(c => c.State, State.Unused);
            var filter = Builders<Role>.Filter.Empty;
            var sortDocument= new BsonDocument("sort", 1);
            var sortDefinition = (SortDefinition<Role>)sortDocument;
            long Counts = 0;
            var list = MongoDbHelper.FindListByPage<Role>(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.Role,
                filter,1,10,out Counts, sortDefinition);
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    Console.WriteLine("姓名{0},年龄{1},状态{2}", item.UserName, item.Age, GetStateDesc(item.State));
                }
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        public static void DeleteTest()
        {
            var ID = "601383d7d25a3a01e9714f93";
            ObjectId id;
            if (!ObjectId.TryParse(ID, out id))
            {
                throw new ArgumentOutOfRangeException("转换类型错误");
            }
            Expression<Func<Role, bool>> filterWhere = x => x.Id == id;
            var res = MongoDbHelper.DeleteByCondition(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.Role,
                filterWhere);
            if (res)
            {
                Console.WriteLine("删除成功");
            }
            else
            {
                Console.WriteLine("删除失败");
            }
        }
        /// <summary>
        /// 测试更新
        /// </summary>
        public static void UpdateTest()
        {
            var ID = "601383d7d25a3a01e9714f93";
            ObjectId id;
            if (!ObjectId.TryParse(ID, out id))
            {
                throw new ArgumentOutOfRangeException("转换类型错误");
            }
            Expression<Func<Role, bool>> filterWhere = x => x.Id == id;
            var update = new UpdateDefinition<Role>[]
            {
                Builders<Role>.Update.Set("UserName","测试")
            };
            var res= MongoDbHelper.Update(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.Role,
                filterWhere, Builders<Role>.Update.Combine(update));
            if (res)
            {
                Console.WriteLine("更新成功");
            }
            else
            {
                Console.WriteLine("更新失败");
            }
        }
        /// <summary>
        /// 获取状态描述
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetStateDesc(State state)
        {
            string result = string.Empty;
            switch (state)
            {
                case State.Normal:
                    result = "正常";
                    break;
                case State.Unused:
                    result = "未使用";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
            return result;
        }
    }
}
