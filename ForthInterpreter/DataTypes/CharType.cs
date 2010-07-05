
namespace ForthInterpreter.DataTypes
{
    public static class CharType
    {
        public static char ToChar(int cellValue) { return (char)(byte)cellValue; }
        public static byte ToByte(char ch) { return (byte)ch; }
        public static int ToCell(char ch) { return (int)ToByte(ch); }
    }
}
