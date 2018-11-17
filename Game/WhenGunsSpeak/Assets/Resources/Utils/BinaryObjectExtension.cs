using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IBinaryObject
{
    byte[] ToBytes();
    void FromBytes(byte[] bytes);
}

[AttributeUsage(AttributeTargets.Class)]
public class BinaryObjectTypeAttribute : Attribute
{
    public BinaryObjectTypeAttribute(string type)
    {
        Type = new Guid(type);
    }

    public Guid Type { get; private set; }
}

public static class BinaryObjectExtension
{
    private const int TypeLength = 16;

    public static T AsBinaryObject<T>(this byte[] bytes) where T : class, IBinaryObject, new()
    {
        var typeId = GetBinaryObjectTypeGuidFromAttribute(typeof(T));

        if (bytes.Length < TypeLength)
        {
            return null;
        }

        var packetIdByteArray = new byte[TypeLength];
        Array.Copy(bytes, 0, packetIdByteArray, 0, packetIdByteArray.Length);
        var packetTypeId = new Guid(packetIdByteArray);

        if (packetTypeId != typeId)
        {
            return null;
        }

        var bodyByteArray = new byte[bytes.Length - TypeLength];
        Array.Copy(bytes, TypeLength, bodyByteArray, 0, bodyByteArray.Length);
        return CreateBinaryObject<T>(bodyByteArray);
    }

    public static IEnumerable<T> OfBinaryObjects<T>(this IEnumerable<byte[]> packets) where T : class, IBinaryObject, new()
    {
        return packets.Select(packet => packet.AsBinaryObject<T>()).Where(result => result != null);
    }

    private static T CreateBinaryObject<T>(byte[] content) where T : class, IBinaryObject, new()
    {
        var newObject = new T();
        newObject.FromBytes(content);
        return newObject;
    }

    private static Guid GetBinaryObjectTypeGuidFromAttribute(Type type)
    {
        var packetTypeGuidAttr = (BinaryObjectTypeAttribute)Attribute.GetCustomAttribute(type, typeof(BinaryObjectTypeAttribute));
        return packetTypeGuidAttr != null ? packetTypeGuidAttr.Type : Guid.Empty;
    }

    public static byte[] CreateBinaryObjectBytes<T>(this T data) where T : class, IBinaryObject, new()
    {
        var binaryObjectTypeId = GetBinaryObjectTypeGuidFromAttribute(typeof(T));
        var typeBytes = binaryObjectTypeId.ToByteArray();
        var bodyBytes = data.ToBytes();
        var result = new byte[typeBytes.Length + bodyBytes.Length];
        typeBytes.CopyTo(result, 0);
        bodyBytes.CopyTo(result, typeBytes.Length);
        return result;
    }
}
