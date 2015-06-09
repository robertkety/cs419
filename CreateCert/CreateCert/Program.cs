using System;
using System.Diagnostics;

namespace CreateCert
{
    class Program
    {
        public static string CertificateName = "crrdirectory.cer";
        public static string CertificatePath = "Certificates";
        
        static void Main(string[] args)
        {
            string command = "cd " + CertificatePath + " & " + System.IO.Directory.GetCurrentDirectory() + "\\" + CertificatePath + "\\makecert.exe -sky exchange -r -n \"CN=" + CertificateName + "\" -pe -a sha1 -len 2048 -ss My \"" + CertificateName + "\"";
            RunCommand(command);
        }

        /* Runs the Windows command: cmd.exe and arguments */
        private static void RunCommand(string Arguments, bool Verbose = false)
        {
            try
            {
                ProcessStartInfo CommandInfo = new ProcessStartInfo("cmd.exe", "/C \"" + Arguments + "\"");
#if RELEASE
                CommandInfo.WindowStyle = ProcessWindowStyle.Hidden;
#endif
                CommandInfo.UseShellExecute = false;
                CommandInfo.RedirectStandardOutput = !Verbose;

                Process Executable = new Process();
                Executable.StartInfo = CommandInfo;
                Executable.Start();
                Executable.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new Exception("cmd.exe Exception: " + ex.ToString());
            }
        }
    }
}
