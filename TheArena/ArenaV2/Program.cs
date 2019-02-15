using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ArenaV2.Api;
using ArenaV2.Api.Bindings;
using ArenaV2.Api.Logging;
using ArenaV2.Bindings;
using ArenaV2.Logging;
using NDesk.Options;
using Ninject;

namespace ArenaV2 {
    internal class Program {
#if DEBUG
        private const bool DEBUG = true;
#else
        private const bool DEBUG = false;
#endif

        private static async Task<int> Main(string[] args) {
            Program.InvokeInDebug(() => Console.WriteLine("TheArena v2"));

            int result = await Program.TryInRelease(() => {
                using (StandardKernel kernel = new StandardKernel(new ArenaModule())) {
                    // Additional bindings
                    kernel.Bind<bool>().ToConstant(Program.DEBUG).Named("Debug");

                    // Initialize the logger
                    ILogger logger = kernel.Get<ILogger>();
                    logger.Log("Kernel loaded", LogLevel.Debug);

                    // Get all possible runtime modes
                    INamedBinding<IRuntimeMode>[] runtimeModes = kernel.GetAll<INamedBinding<IRuntimeMode>>().ToArray();
                    logger.Log($"Available runtime modes: {string.Join(", ", runtimeModes.Select(mode => mode.Name))}", LogLevel.Debug);
                    logger.Log($"Selected runtime mode: {args[0]}", LogLevel.Debug);

                    // Check program arguments
                    if (args.Length == 0) {
                        args = new[] { "help" };
                    }

                    // Try to execute the given runtime mode
                    if (runtimeModes.SingleOrDefault(mode => mode.Name == args[0]) is INamedBinding<IRuntimeMode> runtimeMode) {
                        runtimeMode.Value.Execute(runtimeMode.Value.Options.Parse(args.Skip(1)).ToArray());
                    } else {
                        throw new InvalidOperationException($"Unknown runtime mode '{args[0]}', use `help` for a list of possible runtime modes");
                    }
                }

                return Task.FromResult(0);
            }, ex => {
                // Exception occured during initialization
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine("Exeception during initialization:");
                Console.WriteLine(ex.ToString());
                return 1;
            }).ConfigureAwait(true);

            Program.InvokeInDebug(() => {
                Console.WriteLine("Done");
                Console.ReadLine();
            });

            return result;
        }

        [Conditional("DEBUG")]
        private static void InvokeInDebug(Action action) {
            action();
        }

        private static async Task<T> TryInRelease<T>(Func<Task<T>> task, Func<Exception, T> onException) {
#if !DEBUG
            try {
#endif
            return await task().ConfigureAwait(true);
#if !DEBUG
            } catch (Exception ex) {
                return onException(ex);
            }
#endif
        }
    }
}
