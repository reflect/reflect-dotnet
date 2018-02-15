using System;

namespace Reflect.Extensions
{
    public static class GuidExtensions
    {
        public static byte[] ToRFC4122ByteArray(this Guid guid)
        {
            var bytes = guid.ToByteArray();

            // Once again, Microsoft has shown the world that their way is a
            // different way than everyone else's way.
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, 0, 4);
                Array.Reverse(bytes, 4, 2);
                Array.Reverse(bytes, 6, 2);
            }

            return bytes;
        }
    }
}
