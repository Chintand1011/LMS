using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Entities.View.ProjectRequest
{
    public class ProjectSearchResultVM
    {
        public int page_index { get; set; }
        public int count { get; set; }
        public IEnumerable<ProjectRequestVM> items { get; set; }
        public decimal? total { get; set; }
        public bool vip { get; set; }
    }
}
