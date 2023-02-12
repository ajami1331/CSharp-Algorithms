// Helper.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 17:50

namespace CLown1331.Helper
{
    using System;
    using System.Collections.Generic;
    using CLown1331.IO;

    public static class Helper
    {
        private static readonly IEnumerator<uint> Xsi = Xsc();

        public static uint XorShift
        {
            get
            {
                Xsi.MoveNext();
                return Xsi.Current;
            }
        }

        private static IEnumerator<uint> Xsc()
        {
            uint x = 123456789, y = 362436069, z = 521288629, w = (uint)(DateTime.Now.Ticks & 0xffffffff);
            while (true)
            {
                uint t = x ^ x << 11;
                x = y;
                y = z;
                z = w;
                w = w ^ w >> 19 ^ t ^ t >> 8;
                yield return w;
            }
        }

#if CLown1331
        private static void Debug<T>(
            IEnumerable<T> args,
            int len = int.MaxValue,
            [System.Runtime.CompilerServices.CallerLineNumber]
            int callerLinerNumber = default(int),
            [System.Runtime.CompilerServices.CallerMemberName]
            string callerMemberName = default(string))
        {
            var count = 0;
            foreach (T arg in args)
            {
                Output.ErrorPrinter.Write(arg + " ");
                if (++count >= len)
                {
                    break;
                }
            }

            Output.ErrorPrinter.WriteLine($"Method: {callerMemberName} Line: {callerLinerNumber}");
        }

        private static void Debug(params object[] args)
        {
            foreach (object arg in args)
            {
                Output.ErrorPrinter.Write(arg + " ");
            }

            Output.ErrorPrinter.WriteLine();
        }
#endif
    }
}
