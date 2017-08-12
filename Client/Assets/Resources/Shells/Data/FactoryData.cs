using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shells
{
    [BinaryObjectType("359d0ebf-1b28-49bb-b96d-909c87fbafe0")]
    class FactoryData : IBinaryObject
    {
        public Guid[] InstantiatedGuids { get; set; }

        public void FromBytes(byte[] bytes)
        {
            using (var reader = new BinaryDataReader(bytes))
            {
                InstantiatedGuids = reader.ReadGuids(reader.ReadInt());
            }
        }

        public byte[] ToBytes()
        {
            return new BinaryDataBuilder().Write(InstantiatedGuids.Length)
                                          .Write(InstantiatedGuids)
                                          .Build();
        }
    }
}