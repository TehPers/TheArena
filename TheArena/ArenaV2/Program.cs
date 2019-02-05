using System;
using System.Threading.Tasks;
using ArenaV2.Logging;
using Ninject;

namespace ArenaV2 {
    internal class Program {
#if DEBUG
        private const bool DEBUG = true;
#else
        private const bool DEBUG = false;
#endif

        private static Task Main(string[] args) {
            Console.WriteLine("TheArena v2");
            using (StandardKernel kernel = new StandardKernel()) {
                // Load all modules
                kernel.Load(AppDomain.CurrentDomain.GetAssemblies());

                // Additional bindings
                kernel.Bind<bool>().ToConstant(Program.DEBUG).Named("Debug");

                // Start program
                ILogger logger = kernel.Get<ILogger>();
                logger.Log("Kernel loaded", LogLevel.Info);
            }

            Console.WriteLine("Done");
            Console.ReadLine();

            return Task.CompletedTask;
        }
    }
}
