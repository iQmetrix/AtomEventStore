using System;

namespace Grean.AtomEventStore.UnitTests
{
    public class AtomEntryBuilder<T>
    {
        private readonly AtomEntry entry;
        private readonly T item;

        public AtomEntryBuilder(AtomEntry entry, T item)
        {
            this.entry = entry;
            this.item = item;
        }

        public AtomEntry Build()
        {
            return entry
                .WithContent(entry.Content.WithItem(item))
                .AddLink(AtomLink.CreateSelfLink(
                    new Uri(
                        ((Guid)entry.Id).ToString(),
                        UriKind.Relative)));
        }

        public static implicit operator AtomEntry(AtomEntryBuilder<T> builder)
        {
            return builder.Build();
        }
    }
}
