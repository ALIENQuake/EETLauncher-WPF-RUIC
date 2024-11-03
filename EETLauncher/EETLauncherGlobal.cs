//Copyright © alienquake@hotmail.com
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static EETLauncher.EETLauncherConfig;
using static EETLauncher.EETLauncherExtensionMethod;

namespace EETLauncher {
    public static class EETLauncherGlobal {
        public static bool TestEETBaldurLua() {
            return File.Exists(EETBaldurLua);
        }
        public static bool TestBG2EEDirectory() {
            return File.Exists(AppRootPath + GameCheckFilePath);
        }
        public static bool TestEETInstalled() {
            return File.Exists(AppRootPath + EETFlagFilePath);
        }
        public static bool TestEETReadme() {
            return File.Exists(AppRootPath + EETReadMeFilePath);
        }

        public static string GetGameCfgDirectory() {
            var list = new List<string>();
            var dataFile = File.ReadAllLines(AppRootPath + GameEngineFileName).ToList();
            foreach (var line in dataFile) {
                if (!ContainsIgnoreCase(line, "engine_name")) continue;
                // engine_name = "Baldur's Gate - Enhanced Edition Trilogy"
                //var det = new[] { '=' };
                //list = line.Split( det, StringSplitOptions.RemoveEmptyEntries ).ToList();
                list = line.Split(new[] { " = " }, StringSplitOptions.None).ToList();
                break;
            }
            return list[1].Replace("\"", string.Empty);
        }

        public static string GetEETCurrentGUI() {
            var test = "";
            var dataFile = File.ReadAllLines(AppRootPath + WeiDULogFileName).ToList();

            foreach (var line in dataFile) {
                if (ContainsIgnoreCase(line[0].ToString(), "/")) continue;
                test = ContainsIgnoreCase(line, EETGUIModFileName) ? "SoD" : "BG2";
            }
            switch (test) {
                case "BG2":
                    return "BG2";
                case "SoD":
                    return "SoD";
                default:
                    return "Incorrect value: " + test;
            }
        }
        public static string GetEETChangeToGUI(string Current) {
            switch (Current) {
                case "BG2":
                    return "SoD";
                case "SoD":
                    return "BG2";
                default:
                    //Cannot evaluate aviable GUI change list from: //
                    return EETGUIUnknown + Current;
            }
        }

        public static ProcessStartInfo SetProcessStartInfo(string FileName, dynamic ArgumentList) {
            return new ProcessStartInfo {
                WorkingDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) ?? throw new InvalidOperationException(),
                Arguments = (string)string.Join(" ", ArgumentList),
                FileName = FileName,
                CreateNoWindow = true,
                UseShellExecute = false
            };
        }
        public static string FindEETLanguageID(string filePath) {

            List<string> data = new List<string>(File.ReadAllLines(filePath).ToList());
            foreach (string line in data) {
                if (ContainsIgnoreCase(line, EETModFileName)) {
                    int hashIndex = line.IndexOf('#');
                    if (hashIndex != -1 && hashIndex < line.Length - 1) {
                        var stringAfterHash = line.Substring(hashIndex + 1, 2);
                        return stringAfterHash;
                    }
                }
            }
            return null;
        }
        public static ProcessStartInfo SetEETGUI(string GUI) {
            var procArgList = new List<string>();
            switch (GUI) {
                case "BG2": {
                        procArgList.Add("--uninstall");
                        break;
                    }
                case "SoD": {
                        procArgList.Add("--force-install" + " " + EETGUIComponentNumber);
                        break;
                    }
                default: {
                        throw new NotImplementedException();
                    }
            }

            var languageNumber = FindEETLanguageID(AppRootPath + WeiDULogFileName);

            procArgList.AddRange(new[] { "--noautoupdate", "--no-exit-pause", $"--language {languageNumber}" });

            var StartInfo = SetProcessStartInfo(EETGUIExeFileName, procArgList);
            return StartInfo;
        }
    }
}
