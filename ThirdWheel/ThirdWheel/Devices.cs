using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ThirdWheel
{
    [DataContract]
    public class Devices
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public string FriendlyName { get; set; }
       // [DataMember]
       // public string Text { get; set; }


    }
}
