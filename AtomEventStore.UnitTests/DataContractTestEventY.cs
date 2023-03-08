using System.Runtime.Serialization;

namespace Grean.AtomEventStore.UnitTests
{
    [DataContract(Name = "test-event-y", Namespace = "http://grean.rocks/dc")]
    public class DataContractTestEventY : IDataContractTestEvent
    {
        [DataMember(Name = "number")]
        public long Number { get; set; }

        [DataMember(Name = "text")]
        public bool IsTrue { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as DataContractTestEventY;
            if (other != null)
                return Equals(Number, other.Number)
                    && Equals(IsTrue, other.IsTrue);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return
                Number.GetHashCode() ^
                IsTrue.GetHashCode();
        }

        public IDataContractTestEventVisitor Accept(
            IDataContractTestEventVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
