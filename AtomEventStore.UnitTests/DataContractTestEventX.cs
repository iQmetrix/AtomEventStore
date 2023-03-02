using System.Runtime.Serialization;

namespace Grean.AtomEventStore.UnitTests
{
    [DataContract(Name = "test-event-x", Namespace = "http://grean.rocks/dc")]
    public class DataContractTestEventX : IDataContractTestEvent
    {
        [DataMember(Name = "number")]
        public string Number { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as DataContractTestEventX;
            if (other != null)
                return Equals(Number, other.Number)
                    && Equals(Text, other.Text);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return
                Number.GetHashCode() ^
                Text.GetHashCode();
        }

        public IDataContractTestEventVisitor Accept(
            IDataContractTestEventVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
