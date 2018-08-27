using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Common.Lib.Utility
{
    public static class CommandLineHelper
    {
        public static void RunExeWithArguments(string filename, IList<string> arguments, out string stdOut, out string stdError)
        {
            ProcessStartInfo ps;
            if (arguments.Any())
            {
                string stringArguments = arguments.Aggregate(string.Empty, (current, argument) => current + ("\"" + argument + "\" "));

                ps = new ProcessStartInfo(filename, stringArguments)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
            }
            else
            {
                ps = new ProcessStartInfo(filename)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
            }

            var process = Process.Start(ps);
            {
                stdOut = process.StandardOutput.ReadToEnd();

                stdError = process.StandardError.ReadToEnd();

                process.WaitForExit();
            }
        }

        public static string RunExeWithArguments(string filename, IList<string> arguments)
        {
            string stdOutput;
            string stringArguments = string.Empty;
            ProcessStartInfo ps;
            if (arguments.Any())
            {
                stringArguments = arguments.Aggregate(string.Empty, (current, argument) => current + ("\"" + argument + "\" "));

                ps = new ProcessStartInfo(filename, stringArguments)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
            }
            else
            {
                ps = new ProcessStartInfo(filename)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
            }

            var process = Process.Start(ps);
            {
                stdOutput = process.StandardOutput.ReadToEnd();

                string stdError = process.StandardError.ReadToEnd();
                if (stdError.Length > 0)
                    throw new Exception(Format(filename, stringArguments) + " finished with exit code = " + process.ExitCode + ": " + stdOutput + Environment.NewLine + stdError);

                process.WaitForExit();
            }

            return stdOutput;
        }

        public static string RunExternalExe(string filename, string arguments = null)
        {
            var process = new Process();

            process.StartInfo.FileName = filename;
            if (!string.IsNullOrEmpty(arguments))
            {
                process.StartInfo.Arguments = arguments;
            }

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            var stdOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) => stdOutput.Append(args.Data);

            string stdError = null;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                stdError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception("OS error while executing " + Format(filename, arguments) + ": " + e.Message, e);
            }

            if (process.ExitCode == 0)
            {
                return stdOutput.ToString();
            }
            else
            {
                var message = new StringBuilder();

                if (!string.IsNullOrEmpty(stdError))
                {
                    message.AppendLine(stdError);
                }

                if (stdOutput.Length != 0)
                {
                    message.AppendLine("Std output:");
                    message.AppendLine(stdOutput.ToString());
                }

                throw new Exception(Format(filename, arguments) + " finished with exit code = " + process.ExitCode + ": " + message);
            }
        }

        private static string Format(string filename, string arguments)
        {
            return "'" + filename +
                ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
                "'";
        }
    }
}
