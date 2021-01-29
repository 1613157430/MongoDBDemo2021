using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBDemo.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDBDemo
{
    public class MongoConfig
    {
        //表
        public static readonly string MongodbServiceName = ConfigurationManager.AppSettings["MongodbServiceName"];
        //数据库连接
        public static readonly string MongodbDefaultUrl = ConfigurationManager.AppSettings["MongodbURL"];
        //指定的数据库
        public static readonly string MongodbDefaultDBName = ConfigurationManager.AppSettings["MongodbName"];

        /*构造函数*/
        /*-------------------------------------------------------------------------------------*/

        /// <summary>
        /// mongo连接客户端
        /// </summary>
        public static MongoClient server = null;

        /// <summary>
        /// mongo数据库
        /// </summary>
        public static IMongoDatabase db = null;

        public static IMongoCollection<User> collection= null;

        /*构造函数*/
        /*-------------------------------------------------------------------------------------*/

        /// <summary>
        /// 定义私有构造函数，使外界不能创建该类实例
        /// </summary>
        public MongoConfig()
        {
            //创建连接
            server = new MongoClient(MongodbDefaultUrl);
            //获取数据库
            db = server.GetDatabase(MongodbDefaultDBName);
            collection = db.GetCollection<User>(MongodbServiceName);


        }

        /// <summary>
        /// 数据集插入一条数据
        /// </summary>
        /// <param name="model"></param>
        public  void InsertOne(User model)
        {
             collection.InsertOneAsync(model);
        }

        /// <summary>
        /// 根据ObjectID 删除
        /// </summary>
        public static void DeleteAsync(ObjectId ID)
        {
            collection.DeleteOneAsync(x => x.Id == ID);
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        public static void Delete(Expression<Func<User, bool>> predicate)
        {
            collection.DeleteManyAsync(predicate);
        }
        /// <summary>
        /// 添加一条数据并返回id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Object InsertAndGetId(User model)
        {
            collection.InsertOne(model);
            return model?.Id;
        }
        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static User Get(ObjectId id)
        {
            return collection.Find(x => x.Id == id).FirstOrDefault();
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        public static List<User> GetAll()
        {
            var res= collection.AsQueryable().AsQueryable();
            return res.ToList();
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public  async Task<User> UpdateAsync(User entity)
        {
            ReplaceOneResult result = await collection.ReplaceOneAsync(o => o.Id == entity.Id, entity);
            return UpdateSuccessfulOrFailed(result, entity);
        }

        /// <summary>
        /// 检查更新是否成功
        /// </summary>
        /// <param name="result"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private User UpdateSuccessfulOrFailed(ReplaceOneResult result, User entity)
        {
            if (result.MatchedCount == result.ModifiedCount && result.ModifiedCount > 0)
                return entity;
            return null;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="filterWhere"></param>
        /// <param name="update"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual bool UpdateOne(Expression<Func<User, bool>> filterWhere, UpdateDefinition<User> update, UpdateOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var filterBuilder = Builders<User>.Filter;
            var filter = filterBuilder.Where(filterWhere);

            UpdateResult result = collection.UpdateOne(filter, update, options, cancellationToken);

            return UpdateSuccessfulOrFailed(result);
        }

        /// <summary>
        /// 检查更新是否成功1
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool UpdateSuccessfulOrFailed(UpdateResult result)
        {
            if (result.MatchedCount == result.ModifiedCount && result.ModifiedCount > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 根据条件查找数量
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public  async Task<long> FindCountDocuments(Expression<Func<User, bool>> predicate)
        {
            var result = await collection.CountDocumentsAsync(predicate);

            return result;
        }
    }
}
