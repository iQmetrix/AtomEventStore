using System;
using System.Linq;

namespace Grean.AtomEventStore.UnitTests
{
    public class AtomFeedLikeness
    {
        private readonly DateTimeOffset minimumTime;
        private readonly UuidIri expectedId;
        private readonly object[] expectedEvents;

        public AtomFeedLikeness(
            DateTimeOffset minimumTime,
            UuidIri expectedId,
            params object[] expectedEvents)
        {
            this.minimumTime = minimumTime;
            this.expectedId = expectedId;
            this.expectedEvents = expectedEvents;
        }

        public override bool Equals(object obj)
        {
            var actual = obj as AtomFeed;
            if (actual == null)
                return base.Equals(obj);

            var expectedEntries = expectedEvents
                .Select(e => new AtomEntryLikeness(minimumTime, e))
                .Cast<object>();

            return Equals(expectedId, actual.Id)
                && (Equals("Index of event stream " + (Guid)expectedId, actual.Title) || "Partial event stream" == actual.Title)
                && minimumTime <= actual.Updated
                && actual.Updated <= DateTimeOffset.Now
                && expectedEntries.SequenceEqual(actual.Entries)
                && actual.Links.Any(IsExpectedSelfLink);
        }

        private bool IsExpectedSelfLink(AtomLink link)
        {
            if (!link.IsSelfLink)
                return false;
            return GetIdFromHref(link.Href) == (Guid)expectedId;
        }

        /* Copied from AtomEventStorage, suggesting a potential piece of API
         * that may want to become public. However, I'm still adhering to the
         * Rule of Three. */
        private static Guid GetIdFromHref(Uri href)
        {
            /* The assumption here is that the href argument is always going to
             * be a relative URL. So far at least, that's consistent with how
             * AtomEventStore works.
             * However, the Segments property only works for absolute URLs. */
            var fakeBase = new Uri("http://grean.com");
            var absoluteHref = new Uri(fakeBase, href);
            // The ID is assumed to be contained in the last segment of the URL
            var lastSegment = absoluteHref.Segments.Last();
            return new Guid(lastSegment);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
