using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBDemo.Model
{
    public interface MongoBaseEntity
    {
        ObjectId Id { get; set; }
        string Creator { get; set; }
        string CreateDate { get; set; }
        string LastEditer { get; set; }
        string LastEditDate { get; set; }
        string SystemName { get; }
        string EX { get; set; }
        string IP { get; }
    }
}
