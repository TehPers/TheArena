﻿using Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace TheArena
{
    public static class CSharp
    {
        /// <summary>
        /// Installs dotnet on Ubuntu needed to run the C# Joueur Client
        /// Install for Debian is different, so is not used in current branch
        /// </summary>
        /// <returns></returns>
        public static bool InstallCSharp()
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (isWindows)
            {
                Log.TraceMessage(Log.Nav.NavIn, "Windows Process.", Log.LogType.Info);
                Log.TraceMessage(Log.Nav.NavIn, "Starting Background Process...", Log.LogType.Info);

            }
            else if (isLinux)
            {
                Log.TraceMessage(Log.Nav.NavIn, "Linux Process.", Log.LogType.Info);
                using (Process process = new Process())
                {
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardError = false;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = false;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = Environment.GetEnvironmentVariable("SHELL");
                    Log.TraceMessage(Log.Nav.NavIn, "Grabbing Shell Process...", Log.LogType.Info);
                    if (process.Start())
                    {
                        Log.TraceMessage(Log.Nav.NavIn, "Installing Dotnet...", Log.LogType.Info);
                        process.StandardInput.WriteLine("wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb");
                        process.StandardInput.WriteLine("sudo dpkg -i packages-microsoft-prod.deb");
                        process.StandardInput.WriteLine("sudo apt-get update");
                        process.StandardInput.WriteLine("sudo apt-get install dotnet-sdk-2.1");
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Given the file path, compile the AI and run it using C# -- run until the results file shows a win, loss, or error
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string BuildAndRun(string file, string DA_GAME)
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (isWindows)
            {
                Log.TraceMessage(Log.Nav.NavIn, "Is Windows...", Log.LogType.Info);
                Log.TraceMessage(Log.Nav.NavIn, "Starting Background Process...", Log.LogType.Info);
                //Start commandline in the background
                using (Process cmdProcess = new Process())
                {
                    cmdProcess.StartInfo.FileName = "cmd.exe";
                    cmdProcess.StartInfo.UseShellExecute = false;
                    cmdProcess.StartInfo.CreateNoWindow = true;
                    cmdProcess.StartInfo.RedirectStandardOutput = true;
                    cmdProcess.StartInfo.RedirectStandardInput = true;
                    cmdProcess.StartInfo.RedirectStandardError = true;
                    cmdProcess.Start();

                    Log.TraceMessage(Log.Nav.NavIn, "Printing Microsoft Information...", Log.LogType.Info);
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine(cmdProcess.StandardOutput.ReadLine());
                    }

                    Log.TraceMessage(Log.Nav.NavIn, "Building file...", Log.LogType.Info);
                    cmdProcess.StandardInput.AutoFlush = true;
                    cmdProcess.StandardInput.WriteLine("csc " + file);


                    //Shows command in use
                    Console.WriteLine(cmdProcess.StandardOutput.ReadLine());

                    string result = cmdProcess.StandardOutput.ReadLine();

                    while (result.Length > 0 && !result.ToUpper().Contains("WIN") && !result.ToUpper().Contains("LOSE"))
                    {
                        Console.WriteLine(result);
                        result = cmdProcess.StandardOutput.ReadLine();
                    }
                    if (result.ToUpper().Contains("WIN"))
                    {
                        return "win";
                    }
                    string err = cmdProcess.StandardError.ReadLine();
                    err += cmdProcess.StandardError.ReadLine();
                    Console.WriteLine(err);
                }
            }
            else if (isLinux)
            {
                Log.TraceMessage(Log.Nav.NavIn, "Is Linux.", Log.LogType.Info);
                using (Process process = new Process())
                {
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardError = false;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = false;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = Environment.GetEnvironmentVariable("SHELL");
                    Log.TraceMessage(Log.Nav.NavIn, "Grabbing Shell Process...", Log.LogType.Info);
                    if (process.Start())
                    {
                        process.StandardInput.WriteLine("cd " + file.Substring(0, file.LastIndexOf('/')));

                        if (File.Exists(file.Substring(0, file.LastIndexOf('/') + 1) + "testRun"))
                        {
                            File.Delete(file.Substring(0, file.LastIndexOf('/') + 1) + "testRun");
                        }
                        using (StreamWriter sw = new StreamWriter(file.Substring(0, file.LastIndexOf('/') + 1) + "testRun"))
                        {
                            sw.AutoFlush = true;
                            sw.WriteLine("#!/bin/bash");
                            sw.WriteLine("if [ -z \"$1\" ]");
                            sw.WriteLine("  then");
                            sw.WriteLine("    echo \"No argument(s) supplied. Please specify game session you want to join or make.\"");
                            sw.WriteLine("  else");
                            sw.WriteLine("    ./run "+DA_GAME+" -s 127.0.0.1 -r \"$@\"");
                            sw.WriteLine("fi");
                        }
                        Log.TraceMessage(Log.Nav.NavIn, "Rewrote script-- running", Log.LogType.Info);
                        process.StandardInput.WriteLine("sudo chmod 777 testRun && sudo chmod 777 run && sudo make && ./testRun seth >>results.txt 2>&1");
                        string result = "";
                        do
                        {
                            Log.TraceMessage(Log.Nav.NavIn, "Results file not done waiting 1 min...", Log.LogType.Info);
                            Thread.Sleep(1000 * 60); //Wait 1 min for game to finish
                            string resultsFile = file.Substring(0, file.LastIndexOf('/') + 1) + "results.txt";
                            Log.TraceMessage(Log.Nav.NavIn, "Results File=" + resultsFile, Log.LogType.Info);
                            if (File.Exists(resultsFile))
                            {
                                Log.TraceMessage(Log.Nav.NavIn, "Results file exists reading...", Log.LogType.Info);
                                using (StreamReader sr = new StreamReader(resultsFile))
                                {
                                    result = sr.ReadToEnd() + Environment.NewLine + file;
                                }
                                Log.TraceMessage(Log.Nav.NavIn, "Results=" + result, Log.LogType.Info);
                            }
                            else
                            {
                                Log.TraceMessage(Log.Nav.NavIn, "Results file does not exist...", Log.LogType.Info);
                            }
                        } while (!result.ToLower().Contains("won") && !result.ToLower().Contains("lost") && !result.ToLower().Contains("error"));
                        Log.TraceMessage(Log.Nav.NavIn, "Results contains win or lose or com error--returning...", Log.LogType.Info);
                        return result;
                    }
                }
            }
            return "";
        }
    }
}
