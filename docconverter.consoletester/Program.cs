using System;
using System.IO;
using FileConverter.Watcher;

namespace FileConverter.ConsoleTester
{
    class Program
    {
        private static string _outputDir;

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("need 1 argument: dirpath");
                return;
            }
            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("directory not found.");
                return;
            }

            _outputDir = args[0] + (args[0].EndsWith("\\") ? "" : "\\") + "output\\";
            if (!Directory.Exists(_outputDir)) { Directory.CreateDirectory(_outputDir); }
            DirectoryWatcher watcher = new DirectoryWatcher(args[0]);
            watcher.FileChanged += WatcherOnFileChanged;

            Console.WriteLine("Watching... <ctrl+c> to cancel");
            Console.ReadLine();
        }

        private static void WatcherOnFileChanged(string fileName)
        {
            string outputFileName = Path.Combine(_outputDir, Path.GetFileNameWithoutExtension(fileName) + ".png");
            Converter.Converter converter = new Converter.Converter(fileName, outputFileName);
            
            converter.Convert();
        }
    }
}
