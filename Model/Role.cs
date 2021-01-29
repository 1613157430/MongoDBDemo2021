using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBDemo.Model
{
    public class Role:EntityBase
    {
        public string UserName { get; set; }
        public int Age { get; set; }
        public State State { get; set; }
    }
}
