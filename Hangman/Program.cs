using System.Threading.Tasks;

namespace Hangman
{
    /// <summary>
    /// Describes the entrypoint of the Hangman game.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Instantiates the controller of the Hangman game and runs it.
        /// </summary>
        static async Task Main()
        {
            await new Controller().RunAsync();
        }
    }
}
