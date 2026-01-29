using System;

namespace Space_Expedition
{
    internal class Encode
    {
        private static char[] original = {
            'A','B','C','D','E','F','G','H','I','J','K','L','M',
            'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
        };

        private static char[] mapped = {
            'H','Z','A','U','Y','E','K','G','O','T','I','R','J',
            'V','W','N','M','F','Q','S','D','B','X','L','C','P'
        };

        public static string DecodeName(string encoded)
        {
            string result = "";
            for (int i = 0; i < encoded.Length; i++)
            {
                if (char.IsLetter(encoded[i]) && i + 1 < encoded.Length && char.IsDigit(encoded[i + 1]))
                {
                    char letter = encoded[i];
                    int level = encoded[i + 1] - '0';
                    result += DecodeChar(letter, level);
                    i++;
                }
            }
            return result;
        }
            private static char DecodeChar(char ch, int level)
        {
            if (level == 0)
                return (char)('Z' - (ch - 'A'));

            char next = ch;
            for (int i = 0; i < 26; i++)
            {
                if (original[i] == ch)
                {
                    next = mapped[i];
                    break;
                }
            }
            return DecodeChar(next, level - 1);
        }
    }
}
