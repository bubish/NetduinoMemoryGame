using System;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetduinoMemoryGame
{
    public class Program
    {
        public static void Main()
        {
            var redControl =
                new ControlPair(
                    new InterruptPort(Pins.GPIO_PIN_D7, true, Port.ResistorMode.PullUp,
                                      Port.InterruptMode.InterruptEdgeBoth),
                    new OutputPort(Pins.GPIO_PIN_D13, false));
            var greenControl = 
                new ControlPair(
                    new InterruptPort(Pins.GPIO_PIN_D3, true, Port.ResistorMode.PullUp,
                                      Port.InterruptMode.InterruptEdgeBoth),
                    new OutputPort(Pins.GPIO_PIN_D1, false));

            var controlPairs = new[] {redControl, greenControl};

            var noiseMaker = new NoiseMaker(PWMChannels.PWM_PIN_D9);

            try
            {
                var game = new Game(controlPairs, noiseMaker);

                while (game.IsRunning)
                {
                }

                controlPairs.ClearLeds();

                if (game.PlayerWon)
                {
                    greenControl.ShowLed();
                    noiseMaker.PlayWinningSound(250);
                }
                else
                {
                    redControl.ShowLed();
                    noiseMaker.PlaySadSound();
                }
            }
            catch (Exception)
            {
                new OutputPort(Pins.ONBOARD_LED, true).Write(true);
            }
        }

    }
}
