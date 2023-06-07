using System.Text.Json;

namespace HDDCooling
{
    internal class Settings
    {
        public string HDDPath { get; set; }
        public string PWMPath { get; set; }
        public int MinSpeed { get; set; }
        public int MaxSpeed { get; set; }
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
                Console.WriteLine(ex.ToString());
                settings = null;
            }

            return settings;
        }
    }
}