using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using Network;
using System.Net;

public class BinaryDataBuilder
{
    private MemoryStream stream;
    private BinaryWriter writer;

    public BinaryDataBuilder()
    {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);
    }

    public BinaryDataBuilder Write(bool value)
    {
        writer.Write(value);
        return this;
    }

    public BinaryDataBuilder Write(byte value)
    {
        writer.Write(value);
        return this;
    }

    public BinaryDataBuilder Write(byte[] value)
    {
        writer.Write(value);
        return this;
    }

    public BinaryDataBuilder Write(int value)
    {
        writer.Write(value);
        return this;
    }

    public BinaryDataBuilder Write(float value)
    {
        writer.Write(value);
        return this;
    }

    public BinaryDataBuilder Write(string value)
    {
        writer.Write(value);
        return this;
    }

    public BinaryDataBuilder Write(Guid value)
    {
        writer.Write(value.ToByteArray());
        return this;
    }

    public BinaryDataBuilder Write(Guid[] value)
    {
        foreach(var item in value)
        {
            Write(item);
        }
        return this;
    }

    public BinaryDataBuilder Write(Vector2 value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        return this;
    }

    public BinaryDataBuilder WriteAsJson<T>(T obj)
    {
        var json = JsonUtility.ToJson(obj);
        writer.Write(json);
        return this;
    }

    public BinaryDataBuilder Serialize(object obj)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, obj);
        return this;
    }

    public byte[] Build()
    {
        var result = stream.ToArray();
        stream.Dispose();
        return result;
    }
}
