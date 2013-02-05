using System;
using System.Threading;

namespace NetduinoMemoryGame
{
    enum GameState
    {
        Starting,
        Ended,
        Demonstration,
        Recital
    }

    public class Game
    {
        private GameState _gameState;
        private readonly ControlPair[] _controlPairs;
        private int _round = 0;
        private int _iteration = 0;
        private readonly int[] _gameOrder = new int[10];
        private readonly NoiseMaker _noiseMaker;

        public Game(ControlPair[] controlPairs, NoiseMaker noiseMaker)
        {
            _controlPairs = controlPairs;
            _noiseMaker = noiseMaker;

            var random = new Random();

            for (var i = 0; i < _gameOrder.Length; i++)
            {
                var whichControl = random.Next(_controlPairs.Length);
                _gameOrder[i] = _controlPairs[whichControl].ButtonPin;
            }

            foreach (var controlPair in _controlPairs)
            {
                controlPair.Button.OnInterrupt += ButtonHandler;
            }

            Demonstrate();
        }

        public bool IsRunning
        {
            get { return _gameState != GameState.Ended; }
        }

        public bool PlayerWon { get; private set; }

        private void ButtonHandler(uint data1, uint data2, DateTime time)
        {
            if (_gameState != GameState.Recital) return;

            var controlPair = _controlPairs.Get((int) data1);

            if (data2 == 0)
            {
                controlPair.ShowLed();
                if(controlPair.ButtonPin != _gameOrder[_iteration]) EndGame(false);

                if (_iteration == _round)
                {
                    EndRound();
                    return;
                }

                _iteration++;
            }
            else
            {
                controlPair.ClearLed();
            }
        }

        private void Demonstrate()
        {
            if(_gameState == GameState.Ended) return;

            _gameState = GameState.Demonstration;

            _controlPairs.ClearLeds();

            Thread.Sleep(1000);

            for (var i = 0; i <= _round; i++)
            {
                var control = _controlPairs.Get(_gameOrder[i]);

                control.FlashLed();
                Thread.Sleep(250);
            }

            _gameState = GameState.Recital;
        }

        private void EndGame(bool playerWon)
        {
            PlayerWon = playerWon;
            _gameState = GameState.Ended;
        }

        private void EndRound()
        {
            _noiseMaker.PlayHappySound(250);

            if(_round == _gameOrder.Length - 1) EndGame(true);

            _round++;
            _iteration = 0;

            Demonstrate();
        }
    }
}