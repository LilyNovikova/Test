using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexMarket.utils
{
    public class ListUtils<T>
    {
        public static bool Contains(List<T> expected, List<T> actual)
        {
            
            foreach(T item in actual)
            {
                if(!expected.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public static string ToString(List<string> list)
        {
            string text = "";
            foreach (string item in list)
            {
                text += item + ",\n";
            }
            return text.TrimEnd('\n', ',');
        }
    }
}
