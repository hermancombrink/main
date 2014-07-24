using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace wedding.logic.POCO
{
    [DataContract]
    public class VIPPerson
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public string TypeName { get; set; }
    }
}
