using System.Xml.Serialization;

namespace Grean.AtomEventStore.UnitTests
{
    [XmlRoot("test-event-y", Namespace = "http://grean:rocks")]
    public class XmlAttributedTestEventY : IXmlAttributedTestEvent
    {
        [XmlElement("number")]
        public decimal Number { get; set; }

        [XmlElement("flag")]
        public bool Flag { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as XmlAttributedTestEventY;
            if (other == null)
            return base.Equals(obj);

            return Equals(Number, other.Number)
                && Equals(Flag, other.Flag);
        }

        public override int GetHashCode()
        {
            return
                Number.GetHashCode() ^
                Flag.GetHashCode();
        }

        public IXmlAttributedTestEventVisitor Accept(
            IXmlAttributedTestEventVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
