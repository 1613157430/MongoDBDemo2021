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
    /// mongodb帮助类
    /// </summary>
    public class MongoDbHelper
    {
        /// <summary>
        /// 获取数据库实例对象
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <returns>数据库实例对象</returns>
        private static IMongoDatabase GetDatabase(string connectionString, string dbName)
        {
            //创建数据库链接
            var server = new MongoClient(connectionString);
            //获得数据库实例对象
            return server.GetDatabase(dbName);
        }
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="model">数据对象</param>
        public static void Insert<T>(string connectionString, string dbName, string collectionName, T model) where T: EntityBase
        {
            if (model==null)
            {
                throw new ArgumentNullException("model", "待插入数据不能为空");
            }
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            collection.InsertOne(model);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="query">查询条件</param>
        /// <param name="dictUpdate">更新字段<</param>
        public static bool Update<T>(string connectionString, string dbName, string collectionName,
            Expression<Func<T, bool>> filterWhere, UpdateDefinition<T> update) where T:EntityBase
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);

            var filterBuilder = Builders<T>.Filter;
            var filter = filterBuilder.Where(filterWhere);

            UpdateResult result = collection.UpdateOne(filter, update);

            return UpdateSuccessfulOrFailed(result);
        }
        /// <summary>
        /// 根据ID获取数据对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public static T GetById<T>(string connectionString, string dbName, string collectionName, ObjectId id)
            where T : EntityBase
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            return collection.Find(x => x.Id == id).FirstOrDefault();
        }
        /// <summary>
        /// 根据查询条件获取一条数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public static T GetOneByCondition<T>(string connectionString, string dbName, string collectionName, 
            Expression<Func<T, bool>> predicate) where T : EntityBase
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            return collection.AsQueryable().FirstOrDefault(predicate);
        }
        /// <summary>
        /// 根据查询条件获取多条数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public static List<T> GetManyByCondition<T>(string connectionString, string dbName, string collectionName,
            Expression<Func<T, bool>> predicate)where T : EntityBase
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            return collection.AsQueryable().Where(predicate).ToList();
        }
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <returns></returns>
        public static List<T> GetAll<T>(string connectionString, string dbName, string collectionName)
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            return collection.AsQueryable().ToList();
        }
        /// <summary>
        /// 删除集合中符合条件的数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="predicate">集合名称</param>
        public static bool DeleteByCondition<T>(string connectionString, string dbName, string collectionName,
            Expression<Func<T, bool>> predicate) where T : EntityBase
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            var result= collection.DeleteOne(predicate);

            return DeleteSuccessfulOrFailed(result);
        }

        /// <summary>
        /// 分页查询集合
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="count">总条数</param>
        /// <param name="sort">要排序的字段</param>
        /// <returns></returns>
        public static List<T> FindListByPage<T>(string connectionString, string dbName, string collectionName,
            FilterDefinition<T> filter, int pageIndex, int pageSize, 
            out long count, SortDefinition<T> sort = null)where T:EntityBase
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);

            //[Obsolete("Use CountDocuments or EstimatedDocumentCount instead.")]
            count = collection.CountDocuments(filter);
            if (sort == null)
                return collection.Find(filter).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
            //进行排序
            return collection.Find(filter).Sort(sort).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
        }
        /// <summary>
        /// 检查更新是否成功1
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool UpdateSuccessfulOrFailed(UpdateResult result)
        {
            if (result.MatchedCount == result.ModifiedCount && result.ModifiedCount > 0)
                return true;
            return false;
        }

        /// <summary>
        /// 检查删除是否成功1
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool DeleteSuccessfulOrFailed(DeleteResult result)
        {
            if (result.DeletedCount == result.DeletedCount && result.DeletedCount > 0)
                return true;
            return false;
        }
    }
}
