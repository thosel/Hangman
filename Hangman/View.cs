using static System.Console;

namespace Hangman
{
    /// <summary>
    /// This class describes the view of the Hangman game.
    /// </summary>
    public class View
    {
        /// <summary>
        /// Displays the game menu in the console.
        /// </summary>
        /// <param name="correctLetters">The correct letters guessed so far.</param>
        /// <param name="incorrectLetters">The incorrect letters guessed so far.</param>
        /// <param name="guessesLeft">The guesses left to be used.</param>
        /// <param name="message">The message to be conveyed to the user.</param>
        public void ShowGameMenu(string correctLetters, string incorrectLetters, int guessesLeft, string message)
        {
            Clear();
            WriteLine("*** Hangman ***");
            WriteLine("--------------------");
            Write("Correct guesses:\t");
            WriteLine(correctLetters);
            Write("Incorrect letters:\t");
            WriteLine(incorrectLetters);
            Write("Guesses left:\t\t");
            WriteLine(guessesLeft);
            WriteLine("--------------------");
            WriteLine(message);
            WriteLine();
        }
    }
}
