using System;

namespace Grean.AtomEventStore
{
    internal static  class GuidExtensions
    {
        public static Guid ToGuid(this long value)
        {
            var guidData = new byte[16];
            Array.Copy(BitConverter.GetBytes(value), guidData, 8);
            return new Guid(guidData);
        }

        public static bool TryParseLong(this Guid guid, out long number)
        {
            number = 0;
            if (BitConverter.ToInt64(guid.ToByteArray(), 8) != 0)
                return false;
            number = BitConverter.ToInt64(guid.ToByteArray(), 0);
            return true;
        }
    }
}
