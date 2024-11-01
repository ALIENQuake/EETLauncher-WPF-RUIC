//Copyright © alienquake@hotmail.com
using System;

namespace EETLauncherWPF {
    public static class EETLauncherExtensionMethod {
        // Why this is not build-in method of the .NET?
        public static bool ContainsIgnoreCase(string source, string toCheck) {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
