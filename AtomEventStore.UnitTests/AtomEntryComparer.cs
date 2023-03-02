using System.Collections.Generic;

namespace Grean.AtomEventStore.UnitTests
{
    public class AtomEntryComparer : IEqualityComparer<AtomEntry>
    {
        public bool Equals(AtomEntry x, AtomEntry y)
        {
            return Equals(x.Id, y.Id)
                && Equals(x.Title, y.Title)
                && Equals(x.Published, y.Published)
                && Equals(x.Updated, y.Updated)
                && Equals(x.Author, y.Author)
                && Equals(x.Content, y.Content)
                && new HashSet<AtomLink>(x.Links).SetEquals(y.Links);
        }

        public int GetHashCode(AtomEntry obj)
        {
            return 0;
        }
    }
}
