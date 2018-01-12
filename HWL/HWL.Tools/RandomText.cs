using System;
using System.Text;

namespace HWL.Tools
{
    /// <summary>
    /// 提供随机文本
    /// </summary>
    public sealed class RandomText
    {
        static readonly string[] source ={"2","3","4","5","6","7","8","9",
            "a","b","c","d","e","f","h","j","k","m","n","p","r","s","t","u","v","w","x","y","z",
            "A","B","C","D","E","F","G","H","J","K","M","N","P","Q","R","S","T","U","V","W","X","Y","Z"};
        static readonly string[] source_num = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        private static Random _random;

        static RandomText()
        {
            _random = new Random(Environment.TickCount);
        }

        public static int Number(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public static string GetString(int length)
        {
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = Number(0, source.Length - 1);
                s.Append(source[index]);
            }

            return s.ToString();
        }

        public static string GetNum(int length = 6)
        {
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = Number(0, source_num.Length - 1);
                s.Append(source_num[index]);
            }

            return s.ToString();
        }

        public static string CharOnly(int length)
        {
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int c = _random.Next(97, 123);

                if (_random.Next() % 2 == 0)
                    c -= 32;

                s.Append(Convert.ToChar(c));
            }

            return s.ToString();
        }
    }
}
