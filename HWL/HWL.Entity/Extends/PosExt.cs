using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Entity.Extends
{
    public class AreaModel
    {
        public int value { get; set; }
        public string text { get; set; }
        public List<AreaModel> children { get; set; }
    }
}
