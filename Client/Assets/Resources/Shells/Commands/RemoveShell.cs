using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shells
{
    public class RemoveShellCommand
    {
        public RemoveShellCommand(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }
    }
}
