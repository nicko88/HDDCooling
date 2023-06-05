using System.Text.Json.Serialization;
using System.Text.Json;

namespace HDDCooling
{
    internal class Settings
    {
        public string PWMPath { get; set; }
        public string HDDPath { get; set; }
        public int MinSpeed { get; set; }
        public int MaxSpeed { get; set; }
        public int TargetTemp { get; set; }
        public int CriticalTemp { get; set; }

        public static Settings? LoadSettings()
        {
            Settings? settings = new Settings();
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                options.WriteIndented = true;

                string jsonSettings = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "HDDCoolingSettings.json"));
                settings = JsonSerializer.Deserialize<Settings>(jsonSettings, options);
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