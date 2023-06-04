namespace Montageplan.Classes
{
        public static class OSVersionChecker
        {
            public static bool IsWindows7()
            {
                string os = OSInfo.GetOSName();
                return os == "Windows 7";
            }

            public static bool IsWindows8()
            {
                string os = OSInfo.GetOSName();
                return os == "Windows 8";
            }
    }
}
