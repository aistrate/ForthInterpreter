using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ForthInterpreter.IO
{
    public static class FileLoader
    {
        public static string ReadResource(string resource)
        {
            return ReadResource(Assembly.GetAssembly(typeof(FileLoader)), resource);
        }

        public static string ReadResource(Assembly assembly, string resource)
        {
            try
            {
                Stream resourceStream = assembly.GetManifestResourceStream(resource);
                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                throw new ApplicationException(string.Format("Could not read resource '{0}'.", resource));
            }
        }

        public static IEnumerable<string> GetTextLines(string text)
        {
            using (StringReader reader = new StringReader(text))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line != null)
                        yield return line;
                    else
                        break;
                }
            }
        }
    }
}
