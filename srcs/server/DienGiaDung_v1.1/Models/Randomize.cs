using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public static class Randomize
    {
        static Random _engine = new Random((int)DateTime.Now.Ticks);
        public static int[] Swap(this int[] a, int i, int j)
        {
            int t = a[i];
            a[i] = a[j];
            a[j] = t;
            return a;
        }

        public static int Next(int max) => _engine.Next(max);
        public static int Next(int min, int max) => _engine.Next(min, max);
        public static int[] Index(int length)
        {
            var r = new int[length];
            for (int i = 0; i < length; i++)
            {
                r[i] = i;
            }
            return r;
        }
        public static int[] Mix(this int[] index)
        {
            int n = index.Length;
            while (n > 2)
            {
                int k = Next(n);
                index.Swap(k, --n);
            }
            return index;
        }
    }
}