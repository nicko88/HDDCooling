namespace HDDCooling
{
    public class Log
    {
        private static readonly string _logPath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "eventlog.txt");

        public static void LogMsg(string msg)
        {
            string timestamp = $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss tt}]: ";

            try
            {
                File.AppendAllText(_logPath, $"{timestamp}{msg}{Environment.NewLine}");
            }
            catch { }
        }
    }
}