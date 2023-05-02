using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.Common
{
    public class Result<T>
    {
        public Status status { get; set; }
        public T value { get; set; }
        public int page_count { get; set; }
        public int total_count { get; set; }
        public int page_index { get; set; }

        public Result()
        {
            this.status = new Status() { code = 0, description = "SUCCESS" };
        }
    }
}
