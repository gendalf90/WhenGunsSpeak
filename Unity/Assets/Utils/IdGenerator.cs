using System;

namespace Utils
{
    public static class IdGenerator
    {
        public static string Generate()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
