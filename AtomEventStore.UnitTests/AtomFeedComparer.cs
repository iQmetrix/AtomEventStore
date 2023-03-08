using System.Collections.Generic;
using System.Linq;

namespace Grean.AtomEventStore.UnitTests
{
    public class AtomFeedComparer : IEqualityComparer<AtomFeed>
    {
        private readonly AtomEntryComparer entryComparer;

        public AtomFeedComparer()
        {
            entryComparer = new AtomEntryComparer();
        }

        public bool Equals(AtomFeed x, AtomFeed y)
        {
            return Equals(x.Id, y.Id)
                && Equals(x.Title, x.Title)
                && Equals(x.Updated, y.Updated)
                && Equals(x.Author, y.Author)
                && x.Entries.SequenceEqual(y.Entries, entryComparer)
                && new HashSet<AtomLink>(x.Links).SetEquals(y.Links);
        }

        public int GetHashCode(AtomFeed obj)
        {
            return 0;
        }
    }
}
