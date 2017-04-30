using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace WavSplitter
{
    
    public class CmdOptions
    {
        [Option('s', "source", Required = true, HelpText = "source .Wav file to be processed")]
        public string Source { get; set; }

        [Option('d', "duration", Required = true, HelpText = "Part duration - (for time format use 00:00:02, 00:01:00, etc.")]
        // work only with Nullable type
        public TimeSpan? Duration { get; set; }

        [Option('o', "output", Required = false, DefaultValue = "out", HelpText = "output part file name")]
        public string Output { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

    }
}
