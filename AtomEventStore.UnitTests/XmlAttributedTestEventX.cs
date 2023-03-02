using System.Xml.Serialization;

namespace Grean.AtomEventStore.UnitTests
{
    [XmlRoot("test-event-x", Namespace = "http://grean:rocks")]
    public class XmlAttributedTestEventX : IXmlAttributedTestEvent
    {
        [XmlElement("number")]
        public int Number { get; set; }

        [XmlElement("text")]
        public string Text { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as XmlAttributedTestEventX;
            if (other == null)
            return base.Equals(obj);

            return Equals(Number, other.Number)
                && Equals(Text, other.Text);
        }

        public override int GetHashCode()
        {
            return
                Number.GetHashCode() ^
                Text.GetHashCode();
        }

        public IXmlAttributedTestEventVisitor Accept(
            IXmlAttributedTestEventVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
