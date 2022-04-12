using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.TFSM.Business.Model
{
    public class Cars
    {
        public int Id { get; set; }
        public string Auto { get; set; }
        public string PathCarImage { get; set; }
        public string PathNameImage { get; set; }
        public bool IsMatched { get; set; }
        public string Carpet { get; set; }

    }
}
