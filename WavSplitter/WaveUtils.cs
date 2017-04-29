using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace WavSplitter
{
    class WaveUtils
    {
        public static void SplitWave(string wavFileIn, TimeSpan chunkDuration, string wavFileOut)
        {
            using (var waveFileReader = new WaveFileReader(wavFileIn))
            {
                SplitWave(chunkDuration, wavFileOut, waveFileReader);
            }
        }

        private static void SplitWave(TimeSpan chunkDuration, string wavFileOut, WaveFileReader waveFileReader)
        {
            int tolalMilliseconds = (int) GetWaveFileDurationInMilliseconds(waveFileReader);

            var totalDuration = TimeSpan.FromMilliseconds(tolalMilliseconds);

            if (chunkDuration > totalDuration || chunkDuration.TotalMilliseconds < 0)
                chunkDuration = totalDuration;

            int partDurationMilliseconds = (int) chunkDuration.TotalMilliseconds;
            int numberOfParts = tolalMilliseconds / partDurationMilliseconds;
            if (tolalMilliseconds - numberOfParts * partDurationMilliseconds > 0)
                numberOfParts++;
            long startPos = 0;
            const int minDigits = 4;
            var numberOfDigits = numberOfParts.ToString().Length < minDigits ? minDigits : numberOfParts.ToString().Length;

            string formatString = Enumerable.Repeat("0", numberOfDigits).Aggregate((s, d) => s + d);

            //for PCM AverageBytesPerSecond = sampleRate * (channels * (bits / 8));
            int bytesPerMilliseconds = waveFileReader.WaveFormat.AverageBytesPerSecond / 1000;
            for (int i = 0; i < numberOfParts; i++)
            {
                long endPos = startPos + (partDurationMilliseconds * bytesPerMilliseconds);

                // part name would be something like this: output-0001.wav, output-0002.wav, output-0003.wav, etc
                var partFileName = $"{wavFileOut}-{(i + 1).ToString(formatString)}.wav";
                using (var waveFileWriter = new WaveFileWriter(partFileName, waveFileReader.WaveFormat))
                {
                    WriteWavChunk(waveFileReader, waveFileWriter, startPos, endPos);
                }
                startPos = endPos;
            }
        }

        private static long GetWaveFileDurationInMilliseconds(WaveFileReader waveFileReader)
        {
            return waveFileReader.SampleCount / waveFileReader.WaveFormat.SampleRate * 1000;
        }

        private static void WriteWavChunk(WaveFileReader reader, WaveFileWriter writer, long startPos, long endPos)
        {
            reader.Position = startPos;

            // make sure that buffer is sized to a multiple of our WaveFormat.BlockAlign.
            // WaveFormat.BlockAlign = channels * (bits / 8), so for 16 bit stereo wav it will be 4096 bytes
            var buffer = new byte[reader.BlockAlign * 1024];
            while (reader.Position < endPos)
            {
                long bytesRequired = endPos - reader.Position;
                if (bytesRequired > 0)
                {
                    int bytesToRead = (int)Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.Write(buffer, 0, bytesToRead);
                    }
                }
            }
        }
    }
}
