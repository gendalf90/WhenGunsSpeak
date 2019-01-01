using MessagePack;
using System;
using System.Collections.Generic;

namespace Shells
{
    [MessagePackObject]
    public class ShellsSyncData
    {
        [Key(0)]
        public string TypeMarker { get; set; }

        [Key(1)]
        public HashSet<Guid> ShellIdsCreatedAtNow { get; set; }
    }
}
