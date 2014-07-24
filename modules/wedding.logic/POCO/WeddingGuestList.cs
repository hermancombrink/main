using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace wedding.logic.POCO
{
    [DataContract]
    public class WeddingGuestList
    {
        [DataMember]
        public List<WeddingPerson> BestMen { get; set; }

        [DataMember]
        public List<WeddingPerson> MaidOfHonour { get; set; }

        [DataMember]
        public List<WeddingPerson> Groomsmen { get; set; }

        [DataMember]
        public List<WeddingPerson> Bridesmaids { get; set; }

        [DataMember]
        public List<WeddingPerson> BridesFamily { get; set; }

        [DataMember]
        public List<WeddingPerson> GroomsFamily { get; set; }
        
        [DataMember]
        public List<WeddingPerson> GroomsParents { get; set; }

        [DataMember]
        public List<WeddingPerson> BridesParents { get; set; }
     
        [DataMember]
        public List<WeddingPerson> Friends { get; set; }
    }

}
