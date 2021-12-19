using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;

namespace Hangman
{
    /// <summary>
    /// This class describes the Hangman game.
    /// </summary>
    public class Game
    {
        private Random _random;
        private string[] _words;
        private string _secretWord;
        private StringBuilder _incorrectLetters;
        private char[] _correctLetters;

        /// <summary>
        /// Initializes the game with a random number generator.
        /// </summary>
        public Game()
        {
            _random = new Random((int)DateTime.UtcNow.Ticks);
        }

        /// <summary>
        /// A property that stores the number of guesses the user has left.
        /// </summary>
        public int GuessesLeft { get; private set; }

        /// <summary>
        /// Starts a new game and makes the necessary initializations.
        /// </summary>
        /// <param name="path">Path to text file containing words.</param>
        public async Task StartNewGameAsync(string path)
        {
            GuessesLeft = 10;
            await ReadWordsFromFileAsync(path);
            _secretWord = _words[_random.Next(0, _words.Length)].ToUpper();
            _correctLetters = new String('_', _secretWord.Length).ToCharArray();
            _incorrectLetters = new StringBuilder();
        }

        /// <summary>
        /// Reads an array of possible secret words to use from a text file. If it should fail
        /// a limited collection of hardcoded words will be used instead.
        /// </summary>
        /// <param name="path">Path to text file.</param>
        private async Task ReadWordsFromFileAsync(string path)
        {
            string fileContents;

            try
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    fileContents = await streamReader.ReadToEndAsync();
                }

                _words = fileContents.Split(',');
            }
            catch (Exception)
            {
                _words = new string[] { "morning", "bell", "tempt", "bag", "quantity" };
            }
        }

        /// <summary>
        /// Returns the correct letters in a string where they are separated by spaces.
        /// </summary>
        /// <returns>The string of correct letters.</returns>
        public string GetCorrectLetters()
        {
            return String.Join(" ", _correctLetters);
        }

        /// <summary>
        /// Returns the incorrect letters in a string where they are separated by spaces.
        /// </summary>
        /// <returns>The string of incorrect letters.</returns>
        public string GetIncorrectLetters()
        {
            return String.Join(" ", _incorrectLetters.ToString().ToCharArray());
        }

        /// <summary>
        /// Takes a guessed letter from the user, checks its validity and takes the appropriate 
        /// actions for the game based on that.
        /// </summary>
        /// <param name="guessedLetter">The guessed letter.</param>
        public void GuessLetter(char guessedLetter)
        {
            if (Char.IsLetter(guessedLetter) && GuessesLeft > 0)
            {
                guessedLetter = Char.ToUpper(guessedLetter);

                if (!(_incorrectLetters.ToString().Contains(guessedLetter) || _correctLetters.Contains(guessedLetter) ))
                {
                    GuessesLeft--;

                    if (_secretWord.Contains(guessedLetter))
                    {
                        char[] secretWord = _secretWord.ToCharArray();

                        for (int i = 0; i < secretWord.Length; i++)
                        {
                            if (secretWord[i] == guessedLetter)
                            {
                                _correctLetters[i] = guessedLetter;
                            }
                        }
                    }
                    else
                    {
                        _incorrectLetters.Append(Char.ToUpper(guessedLetter));
                    }
                }
            }
        }

        /// <summary>
        /// Takes a guessed word from the user, checks its validity and takes the appropriate 
        /// actions for the game based on that.
        /// </summary>
        /// <param name="guessedWord">The guessed word.</param>
        public void GuessWord(string guessedWord)
        {
            if (Regex.IsMatch(guessedWord, @"^[a-zA-Z]+$") && GuessesLeft > 0)
            {
                guessedWord = guessedWord.ToUpper();

                GuessesLeft--;

                if (_secretWord.Equals(guessedWord))
                {
                    _correctLetters = guessedWord.ToCharArray();
                }
            }
        }

        /// <summary>
        /// Retrieves the game status which can be either active, won or lost.
        /// </summary>
        /// <returns>The game status.</returns>
        public GameStatus GetGameStatus()
        {
            GameStatus statusToReturn = GameStatus.Active; ;

            if (_secretWord.Equals(new String(_correctLetters)))
            {
                statusToReturn = GameStatus.Won;
            }
            else if (GuessesLeft == 0)
            {
                statusToReturn = GameStatus.Lost;
            }

            return statusToReturn;
        }
    }
}
