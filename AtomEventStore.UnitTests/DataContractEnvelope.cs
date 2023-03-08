using System;
using System.Runtime.Serialization;

namespace Grean.AtomEventStore.UnitTests
{
    [DataContract(Name = "envelope", Namespace = "http://grean.rocks/dc")]
    [KnownType(typeof(DataContractTestEventX))]
    [KnownType(typeof(DataContractTestEventY))]
    public class DataContractEnvelope<T> where T : IDataContractTestEvent
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "item")]
        public T Item { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DataContractEnvelope<T>)
            {
                var other = (DataContractEnvelope<T>)obj;
                return Equals(Id, other.Id)
                    && Equals(Item, other.Item);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public DataContractEnvelope<TResult> Cast<TResult>() where TResult : IDataContractTestEvent
        {
            return new DataContractEnvelope<TResult>
            {
                Id = Id,
                Item = (TResult)(object)Item
            };
        }
    }
}
