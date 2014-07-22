using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DbLogic.POCO
{
    [DataContract]
    public class MenuNodes
    {
        [DataMember(Name="NodeItems")]
        public List<MenuNode> Nodes = new List<MenuNode>();
    }
    [DataContract]
    public class MenuNode
    {
        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

    }
}
