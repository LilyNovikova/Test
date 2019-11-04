using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.UnitTests
{
    class TestUtils
    {
        public static Object GetClassField(Object obj, String FieldName)
        {
                var field = obj.GetType().GetField(FieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                return field != null ? field.GetValue(obj): null;

            //return typeof(Object).GetField(FieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        }

        public static String GenerateSymbols(long length)
        {
            if (length < 0)
            {
                return null;
            }
            else
            if (length == 0)
            {
                return "";
            }
            return "a" + GenerateSymbols(length - 1);
        }
    }
}
