using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Hangman.Tests
{
    /// <summary>
    /// The data attribute to be used in testing the relationship between a won 
    /// game and game status.
    /// </summary>
    class GameStatusWonDataAttribute : DataAttribute
    {
        /// <summary>
        /// Returns the data to be used for the test.
        /// </summary>
        /// <param name="testMethod">The method that is being tested.</param>
        /// <returns>The data.</returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { "TeStInG", GameStatus.Won };
        }
    }
}
