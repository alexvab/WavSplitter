using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavSplitter
{
    class Program
    {
        static void Usage()
        {
            Console.WriteLine("<fileIn> <duration>");
        }
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Usage();
                return;
            }
            var fileIn = args[0];
            TimeSpan duration;
            if (!TimeSpan.TryParse(args[1], out duration))
            {
                Usage();
                return;
            }
            WaveUtils.SplitWave(fileIn, duration, "note");
        }
    }
}
