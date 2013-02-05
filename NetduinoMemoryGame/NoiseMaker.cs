using System.Threading;
using Microsoft.SPOT.Hardware;

namespace NetduinoMemoryGame
{
    public class NoiseMaker
    {
        private const int DefaultLength = 750;
        private readonly Cpu.PWMChannel _channel;

        public NoiseMaker(Cpu.PWMChannel channel)
        {
            _channel = channel;
        }

        public void PlayHappySound(int length = DefaultLength)
        {
            PlaySound(800, 700, length);
        }

        public void PlayWinningSound(int length = DefaultLength)
        {
            PlaySound(600, 500, length);
            PlaySound(400, 300, length);
            PlaySound(800, 700, length);
            PlaySound(600, 500, length);
            PlaySound(400, 300, length);
        }

        public void PlaySadSound(int length = DefaultLength)
        {
            PlaySound(4400, 200, length);
        }

        private void PlaySound(uint period, uint duration, int length)
        {
            var pwm = new PWM(_channel, period, duration, PWM.ScaleFactor.Microseconds, false);

            pwm.Start();
            Thread.Sleep(length);
            pwm.Stop();
        }
    }
}
