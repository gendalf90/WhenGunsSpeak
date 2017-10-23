using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soldier.Motion
{
    class MoveCommand
    {
        public MoveCommand(string session, bool isToRight, bool isToLeft, bool isJump)
        {
            Session = session;
            IsToRight = isToRight;
            IsToLeft = IsToLeft;
            IsJump = isJump;
        }

        public string Session { get; private set; }

        public bool IsToRight { get; private set; }

        public bool IsToLeft { get; private set; }

        public bool IsJump { get; private set; }
    }
}
