using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;


public class ByteBuffer
{

    

    List<byte> buffer;
    public ByteBuffer()
    {
        if (!BitConverter.IsLittleEndian)
        {
            Debug.Log(BitConverter.IsLittleEndian);
        }
        buffer = new List<byte>();
    }
    public int getInt()
    {
        if (buffer == null || buffer.Count==0)
        {
            throw new Exception("message size not match protocol model and received message");
        }
        byte[] b = buffer.ToArray();
        int value = BitConverter.ToInt32(b, 0);
        b = b.Skip(4).ToArray();
        buffer = b.ToList<byte>();
        return value;
    }

    public bool get()
    {
        byte[] b = buffer.ToArray();
        bool value = BitConverter.ToBoolean(b, 0);
        b = b.Skip(4).ToArray();
        buffer = b.ToList<byte>();
        return value;
    }

    public void put(bool value)
    {
        byte b = Convert.ToByte(value);
        buffer.Add(b);
    }

    public void putInt(int value)
    {
        byte[] intBytes = BitConverter.GetBytes(value);
        for (int i = 0; i < intBytes.Length; i++)
        {
            buffer.Add(intBytes[i]);
        }
    }
    
    public void putFloat(float value)
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
