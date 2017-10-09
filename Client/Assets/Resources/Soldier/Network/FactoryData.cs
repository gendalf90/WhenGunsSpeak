using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Factory
{
    [BinaryObjectType("47642601-D350-429D-982E-CEE966FCB001")]
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