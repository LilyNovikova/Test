using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage;

namespace FileStorage
{
    public class Program
    {
        public static void Main(String[]args)
        {
            ;
        }

        public int field;

        public Program(int i)
        {
            field = i;
        }

        public double getHalf()
        {
            return (double) field / 2;
        }
    }
}
