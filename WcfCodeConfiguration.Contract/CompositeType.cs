using System.Runtime.Serialization;

namespace WcfCodeConfiguration.Contract
{
    [DataContract]
    public class CompositeType
    {
        public CompositeType()
        {
            StringValue = "Hello ";
            BoolValue = true;
        }

        [DataMember]
        public bool BoolValue { get; set; }

        [DataMember]
        public string StringValue { get; set; }
    }
}