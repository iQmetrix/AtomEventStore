﻿using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Grean.AtomEventStore.UnitTests
{
    public class XmlWritableTests
    {
        [Theory, AutoAtomData]
        public void ToXmlStringReturnsCorrectResult(
            TestXmlWritable writable,
            IContentSerializer dummySerializer)
        {
            var actual = writable.ToXmlString(dummySerializer);

            var sb = new StringBuilder();
            using (var w = XmlWriter.Create(sb))
            {
                writable.WriteTo(w);
                w.Flush();
            }
            var expected = XDocument.Parse(sb.ToString());
            Assert.Equal(
                expected,
                XDocument.Parse(actual),
                new XNodeEqualityComparer());
        }

        [Theory, AutoAtomData]
        public void ToXmlStringWithSettingsReturnsCorrectResult(
            TestXmlWritable writable,
            IContentSerializer dummySerializer)
        {
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            };

            var actual = writable.ToXmlString(dummySerializer, settings);

            var sb = new StringBuilder();
            using (var w = XmlWriter.Create(sb, settings))
            {
                writable.WriteTo(w);
                w.Flush();
            }
            var expected = XDocument.Parse(sb.ToString());
            Assert.Equal(
                expected,
                XDocument.Parse(actual),
                new XNodeEqualityComparer());
            Assert.False(
                actual.StartsWith("<?"),
                "XML declaration not expected due to XmlWriterSettings");
        }

        public class TestXmlWritable : IXmlWritable
        {
            public readonly string documentName;
            public readonly string elementName;
            public readonly string elementValue;

            public TestXmlWritable(
                string documentElement,
                string elementName,
                string elementValue)
            {
                documentName = documentElement;
                this.elementName = elementName;
                this.elementValue = elementValue;
            }

            public void WriteTo(XmlWriter xmlWriter)
            {
                xmlWriter.WriteStartElement(documentName);
                xmlWriter.WriteElementString(elementName, elementValue);
                xmlWriter.WriteEndElement();
            }

            public void WriteTo(XmlWriter xmlWriter, IContentSerializer serializer)
            {
                WriteTo(xmlWriter);
            }
        }
    }
}
