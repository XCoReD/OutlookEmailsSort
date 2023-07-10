using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsUI
{
    public static class StringExtensions
    {
        //https://stackoverflow.com/questions/6219454/efficient-way-to-remove-all-whitespace-from-string
        public static string RemoveWhitespaces(this string input)
        {
            int j = 0, inputlen = input.Length;
            char[] newarr = new char[inputlen];

            for (int i = 0; i < inputlen; ++i)
            {
                char tmp = input[i];

                if (!char.IsWhiteSpace(tmp))
                {
                    newarr[j] = tmp;
                    ++j;
                }
            }
            return new String(newarr, 0, j);
        }

        public static string RemoveNonAlpha(this string input)
        {
            int j = 0, inputlen = input.Length;
            char[] newarr = new char[inputlen];

            for (int i = 0; i < inputlen; ++i)
            {
                char tmp = input[i];

                if (char.IsDigit(tmp) || char.IsLetter(tmp))
                {
                    newarr[j] = tmp;
                    ++j;
                }
            }
            return new String(newarr, 0, j);
        }

        static string[] _1stLevelEnds = new string[]
        {
            ".com",
            ".ru",
            ".net",
            ".org",
            ".by",
            ".com.by",
            ".de",
            ".fr",
            ".it",
            ".eu",
            ".uk",
            ".co.uk",
            ".in",
            ".us",
            ".ua",
            ".com.ua",
            ".io",
            ".biz",
            ".media",
            ".info",
            ".name",
            ".software",
        };
        public static string RemoveDomain1stLevel(this string input)
        {
            int ip = input.LastIndexOf('.');
            if (ip == -1)
                return input;
            input = input.ToLowerInvariant();
            foreach(var v in _1stLevelEnds)
            {
                int i = input.LastIndexOf(v);
                if (i != -1)
                    return input.Substring(0, i);
            }
            return input;
        }


    }
}
