using System.IO;

namespace Battleships
{
    public static class GlobalVar
    {
        public static readonly string[] Letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        public static readonly string Path = Directory.GetCurrentDirectory();
    }
}
