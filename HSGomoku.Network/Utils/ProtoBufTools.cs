using System;
using System.IO;

using ProtoBuf;

namespace HSGomoku.Network.Utils
{
    public static class ProtoBufTools
    {
        public static Byte[] Serialize(Object obj)
        {
            try
            {
                using (var memory = new MemoryStream())
                {
                    Serializer.Serialize(memory, obj);
                    return memory.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static T Deserialize<T>(Byte[] data)
        {
            try
            {
                using (var memory = new MemoryStream(data))
                {
                    return Serializer.Deserialize<T>(memory);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(T);
            }
        }
    }
}