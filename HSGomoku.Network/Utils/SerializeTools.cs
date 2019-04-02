using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HSGomoku.Network.Utils
{
    public static class SerializeTools
    {
        public static Byte[] Serialize(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T Deserialize<T>(Byte[] arrBytes) where T : class
        {
            using (var memStream = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = bf.Deserialize(memStream);
                return obj as T;
            }
        }
    }
}