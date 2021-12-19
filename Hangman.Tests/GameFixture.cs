using System;

namespace Hangman.Tests
{
    /// <summary>
    /// Makes it possible for tests to share a game instance.
    /// </summary>
    public class GameFixture : IDisposable
    {
        /// <summary>
        /// Initializes and sets a game instance property.
        /// </summary>
        public GameFixture()
        {
            GameInstance = new Game();
        }

        /// <summary>
        /// The game instance property.
        /// </summary>
        public Game GameInstance { get; private set; }

        /// <summary>
        /// The purpose of this method is to free unmanaged resources but in this case the method
        /// is only here because the interface IDisposable requires it.
        /// </summary>
        public void Dispose(){}
    }
}
