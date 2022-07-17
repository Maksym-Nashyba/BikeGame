using System;
using System.Linq;

namespace IGUIDResources
{
    public static class GUIDs
    {
        public const string Default = "111111111";
        public const int Length = 9;
        private static Random random = new Random();
        
        public static string GetNew()
        {
            return RandomString(Length);
        }
        
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool IsValidGUID(string guid)
        {
            return guid.Length == 9;
        }
    }
}