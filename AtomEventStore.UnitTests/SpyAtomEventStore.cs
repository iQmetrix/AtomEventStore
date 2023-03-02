using System;
using System.Collections.Generic;
using System.Xml;

namespace Grean.AtomEventStore.UnitTests
{
    public class SpyAtomEventStore : IAtomEventStorage
    {
        private readonly AtomEventsInMemory store;
        private readonly List<object> observedArguments;

        public SpyAtomEventStore()
        {
            store = new AtomEventsInMemory();
            observedArguments = new List<object>();
        }

        public IEnumerable<object> ObservedArguments
        {
            get { return observedArguments; }
        }

        public IEnumerable<string> Feeds
        {
            get { return store.Feeds; }
        }

        public XmlReader CreateFeedReaderFor(Uri href)
        {
            observedArguments.Add(href);
            return store.CreateFeedReaderFor(href);
        }

        public XmlWriter CreateFeedWriterFor(AtomFeed atomFeed)
        {
            observedArguments.Add(atomFeed);
            return store.CreateFeedWriterFor(atomFeed);
        }
    }
}
