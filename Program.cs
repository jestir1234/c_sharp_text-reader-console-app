using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TeleprompterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // var lines = ReadFrom("sampleQuotes.txt");
            // foreach (var line in lines) 
            // {
            //     Console.WriteLine(line);
            //     if (!string.IsNullOrWhiteSpace(line))
            //     {
            //         var pause = Task.Delay(200);
            //         pause.Wait();
            //     }
            // }
            RunTeleprompter().Wait();
        }
        static IEnumerable<string> ReadFrom(string file)
        {
            string line;
            /*
            'using' statement manages resource cleanup. 
            variable 'reader' implements IDisposable interface that defines
            a single method Dispose, called when the resource should be release
            Compiler generates the call when execution reaches the closing brace of the 'using' statement
            */
            using (var reader = File.OpenText(file))
            {
            /* 
             'var' defines implicitly typed local variable
            where the type is determined at compile-time. Here, the return value
            from OpenText(String), which is a StreamReader object 
            */ 
                while ((line = reader.ReadLine()) != null)
                {
                    var words = line.Split(' ');
                    var lineLength = 0;
                    foreach (var word in words) {
                        yield return word + " ";
                    }
                    lineLength += words.Length + 1;
                    if (lineLength > 70)
                    {
                        yield return Environment.NewLine;
                        lineLength = 0;
                    }
                }
            }
        }
        
        private static async Task ShowTeleprompter(TeleprompterConfig config)
        {
            var words = ReadFrom("sampleQuotes.txt");
            foreach (var word in words)
            {
                Console.WriteLine(word);
                if (!string.IsNullOrWhiteSpace(word))
                {
                    await Task.Delay(config.DelayInMilliseconds);
                }
            }
            config.SetDone();
        }

        private static async Task GetInput(TeleprompterConfig config)
        {
            Action work = () => 
            {
                do {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == '>')
                    {
                        config.UpdateDelay(-10);
                    }
                    else if (key.KeyChar == '<')
                    {
                        config.UpdateDelay(10);
                    }
                    else if (key.KeyChar == 'X' || key.KeyChar == 'x') 
                    {
                        config.SetDone();
                    }
                } while(!config.Done);
            };
            await Task.Run(work);
        }

        private static async Task RunTeleprompter()
        {
            var config = new TeleprompterConfig();
            var displayTask = ShowTeleprompter(config);

            var speedTask = GetInput(config);
            await Task.WhenAny(displayTask, speedTask);
        }
    }
}
