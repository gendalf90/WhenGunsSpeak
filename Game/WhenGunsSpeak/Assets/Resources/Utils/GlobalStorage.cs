using System.Collections.Generic;

namespace Utils
{
    public static class GlobalStorage
    {
        public static IDictionary<string, string> Parameters { get; } = new Dictionary<string, string>();
    }
}
