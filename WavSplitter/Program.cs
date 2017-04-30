using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavSplitter
{
    class Program
    {
        /// <summary>
        /// command line example:
        /// <example>WavSplitter --source 3Notes.wav --duration 00:00:02 --output note</example>
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var cmdOptions = new CmdOptions();
            if (CommandLine.Parser.Default.ParseArgumentsStrict(args, cmdOptions))
            {
                WaveUtils.SplitWave(cmdOptions.Source, cmdOptions.Duration ?? new TimeSpan(), cmdOptions.Output);
                Console.WriteLine("Done.");
            }
        }
    }
}
