using System;
using System.IO;
using System.Collections.Generic;

namespace NewestFilePicker
{
    class Program
    {
        struct LastWriteTimeInfo
        {
            public string FilePath;
            public DateTime Time;
        }
        static void Main(string[] args)
        {
            var list = new Dictionary<string, LastWriteTimeInfo>();
            foreach (var directory in Directory.GetDirectories("test"))
            {
                foreach (var filePath in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
                {
                    var time = File.GetLastWriteTime(filePath);
                    var relativePath = Path.GetRelativePath(directory, filePath);
                    if (list.TryGetValue(relativePath, out var info))
                    {
                        if (time <= info.Time)
                        {
                            continue;
                        }
                    }
                    list[relativePath] = new LastWriteTimeInfo { FilePath = filePath, Time = time };
                }
            }

            const string OUT_FOLDER_NAME = "newest";
            Directory.CreateDirectory(OUT_FOLDER_NAME);
            foreach (var info in list)
            {
                try
                {
                    var destPath = $"{OUT_FOLDER_NAME}/{info.Key}";
                    Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                    File.Copy(info.Value.FilePath, destPath);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"error : {info.Key} : {e.Message}");
                }
            }
        }
    }
}
