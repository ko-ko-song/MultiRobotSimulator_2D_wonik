using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class ByteBuffer
    {
        List<byte> buffer;
        
        public ByteBuffer()
        {
            buffer = new List<byte>();
        }

        public float getFloat()
        {
            byte[] b = buffer.ToArray();
            float value = BitConverter.ToSingle(b, 0);
            b = b.Skip(4).ToArray();
            buffer = b.ToList<byte>();
            return value;
        }
        public int getInt()
        {
            byte[] b = buffer.ToArray();
            int value = BitConverter.ToInt32(b, 0);
            b = b.Skip(4).ToArray();
            buffer = b.ToList<byte>();
            return value;
        }
        
        public bool get()
        {
            byte[] b = buffer.ToArray();
            bool value = BitConverter.ToBoolean(b,0);
            b = b.Skip(4).ToArray();
            buffer = b.ToList<byte>();
            return value;
        }


        public void putInt(int value)
        {
            byte[] intBytes = BitConverter.GetBytes(value);
            for (int i = 0; i < intBytes.Length; i++)
            {
                buffer.Add(intBytes[i]);
            }
        }

        public void wrap(byte[] b)
        {
            buffer = b.ToList<byte>();
        }
        
        public byte[] getArray()
        {
            return buffer.ToArray();
        }

        public int getSize()
        {
            return buffer.Count;
        }


    }
}
