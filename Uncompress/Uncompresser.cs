using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NthsKeys.Uncompresser
{
    public delegate void FileOrDirectoryObjCallback(string objPath);
    public class Uncompresser
    {
        private static void WalkDir(string baseDir, FileOrDirectoryObjCallback directoryCallback, FileOrDirectoryObjCallback fileCallback)
        {
            foreach (string dirPath in Directory.GetDirectories(baseDir))
            {
                directoryCallback(dirPath);
            }
            foreach (string filePath in Directory.GetFiles(baseDir))
            {
                fileCallback(filePath);
            }
        }

        public string Pass { get; }
        public string FilePath { get; }
        public string Target { get; }
        private string _RealPath
        {
            get
            {
                return Path.GetFullPath(Path.GetDirectoryName(FilePath)) + "\\temp";
            }
        }
        private string getTargetPathWithCategory(string category)
        {
            string ret = Target + "\\" + category;
            if (!Directory.Exists(ret))
            {
                Directory.CreateDirectory(ret);
            }
            return ret;
        }
        public Uncompresser(string _Pass, string _FilePath, string _Target)
        {
            Pass = _Pass;
            FilePath = Path.GetFullPath(_FilePath);
            Target = Path.GetFullPath(_Target);
            if (!File.Exists(FilePath))
            {
                throw new IOException("File not exists.");
            }
            if (!Directory.Exists(Target))
            {
                throw new IOException("Target dir not exists.");
            }
        }

        public void Uncompress()
        {
            if (Directory.Exists(_RealPath))
            {
                Directory.Delete(_RealPath, true);
            }

            string argument = string.Empty;
            argument = string.Format("x -p{0} \"{1}\" -o\"{2}\"", Pass, FilePath, _RealPath);
            Process qzProcess = new Process();
            ProcessStartInfo qzStart = new ProcessStartInfo("./7z.exe", argument);
            qzStart.CreateNoWindow = false;
            qzStart.RedirectStandardOutput = true;
            qzStart.UseShellExecute = false;
            qzProcess.StartInfo = qzStart;
            qzProcess.Start();
            using (var outputReader = qzProcess.StandardOutput)
            {
                qzProcess.WaitForExit();
                string line = outputReader.ReadToEnd();
                if (line.Contains("Wrong password"))
                {
                    Directory.Delete(_RealPath, true);
                    throw new WrongPasswordException("Wrong password");
                }
            }

            // Move the files.
            WalkDir(_RealPath,
                new FileOrDirectoryObjCallback((dir) =>
                {
                    string part = Path.GetFileName(dir);
                    string category = Category.MatchCategory(dir);
                    try
                    {
                        Directory.Move(dir, getTargetPathWithCategory(category) + "\\" + part);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("已存在"))
                        {
                            return;
                            //Directory.Move(dir, getTargetPathWithCategory(category) + "\\" + part + DateTime.Now.Millisecond);
                        }
                    }
                }),
                new FileOrDirectoryObjCallback((file) =>
                {
                    string part = Path.GetFileName(file);
                    string category = Category.MatchCategory(file);
                    try
                    {
                        Directory.Move(file, getTargetPathWithCategory(category) + "\\" + part);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("已存在"))
                        {
                            Directory.Move(file, getTargetPathWithCategory(category) + "\\" + DateTime.Now.Millisecond + part);
                        }
                    }
                }));
            Directory.Delete(_RealPath, true);
        }

    }

    public class WrongPasswordException : Exception
    {
        public WrongPasswordException(string msg) : base(msg)
        {
            message = msg;
        }

        private string message;
        public override string Message
        {
            get
            {
                return message;
            }
        }
    }
}
