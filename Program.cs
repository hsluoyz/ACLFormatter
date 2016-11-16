using System;
using System.IO;

namespace ACLFormatter
{
    class Program
    {
        static string ReadFile(string sFilePath)
        {
            string str = File.ReadAllText(sFilePath);
            return str;
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: ACLFormatter <INPUT_FILE>");
            }
            else if (args.Length == 1)
            {
                if (args[0] == "-h")
                {
                    Console.WriteLine("Usage: ACLFormatter <INPUT_FILE>");
                }
                else
                {
                    if (!File.Exists(args[0]))
                    {
                        Console.WriteLine("Error: File not found: " + args[0]);
                    }
                    else
                    {
                        Console.WriteLine(ReadFile(args[0]));
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Too many parameters, please enter \"ACLFormatter -h\" to see the usage.");
            }
        }
    }
}
