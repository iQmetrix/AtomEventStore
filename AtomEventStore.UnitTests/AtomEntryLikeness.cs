using System;

namespace Grean.AtomEventStore.UnitTests
{
    public class AtomEntryLikeness
    {
        private readonly DateTimeOffset minimumTime;
        private readonly object expectedEvent;

        public AtomEntryLikeness(
            DateTimeOffset minimumTime,
            object expectedEvent)
        {
            this.minimumTime = minimumTime;
            this.expectedEvent = expectedEvent;
        }

        public override bool Equals(object obj)
        {
            var actual = obj as AtomEntry;
            if (actual == null)
                return base.Equals(obj);

            return !Equals(default(UuidIri), actual.Id)
                && Equals("Changeset " + (Guid)actual.Id, actual.Title)
                && minimumTime <= actual.Published
                && actual.Published <= DateTimeOffset.Now
                && minimumTime <= actual.Updated
                && actual.Updated <= DateTimeOffset.Now
                && Equals(expectedEvent, actual.Content.Item);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
