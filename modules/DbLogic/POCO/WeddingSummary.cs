using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbLogic.POCO
{
    public class WeddingSummary
    {
        public Wedding Wedding { get; set; }
        public WeddingPerson Groom { get; set; }
        public WeddingPerson Bride { get; set; }
    }
}
