using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace NetduinoMemoryGame
{
    public class ControlPair
    {
        private const int DefaultDisplayTime = 750;

        private readonly InterruptPort _button;
        private readonly OutputPort _led;

        public ControlPair(InterruptPort button, OutputPort led)
        {
            _button = button;
            _led = led;
        }

        public InterruptPort Button { get { return _button; } }
        public OutputPort Led { get { return _led; } }
        public int ButtonPin { get { return (int) _button.Id; } }

        public void ShowLed()
        {
            Led.Write(true);
        }

        public void ClearLed()
        {
            Led.Write(false);
        }

        public void FlashLed(int displayTime = DefaultDisplayTime)
        {
            ShowLed();
            Thread.Sleep(displayTime);
            ClearLed();
        }
    }

    public static class ControlPairCollectionExtensions
    {
        public static ControlPair Get(this ControlPair[] controls, int buttonPin)
        {
            foreach (var controlPair in controls)
            {
                if (controlPair.ButtonPin == buttonPin) return controlPair;
            }
            throw new ArgumentOutOfRangeException("pin", "The control pair was not found.");
        }

        public static void ClearLeds(this ControlPair[] controls)
        {
            foreach (var controlPair in controls)
            {
                controlPair.ClearLed();
            }
        }
    }
}