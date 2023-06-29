using System.Text.Json;

namespace HDDCooling
{
    internal class Settings
    {
        public string HDDPath { get; set; }
        public string PWMPath { get; set; }
        public int MinSpeed { get; set; }
        public int MaxSpeedHome { get; set; }
        public int MaxSpeedAway { get; set; }
        public int WeekdayAwayHourStart { get; set; }
        public int WeekdayAwayHourEnd { get; set; }
        public int WeekendAwayHourStart { get; set; }
        public int WeekendAwayHourEnd { get; set; }
        public int TargetTemp { get; set; }
        public int CriticalTemp { get; set; }

        public static Settings? LoadSettings()
        {
            Settings? settings;
            try
            {
                string settingsJSON = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "HDDCoolingSettings.json"));
                settings = JsonSerializer.Deserialize<Settings>(settingsJSON);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                Log.LogMsg(ex.Message);

                settings = null;
            }

            return settings;
        }
    }
}