﻿using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ACLFormatter
{
    class Program
    {
        static string ReadFile(string filePath)
        {
            string res = File.ReadAllText(filePath);
            return res;
        }

        static string Template2XML(string text)
        {
            string sResult;
            string pattern = "{% (for|if)([^}]*)%}";
            Regex rgx = new Regex(pattern);
            sResult = rgx.Replace(text, "<$1 value=\"$2\">");

            rgx = new Regex("{% end((for|if)[^}]*)%}");
            sResult = rgx.Replace(sResult, "</$2>");

            rgx = new Regex("{% (assign)([^}]*)%}");
            sResult = rgx.Replace(sResult, "<$1 value=\"$2\" ></$1>");
            return sResult;
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
                        Console.WriteLine(Template2XML(ReadFile(args[0])));
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
