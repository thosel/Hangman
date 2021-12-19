using System;
using System.Threading.Tasks;
using static System.Console;

namespace Hangman
{
    /// <summary>
    /// This class describes the controller of the Hangman game.
    /// </summary>
    public class Controller
    {
        private View _view;
        private Game _game;
        private bool _isGameRunning;

        /// <summary>
        /// Initializes the controller with a view instance and a game instance.
        /// </summary>
        public Controller()
        {
            _view = new View();
            _game = new Game();
        }

        /// <summary>
        /// Runs the Hangman game controller.
        /// </summary>
        public async Task RunAsync()
        {
            await _game.StartNewGameAsync("Words.txt");
            _isGameRunning = true;

            do
            {
                switch (_game.GetGameStatus())
                {
                    case GameStatus.Won:
                        await PerformFinishedGameActivitiesAsync("You won!");
                        break;
                    case GameStatus.Lost:
                        await PerformFinishedGameActivitiesAsync("You lost!");
                        break;
                    case GameStatus.Active:
                        PerformActiveGameActivities();
                        break;
                    default:
                        break;
                }

            } while (_isGameRunning);
        }

        /// <summary>
        /// Calls the method to show the view game menu.
        /// </summary>
        /// <param name="message">The message to be shown in the game menu.</param>
        private void CallShowGameMenu(string message)
        {
            _view.ShowGameMenu(
                    _game.GetCorrectLetters(),
                    _game.GetIncorrectLetters(),
                    _game.GuessesLeft,
                    message);
        }

        /// <summary>
        /// Performs the activities to be performed when the game is active.
        /// </summary>
        private void PerformActiveGameActivities()
        {
            CallShowGameMenu("Press 1 to guess a letter, 2 to guess the word or escape to exit the game!");

            switch (ReadKey().KeyChar)
            {
                case '1':
                    CallShowGameMenu("Press any letter to make a guess!");
                    _game.GuessLetter(ReadKey().KeyChar);
                    break;
                case '2':
                    CallShowGameMenu("Enter a word and press enter!");
                    _game.GuessWord(ReadLine());
                    break;
                case (char)ConsoleKey.Escape:
                    _isGameRunning = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Performs the activities to be performed when the game is finished.
        /// </summary>
        private async Task PerformFinishedGameActivitiesAsync(string message)
        {
            CallShowGameMenu($"{message} Press 1 to start a new game or escape to exit the game!");

            switch (ReadKey().KeyChar)
            {
                case '1':
                    await _game.StartNewGameAsync("Words.txt");
                    break;
                case (char)ConsoleKey.Escape:
                    _isGameRunning = false;
                    break;
                default:
                    break;
            }
        }
    }
}
