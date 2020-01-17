using HandlebarsDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace Galilu
{
    class Program
    {
        static int Main(string[] args)
        {
            args = new string[] { ".\\test.bc" };
            //args = new string[] { ".\\tests.bc" };
            //args = new string[] { ".\\Galilu.exe" };

            //Validation
            int errCode = 0;
            errCode = Validation(args);
            if (errCode > 0) return errCode;

            //Get Files
            string path = Path.GetDirectoryName(args[0]);
            string fileName = Path.GetFileName(args[0]);
            string newFile = $"{path}\\{fileName}.cs";
            Console.WriteLine($"New file: {newFile}");


            //Get Data to compile
            string jsonString = CreateJson(args[0]);

            //Render compiled file
            string template = Render(".\\Templates\\Entity.cs.template", jsonString);

            //Write file
            var fileWriter = new System.IO.StreamWriter(newFile);
            fileWriter.WriteLine(template);
            fileWriter.Dispose();

            return errCode;
        }

        static int Validation(string[] args)
        {
            int errCode = 0;
            if (args.Length > 1 || args.Length == 0)
            {
                Console.WriteLine("Err: 1, Invalid number of arguments");
                errCode = 1;
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine("Err: 2, File doesn't exist");
                errCode = 2;
            }
            else if (Path.GetExtension(args[0]) != ".bc")
            {
                Console.WriteLine("Err 3, File type not compatible");
                errCode = 3;
            }

            return errCode;
        }

        static string CreateJson(string FileName)
        {
            //List<dynamic> items = JsonConvert.DeserializeObject<List<dynamic>>(contract);
            string contract;
            contract = GetFile(FileName);

            string[] contractLines = contract.Split("\r");
            StringBuilder jsonString = new StringBuilder("{");

            foreach (var item in contractLines)
            {
                var itemC = Clean(item).Split(':');
                if (itemC.Length == 2)
                {
                    itemC[0] = Clean(itemC[0]);
                    itemC[1] = Clean(itemC[1]);

                    if (itemC[0] == "End")
                    {
                        jsonString.Append("],");
                    }
                    else if (itemC[1] == "")
                    {
                        jsonString.Append($"\"{itemC[0]}\":[");
                    }
                    else if (itemC[0] == "")
                    {
                        var fields = itemC[1].Split(',').Select(x => x.Split(' ')).ToArray();
                        jsonString.Append("{");
                        foreach (var item2 in fields)
                        {
                            jsonString.Append($"\"Type\":\"{item2[0]}\",\"VarName\":\"{item2[1]}\"");
                        }
                        jsonString.Append("},");
                    }
                    else
                    {
                        jsonString.Append($"\"{itemC[0]}\":\"{itemC[1]}\",");
                    }
                }
            }
            jsonString.Append("}");
            return jsonString.ToString();
        }

        static string GetFile(string FileName)
        {
            string file;
            using (StreamReader r = new StreamReader(FileName))
            {
                file = r.ReadToEnd();
            }
            return file;
        }

        static string Render(string templateFilePath, string data)
        {
            string input;
            using (StreamReader streamReader = new StreamReader(templateFilePath, Encoding.UTF8))
            {
                input = streamReader.ReadToEnd();
            }

            ExpandoObject source = JsonConvert.DeserializeObject<ExpandoObject>(data);
            var template = Handlebars.Compile(input);
            var result = template(source);

            return result;
        }

        static string Clean(string str)
        {
            str = str ?? "";
            str = str.Replace("\r", "")
                     .Replace("\n", "")
                     .Replace("\t", "")
                     .Replace("\b", "")
                     .Replace("\a", "")
                     .Replace("\f", "")
                     .Replace("\v", "")
                     .Trim();
            return str;
        }
    }
}
