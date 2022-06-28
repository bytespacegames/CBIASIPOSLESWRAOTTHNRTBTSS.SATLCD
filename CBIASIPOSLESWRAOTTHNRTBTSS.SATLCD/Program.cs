using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CBIASIPOSLESWRAOTTHNRTBTSS.SATLCD
{
    class Program
    {
        static List<string> codeLines = new List<string>();
        static string filename = "code";

        static Dictionary<string, int> vars = new Dictionary<string, int>();

        static int codeLine = 1;
        static void Main(string[] args)
        {
            TextReader tr = new StreamReader(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\" + @filename + ".coballs");

            string line = tr.ReadLine();
            while (line != null)
            {
                codeLines.Add(line);
                line = tr.ReadLine();
            } 

            while (codeLine <= codeLines.Count)
            {
                string[] split = codeLines[codeLine - 1].Trim().Split(" ");
                string fullLine = codeLines[codeLine - 1].Trim();
                string lower = fullLine.ToLower();
                switch (split[0])
                {
                    case "write":
                        if (lower.Contains("write the string:")) {
                            Console.Write(fullLine.Substring(fullLine.IndexOf(":")).Remove(0, 1));
                        }
                        if (lower.Contains("write the ascii character for the variable:"))
                        {
                            Console.Write(Encoding.ASCII.GetString(new byte[] { (byte) vars[fullLine.Substring(fullLine.IndexOf(":")).Remove(0, 1)]  }));
                        }
                        break;
                    case "set":
                        if (lower.Contains("set the variable called ") && lower.Contains("to:"))
                        {
                            if (vars.ContainsKey(split[4]))
                            {
                                vars[split[4]] = int.Parse(fullLine.Substring(fullLine.IndexOf(":")).Remove(0, 1));
                            } else
                            {
                                vars.Add(split[4], int.Parse(fullLine.Substring(fullLine.IndexOf(":")).Remove(0, 1)));
                            }
                        } else if (lower.Contains("set the value of ") && lower.Contains("to the value of:"))
                        {
                            if (vars.ContainsKey(split[4]))
                            {
                                vars[split[4]] = vars[fullLine.Substring(fullLine.IndexOf(":")).Remove(0, 1)];
                            }
                            else
                            {
                                vars.Add(split[4], vars[fullLine.Substring(fullLine.IndexOf(":")).Remove(0, 1)]);
                            }
                        }
                        else if (lower.Contains("set the value of ") && lower.Contains("to the ascii value of a user input character"))
                        {
                            if (vars.ContainsKey(split[4]))
                            {
                                vars[split[4]] = (int) Console.ReadKey().KeyChar;
                            }
                            else
                            {
                                vars.Add(split[4], (int)Console.ReadKey().KeyChar);
                            }
                            Console.Write("\n");
                        }
                        else if (lower.Contains("set the title of the application to:"))
                        {
                            Console.Title = fullLine.Substring(fullLine.IndexOf(":")).Remove(0, 1);
                        }
                        break;
                    case "preform":
                        if (lower.Contains("preform operation ") && lower.Contains(" on ") && lower.Contains(" by:"))
                        {
                            string varname = split[4];
                            string type = split[2];
                            int by = int.Parse(fullLine.Substring(fullLine.IndexOf(":")).Remove(0, 1));
                            if (type.Equals("+"))
                            {
                                vars[varname] += by;
                            }
                            if (type.Equals("-"))
                            {
                                vars[varname] -= by;
                            }
                            if (type.Equals("*"))
                            {
                                vars[varname] *= by;
                            }
                            if (type.Equals("/"))
                            {
                                vars[varname] /= by;
                            }
                            if (type.Equals("%"))
                            {
                                vars[varname] %= by;
                            }
                        }
                        break;
                    case "goto":
                        if (lower.Contains("goto line of the number:"))
                        {
                            codeLine = int.Parse(fullLine.Substring(fullLine.IndexOf(":")).Remove(0, 1)) - 1;
                        }
                        break;
                    case "if:":
                        if (lower.Contains("if variable is not 0:"))
                        {
                            if (vars[fullLine.Substring(fullLine.IndexOf(":")).Remove(0, 1)] == 0)
                            {
                                codeLine += 5;
                            }
                        }
                        break;

                }
                codeLine += 1;
            }

            Console.ReadLine();
        }
    }
}
