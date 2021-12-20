using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Hangman.Tests
{
    /// <summary>
    /// The test class for the game class in the Hangman game.
    /// </summary>
    public class GameShould : IClassFixture<GameFixture>, IAsyncLifetime
    {
        private readonly Game _sut;
        private readonly ITestOutputHelper _output;
        private ITest _test;

        /// <summary>
        /// Initializes a game instance to be the system under test plus the 
        /// current test representation.
        /// </summary>
        /// <param name="gameFixture"></param>
        public GameShould(GameFixture gameFixture, ITestOutputHelper output)
        {
            _sut = gameFixture.GameInstance;
            _output = output;

            var type = output.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            _test = (ITest)testMember.GetValue(output);
        }

        /// <summary>
        /// Starts the game with a valid text file as input apart from the method
        /// InitializeCorrectLettersAccuratelyOnInvalidFileInputAsync which demands
        /// a invalid text file input.
        /// </summary>
        public async Task InitializeAsync()
        {
            if (_test.DisplayName !=
                "Hangman.Tests.GameShould.InitializeCorrectLettersAccuratelyOnInvalidFileInputAsync()")
            {
                await _sut.StartNewGameAsync("TestWords.txt");
            }
        }

        /// <summary>
        /// The purpose of this method is to free unmanaged resources but in this case the method
        /// is only here because the interface IAsyncLifetime requires it.
        /// </summary>
        /// <returns>The successfully completed task.</returns>
        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        #region Initialization

        /// <summary>
        /// Tests that there is 10 guesses left at game initialization.
        /// </summary>
        [Fact]
        [Trait("Category", "Initialization")]
        public void InitializeGuessesLeftAccurately()
        {
            Assert.Equal(10, _sut.GuessesLeft);
        }

        /// <summary>
        /// Tests that the correct letters are correct at game initialization.
        /// </summary>
        [Fact]
        [Trait("Category", "Initialization")]
        public void InitializeCorrectLettersAccurately()
        {
            Assert.Matches(@"^([_])(?!\1)([\s])(?:\1\2)*\1$", _sut.GetCorrectLetters());
        }

        /// <summary>
        /// Tests that the correct letters are correct at game initialization
        /// despite invalid input.
        /// </summary>
        [Fact]
        [Trait("Category", "Initialization")]
        public async Task InitializeCorrectLettersAccuratelyOnInvalidFileInputAsync()
        {
            await _sut.StartNewGameAsync("notavalidfile.txt");

            Assert.Matches(@"^([_])(?!\1)([\s])(?:\1\2)*\1$", _sut.GetCorrectLetters());
        }

        /// <summary>
        /// Tests that the incorrect letters are correct at game initialization.
        /// </summary>
        [Fact]
        [Trait("Category", "Initialization")]
        public void InitializeIncorrectLettersAccurately()
        {
            Assert.Equal("", _sut.GetIncorrectLetters());
        }

        /// <summary>
        /// Tests that the game status are correct at game initialization.
        /// </summary>
        [Fact]
        [Trait("Category", "Initialization")]
        public void InitializeGameStatusAccurately()
        {
            Assert.Equal(GameStatus.Active, _sut.GetGameStatus());
        }

        #endregion

        #region GamePlay

        /// <summary>
        /// Tests that the correct letters are correct at game play guessing letters.
        /// </summary>
        /// <param name="guessedLetters">The guessed letters.</param>
        /// <param name="expectedCorrectLetters">The expected outcome of the correct letters.</param>
        [Theory]
        [Trait("Category", "GamePlay")]
        [CorrectLettersData]
        public void OutputCorrectLettersAccuratelyOnGuessedLetters(string guessedLetters, string expectedCorrectLetters)
        {
            foreach (var letter in guessedLetters)
            {
                _sut.GuessLetter(letter);
            }

            Assert.Equal(expectedCorrectLetters, _sut.GetCorrectLetters());
        }

        /// <summary>
        /// Tests that the correct letters are correct at game play guessing the correct word.
        /// </summary>
        /// <param name="guessedWord">The correct guessed word.</param>
        /// <param name="expectedCorrectLetters">The expected outcome of the correct letters.</param>
        [Theory]
        [Trait("Category", "GamePlay")]
        [CorrectLettersData]
        public void OutputCorrectLettersAccuratelyOnGuessedWord(string guessedWord, string expectedCorrectLetters)
        {
            _sut.GuessWord(guessedWord);

            Assert.Equal(expectedCorrectLetters, _sut.GetCorrectLetters());
        }

        /// <summary>
        /// Tests that the incorrect letters are correct at game play guessing letters.
        /// </summary>
        /// <param name="guessedLetters">The guessed letters.</param>
        /// <param name="expectedIncorrectLetters">The expected outcome of the incorrect letters.</param>
        [Theory]
        [Trait("Category", "GamePlay")]
        [InlineData("QwRyUoPlKj", "Q W R Y U O P L K J")]
        public void OutputIncorrectLettersAccurately(string guessedLetters, string expectedIncorrectLetters)
        {
            foreach (var letter in guessedLetters)
            {
                _sut.GuessLetter(letter);
            }

            Assert.Equal(expectedIncorrectLetters, _sut.GetIncorrectLetters());
        }

        /// <summary>
        /// Tests that no output is done for invalid letter guesses.
        /// </summary>
        /// <param name="guessedLetter">The guessed letter.</param>
        [Theory]
        [Trait("Category", "GamePlay")]
        [InlineData(',')]
        [InlineData('g')]
        [InlineData('8')]
        [InlineData(null)]
        public void NotOutputLettersInaccurately(char guessedLetter)
        {
            _sut.GuessLetter(guessedLetter);

            Assert.False(_sut.GetCorrectLetters().Contains(guessedLetter) ||
                _sut.GetIncorrectLetters().Contains(guessedLetter));
        }

        #endregion

        #region GameCompletion

        /// <summary>
        /// Tests that there is 0 guesses left after 10 guesses.
        /// </summary>
        /// <param name="guess">The guess containing 10 letters.</param>
        /// <param name="expectedGuessesLeft">The expected guesses left.</param>
        [Theory]
        [Trait("Category", "GameCompletion")]
        [InlineData("QwRyUoPlKj", 0)]
        public void HaveZeroGuessesLeftAfterTenGuesses(string guess, int expectedGuessesLeft)
        {
            string wordGuess = guess.Substring(0, 5);
            string letterGuesses = guess.Substring(5);

            for (int i = 0; i < 5; i++)
            {
                _sut.GuessWord(wordGuess);
            }

            foreach (var letter in letterGuesses)
            {
                _sut.GuessLetter(letter);
            }

            Assert.Equal(expectedGuessesLeft, _sut.GuessesLeft);
        }

        /// <summary>
        /// Tests that the game status is set accurately when all guessed letters are correct.
        /// </summary>
        /// <param name="guessedLetters">The guessed letters.</param>
        /// <param name="expectedGameStatus">The expected game status.</param>
        [Theory]
        [Trait("Category", "GameCompletion")]
        [GameStatusWonData]
        public void SetGameStatusWonWhenAllGuessedLettersAreCorrect(
            string guessedLetters, GameStatus expectedGameStatus)
        {
            foreach (var letter in guessedLetters)
            {
                _sut.GuessLetter(letter);
            }

            Assert.Equal(expectedGameStatus, _sut.GetGameStatus());
        }

        /// <summary>
        /// Tests that the game status is set accurately when guessed word is correct.
        /// </summary>
        /// <param name="guessedWord">The guessed word.</param>
        /// <param name="expectedGameStatus">The expected game status.</param>
        [Theory]
        [Trait("Category", "GameCompletion")]
        [GameStatusWonData]
        public void SetGameStatusWonWhenGuessedWordIsCorrect(
            string guessedWord, GameStatus expectedGameStatus)
        {
            _sut.GuessWord(guessedWord);

            Assert.Equal(expectedGameStatus, _sut.GetGameStatus());
        }

        /// <summary>
        /// Tests that the game status is set accurately when all guessed words was incorrect.
        /// </summary>
        /// <param name="guessedWord">The guessed word.</param>
        /// <param name="expectedGameStatus">The expected game status.</param>
        [Theory]
        [Trait("Category", "GameCompletion")]
        [GameStatusLostData]
        public void SetGameStatusLostAfterTenIncorrectGuessedWords(
            string guessedWord, GameStatus expectedGameStatus)
        {
            for (int i = 0; i < 10; i++)
            {
                _sut.GuessWord(guessedWord);
            }

            Assert.Equal(expectedGameStatus, _sut.GetGameStatus());
        }

        /// <summary>
        /// Tests that the game status is set accurately when all guessed letters was incorrect.
        /// </summary>
        /// <param name="guessedLetters">The guessed letters.</param>
        /// <param name="expectedGameStatus">The expected game status.</param>
        [Theory]
        [Trait("Category", "GameCompletion")]
        [GameStatusLostData]
        public void SetGameStatusLostAfterTenIncorrectGuessedLetters(
            string guessedLetters, GameStatus expectedGameStatus)
        {
            foreach (var letter in guessedLetters)
            {
                _sut.GuessLetter(letter);
            }

            Assert.Equal(expectedGameStatus, _sut.GetGameStatus());
        }

        #endregion
    }
}
