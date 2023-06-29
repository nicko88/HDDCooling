using System.Text;
using CliWrap;

namespace HDDCooling
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log.LogMsg("PROGRAM STARTUP");

            try
            {
                Settings? settings = Settings.LoadSettings();

                if (settings is not null)
                {
                    PWM pwm = new PWM(settings);

                    PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
                    while (await timer.WaitForNextTickAsync())
                    {
                        int? temp = await GetTemp(settings.HDDPath);

                        if (temp is not null)
                        {
                            if (temp > settings.TargetTemp)
                            {
                                pwm.AdjustSpeed(5);
                            }
                            else if (temp < settings.TargetTemp)
                            {
                                pwm.AdjustSpeed(-5);
                            }

                            if (temp > settings.CriticalTemp)
                            {
                                pwm.Speed = 100;
                            }

                            Log.LogMsg($"Speed: {pwm.Speed}");
                        }
                        else
                        {
                            pwm.Speed = settings.MaxSpeedAway;
                            Console.Out.WriteLine("Could not get HDD temp...");
                            Log.LogMsg("Could not get HDD temp...");
                            Console.Out.WriteLine("Setting fan to MaxSpeed...");
                            Log.LogMsg("Setting fan to MaxSpeed...");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Could not load settings...");
                    Log.LogMsg("Could not load settings...");
                }
            }
            catch (Exception ex)
            {
                Log.LogMsg(ex.Message);
            }

            Log.LogMsg("PROGRAM EXITING...");
        }

        private static async Task<int?> GetTemp(string HDDPath)
        {
            int? iTemp = null;

            try
            {
                StringBuilder stdOutBuffer = new StringBuilder();

                await Cli.Wrap("/usr/sbin/hddtemp")
                    .WithArguments(new[] { HDDPath })
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                    .ExecuteAsync();

                string stdOut = stdOutBuffer.ToString();
                string strTemp = stdOut.Substring(stdOut.Length - 5, 2);
                iTemp = Convert.ToInt32(strTemp);
            }
            catch { }

            return iTemp;
        }
    }
}