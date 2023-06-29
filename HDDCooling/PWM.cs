namespace HDDCooling
{
    internal class PWM
    {
        private readonly Settings _settings;
        private int _speed;

        public int Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;

                if (_speed > 100)
                {
                    _speed = 100;
                }

                SetPWMSpeed();
            }
        }

        private int MaxSpeed
        {
            get
            {
                DateTime now = DateTime.Now;

                //weekend
                if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
                {
                    //between 3am and 10am
                    if (_settings.WeekendAwayHourStart <= now.Hour && now.Hour < _settings.WeekendAwayHourEnd)
                    {
                        return _settings.MaxSpeedAway;
                    }
                    else
                    {
                        return _settings.MaxSpeedHome;
                    }
                }
                //weekday
                else
                {
                    //between 2am and 5pm
                    if (_settings.WeekdayAwayHourStart <= now.Hour && now.Hour < _settings.WeekdayAwayHourEnd)
                    {
                        return _settings.MaxSpeedAway;
                    }
                    else
                    {
                        return _settings.MaxSpeedHome;
                    }
                }
            }
        }

        public PWM(Settings settings)
        {
            _settings = settings;

            Speed = settings.MinSpeed;
        }

        public void AdjustSpeed(int adjustBy)
        {
            Speed = Math.Max(_settings.MinSpeed, Math.Min(MaxSpeed, _speed + adjustBy));
        }

        private void SetPWMSpeed()
        {
            double pwmSpeed = Math.Round(_speed / 100d * 255d);
            try
            {
                File.WriteAllText(_settings.PWMPath, pwmSpeed.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to set PWM...");
                Console.WriteLine($"{ex.Message}");
                Log.LogMsg("Failed to set PWM...");
                Log.LogMsg(ex.Message);
            }
        }
    }
}