using System.IO;

namespace Loto
{
    public static class Loger
    {
        public static void Write(string[] info)
        {
            File.WriteAllLines(@"C:\Users\Maksym\Desktop\WonNumbers.txt", info);
        }
    }
}