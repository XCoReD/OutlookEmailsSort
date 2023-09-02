using System;
using System.Collections.Generic;
using System.Text;
using Nager.PublicSuffix;

namespace SettingsUI
{
    public static class StringEx
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

        public static string RemoveDomain1stLevel(this string input)
        {
            int ip = input.LastIndexOf('.');
            if (ip == -1)
                return input;

            if(domainParser == null)
            {
                domainParser = new DomainParser(new WebTldRuleProvider());
            }

            var domainInfo = domainParser.Parse(input);
            if (string.IsNullOrEmpty(domainInfo.TLD))
                return input;

            var tp = input.LastIndexOf(domainInfo.TLD);
            if (tp <= 0)
                return input;
            return input.Substring(0, tp - 1);
        }

        static DomainParser domainParser; 

    }
}
