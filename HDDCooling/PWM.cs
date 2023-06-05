namespace HDDCooling
{
    internal class PWM
    {
        private readonly string _pwmFile;
        private readonly int _minSpeed;
        private readonly int _maxSpeed;
        private int _speed;

        public int Speed
        {
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

        public PWM(string pwmFile, int minSpeed, int maxSpeed)
        {
            _pwmFile = pwmFile;
            _minSpeed = minSpeed;
            _maxSpeed = maxSpeed;

            Speed = minSpeed;
        }

        public void AdjustSpeed(int adjustBy)
        {
            int newSpeed = _speed + adjustBy;

            if (newSpeed > _maxSpeed)
            {
                newSpeed = _maxSpeed;
            }
            else if (newSpeed < _minSpeed)
            {
                newSpeed = _minSpeed;
            }

            Speed = newSpeed;
        }

        private void SetPWMSpeed()
        {
            double pwmSpeed = Math.Round(_speed / 100d * 255d);
            try
            {
                File.WriteAllText(_pwmFile, pwmSpeed.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to set PWM...");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}