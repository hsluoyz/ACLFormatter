using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ACLFormatter
{
    class Program
    {
        static string ReadFile(string filePath)
        {
            string res = File.ReadAllText(filePath);
            return res;
        }

        static public void WriteFile(string filePath, string text)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            sw.Write(text);
            sw.Close();
            fs.Close();
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

        static string XML2Template(string text)
        {
            string sResult;
            string pattern = "<(for|if) value=\"([^>]*)\">";
            Regex rgx = new Regex(pattern);
            sResult = rgx.Replace(text, "{% $1$2%}");

            rgx = new Regex("</(for|if)>");
            sResult = rgx.Replace(sResult, "{% end$1 %}");

            rgx = new Regex("<assign value=\"([^>]*)\">[^>]*</assign>");
            sResult = rgx.Replace(sResult, "{% assign$1%}");
            return sResult;
        }

        static string FormatXML(string text)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(text);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 2;
                xtw.IndentChar = ' ';
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
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
                        string output = XML2Template(FormatXML(Template2XML(ReadFile(args[0]))));
                        string outputFile = args[0].Replace(".xml", "_out.xml");
                        WriteFile(outputFile, output);
                        Console.WriteLine("Success, output to: " + outputFile);
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
