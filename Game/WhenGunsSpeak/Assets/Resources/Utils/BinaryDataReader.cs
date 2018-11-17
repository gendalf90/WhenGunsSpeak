using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;

public class BinaryDataReader : IDisposable
{
    private const int GuidLength = 16;

    private MemoryStream stream;
    private BinaryReader reader;

    public BinaryDataReader(byte[] data)
    {
        stream = new MemoryStream(data);
        reader = new BinaryReader(stream);
    }

    public bool ReadBoolean()
    {
        return reader.ReadBoolean();
    }

    public byte ReadByte()
    {
        return reader.ReadByte();
    }

    public byte[] ReadBytes(int count)
    {
        return reader.ReadBytes(count);
    }

    public byte[] ReadBytes()
    {
        var result = new byte[stream.Length - stream.Position];
        stream.Read(result, 0, result.Length);
        return result;
    }

    public float ReadFloat()
    {
        return reader.ReadSingle();
    }

    public int ReadInt()
    {
        return reader.ReadInt32();
    }

    public string ReadString()
    {
        return reader.ReadString();
    }

    public Guid ReadGuid()
    {
        return new Guid(reader.ReadBytes(GuidLength));
    }

    public Guid[] ReadGuids(int count)
    {
        var result = new Guid[count];

        for(int i = 0; i < result.Length; i++)
        {
            result[i] = ReadGuid();
        }

        return result;
    }

    public Vector2 ReadVector()
    {
        return new Vector2(reader.ReadSingle(), reader.ReadSingle());
    }

    public T ReadFromJson<T>()
    {
        var json = reader.ReadString();
        return JsonUtility.FromJson<T>(json);
    }

    public object Deserialize()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        return formatter.Deserialize(stream);
    }

    public void Dispose()
    {
        stream.Dispose();
    }
}
