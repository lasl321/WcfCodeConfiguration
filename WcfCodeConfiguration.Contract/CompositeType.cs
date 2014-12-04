using System.Runtime.Serialization;

namespace WcfCodeConfiguration.Contract
{
    [DataContract]
    public class CompositeType
    {
        [DataMember]
        public bool BoolValue { get; set; }

        [DataMember]
        public string StringValue { get; set; }

        public override string ToString()
        {
            return string.Format("BoolValue: {0}, StringValue: {1}", BoolValue, StringValue);
        }

        protected bool Equals(CompositeType other)
        {
            return BoolValue.Equals(other.BoolValue) && string.Equals(StringValue, other.StringValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CompositeType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (BoolValue.GetHashCode() * 397) ^ (StringValue != null ? StringValue.GetHashCode() : 0);
            }
        }
    }
}