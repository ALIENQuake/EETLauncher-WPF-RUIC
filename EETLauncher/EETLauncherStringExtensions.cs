using System;

namespace EETLauncherWPF {
    public static class EETLauncherStringExtensions {
        // not included as .NET default ?
        public static bool ContainsIgnoreCase( string source, string toCheck ) {
            return source?.IndexOf( toCheck, StringComparison.OrdinalIgnoreCase ) >= 0;
        }
    }
}
